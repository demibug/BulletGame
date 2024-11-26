using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace TEngine
{
    /// <summary>
    /// FUI模块
    /// 框架层，不做热更，逻辑修改不要放在这里
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FUIModule : Module
    {
        [SerializeField] private bool m_dontDestroyUIRoot = true;

        [SerializeField] private bool m_enableErrorLog = true;

        [SerializeField] private Camera m_UICamera = null;

        private GRoot m_instanceRoot = null;

        private readonly List<FUIWindow> m_stack = new List<FUIWindow>(100);

        public const int LAYER_DEEP = 2000;
        public const int WINDOW_DEEP = 100;
        public const int WINDOW_HIDE_LAYER = 2; // Ignore Raycast
        public const int WINDOW_SHOW_LAYER = 5; // UI

        /// <summary>
        /// UI根节点
        /// </summary>
        public GRoot UIRoot => m_instanceRoot;

        /// <summary>
        /// GUI Y轴偏移量
        /// </summary>
        public int UITopOffsetY { get; private set; } = 0;
        public int UIBottomOffsetY { get; private set; } = 0;

        public static Transform UIRootStatic;

        public Camera UICamera => m_UICamera;

        private FUIModuleImpl m_uiModelImpl;

        private ErrorLogger m_errorLogger;

        private void Start()
        {
            RootModule rootModule = ModuleSystem.GetModule<RootModule>();
            if (rootModule == null)
            {
                Log.Fatal("Base component is invalid");
                return;
            }

            m_uiModelImpl = ModuleImpSystem.GetModuleNotSubString<FUIModuleImpl>();
            m_uiModelImpl.Initialize(m_stack);

            // 适配 19220 * 1080
            GRoot.inst.SetContentScaleFactor(Constant.DesignWidth, Constant.DesignHeight);

            //当前屏幕屏占比计算
            float ws = Screen.width * 1f;
            float wh = Screen.height * 1f;
            float per = (ws > wh) ? ws / wh : wh / ws;
            Log.Debug("Screen Size: " + ws + ":" + wh + " --> " + per);

            bool isProcessLiuhai = false;
            if (isProcessLiuhai)
            {
                //顶部流海处理
                UITopOffsetY = (per < 2) ? 0 : 40;
                UIBottomOffsetY = (per < 2) ? 0 : 20;
                GRoot.inst.y = UITopOffsetY;
                GRoot.inst.height -= Mathf.CeilToInt((UITopOffsetY + UIBottomOffsetY) / UIContentScaler.scaleFactor);
            }

            UIConfig.enhancedTextOutlineEffect = true;  //开启8方向描边渲染

            //Log.Warning("size: screen->" + Screen.height + " : ui->" + GRoot.inst.height);

            if (m_instanceRoot == null)
            {
                m_instanceRoot = GRoot.inst;
                //m_instanceRoot = new GameObject("FUI Form Instances").transform;
                //m_instanceRoot.SetParent(gameObject.transform);
                //m_instanceRoot.localScale = Vector3.one;
            }
            else if (m_dontDestroyUIRoot)
            {
                Transform trans = m_instanceRoot.displayObject.gameObject.transform;
                DontDestroyOnLoad(trans.parent != null ? trans.parent : trans);
            }

            Transform camTrans = m_UICamera.transform;
            camTrans.name = "FUI Camera";
            DontDestroyOnLoad(camTrans);

            m_instanceRoot.displayObject.gameObject.layer = LayerMask.NameToLayer("UI");
            UIRootStatic = m_instanceRoot.displayObject.gameObject.transform;

            switch (GameModule.Debugger.ActiveWindowType)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    m_enableErrorLog = true;
                    break;
                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    m_enableErrorLog = Debug.isDebugBuild;
                    break;
                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    m_enableErrorLog = Application.isEditor;
                    break;

                default:
                    m_enableErrorLog = false;
                    break;
            }

            if (m_enableErrorLog)
            {
                m_errorLogger = new ErrorLogger();
            }
        }

        private void OnDestroy()
        {
            if (m_errorLogger != null)
            {
                m_errorLogger.Dispose();
                m_errorLogger = null;
            }

            //CloseAll();
            //if (m_instanceRoot != null && m_instanceRoot.parent != null && !m_instanceRoot.parent.isDisposed)
            //{
            //    m_instanceRoot.Dispose();
            //    //Destroy(m_instanceRoot.parent.displayObject.gameObject);
            //}
        }


        #region Safe Area

        public static bool IsNotchScreen()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject window = currentActivity.Call<AndroidJavaObject>("getWindow");

            AndroidJavaObject decorView = window.Call<AndroidJavaObject>("getDecorView");
            AndroidJavaObject rootView = decorView.Call<AndroidJavaObject>("getRootView");
            AndroidJavaObject displayCutout = rootView.Call<AndroidJavaObject>("getDisplayCutout");
            if(displayCutout != null)
            {
                Debug.Log("This device has a notch!");
                return true;
            }
            else
            {
                Debug.Log("This device does not has a notch!");
            }
            return false;
        }


        /// <summary>
        /// 设置屏幕安全区域(异形屏支持)
        /// </summary>
        /// <param name="safeRect">安全区域</param>
        public static void ApplyScreenSafeRect(Rect safeRect)
        {
            CanvasScaler scaler = UIRootStatic.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                Log.Error($"Not found {nameof(CanvasScaler)} !");
                return;
            }

            // Convert safe area rectangle from absolute pixels to FGUI coordinates
            float rateX = scaler.referenceResolution.x / Screen.width;
            float rateY = scaler.referenceResolution.y / Screen.height;
            float posX = (int)(safeRect.position.x * rateX);
            float posY = (int)(safeRect.position.y * rateY);
            float width = (int)(safeRect.size.x * rateX);
            float height = (int)(safeRect.size.y * rateY);

            float offsetMaxX = scaler.referenceResolution.x - width - posX;
            float offsetMaxY = scaler.referenceResolution.y - height - posY;

            // 注意:安全区左边系的原点为左下角
            var rectTrans = UIRootStatic.transform as RectTransform;
            if (rectTrans != null)
            {
                rectTrans.offsetMin = new Vector2(posX, posY); // 锚框状态下的屏幕左下角偏移向量
                rectTrans.offsetMax = new Vector2(-offsetMaxX, -offsetMaxY); // 锚框状态下的屏幕右上角偏移量
            }
        }

        /// <summary>
        /// 模拟IPhoneX异形屏
        /// </summary>
        public static void SimulateIPhoneXNotchScreen()
        {
            Rect rect;
            if (Screen.height > Screen.width)
            {
                // 竖屏 Portrait
                float deviceWidth = 1125f;
                float deviceHeight = 2436f;
                rect = new Rect(0f / deviceWidth, 102f / deviceHeight, 1125f / deviceWidth, 2202f / deviceHeight);
            }
            else
            {
                // 横屏 Landscape
                float deviceWidth = 2436f;
                float deviceHeight = 1125f;
                rect = new Rect(132f / deviceWidth, 63f / deviceHeight, 2172f / deviceWidth, 1062f / deviceHeight);
            }

            Rect safeArea = new Rect(Screen.width * rect.x, Screen.height * rect.y, Screen.width * rect.width, Screen.height * rect.height);
            ApplyScreenSafeRect(safeArea);
        }

        //private static Rect GetRectWindow()
        //{

        //}

        #endregion

        /// <summary>
        /// 获取所有层级下顶部的窗口名称
        /// </summary>
        /// <returns></returns>
        public string GetTopWindow()
        {
            if (m_stack.Count == 0)
            {
                return string.Empty;
            }

            FUIWindow topWindow = m_stack[^1];
            return topWindow.WindowName;
        }

        /// <summary>
        /// 是否有任意窗口正在加载
        /// </summary>
        /// <returns></returns>
        public bool IsAnyLoading()
        {
            for (int i = 0; i < m_stack.Count; i++)
            {
                var window = m_stack[i];
                if (window.IsLoadDone == false)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 查询窗口是否存在
        /// </summary>
        /// <typeparam name="T">界面类型</typeparam>
        /// <returns>是否存在</returns>
        public bool HasWindow<T>()
        {
            return HasWindow(typeof(T));
        }


        /// <summary>
        /// 查询窗口是否存在
        /// </summary>
        /// <param name="type">界面类型</param>
        /// <returns>是否存在</returns>
        public bool HasWindow(System.Type type)
        {
            return IsContains(type.FullName);
        }

        /// <summary>
        /// 查询窗口是否加载中
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsWindowLoading(System.Type type)
        {
            FUIWindow window = GetWindow(type.FullName);
            if (window != null)
            {
                return !window.IsLoadDone;
            }

            return false;
        }

        /// <summary>
        /// 异步打开窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userDatas">用户自定义数据</param>
        /// <returns>打开窗口操作句柄</returns>
        public void ShowUIAsync<T>(params System.Object[] userDatas) where T : FUIWindow
        {
            ShowUIImp(typeof(T), userDatas);
        }
        
        /// <summary>
        /// 同步打开窗口。
        /// </summary>
        /// <typeparam name="T">窗口类。</typeparam>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public void ShowUI<T>(params System.Object[] userDatas) where T : UIWindow
        {
            ShowUIImp(typeof(T), userDatas);
        }
        
        /// <summary>
        /// 异步打开窗口。
        /// </summary>
        /// <param name="userDatas">用户自定义数据。</param>
        /// <returns>打开窗口操作句柄。</returns>
        public async UniTask<FUIWindow> ShowUIAsyncAwait<T>(params System.Object[] userDatas) where T : UIWindow
        {
            return await ShowUIAwaitImp(typeof(T), true, userDatas);
        }

        private void ShowUIImp(System.Type type, params System.Object[] userDatas)
        {
            string windowName = type.FullName;
            Log.Info("ShowUI " + windowName);

            if (IsContains(windowName))
            {
                FUIWindow window = GetWindow(windowName);
                Pop(window); //弹出窗口
                Push(window); //重新压入
                window.TryInvoke(OnWindowPrepare, userDatas);
            }
            else
            {
                FUIWindow window = CreateInstance(type);
                Push(window); //首次压入
                window.InternalLoad(OnWindowPrepare, userDatas);
            }
        }
        
        private async UniTask<FUIWindow> ShowUIAwaitImp(System.Type type, params System.Object[] userDatas)
        {
            string windowName = type.FullName;
            Log.Info("ShowUIAwait " + windowName);

            // 如果窗口已经存在
            if (IsContains(windowName))
            {
                FUIWindow window = GetWindow(windowName);
                Pop(window); //弹出窗口
                Push(window); //重新压入
                window.TryInvoke(OnWindowPrepare, userDatas);
                return window;
            }
            else
            {
                FUIWindow window = CreateInstance(type);
                Push(window); //首次压入
                window.InternalLoad(OnWindowPrepare, userDatas);
                float time = 0f;
                while (!window.IsLoadDone)
                {
                    time += Time.time;
                    if (time > 60f)
                    {
                        break;
                    }
                    await UniTask.Yield();
                }
                return window;
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseUI<T>() where T : FUIWindow
        {
            CloseUI(typeof(T));
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="type"></param>
        public void CloseUI(System.Type type)
        {
            string windowName = type.FullName;
            FUIWindow window = GetWindow(windowName);
            if (window == null)
                return;

            //Log.Info($"<< CloseUI >> : {windowName}");
            Log.Debug($"<color=#5f9ea0><< CloseUI >> : " + windowName + " </color>");

            window.InternalDestroy();
            Pop(window);
            OnSortWindowDepth(window.WindowLayer);
            OnSetWindowVisible();
        }
        
        public void HideUI<T>() where T : UIWindow
        {
            HideUI(typeof(T));
        }

        public void HideUI(System.Type type)
        {
            string windowName = type.FullName;
            FUIWindow window = GetWindow(windowName);
            if (window == null)
            {
                return;
            }

            if (window.HideTimeToClose <= 0)
            {
                CloseUI(type);
                return;
            }
            
            window.Visible = false;
            window.HideTimerId = GameModule.Timer.AddTimer((arg) =>
            {
                CloseUI(type);
            },window.HideTimeToClose);
        }

        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        public void CloseAll()
        {
            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
                window?.InternalDestroy();
            }

            m_stack.Clear();
        }

        /// <summary>
        /// 关闭所有窗口除了
        /// </summary>
        /// <param name="withoutWnd"></param>
        public void CloseAllWithout(FUIWindow withoutWnd)
        {
            for (int i = m_stack.Count - 1; i >= 0; i--)
            {
                FUIWindow window = m_stack[i];
                if (window == withoutWnd)
                {
                    continue;
                }

                window.InternalDestroy();
                m_stack.RemoveAt(i);
            }
        }

        /// <summary>
        /// 关闭所有窗口除了
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseAllWithout<T>() where T : FUIWindow
        {
            for (int i = m_stack.Count - 1; i >= 0; i--)
            {
                FUIWindow window = m_stack[i];
                if (window.GetType() == typeof(T))
                {
                    continue;
                }

                window.InternalDestroy();
                m_stack.RemoveAt(i);
            }
        }


        private void OnWindowPrepare(FUIWindow window)
        {
            if (window != null)
            {
                OnSortWindowDepth(window.WindowLayer);
                window.InternalCreate();
                window.InternalRefresh();
                OnSetWindowVisible();
            }
        }

        private void OnSortWindowDepth(int layer)
        {
            int depth = layer * LAYER_DEEP;
            for (int i = 0; i < m_stack.Count; i++)
            {
                if (m_stack[i].WindowLayer == layer)
                {
                    m_stack[i].Depth = depth;
                    depth += WINDOW_DEEP;
                }
            }
        }

        private void OnSetWindowVisible()
        {
            bool isHideNext = false;
            for (int i = m_stack.Count - 1; i >= 0; i--)
            {
                FUIWindow window = m_stack[i];
                if (isHideNext == false)
                {
                    window.Visible = true;
                    if (window.IsPrepare && window.FullScreen)
                    {
                        isHideNext = true;
                    }
                }
                else
                {
                    if(window.WindowLayer == (int)FUILayer.Bottom)
                    {
                        window.Visible = true;
                    }
                    else
                    {
                        window.Visible = false;
                    }
                }
            }
        }

        private FUIWindow CreateInstance(System.Type type)
        {
            FUIWindow window = System.Activator.CreateInstance(type) as FUIWindow;
            FUIWindowAttribute attribute = System.Attribute.GetCustomAttribute(type, typeof(FUIWindowAttribute)) as FUIWindowAttribute;
            if (window == null)
                throw new GameFrameworkException($"Window {type.FullName} create instance failed.");
            if (attribute == null)
                throw new GameFrameworkException($"Window {type.FullName} not found {nameof(FUIWindowAttribute)} attribute.");

            window.Init(type.FullName, attribute.WindowLayer, attribute.FullScreen, attribute.FromResources, attribute.Packages);
            window.CheckBindAll();
            return window;
        }

        public FUIWindow GetWindow(string windowName)
        {
            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
                if (window.WindowName == windowName)
                {
                    return window;
                }
            }

            return null;
        }

        private bool IsContains(string windowName)
        {
            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
                if (window.WindowName == windowName)
                {
                    return true;
                }
            }

            return false;
        }

        private void Push(FUIWindow window)
        {
            // 如果已经存在
            if (IsContains(window.WindowName))
                throw new GameFrameworkException($"Window {window.WindowName} is exist.");

            // 获取插入到所属层级的位置
            int insertIndex = -1;
            for (int i = 0; i < m_stack.Count; i++)
            {
                if (window.WindowLayer == m_stack[i].WindowLayer)
                {
                    insertIndex = i + 1;
                }
            }

            // 如果没有所属层级，找到相邻层级
            if (insertIndex == -1)
            {
                for (int i = 0; i < m_stack.Count; i++)
                {
                    if (window.WindowLayer > m_stack[i].WindowLayer)
                    {
                        insertIndex = i + 1;
                    }
                }
            }

            // 如果是空栈或没有找到插入位置
            if (insertIndex == -1)
            {
                insertIndex = 0;
            }

            // 最后插入到堆栈
            m_stack.Insert(insertIndex, window);
        }

        private void Pop(FUIWindow window)
        {
            // 从堆栈里移除
            m_stack.Remove(window);
        }

        #region UIPackage Manager

        /// <summary>
        /// 加载UIPackage列表
        /// 图集纹理数量不能超过一个
        /// </summary>
        /// <param name="uiObject">GObject对象</param>
        /// <param name="fuiPackageNames">包名列表</param>
        /// <param name="onAddPackage">加载完成回调代理</param>
        /// <param name="needCache">是否需要缓存</param>
        public void AddPackage(GObject uiObject, string[] fuiPackageNames, System.Action<bool> onAddPackage, string assetsPackageName)
        {
            FUIPackageManager.AddPackage(uiObject, fuiPackageNames, onAddPackage, assetsPackageName);
        }

        /// <summary>
        /// 包是否已加载
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool IsLoadedPackage(string packageName)
        {
            return FUIPackageManager.IsLoadedPackage(packageName);
        }
        
        /// <summary>
        /// 检测并进行包绑定
        /// </summary>
        /// <param name="binderId"></param>
        /// <param name="bindFunc"></param>
        public void CheckBindAll(int binderId, System.Action bindFunc)
        {
            FUIPackageManager.CheckBindAll(binderId, bindFunc);
        }

        #endregion
    }

    [UpdateModule]
    internal sealed partial class FUIModuleImpl : ModuleImp
    {
        private List<FUIWindow> m_stack;

        internal void Initialize(List<FUIWindow> stack)
        {
            m_stack = stack;
        }

        internal override void Shutdown()
        {
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_stack == null)
            {
                return;
            }

            int count = m_stack.Count;
            for (int i = 0; i < m_stack.Count; i++)
            {
                if (m_stack.Count != count)
                {
                    break;
                }

                var window = m_stack[i];
                window.InternalUpdate();
            }
        }
    }
}
