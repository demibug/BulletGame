using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// FUI类型
    /// </summary>
    public enum FUIBaseType
    {
        /// <summary>
        /// 类型无
        /// </summary>
        None,

        /// <summary>
        /// 类型window
        /// </summary>
        Window,

        /// <summary>
        /// 类型widget
        /// </summary>
        Widget,
    }

    /// <summary>
    /// FUI基类
    /// </summary>
    public class FUIBase : IFUIBehaviour
    {
        /// <summary>
        /// 所属UI父节点
        /// </summary>
        protected FUIBase m_parent = null;

        /// <summary>
        /// UI父节点
        /// </summary>
        public FUIBase Parent => m_parent;

        /// <summary>
        /// 自定义数据集
        /// </summary>
        protected System.Object[] m_userDatas;

        protected GComponent m_uiObject;

        /// <summary>
        /// 窗口实例资源对象
        /// </summary>
        public virtual GameObject GameObject { protected set; get; }

        /// <summary>
        /// 窗口位置组件
        /// </summary>
        public virtual Transform Transform { protected set; get; }

        /// <summary>
        /// 窗口矩阵位置组件
        /// </summary>
        //public virtual RectTransform RectTransform { protected set; get; }

        /// <summary>
        /// UI类型
        /// </summary>
        public virtual FUIBaseType BaseType => FUIBaseType.None;

        /// <summary>
        /// 资源是否准备完毕
        /// </summary>
        public bool IsPrepare { protected set; get; }

        /// <summary>
        /// 是否已销毁
        /// </summary>
        public bool IsDestroyed { protected set; get; }

        /// <summary>
        /// 使用到的包
        /// </summary>
        public string[] FuiPackages { protected set; get; }

        /// <summary>
        /// UI子组件列表
        /// </summary>
        public List<FUIWidget> LstChild = new List<FUIWidget>();

        /// <summary>
        /// 存在Update更新的UI子组件列表
        /// </summary>
        protected List<FUIWidget> m_listUpdateChild = null;

        /// <summary>
        /// 是否持有Update行为
        /// </summary>
        protected bool m_updateListValid = false;

        /// <summary>
        /// 注册事件
        /// </summary>
        public virtual void RegisterEvent()
        {
        }

        /// <summary>
        /// 包绑定
        /// </summary>
        public virtual void CheckBindAll()
        {
        }

        /// <summary>
        /// 同步创建FUI对象
        /// </summary>
        protected virtual GObject FUICreateInstance()
        {
            return null;
        }

        /// <summary>
        /// 异步创建FUI对象
        /// </summary>
        protected virtual async UniTask<GObject> FUICreateInstanceAsync()
        {
            await UniTask.Yield();
            return null;
        }

        /// <summary>
        /// 窗口创建
        /// </summary>
        public virtual void OnCreate()
        {
        }

        /// <summary>
        /// 窗口销毁
        /// </summary>
        public virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 窗口刷新
        /// </summary>
        public virtual void OnRefresh()
        {
        }

        /// <summary>
        /// 是否需要Update
        /// </summary>
        protected bool m_hasOverrideUpdate = true;

        /// <summary>
        /// 窗口更新
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 当出发窗口的层级排序
        /// </summary>
        /// <param name="depth"></param>
        protected virtual void OnSortDepth(int depth)
        {
        }

        /// <summary>
        /// 当因为全屏遮挡出发窗口的显隐
        /// </summary>
        /// <param name="visible"></param>
        protected virtual void OnSetVisible(bool visible)
        {
        }


        internal void SetUpdateDirty()
        {
            m_updateListValid = false;
            m_parent?.SetUpdateDirty();
        }

        //#region FindChildComponent

        //public Transform FindChild(string path)
        //{
        //    return UnityExtension.FindChild(RectTransform, path);
        //}

        //public Transform FindChild(Transform trans, string path)
        //{
        //    return UnityExtension.FindChild(trans, path);
        //}

        //public T FindChildComponent<T>(string path) where T : Component
        //{
        //    return RectTransform.FindChildComponent<T>(path);
        //}

        //public T FindChildComponent<T>(Transform trans, string path) where T : Component
        //{
        //    return trans.FindChildComponent<T>(path);
        //}

        //#endregion

        #region UIEvent

        private GameEventMgr m_eventMgr;

        protected GameEventMgr eventMgr
        {
            get
            {
                if (m_eventMgr == null)
                {
                    m_eventMgr = MemoryPool.Acquire<GameEventMgr>();
                }

                return m_eventMgr;
            }
        }

        public void AddUIEvent(int eventType, System.Action handler)
        {
            eventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T>(int eventType, System.Action<T> handler)
        {
            eventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U>(int eventType, System.Action<T, U> handler)
        {
            eventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U, V>(int eventType, System.Action<T, U, V> handler)
        {
            eventMgr.AddEvent(eventType, handler);
        }

        protected void AddUIEvent<T, U, V, W>(int eventType, System.Action<T, U, V, W> handler)
        {
            eventMgr.AddEvent(eventType, handler);
        }


        protected void RemoveAllUIEvent()
        {
            if (m_eventMgr != null)
            {
                MemoryPool.Release(m_eventMgr);
            }
        }

        #endregion

        #region UIWidget

        /// <summary>
        /// 创建窗口组件(会加载组件所需的包)
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="prepareCallback">创建窗口组件回调,成功回调组件自身,失败回调 null </param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件,实际可用要根据prepareCallback</returns>
        public T CreateWidget<T>(System.Action<T> prepareCallback, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            return CreateWidget<T>(m_uiObject, prepareCallback, userDatas);
        }

        /// <summary>
        /// 创建窗口组件 父UI为当前UI, 父节点自定义(会加载组件所需的包)
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="prepareCallback">创建窗口组件回调,成功回调组件自身,失败回调 null</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件,实际可用要根据prepareCallback</returns>
        public T CreateWidget<T>(GObject parentObj, System.Action<T> prepareCallback, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            return CreateWidget<T>(this, parentObj, prepareCallback, userDatas);
        }

        /// <summary>
        /// 创建窗口组件 父UI,父节点都自定义(会加载组件所需的包)
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentUI">创建窗口组件的父UI</param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="prepareCallback">创建窗口组件回调,成功回调组件自身,失败回调 null</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件,当prepareCallback回调时真正可用</returns>
        public T CreateWidget<T>(FUIBase parentUI, GObject parentObj, System.Action<T> prepareCallback, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            FUIBase _parentUI = parentUI;
            GObject _parentObj = parentObj;
            if (_parentObj != null)
            {
                T widget = CreateWidgetInstance<T>();
                widget.InternalLoad(_parentUI, parentObj, prepareCallback as System.Action<FUIWidget>, userDatas);
                return widget;
            }

            return null;
        }

        /// <summary>
        /// 异步创建窗口组件 父UI为当前UI, 父节点自定义(不会主动加载组件所需的包,需要先手动加载)
        /// 需要调用 LoadPackages 主动加载一次包
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件</returns>
        public async UniTask<T> CreateWidgetAsync<T>(GObject parentObj, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            return await CreateWidgetAsync<T>(this, parentObj, userDatas);
        }

        /// <summary>
        /// 异步创建窗口组件 父UI,父节点都自定义(不会主动加载组件所需的包，需要先手动加载)
        /// 需要调用 LoadPackages 主动加载一次包
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentUI">创建窗口组件的父UI</param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件</returns>
        public async UniTask<T> CreateWidgetAsync<T>(FUIBase parentUI, GObject parentObj, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            FUIBase _parentUI = parentUI;
            GObject _parentObj = parentObj;
            if (_parentObj != null)
            {
                T widget = CreateWidgetInstance<T>();
                FUIWidget fui = await widget.InternalLoadAsync(_parentUI, parentObj, userDatas);
                if (fui != null)
                {
                    return widget;
                }
                else
                {
                    // 销毁
                    fui.Destroy();
                }
            }

            return null;
        }

        /// <summary>
        /// 同步创建窗口组件 父UI为当前UI, 父节点自定义(不会主动加载组件所需的包)
        /// 需要调用 LoadPackages 主动加载一次包
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentUI">创建窗口组件的父UI</param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件</returns>
        public T CreateWidgetSync<T>(GObject parentObj, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            return CreateWidgetSync<T>(this, parentObj, userDatas);
        }

        /// <summary>
        /// 同步创建窗口组件 父UI,父节点都自定义(不会主动加载组件所需的包)
        /// 需要调用 LoadPackages 主动加载一次包
        /// </summary>
        /// <typeparam name="T">窗口组件类型</typeparam>
        /// <param name="parentUI">创建窗口组件的父UI</param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="userDatas">自定义数据</param>
        /// <returns>窗口组件</returns>
        public T CreateWidgetSync<T>(FUIBase parentUI, GObject parentObj, params System.Object[] userDatas) where T : FUIWidget, new()
        {
            FUIBase _parentUI = parentUI;
            GObject _parentObj = parentObj;
            if (_parentObj != null)
            {
                T widget = CreateWidgetInstance<T>();
                FUIWidget fui = widget.InternalLoadSync(_parentUI, parentObj, userDatas);
                if (fui != null)
                {
                    return widget;
                }
                else
                {
                    // 销毁
                    fui.Destroy();
                }
            }

            return null;
        }

        private T CreateWidgetInstance<T>() where T : FUIWidget, new()
        {
            T widget = new();
            System.Type type = typeof(T);
            FUIWidgetAttribute attribute = System.Attribute.GetCustomAttribute(type, typeof(FUIWidgetAttribute)) as FUIWidgetAttribute;
            if (widget == null)
            {
                throw new GameFrameworkException($"FUIWidget {type.FullName} create instance failed.");
            }

            if (attribute == null)
            {
                throw new GameFrameworkException($"FUIWidget {type.FullName} not found {nameof(FUIWidgetAttribute)} attribute.");
            }

            widget.Init(attribute.Packages);
            // 检测包是否绑定
            widget.CheckBindAll();
            return widget;
        }

        /// <summary>
        /// 调整图标数量
        /// 常用于Icon创建
        /// </summary>
        /// <typeparam name="T">图标类型</typeparam>
        /// <param name="listIcon">存放Icon的列表</param>
        /// <param name="number">创建数目</param>
        /// <param name="parentObj">窗口组件父节点</param>
        public void AdjustIconNum<T>(List<T> listIcon, int number, GObject parentObj) where T : FUIWidget, new()
        {
            if (listIcon == null)
            {
                listIcon = new List<T>();
            }

            if (listIcon.Count < number)
            {
                int needNum = number - listIcon.Count;
                for (int iconIdx = 0; iconIdx < needNum; iconIdx++)
                {
                    T tmpT = CreateWidgetSync<T>(parentObj);
                    if (tmpT != null)
                    {
                        listIcon.Add(tmpT);
                    }
                }
            }
            else if (listIcon.Count > number)
            {
                RemoveUnUseItem<T>(listIcon, number);
            }
        }

        /// <summary>
        /// 异步调整图标数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listIcon"></param>
        /// <param name="tarNum"></param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="maxNumPerFrame"></param>
        /// <param name="updateAction"></param>
        public void AdjustIconNumAsync<T>(List<T> listIcon, int tarNum, GObject parentObj, int maxNumPerFrame = 5, System.Action<T, int> updateAction = null)
            where T : FUIWidget, new()
        {
            InternalAdjustIconMunAsync<T>(listIcon, tarNum, parentObj, maxNumPerFrame, updateAction).Forget();
        }

        /// <summary>
        /// 异步调整图标数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listIcon"></param>
        /// <param name="tarNum"></param>
        /// <param name="parentObj">窗口组件父节点</param>
        /// <param name="maxNumPerFrame"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        private async UniTaskVoid InternalAdjustIconMunAsync<T>(List<T> listIcon, int tarNum, GObject parentObj, int maxNumPerFrame, System.Action<T, int> updateAction)
            where T : FUIWidget, new()
        {
            if (listIcon == null)
            {
                listIcon = new List<T>();
            }

            int createCnt = 0;

            for (int i = 0; i < tarNum; i++)
            {
                T tmpT = null;
                if (i < listIcon.Count)
                {
                    tmpT = listIcon[i];
                }
                else
                {
                    tmpT = await CreateWidgetAsync<T>(parentObj);
                    if (tmpT != null)
                    {
                        listIcon.Add(tmpT);
                    }
                }

                int index = i;
                if (updateAction != null)
                {
                    updateAction(tmpT, index);
                }

                createCnt++;
                if (createCnt >= maxNumPerFrame)
                {
                    createCnt = 0;
                    await UniTask.Yield();
                }
            }

            if (listIcon.Count > tarNum)
            {
                RemoveUnUseItem(listIcon, tarNum);
            }
        }

        private void RemoveUnUseItem<T>(List<T> listIcon, int tarNum) where T : FUIWidget
        {
            var removeIcon = new List<T>();
            for (int iconIdx = 0; iconIdx < listIcon.Count; iconIdx++)
            {
                var icon = listIcon[iconIdx];
                if (iconIdx >= tarNum)
                {
                    removeIcon.Add(icon);
                }
            }

            for (var index = 0; index < removeIcon.Count; index++)
            {
                var icon = removeIcon[index];
                listIcon.Remove(icon);
                icon.OnDestroy();
                icon.OnDestroyWidget();
                LstChild.Remove(icon);
                if (icon.GameObject != null)
                {
                    UnityEngine.Object.Destroy(icon.GameObject);
                }
            }
        }

        #endregion

        #region Load Packages

        /// <summary>
        /// 加载UIPackage
        /// </summary>
        /// <param name="fuiPackageNames"></param>
        protected void LoadPackages(string[] fuiPackageNames,  bool fromResources, string assetsPackageName = "")
        {
            if (m_uiObject == null)
            {
                throw new GameFrameworkException("Load FUI Failed because m_uiObject is null.");
            }

            if (!m_uiObject.isDisposed)
            {
                if (!fromResources)
                {
                    GameModule.FUI.AddPackage(m_uiObject, fuiPackageNames, OnLoadPackages, assetsPackageName);
                }
                else
                {
                    for (int i = 0; i < fuiPackageNames.Length; i++)
                    {
                        string pack = fuiPackageNames[i];
                        if (!string.IsNullOrEmpty(pack))
                        {
                            UIPackage.AddPackage(pack);
                        }
                    }

                    OnLoadPackagesComplete(true);
                }
            }
            else
            {
                OnLoadPackagesComplete(false);
            }
        }

        /// <summary>
        /// UIPackage 全部加载完成后的回调
        /// </summary>
        /// <param name="isSucceed"></param>
        protected virtual void OnLoadPackagesComplete(bool isSucceed)
        {
        }

        private void OnLoadPackages(bool isSucceed)
        {
            // 是否已完成
            OnLoadPackagesComplete(isSucceed);
        }

        #endregion
    }
}
