using FairyGUI;
using GameFgui;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class FUIComponent<FuiType> : IFUIComponent where FuiType : IFUIConstructObject, new()
    {
        private Dictionary<GLoader, string> _dicImgUrlCache = new();

        protected FuiType m_view;

        public GComponent ViewInput { get; private set; }

        public FUIComponent(GComponent view)
        {
            ViewInput = view;

            m_view = new FuiType(); // view?.BindNewWidget<T1>();
            m_view.ConstructObject(view);

            OnCreate();
            RegisterEvent();
            OnRefresh();
        }

        public void Destroy()
        {
            CleanAllFuiComponent();
            RemoveAllUIEvent();
            UnRegisterEvent();
            OnDestroy();

            //m_view?.Dispose();
            // m_view = null;
        }

        #region 子类处理

        private List<IFUIComponent> m_lstChildren = new List<IFUIComponent>();

        protected void RegisterFuiComponent(IFUIComponent component)
        {
            m_lstChildren.Add(component);
        }

        private void CleanAllFuiComponent()
        {
            for (int i = 0; i < m_lstChildren.Count; i++)
            {
                IFUIComponent fun = m_lstChildren[i];
                if (fun != null)
                {
                    fun.Destroy();
                }
            }

            m_lstChildren = null;
        }

        #endregion

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

        #region 属性定义

        public bool Visible
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.visible = value;
                }
            }
            get => (ViewInput != null) && ViewInput.visible;
        }


        public bool Grayed
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.grayed = value;
                }
            }
            get => (ViewInput != null) && ViewInput.grayed;
        }

        public float Alpha
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.alpha = value;
                }
            }
            get => (ViewInput != null) ? ViewInput.alpha : 0;
        }

        public float X
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.x = value;
                }
            }
            get => (ViewInput != null) ? ViewInput.x : 0;
        }

        public float Y
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.y = value;
                }
            }
            get => (ViewInput != null) ? ViewInput.y : 0;
        }

        public float Rotation
        {
            set
            {
                if (ViewInput != null)
                {
                    ViewInput.rotation = value;
                }
            }
            get => (ViewInput != null) ? ViewInput.rotation : 0;
        }

        /// <summary>
        /// 设置是否可用
        /// </summary>
        public bool Enabled
        {
            get { return ViewInput.touchable; }
            set { ViewInput.touchable = value; }
        }

        /// <summary>
        /// 判断ui是否已经销毁 (不要每帧调用)
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (ViewInput != null && ViewInput.displayObject != null && ViewInput.displayObject.gameObject != null)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region 常用方法

        public void SetScale(float scale)
        {
            if (ViewInput != null)
            {
                ViewInput.SetScale(scale, scale);
            }
        }

        public void SetScale(float xScale, float yScale)
        {
            if (ViewInput != null)
            {
                ViewInput.SetScale(xScale, yScale);
            }
        }

        public void SetPosition(float px, float py)
        {
            if (ViewInput != null)
            {
                ViewInput.x = px;
                ViewInput.y = py;
            }
        }

        public Vector3 LocalToGlobal()
        {
            if (ViewInput != null && ViewInput.parent != null)
            {
                Vector2 pos = ViewInput.parent.LocalToGlobal(new Vector2(ViewInput.x, ViewInput.y));
                return new Vector3(pos.x, -pos.y, 0);
            }

            return Vector3.zero;
        }

        #endregion

        #region protected 接口

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCreate()
        {
        }

        protected virtual void RegisterEvent()
        {
        }

        protected virtual void UnRegisterEvent()
        {
        }

        protected virtual void OnRefresh()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        #endregion

        /// <summary>
        /// 加载一张图片
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="imgName"></param>
        /// <param name="isFromResources"></param>
        protected void SetImage(GLoader loader, string imgName, bool isFromResources = false)
        {
            if (loader != null)
            {
                if (string.IsNullOrEmpty(imgName))
                {
                    if (_dicImgUrlCache.ContainsKey(loader))
                    {
                        FUIExtension.ReleaseImage(_dicImgUrlCache[loader]);
                    }
                    _dicImgUrlCache.Remove(loader);
                }
                else
                {
                    if (_dicImgUrlCache.ContainsKey(loader))
                    {
                        string oldUrl = _dicImgUrlCache[loader];
                        if (oldUrl != imgName)
                        {
                            FUIExtension.ReleaseImage(oldUrl);
                        }
                        _dicImgUrlCache[loader] = imgName;
                    }
                    else
                    {
                        _dicImgUrlCache[loader] = imgName;
                    }
                }
                loader.SetFuiTexture(imgName);
            }
        }

        /// <summary>
        /// 设置组件中指定名称对象的图片
        /// </summary>
        /// <param name="gCom"></param>
        /// <param name="objName"></param>
        /// <param name="imgName"></param>
        protected void SetImage(GComponent gCom, string objName, string imgName)
        {
            if (gCom != null)
            {
                GObject gObj = gCom.GetChild(objName);
                if (gObj != null)
                {
                    SetImage(gObj.asLoader, imgName);
                }
            }
        }

        protected void ReleaseFuiImage()
        {
            foreach (KeyValuePair<GLoader, string> keyValuePair in _dicImgUrlCache)
            {
                string res = keyValuePair.Value;
                FUIExtension.ReleaseImage(res);
            }
        }
    }
}
