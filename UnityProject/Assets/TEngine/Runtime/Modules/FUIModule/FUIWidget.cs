using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// 窗口组件
    /// </summary>
    public abstract class FUIWidget : FUIBase, IFUIBehaviour
    {
        #region Properties

        private System.Action<FUIWidget> m_prepareCallback;

        /// <summary>
        /// 窗口组件的实例资源对象
        /// </summary>
        public override GameObject GameObject { protected set; get; }


        /// <summary>
        /// 窗口组件矩阵位置组件
        /// </summary>
        //public override RectTransform RectTransform { protected set; get; }

        /// <summary>
        /// 窗口位置组件
        /// </summary>
        public override Transform Transform { protected set; get; }

        /// <summary>
        /// 窗口组件名称
        /// </summary>
        public string Name { protected set; get; } = string.Empty;

        /// <summary>
        /// FUI类型
        /// </summary>
        public override FUIBaseType BaseType => FUIBaseType.Widget;

        /// <summary>
        /// 窗口组件父节点
        /// </summary>
        private GObject m_widgetRoot;


        /// <summary>
        /// 所属的窗口
        /// </summary>
        public FUIWindow OwnerWindow
        {
            get
            {
                var parentUI = base.m_parent;
                while (parentUI != null)
                {
                    if (parentUI.BaseType == FUIBaseType.Window)
                    {
                        return parentUI as FUIWindow;
                    }

                    parentUI = parentUI.Parent;
                }

                return null;
            }
        }


        /// <summary>
        /// 窗口可见性
        /// </summary>
        public bool Visible
        {
            get => m_uiObject.visible;

            set
            {
                m_uiObject.visible = value;
                OnSetVisible(value);
            }
        }

        #endregion


        internal bool InternalUpdate()
        {
            if (!IsPrepare)
            {
                return false;
            }

            List<FUIWidget> listNextUpdateChild = null;
            if (LstChild != null && LstChild.Count > 0)
            {
                listNextUpdateChild = m_listUpdateChild;
                var updateListValid = m_updateListValid;
                List<FUIWidget> _lstChild;
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

                    _lstChild = LstChild;
                }
                else
                {
                    _lstChild = listNextUpdateChild;
                }

                for (int i = 0; i < _lstChild.Count; i++)
                {
                    var uiWidget = _lstChild[i];
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

            bool needUpdate = false;
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

        #region Create

        /// <summary>
        /// 初始化创建
        /// </summary>
        /// <param name="packages"></param>
        public void Init(params string[] packages)
        {
            FuiPackages = packages;
            IsDestroyed = false;
        }

        /// <summary>
        /// 检测widget关联的package是否已加载
        /// </summary>
        /// <returns></returns>
        private bool CheckPackageLoaded()
        {
            bool isAllLoaded = true;
            for (int i = 0; i < FuiPackages.Length; i++)
            {
                string packageName = FuiPackages[i];
                if (!string.IsNullOrEmpty(packageName))
                {
                    bool isLoaded = GameModule.FUI.IsLoadedPackage(packageName);
                    if (!isLoaded)
                    {
                        throw new GameFrameworkException($"FUIWidget CheckPackageLoaded  Failed because package: {packageName} is not loaded.");
                        //isAllLoaded = false;
                        //Log.Error($"FUIWidget CheckPackageLoaded  Failed because package: {packageName} is not loaded.");
                        //break;
                    }
                }
            }

            return isAllLoaded;
        }

        internal void InternalLoad(FUIBase parentUI, GObject widgetRoot, System.Action<FUIWidget> prepareCallback, System.Object[] userDatas)
        {
            ResetChildCanvas(parentUI);
            m_parent = parentUI;
            m_parent?.LstChild.Add(this);
            m_prepareCallback = prepareCallback;
            m_userDatas = userDatas;
            m_widgetRoot = widgetRoot;

            // 创建UIObject
            m_uiObject = new GComponent();
            widgetRoot.asCom?.AddChild(m_uiObject);
            m_uiObject.scale = Vector3.one;
            m_uiObject.position = Vector3.zero;
            m_uiObject.displayObject.gameObject.name = GetType().Name;

            LoadPackages(FuiPackages, false);
        }

        /// <summary>
        /// 同步加载窗口组件,只检查package是否加载,本身不会加载package, 但是会先检测package是否已加载,如果未加载会抛出异常
        /// 如果需要加载package,请调用 LoadedPackages 函数,并且覆写 OnLoadPackagesComplete 函数
        /// </summary>
        /// <param name="parentUI">父UI窗口</param>
        /// <param name="widgetRoot">组件父节点</param>
        /// <param name="userDatas"></param>
        internal FUIWidget InternalLoadSync(FUIBase parentUI, GObject widgetRoot, System.Object[] userDatas)
        {
            // 先检测package是否已加载,如果未加载会抛出异常
            CheckPackageLoaded();
            ResetChildCanvas(parentUI);
            m_parent = parentUI;
            m_parent?.LstChild.Add(this);
            m_userDatas = userDatas;
            m_widgetRoot = widgetRoot;

            if (!IsDestroyed && CreateInstance())
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 异步加载窗口组件,只检查package是否加载,本身不会加载package, 但是会先检测package是否已加载,如果未加载会抛出异常
        /// 如果需要加载package,请调用 LoadedPackages 函数,并且覆写 OnLoadPackagesComplete 函数
        /// </summary>
        /// <param name="parentUI">父UI窗口</param>
        /// <param name="widgetRoot">组件父节点</param>
        /// <param name="userDatas"></param>
        internal async UniTask<FUIWidget> InternalLoadAsync(FUIBase parentUI, GObject widgetRoot, System.Object[] userDatas)
        {
            // 先检测package是否已加载,如果未加载会抛出异常
            CheckPackageLoaded();
            ResetChildCanvas(parentUI);
            m_parent = parentUI;
            m_parent?.LstChild.Add(this);
            m_userDatas = userDatas;
            m_widgetRoot = widgetRoot;

            if (!IsDestroyed)
            {
                await CreateInstanceAsync();
                if (!IsDestroyed)
                {
                    return this;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected override void OnLoadPackagesComplete(bool isSucceed)
        {
            if (!isSucceed || !CreateInstance())
            {
                // 通知UI管理器 失败
                CreateFail();
            }
        }

        private bool CreateInstance()
        {
            GObject view = FUICreateInstance();

            bool isSucceed = false;

            if (m_uiObject != null)
            {
                if (view != null)
                {
                    m_uiObject?.AddChild(view);
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
                // 通知UI管理器 失败
                CreateFail();
            }

            return isSucceed;
        }

        private async UniTask CreateInstanceAsync()
        {
            GObject view = await FUICreateInstanceAsync();

            bool isSucceed = false;

            if (m_uiObject != null)
            {
                if (view != null)
                {
                    m_uiObject?.AddChild(view);
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
                // 通知UI管理器 失败
                CreateFail();
            }
        }

        private void HandleOpenComplete()
        {
            if (Create(m_widgetRoot))
            {
                // 通知UI管理器 成功
                m_prepareCallback?.Invoke(this);
                m_prepareCallback = null;
            }
            else
            {
                // 通知UI管理器 失败
                CreateFail();
            }
        }

        /// <summary>
        /// 创建窗口内嵌的界面
        /// </summary>
        /// <param name="parentUI">父节点UI</param>
        /// <param name="widgetRoot">组件根节点</param>
        /// <returns></returns>
        private bool Create(GObject widgetRoot)
        {
            if (widgetRoot == null || widgetRoot.isDisposed)
            {
                Log.Error("Load FUIWidget Failed because widgetRoot is null.");
                return false;
            }

            if (IsDestroyed)
            {
                Log.Error("Load FUIWidget Failed because widget is destroyed.");
                return false;
            }

            if (m_uiObject == null)
            {
                Log.Error("Load FUIWidget Failed because m_uiObject is null");
                return false;
            }

            return CreateImp(m_uiObject);
        }

        private bool CreateImp(GObject uiObject)
        {
            if (!CreateBase(uiObject))
            {
                return false;
            }

            m_parent?.SetUpdateDirty();
            RegisterEvent();
            OnCreate();
            OnRefresh();
            IsPrepare = true;
            m_uiObject.visible = true;

            return true;
        }

        protected bool CreateBase(GObject uiObject)
        {
            if (uiObject == null)
            {
                return false;
            }

            Name = GetType().Name;
            GameObject go = uiObject.displayObject.gameObject;
            Transform = go.GetComponent<Transform>();
            //RectTransform = Transform as RectTransform;
            //GameObject = go;
            //Log.Assert(RectTransform != null, $"{go.name} ui base element need to be RectTransform");
            return true;
        }

        protected void ResetChildCanvas(FUIBase parentUI)
        {
            if (parentUI == null || parentUI.GameObject == null)
            {
                return;
            }

            Canvas parentCanvas = parentUI.GameObject.GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                return;
            }

            if (GameObject != null)
            {
                var lstCanvas = GameObject.GetComponentsInChildren<Canvas>(true);
                for (int i = 0; i < lstCanvas.Length; i++)
                {
                    var childCanvas = lstCanvas[i];
                    childCanvas.sortingOrder = parentCanvas.sortingOrder + childCanvas.sortingOrder % FUIModule.WINDOW_DEEP;
                }
            }
        }

        private void CreateFail()
        {
            m_prepareCallback?.Invoke(null);
            m_prepareCallback = null;
            if (!IsDestroyed)
            {
                Destroy();
            }
        }

        #endregion

        #region Destroy

        /// <summary>
        /// 组件被销毁调用
        /// 请勿手动调用
        /// </summary>
        internal void OnDestroyWidget()
        {
            m_parent?.SetUpdateDirty();

            RemoveAllUIEvent();
            DestroyFuiComponent();

            foreach (var uiChild in LstChild)
            {
                uiChild?.OnDestroy();
                uiChild?.OnDestroyWidget();
            }

            // 注销回调函数
            m_prepareCallback = null;

            m_uiObject?.Dispose();
            m_uiObject = null;
            m_widgetRoot = null;
            m_userDatas = null;
            IsDestroyed = true;
        }

        /// <summary>
        /// 主动销毁组件
        /// </summary>
        public void Destroy()
        {
            if (m_parent != null)
            {
                // 用?.防止多线程错误
                m_parent?.LstChild.Remove(this);
                OnDestroy();
                OnDestroyWidget();
            }
        }

        #endregion
    }
}