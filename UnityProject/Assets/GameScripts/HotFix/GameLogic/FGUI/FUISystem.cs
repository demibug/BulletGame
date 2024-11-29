using System;
using System.Collections.Generic;
using GameData;
using GameRes;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    //[Update]

    /// <summary>
    /// FUI系统
    /// UI操作尽量走这里，可以热更，如果修改 FUIModule 则改动到了框架，无法热更。
    /// </summary>
    public partial class FUISystem : BehaviourSingleton<FUISystem>
    {
        public static bool EnabledLogPanel = true; //是否开启日志面板


        public override void Awake()
        {
            InitUICamera();
            //CreateUILight();
        }

        private void InitUICamera()
        {
            Transform camTrans = GameModule.FUI.UICamera.transform;
            Vector3 camPos = camTrans.position;
            camPos.z = -10f;
            camTrans.position = camPos;
        }

        private void CreateUILight()
        {
            GameObject go = ResSystem.Instance.LoadAsset<GameObject>("light_Prefab");
            go.name = "UILight";
            GameObject.DontDestroyOnLoad(go);
        }


        /// <summary>
        /// 检测并进行包绑定
        /// </summary>
        /// <param name="binderId"></param>
        /// <param name="bindFunc"></param>
        public void CheckBindAll(int binderId, Action bindFunc)
        {
            GameModule.FUI.CheckBindAll(binderId, bindFunc);
        }


        /// <summary>
        /// 包是否已加载
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool IsLoadedPackage(string packageName)
        {
            return GameModule.FUI.IsLoadedPackage(packageName);
        }

        /// <summary>
        /// 同步打开窗口
        /// </summary>
        public bool ShowUI(Type type, bool isCheckMutex, params object[] userDatas)
        {
            if (!GameModule.FUI.IsWindowLoading(type))
            {
                GameEvent.Send(GEvent.UIShowEvent, type.Name);
                GameModule.FUI.ShowUI(type, userDatas);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 同步打开窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userDatas"></param>
        public bool ShowUI<T>(params object[] userDatas) where T : FUIWindow
        {
            Type type = typeof(T);
            if (!GameModule.FUI.IsWindowLoading(typeof(T)))
            {
                GameEvent.Send(GEvent.UIShowEvent, type.Name);
                GameModule.FUI.ShowUI<T>(userDatas);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 异步打开窗口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userDatas"></param>
        public bool ShowUIAsync(Type type, bool isCheckMutex, params object[] userDatas)
        {
            if (!GameModule.FUI.IsWindowLoading(type))
            {
                GameEvent.Send(GEvent.UIShowEvent, type.Name);
                GameModule.FUI.ShowUIAsync(type, userDatas);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 异步打开窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userDatas"></param>
        public bool ShowUIAsync<T>(params object[] userDatas) where T : FUIWindow
        {
            Type type = typeof(T);
            if (!GameModule.FUI.IsWindowLoading(typeof(T)))
            {
                GameEvent.Send(GEvent.UIShowEvent, type.Name);
                GameModule.FUI.ShowUIAsync<T>(userDatas);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI(Type type)
        {
            GameEvent.Send(GEvent.UICloseEvent, type.Name);
            GameModule.FUI.CloseUI(type);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI<T>() where T : FUIWindow
        {
            GameEvent.Send(GEvent.UICloseEvent, typeof(T).Name);
            GameModule.FUI.CloseUI<T>();
        }

        /// <summary>
        /// UI是否存在
        /// </summary>
        /// <param name="type"></param>
        public bool HasUI(Type type)
        {
            return GameModule.FUI.HasWindow(type);
        }

        /// <summary>
        /// UI是否存在
        /// </summary>
        public bool HasUI<T>()
        {
            return GameModule.FUI.HasWindow(typeof(T));
        }

        public string GetTopWindow()
        {
            return GameModule.FUI.GetTopWindow();
        }


        /// <summary>
        /// 隐藏包括通用奖励面板在内的所有其它UI
        /// </summary>
        private Dictionary<string, bool> m_dicHiddenUIName = new();
        private void HiddenAllUI()
        {
            m_dicHiddenUIName.Clear();

            var fuiStack = GameModule.FUI.Stack;

            for (int i = 0; i < fuiStack.Count; i++)
            {
                FUIWindow window = fuiStack[i];
                if (window.Component.displayObject.gameObject.activeSelf)
                {
                    window.Component.displayObject.gameObject.SetActive(false);
                    m_dicHiddenUIName.Add(window.WindowName, true);
                }
            }
        }

        /// <summary>
        /// 恢复之前隐藏的UI的显示
        /// </summary>
        private void ReshowAllUI()
        {
            var fuiStack = GameModule.FUI.Stack;

            for (int i = 0; i < fuiStack.Count; i++)
            {
                FUIWindow window = fuiStack[i];
                string uiName = window.WindowName;
                if (m_dicHiddenUIName.ContainsKey(uiName))
                {
                    window.Component.displayObject.gameObject.SetActive(true);
                }
            }

            m_dicHiddenUIName.Clear();
        }


        private string GetUIName<T>()
        {
            Type type = typeof(T);
            return type.FullName;
        }

        ///----------------------------------------------------------------
        ///
        ///
        ///
        ///
        ///
        ///
        ///----------------------------------------------------------------
        /// <summary>
        /// 互斥界面列表
        /// </summary>
        //private readonly List<Type> m_lstMutexPanel = new();

        ///// <summary>
        ///// 全局界面（不受互斥界面的影响，一直影响）
        ///// </summary>
        //private readonly List<Type> m_lstGlobalPanel = new();

        ///// <summary>
        ///// 等待显示的互斥界面列表
        ///// </summary>
        //private readonly LinkedList<KeyValuePair<Type, object[]>> m_lstWaitMutexPanel = new();

        ///// <summary>
        ///// 延迟进行显示的界面列表（主要是一些需要在登陆时弹出的活动界面）
        ///// </summary>
        //private readonly LinkedList<KeyValuePair<Type, object[]>> m_lstDelayShowPanel = new();

        private bool m_isInitComplete = false;

        public void InitUISystem()
        {
            if (m_isInitComplete) return;

            m_isInitComplete = true;

            RegUIGroup();
            RegUIEvent();
        }

        /// <summary>
        /// 初始化UI分组信息
        /// </summary>
        private void RegUIGroup()
        {
            //m_lstMutexPanel.Clear();
            //m_lstGlobalPanel.Clear();
            //m_lstWaitMutexPanel.Clear();

            ////注册互斥界面列表
            ////m_lstMutexPanel.Add(typeof(CGetRewardPanel));
            //m_lstMutexPanel.Add(typeof(CommonGetRewardPanel));
            //m_lstMutexPanel.Add(typeof(PlayerLevelUpPanel));
            //m_lstMutexPanel.Add(typeof(AlbumGiftsResultPanel));
            //m_lstMutexPanel.Add(typeof(AlbumExchangeResultPanel));
            //m_lstMutexPanel.Add(typeof(ExhibitionUnlockMoviePanel));
            //m_lstMutexPanel.Add(typeof(ChessboardFinishPanel));
            //m_lstMutexPanel.Add(typeof(AlbumUnlockPanel));

            ////注册全局界面列表
            //m_lstGlobalPanel.Add(typeof(CTipPanel));
            //m_lstGlobalPanel.Add(typeof(CClickTipPanel));
            //m_lstGlobalPanel.Add(typeof(CItemTipPanel));
            //m_lstGlobalPanel.Add(typeof(CActivityBigRewardPanel));
        }

        /// <summary>
        /// 初始化UI事件注册
        /// </summary>
        private void RegUIEvent()
        {
            //注册全局UI事件
            //GameEvent.AddEventListener(GEvent.LoginAndShowHUD, OnShowHUDWnd);
        }
    }
}
