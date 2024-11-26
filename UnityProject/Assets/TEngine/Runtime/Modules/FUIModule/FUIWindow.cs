using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    public abstract class FUIWindow : FUIBase
    {
        #region Properties

        private System.Action<FUIWindow> m_prepareCallback;

        private bool m_isCreate = false;

        private GameObject m_gameObject;
        //private Canvas m_canvas;

        //private Canvas[] m_childCanvas;

        //private GraphicRaycaster m_raycaster;

        //private GraphicRaycaster[] m_childRaycaster;

        public override FUIBaseType BaseType => FUIBaseType.Window;
        private static Dictionary<GLoader, string> m_gloaderTextureNames = new();

        public GComponent Component
        {
            get { return m_uiObject; }
            //private set { m_uiObject = value; }
        }

        /// <summary>
        /// 窗口实例资源对象
        /// </summary>
        public override GameObject GameObject
        {
            get
            {
                if (m_gameObject == null)
                {
                    if (m_uiObject != null && !m_uiObject.isDisposed)
                    {
                        m_gameObject = m_uiObject.displayObject.gameObject;
                    }
                }

                return m_gameObject;
            }
        }

        /// <summary>
        /// 窗口位置组件
        /// </summary>
        public override Transform Transform => GameObject.transform;

        /// <summary>
        /// 窗口矩阵位置组件
        /// </summary>
        //public override RectTransform RectTransform => GameObject.transform as RectTransform;


        /// <summary>
        /// 窗口名称
        /// </summary>
        public string WindowName { private set; get; }

        /// <summary>
        /// 窗口层级
        /// </summary>
        public int WindowLayer { private set; get; }

        /// <summary>
        /// 是否为全屏窗口
        /// </summary>
        public bool FullScreen { private set; get; }

        /// <summary>
        /// 是否为内置资源
        /// </summary>
        public bool FromResources { private set; get; }
        
        /// <summary>
        /// 隐藏窗口关闭时间。
        /// </summary>
        public int HideTimeToClose { get; set; }

        public int HideTimerId { get; set; }
        /// <summary>
        /// UI打开句柄
        /// </summary>
        internal FUIOpenOperationHandle Handle { private set; get; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public System.Object UserData
        {
            get
            {
                if (m_userDatas != null && m_userDatas.Length > 0)
                {
                    return m_userDatas[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 自定义数据集
        /// </summary>
        public System.Object[] UserDatas => m_userDatas;


        public int Depth
        {
            get
            {
                if (m_uiObject != null)
                {
                    return m_uiObject.sortingOrder;
                }
                else
                {
                    return 0;
                }
            }

            set
            {
                if (m_uiObject != null)
                {
                    if (m_uiObject.sortingOrder == value)
                    {
                        return;
                    }

                    // 设置父类
                    m_uiObject.sortingOrder = value;

                    // 设置子类
                    //int depth = value;
                    //for (int i = 0; i < m_childCanvas.Length; i++)
                    //{
                    //    var canvas = m_childCanvas[i];
                    //    if (canvas != m_canvas)
                    //    {
                    //        depth += 5; // 注意递增值
                    //        canvas.sortingOrder = depth;
                    //    }
                    //}

                    // 虚函数
                    if (m_isCreate)
                    {
                        OnSortDepth(value);
                    }
                }
            }
        }

        /// <summary>
        /// 窗口可见性
        /// </summary>
        public bool Visible
        {
            get
            {
                if (GameObject != null)
                {
                    return GameObject.layer == FUIModule.WINDOW_SHOW_LAYER;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (GameObject != null)
                {
                    int setLayer = value ? FUIModule.WINDOW_SHOW_LAYER : FUIModule.WINDOW_HIDE_LAYER;
                    if (GameObject.layer == setLayer)
                    {
                        return;
                    }

                    // 显示设置
                    ForceChangeLayers(GameObject.transform, setLayer);
                    //for (int i = 0; i < m_childCanvas.Length; i++)
                    //{
                    //    m_childCanvas[i].gameObject.layer = setLayer;
                    //}

                    // 交互设置
                    Interactable = value;

                    // 虚函数
                    if (m_isCreate)
                    {
                        OnSetVisible(value);
                    }
                }
            }
        }

        private void ForceChangeLayers(Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            foreach (Transform child in trans)
            {
                ForceChangeLayers(child, layer);
            }
        }

        /// <summary>交互性
        /// </summary>
        private bool Interactable
        {
            get
            {
                if (m_uiObject != null)
                {
                    return m_uiObject.touchable;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (m_uiObject != null)
                {
                    m_uiObject.touchable = value;
                    //for (int i = 0; i < m_childRaycaster.Length; i++)
                    //{
                    //    m_childRaycaster[i].enabled = value;
                    //}
                }
            }
        }

        /// <summary>
        /// 是否加载完毕
        /// </summary>
        internal bool IsLoadDone => Handle.IsDone;

        #endregion


        public void Init(string name, int layer, bool fullScreen, bool fromResources = false, params string[] packages)
        {
            WindowName = name;
            WindowLayer = layer;
            FullScreen = fullScreen;
            FromResources = fromResources;
            FuiPackages = packages;
            IsDestroyed = false;
        }

        /// <summary>
        /// 根据名称取得相关的对象
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public virtual GObject GetUIObject(string objName)
        {
            return null;
        }


        internal void TryInvoke(System.Action<FUIWindow> prepareCallback, System.Object[] userDatas)
        {
            base.m_userDatas = userDatas;
            if (IsPrepare)
            {
                prepareCallback?.Invoke(this);
            }
            else
            {
                m_prepareCallback = prepareCallback;
            }
        }

        internal void InternalLoad(System.Action<FUIWindow> prepareCallback, System.Object[] userDatas)
        {
            m_prepareCallback = prepareCallback;

            m_userDatas = userDatas;


            // 创建UIObject
            m_uiObject = new GComponent();
            //直接加到GRoot显示出来
            GameModule.FUI.UIRoot.AddChild(m_uiObject);
            m_uiObject.position = Vector3.zero;
            m_uiObject.scale = Vector3.one;
            string typeName = GetType().Name;
            m_uiObject.name = typeName;
            m_uiObject.displayObject.gameObject.name = typeName;

            Handle = MemoryPool.Acquire<FUIOpenOperationHandle>();
            LoadPackages(FuiPackages, FromResources);

            //if (!FromResources)
            //{
            //    Handle = GameModule.Resource.LoadAssetAsyncHandle<GameObject>(location, needCache: NeedCache);
            //    Handle.Completed += Handle_Completed;
            //}
            //else
            //{
            //    GameObject panel = Object.Instantiate(Resources.Load<GameObject>(location), FUIModule.UIRootStatic);
            //    Handle_Completed(panel);
            //}
        }

        protected override void OnLoadPackagesComplete(bool isSucceed)
        {
            if (isSucceed && !IsDestroyed)
            {
                CreateInstanceAsync().Forget();
            }
            else
            {
                Handle?.OpenFailed();
                CreateFail();
            }
        }

        private async UniTaskVoid CreateInstanceAsync()
        {
            GObject view = await FUICreateInstanceAsync();

            bool isSucceed = false;

            if (m_uiObject != null)
            {
                if (view != null)
                {
                    m_uiObject?.AddChild(view);
                    view.MakeFullScreen();
                    isSucceed = true;
                }
            }
            else
            {
                if (view != null && !view.isDisposed)
                {
                    view?.Dispose();
                }
            }

            if (isSucceed)
            {
                HandleOpenComplete();
            }
            else
            {
                Handle?.OpenFailed();
                CreateFail();
            }
        }


        internal void InternalCreate()
        {
            if (m_isCreate == false)
            {
                m_isCreate = true;
                RegisterEvent();
                OnCreate();
            }
        }

        internal void InternalRefresh()
        {
            OnRefresh();
        }

        internal bool InternalUpdate()
        {
            if (!IsPrepare || !Visible)
            {
                return false;
            }

            List<FUIWidget> listNextUpdateChild = null;
            if (LstChild != null && LstChild.Count > 0)
            {
                listNextUpdateChild = m_listUpdateChild;
                var updateListValid = m_updateListValid;
                List<FUIWidget> listChild;
                if (!updateListValid)
                {
                    if (listNextUpdateChild == null)
                    {
                        listNextUpdateChild = new List<FUIWidget>();
                        m_listUpdateChild = listNextUpdateChild;
                    }
                    else
                    {
                        listNextUpdateChild.Clear();
                    }

                    listChild = LstChild;
                }
                else
                {
                    listChild = listNextUpdateChild;
                }

                for (int i = 0; i < listChild.Count; i++)
                {
                    var uiWidget = listChild[i];

                    if (uiWidget == null)
                    {
                        continue;
                    }

                    TProfiler.BeginSample(uiWidget.Name);
                    var needValid = uiWidget.InternalUpdate();
                    TProfiler.EndSample();

                    if (!updateListValid && needValid)
                    {
                        listNextUpdateChild?.Add(uiWidget);
                    }
                }

                if (!updateListValid)
                {
                    m_updateListValid = true;
                }
            }

            TProfiler.BeginSample("OnUpdate");

            bool needUpdate;
            if (listNextUpdateChild == null || listNextUpdateChild.Count == 0)
            {
                m_hasOverrideUpdate = true;
                OnUpdate();
                needUpdate = m_hasOverrideUpdate;
            }
            else
            {
                OnUpdate();
                needUpdate = true;
            }

            TProfiler.EndSample();

            return needUpdate;
        }

        internal void InternalDestroy()
        {
            m_isCreate = false;
            IsDestroyed = true;

            RemoveAllUIEvent();
            DestroyFuiComponent();
            ReleaseFuiImage();

            for (int i = 0; i < LstChild.Count; i++)
            {
                var uiChild = LstChild[i];
                uiChild.OnDestroy();
                uiChild.OnDestroyWidget();
            }

            // 注销回调函数
            m_prepareCallback = null;

            OnDestroy();

            // 销毁面板对象
            m_uiObject?.Dispose();
            m_uiObject = null;

            if (Handle != null)
            {
                MemoryPool.Release(Handle);
                Handle = null;
            }

            m_userDatas = null;
        }

        /// <summary>
        /// 处理资源加载完成回调
        /// </summary>
        /// <param name="handle"></param>
        //private void Handle_Completed(AssetOperationHandle handle)
        //{
        //    if (handle == null)
        //    {
        //        throw new GameFrameworkException("Load fuiWindow Failed because AssetOperationHandle is null");
        //    }
        //    if (handle.AssetObject == null)
        //    {
        //        throw new GameFrameworkException("Load fuiWindow Failed because AssetObject is null");
        //    }

        //    // 实例化对象
        //    var panel = handle.InstantiateSync(FUIModule.UIRootStatic);
        //    if (!NeedCache)
        //    {
        //        AssetReference.BindAssetReference(panel, Handle, WindowName);
        //    }
        //    Handle_Completed(panel);
        //}


        /// <summary>
        /// 处理资源加载完成回调
        /// </summary>
        private void HandleOpenComplete()
        {
            //// 获取组件
            //m_canvas = GameObject.GetComponent<Canvas>();
            //if (m_canvas == null)
            //{
            //    throw new GameFrameworkException($"Not found {nameof(Canvas)} in panel {WindowName}");
            //}

            //m_canvas.overrideSorting = true;
            //m_canvas.sortingOrder = 0;
            //m_canvas.sortingLayerName = "Default";

            //// 获取组件
            //m_raycaster = GameObject.GetComponent<GraphicRaycaster>();
            //m_childCanvas = GameObject.GetComponentsInChildren<Canvas>(true);
            //m_childRaycaster = GameObject.GetComponentsInChildren<GraphicRaycaster>(true);

            // 通知UI管理器
            IsPrepare = true;
            m_prepareCallback?.Invoke(this);
            Handle?.OpenSucceed();
        }

        private void CreateFail()
        {
            m_prepareCallback?.Invoke(null);
            m_prepareCallback = null;
            if (!IsDestroyed)
            {
                Close();
            }
        }

        protected virtual void Close()
        {
            GameModule.FUI.CloseUI(this.GetType());
        }

        internal void CancelHideToCloseTimer()
        {
            if (HideTimerId > 0)
            {
                GameModule.Timer.RemoveTimer(HideTimerId);
                HideTimerId = 0;
            }
        }

        protected void SetFuiTexture(GLoader loader, string imgName, bool isFromResources = false, string assetsPackageName = "")
        {
            if (loader != null)
            {
                if (string.IsNullOrEmpty(imgName))
                {
                    if(m_gloaderTextureNames.ContainsKey(loader))
                    {
                        FUIExtension.ReleaseImage(m_gloaderTextureNames[loader]);
                    }
                    m_gloaderTextureNames.Remove(loader);
                }
                else
                {
                    if (m_gloaderTextureNames.ContainsKey(loader))
                    {
                        string oldUrl = m_gloaderTextureNames[loader];
                        if (oldUrl != imgName)
                        {
                            FUIExtension.ReleaseImage(oldUrl);
                        }
                        m_gloaderTextureNames[loader] = imgName;
                    }
                    else
                    {
                        m_gloaderTextureNames[loader] = imgName;
                    }
                }

                loader.SetFuiTexture(imgName, isFromResources, assetsPackageName);
            }
        }

        protected void ReleaseFuiImage()
        {
            foreach (KeyValuePair<GLoader,string> keyValuePair in m_gloaderTextureNames)
            {
                string res = keyValuePair.Value;
                FUIExtension.ReleaseImage(res);
            }
        }
    }
}
