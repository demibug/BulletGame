using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameBase;
using GameConfig;
using GameData;
using GameData.GDefine;
using GameNetwork;
using GameRes;
using Google.Protobuf.Collections;
using PlayCore;
using Protos;
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
            CreateUILight();
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
            // Light light = go.transform.GetComponentInChildren<Light>();
            // // 获取UI层的掩码
            // int uiLayer = LayerMask.NameToLayer("UI");
            // int uiLayerMask = 1 << uiLayer;
            // // 设置层
            // light.cullingMask = uiLayerMask;
            // light.shadows = LightShadows.None;
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
            if (!ChecHasMutexPanel(type) || !isCheckMutex || IsGlobalPanel(type))
            {
                if (!GameModule.FUI.IsWindowLoading(type))
                {
                    GameModule.FUI.ShowUI(type, userDatas);
                    return true;
                }
                else if (IsMutexPanel(type) && isCheckMutex)
                {
                    AddMutexPanel(type, userDatas);
                }
            }
            else if (isCheckMutex)
            {
                AddMutexPanel(type, userDatas);
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
            if (!ChecHasMutexPanel(type) || IsGlobalPanel(type))
            {
                if (!GameModule.FUI.IsWindowLoading(typeof(T)))
                {
                    GameEvent.Send(GEvent.UIShowEvent, type.Name);
                    GameModule.FUI.ShowUI<T>(userDatas);

                    return true;
                }
                else if (IsMutexPanel(type))
                {
                    AddMutexPanel(type, userDatas);
                }
            }
            else
            {
                AddMutexPanel(type, userDatas);
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
            if (!ChecHasMutexPanel(type) || !isCheckMutex || IsGlobalPanel(type))
            {
                if (!GameModule.FUI.IsWindowLoading(type))
                {
                    GameEvent.Send(GEvent.UIShowEvent, type.Name);
                    GameModule.FUI.ShowUIAsync(type, userDatas);
                    return true;
                }
                else if (IsMutexPanel(type) && isCheckMutex)
                {
                    AddMutexPanel(type, userDatas);
                }
            }
            else if (isCheckMutex)
            {
                AddMutexPanel(type, userDatas);
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
            if (!ChecHasMutexPanel(type) || IsGlobalPanel(type))
            {
                if (!GameModule.FUI.IsWindowLoading(typeof(T)))
                {
                    GameEvent.Send(GEvent.UIShowEvent, type.Name);
                    GameModule.FUI.ShowUIAsync<T>(userDatas);
                    return true;
                }
                else if (IsMutexPanel(type))
                {
                    AddMutexPanel(type, userDatas);
                }
            }
            else
            {
                AddMutexPanel(type, userDatas);
            }

            return false;
        }

        /// <summary>
        /// 延迟显示一些界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userDatas"></param>
        public bool DelayShowUI<T>(params object[] userDatas) where T : FUIWindow
        {
            Type type = typeof(T);

            //Log.Warning("DelayShowUI: " + type.FullName);
            if (ChkMainWindIdle())
            {
                //Log.Warning("DelayShowUI: checkShow " + type.FullName);
                if (!ShowUI(type, true, userDatas))
                {
                    AddDelayShowPanel(type, userDatas);
                    return false;
                }
            }
            else
            {
                AddDelayShowPanel(type, userDatas);
                return false;
            }

            return true;
        }

        public void DelayShowUI<T>(float delay, params object[] userDatas) where T : FUIWindow
        {
            if(delay > 0)
            {
                WaitTimeForShowUI(typeof(T), delay, userDatas).Forget();
            }
            else
            {
                DelayShowUI<T>(userDatas);
            }
        }

        /// <summary>
        /// 等待一段时间后显示UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="delay"></param>
        /// <param name="userDatas"></param>
        /// <returns></returns>
        private async UniTaskVoid WaitTimeForShowUI(Type uiType, float delay, params object[] userDatas)
        {
            await UniTask.WaitForSeconds(delay);

            if (ChkMainWindIdle())
            {
                if (!ShowUI(uiType, true, userDatas))
                {
                    AddDelayShowPanel(uiType, userDatas);
                }
            }
            else
            {
                AddDelayShowPanel(uiType, userDatas);
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI(Type type)
        {
            GameEvent.Send(GEvent.UICloseEvent, type.Name);
            GameModule.FUI.CloseWindow(type);

            CheckNextUIShow();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseUI<T>() where T : FUIWindow
        {
            GameEvent.Send(GEvent.UICloseEvent, typeof(T).Name);
            GameModule.FUI.CloseWindow<T>();

            CheckNextUIShow();
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
        /// 检测点击
        /// </summary>
        public void CheckAndHiddenTip()
        {
            if (HasUI<CClickTipPanel>())
            {
                CloseUI<CClickTipPanel>();
            }

            if (HasUI<CActivityBigRewardPanel>())
            {
                CloseUI<CActivityBigRewardPanel>();
            }

            if (HasUI<CItemTipPanel>())
            {
                CloseUI<CItemTipPanel>();
            }

            if (HasUI<CTextTipPanel>())
            {
                CloseUI<CTextTipPanel>();
            }
        }

        public void CheckAndShowDelayWindow()
        {
            CheckNextUIShow();
        }

        /// <summary>
        /// 显示金币不足面板
        /// </summary>
        public void ShowMoneyNotEnoughTip()
        {
            string strTitle = LMgr.TC(103900); //提示
            string strInfo = LMgr.TC(103901); //金币不足
            TipsInfoEntity entity = new TipsInfoEntity();
            entity.TitleStr = strTitle;
            entity.Desc = strInfo;
            ShowTextTipPanelEvent(entity);
        }

        /// <summary>
        /// 检测当前状态是否是空闲状态
        /// </summary>
        /// <param name="WindName"></param>
        /// <returns></returns>
        private List<string> m_lstCheckWindow = new();
        public bool ChkMainWindIdle(string WindName = "")
        {
            if(m_lstCheckWindow.Count == 0)
            {
                m_lstCheckWindow = new()
                {
                    GetUIName<HudMainWnd>(),
                    GetUIName<HudMainTopWnd>(),
                    GetUIName<CMovieWnd>(),
                    GetUIName<GuideMainWnd>(),
                    GetUIName<CTouchEffectWind>(),
                    GetUIName<CBottomEffectWnd>(),
                };
            }


            int Add = 0;
            int def = (WindName.Length > 0) ? 1 : 0;
            int Count = m_lstCheckWindow.Count;

            var m_stack = GameModule.FUI.List_Stack;
            int stackCount = m_stack.Count;
            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
                for(int j = 0; j < m_lstCheckWindow.Count; j++)
                {
                    if (m_lstCheckWindow[j].Equals(window.WindowName))
                    {
                        Add++;
                    }
                }

                if (window.WindowName == GetUIName<CDebugPanel>())
                {
                    stackCount--;
                }
            }

            if (Add == Count && Add == (stackCount - def))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检测是否有UI在显示中
        /// </summary>
        /// <returns></returns>
        public bool IsNoDelayWindow()
        {
            if (m_lstWaitMutexPanel.Count > 0 ||
                m_lstDelayShowPanel.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 隐藏包括通用奖励面板在内的所有其它UI
        /// </summary>
        private Dictionary<string, bool> m_dicHiddenUIName = new();
        private void HiddenAllUI()
        {
            m_dicHiddenUIName.Clear();

            var m_stack = GameModule.FUI.List_Stack;

            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
                if (window.WindowName == GetUIName<HudMainWnd>()
                    || window.WindowName == GetUIName<HudMainTopWnd>()
                    || window.WindowName == GetUIName<CMovieWnd>()
                    || window.WindowName == GetUIName<GuideMainWnd>()
                    || window.WindowName == GetUIName<CTouchEffectWind>()
                    || window.WindowName == GetUIName<CBottomEffectWnd>()
                    || window.WindowName == GetUIName<CommonGetRewardPanel>())
                {
                    continue;
                }
                else if (EnabledLogPanel && window.WindowName == GetUIName<CDebugPanel>())
                {
                    continue;
                }
                else
                {
                    if (window.Component.displayObject.gameObject.activeSelf)
                    {
                        window.Component.displayObject.gameObject.SetActive(false);
                        m_dicHiddenUIName.Add(window.WindowName, true);
                    }
                }
            }
        }

        /// <summary>
        /// 恢复之前隐藏的UI的显示
        /// </summary>
        private void ReshowAllUI()
        {
            var m_stack = GameModule.FUI.List_Stack;

            for (int i = 0; i < m_stack.Count; i++)
            {
                FUIWindow window = m_stack[i];
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
        private readonly List<Type> m_lstMutexPanel = new();

        /// <summary>
        /// 全局界面（不受互斥界面的影响，一直影响）
        /// </summary>
        private readonly List<Type> m_lstGlobalPanel = new();

        /// <summary>
        /// 等待显示的互斥界面列表
        /// </summary>
        private readonly LinkedList<KeyValuePair<Type, object[]>> m_lstWaitMutexPanel = new();

        /// <summary>
        /// 延迟进行显示的界面列表（主要是一些需要在登陆时弹出的活动界面）
        /// </summary>
        private readonly LinkedList<KeyValuePair<Type, object[]>> m_lstDelayShowPanel = new();

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
            m_lstMutexPanel.Clear();
            m_lstGlobalPanel.Clear();
            m_lstWaitMutexPanel.Clear();

            //注册互斥界面列表
            //m_lstMutexPanel.Add(typeof(CGetRewardPanel));
            m_lstMutexPanel.Add(typeof(CommonGetRewardPanel));
            m_lstMutexPanel.Add(typeof(PlayerLevelUpPanel));
            m_lstMutexPanel.Add(typeof(AlbumGiftsResultPanel));
            m_lstMutexPanel.Add(typeof(AlbumExchangeResultPanel));
            m_lstMutexPanel.Add(typeof(ExhibitionUnlockMoviePanel));
            m_lstMutexPanel.Add(typeof(ChessboardFinishPanel));
            m_lstMutexPanel.Add(typeof(AlbumUnlockPanel));

            //注册全局界面列表
            m_lstGlobalPanel.Add(typeof(CTipPanel));
            m_lstGlobalPanel.Add(typeof(CClickTipPanel));
            m_lstGlobalPanel.Add(typeof(CItemTipPanel));
            m_lstGlobalPanel.Add(typeof(CActivityBigRewardPanel));
        }

        /// <summary>
        /// 初始化UI事件注册
        /// </summary>
        private void RegUIEvent()
        {
            //注册全局UI事件
            GameEvent.AddEventListener(GEvent.LoginAndShowHUD, OnShowHUDWnd);
            GameEvent.AddEventListener<CloudOpenType, string, int>(GEvent.CloudLoadingShow, ShowCloudPanel);
            GameEvent.AddEventListener<int>(GEvent.CloudLoadingHide, HideCloudPanel);
            GameEvent.AddEventListener<long, int, long, string>(GEvent.ShowItemTipPanel, ShowItemTipPanel);
            GameEvent.AddEventListener<CItemTipOption>(GEvent.ShowRewardTipPanel, ShowRewardTipPanel);

            //通用奖励面板专用
            GameEvent.AddEventListener(GEvent.RewardMovieHidenUI, HiddenAllUI);
            GameEvent.AddEventListener(GEvent.RewardMovieReshowUI, ReshowAllUI);

            GameEvent.AddEventListener(GEvent.ShowShoppingPanel, OnShowShoppingPanel);
            GameEvent.AddEventListener<StoreGiftEntity>(GEvent.ShowBuyLackPanel, OnShowBuyLackPanel);
            GameEvent.AddEventListener<int>(GEvent.ShowFunOpenTipPanel, ShowFunOpenTipPanel);
            GameEvent.AddEventListener<List<long[]>, bool, string>(GEvent.ShowGetRewardPaneel, ShowGetRewardPanel);
            GameEvent.AddEventListener<CGetRewardOption, float>(GEvent.ShowGetRewardPanelByOption, ShowGetRewardPanel);
            GameEvent.AddEventListener<int, RepeatedField<PItem>>(GEvent.PlayerUpgradeLevel, ShowPlayerUpgradePanel);
            GameEvent.AddEventListener<int>(GEvent.PlayerUpgradeLevel_Client, ShowPlayerUpgradePanel_Client);
            GameEvent.AddEventListener<int, int, bool>(GEvent.ChessEvent_ShowWind_LargeTaxes, ShowWind_LargeTaxes);
            GameEvent.AddEventListener(GEvent.ChessEvent_ShowWind_Aladdin, ShowWind_Aladdin);
            GameEvent.AddEventListener(GEvent.ChessEvent_ShowWind_AladdinReady, ShowWind_AladdinReady);
            GameEvent.AddEventListener(GEvent.ChessEvent_ShowWind_AladdinEnter, ShowWind_AladdinEnter);
            GameEvent.AddEventListener(GEvent.AladinLoadingShow, ShowWind_AladdinEnterClose);
            GameEvent.AddEventListener(GEvent.ChessEvent_ShowWind_AladdinScene, ShowWind_AladdinScene);
            GameEvent.AddEventListener(GEvent.ChessEvent_HideWind_AladdinScene, HideWind_AladdinScene);
            GameEvent.AddEventListener<Action>(GEvent.ChessEvent_ShowWind_AladdinResult, ShowWind_AladdinResult);
            GameEvent.AddEventListener<Action>(GEvent.ChessEvent_ShowWind_AladdinFriendReward, ShowWind_AladdinFriendReward);
            // GameEvent.AddEventListener<List<Transform>>(GEvent.ChessEvent_ShowWind_DifferentDimensionalSpacePanel, ShowWind_DifferentDimensionalSpacePanel);

            //异世界-显示转盘
            GameEvent.AddEventListener(GEvent.ShowWorldTurntablePanel, OnShowWorldTurntablePanel);
            //异世界-显示机会卡
            GameEvent.AddEventListener<int, RespGo.Types.CardEvent, MapField<int, long>>(GEvent.ShowWorldChanceEventPanel, ShowWorldChanceEventPanel);

            //显示随机宝箱
            GameEvent.AddEventListener<OtherChessRandomBox, RepeatedField<PItem>>(GEvent.ShowRandomBoxPanel, OnShowRandomBoxPanel);

            //显示神灵表现界面
            GameEvent.AddEventListener<OtherChessFloatGod, Action, bool, RepeatedField<PItem>>(GEvent.ShowFloatGodEventPanel, OnShowFloatGodEventPanel);
            GameEvent.AddEventListener<OtherChessFloatGod, Action, bool>(GEvent.ShowFloatGodEventPanel, OnShowFloatGodEventPanel);




            //显示万能胶片
            GameEvent.AddEventListener<bool>(GEvent.ShowAllPowerfulFilmEnterPanel, OnShowAllPowerfulFilmEnterPanel);
            GameEvent.AddEventListener(GEvent.ShowAllPowerfulFilmPanel, OnShowAllPowerfulFilmPanel);
            GameEvent.AddEventListener<int>(GEvent.ShowAllPowerfulFilmConfirmPanel, OnShowAllPowerfulFilmConfirmPanel);
            GameEvent.AddEventListener<int>(GEvent.ShowTComGetAloneCardPanel, OnShowTComGetAloneCardPanel);

            //显示彩票屋
            GameEvent.AddEventListener(GEvent.ShowLotteryHouseEventPanel, OnShowLotteryHouseEventPanel);
            GameEvent.AddEventListener(GEvent.ShowLotteryHouseResultPanel, OnShowLotteryHouseResultPanel);



            //显示神灵奖励界面
            // GameEvent.AddEventListener<RepeatedField<PItem>, Action>(GEvent.ShowFloatGodRewardPanel, OnShowFloatGodRewardPanel);

            //显示表情界面
            GameEvent.AddEventListener<int>(GEvent.ShowEmojiPanel, OnShowEmojiPanel);

            //显示限时活动界面
            GameEvent.AddEventListener<ActivityEntity>(GEvent.ShowLimitTimePanel, OnShowLimitTimePanel);

            //显示活动礼包界面
            GameEvent.AddEventListener<ActivityEntity>(GEvent.ShowStoreGiftActivityPanel, OnShowStoreGiftActivityPanel);

            //显示锦标赛活动界面
            GameEvent.AddEventListener<ActivityEntity>(GEvent.ShowRankActivityPanel, OnShowRankActivityPanel);

            //显示周活动界面
            GameEvent.AddEventListener<ActivityEntity>(GEvent.ShowWeekActivityPanel, OnShowWeekActivityPanel);

            GameEvent.AddEventListener(GEvent.ShowDesertTreasurePanel, OnShowDesertTreasurePanel);

            //主题传送门界面
            GameEvent.AddEventListener(GEvent.ShowThemePanel, OnShowThemePanel);
            //主题详细信息界面
            GameEvent.AddEventListener(GEvent.ShowThemeInfoPanel, OnShowThemeInfoPanel);


            //传送门loading界面
            // GameEvent.AddEventListener(GEvent.ShowPortalLoading, OnShowPortalLoading);
            GameEvent.AddEventListener(GEvent.ShowPortalLoading2, OnShowPortalLoading2);

            //异次元转场loading界面
            GameEvent.AddEventListener<bool>(GEvent.ShowOtherWorldLoading, OnShowOtherWorldLoading);

            //显示异世界结算界面
            GameEvent.AddEventListener(GEvent.ShowOtherWorldSettleAccountsPanel, OnShowOtherWorldSettleAccountsPanel);

            //主界面相关显示及隐藏
            GameEvent.AddEventListener(GEvent.ShowMainHUD, OnShowMainHUDEvent);
            GameEvent.AddEventListener(GEvent.HideMainHUD, OnHideMainHUDEvent);
            GameEvent.AddEventListener(GEvent.ShowWorldMainHUD, OnShowWorldMainHUDEvent);
            GameEvent.AddEventListener(GEvent.HideWorldMainHUD, OnHideWorldMainHUDEvent);

            GameEvent.AddEventListener<Vector2, List<long[]>, string, string>(GEvent.ShowActivityBigRewardPanel, OnShowActivityBigRewardPanel);

            //陈列室解锁
            GameEvent.AddEventListener<int, int, RepeatedField<PItem>>(GEvent.PlayShowRoomUnlockReward, OnShowRoomUnlockReward);

            GameEvent.AddEventListener(GEvent.ChessMiniGameShowUI, OnChessMiniGameShowUI);
            GameEvent.AddEventListener(GEvent.ChessMiniGameShowReward, OnChessMiniGameShowReward);
            GameEvent.AddEventListener<TipsInfoEntity>(GEvent.ShowTextTipPanel, ShowTextTipPanelEvent);
            GameEvent.AddEventListener<bool>(GEvent.ShowChessBoardFinishPanel, OnShowChessBoardFinishPanel);
            GameEvent.AddEventListener<int>(GEvent.SendRoomAutoUnlock, OnSendShowRoomAutoUnlock);

            GameEvent.AddEventListener<int, MapField<int, long>>(GEvent.ChessEvent_ShowWind_ShieldGet, OnShowGetShielEventdPanel);

            //注册地图事件显示处理
            GameEvent.AddEventListener<int, MapField<int, long>>(GEvent.ShowCrazyRentEventPanel, OnShowCrazyRentEventPanel);
            GameEvent.AddEventListener<List<int>>(GEvent.ShowColorTurntableEventPanel, OnShowColorTurntableEventPanel);
            GameEvent.AddEventListener<int, RespGo.Types.CardEvent, MapField<int, long>>(GEvent.ShowChanceEventPanel, ShowChanceEventPanel);
            GameEvent.AddEventListener<int>(GEvent.ShowMiniGameBalloonFightPanel, ShowMiniGameBalloonFightPanel);
            GameEvent.AddEventListener(GEvent.ShowPrisonEventPanel, OnShowPrisonEventPanel);
            GameEvent.AddEventListener(GEvent.ShowBreakDownEventPanel, OnShowBreakDownEventPanel);
            GameEvent.AddEventListener(GEvent.ShowBankRobberyEventPanel, OnShowBankRobberyEventPanel);

            //主场景上的特殊事件（对一些特殊值进行处理）
            GameEvent.AddEventListener(GEvent.CameraBuildingWatchFinish, OnCameraWatchFinish);

            //事件完成之后触发
            GameEvent.AddEventListener(GEvent.ChessEventComplete, OnChessEventComplete);

            GameEvent.AddEventListener<Action>(GEvent.ShowBlackLoding, OnShowBlackLoding);
            GameEvent.AddEventListener(GEvent.ShowBlackLoding, OnShowBlackLoding);
            GameEvent.AddEventListener(GEvent.DarkLoadingShow, OnShowBlackLoding);

            //贴纸相关的表现
            GameEvent.AddEventListener<RoleBase, int, int, bool>(GEvent.AlbumShowGetRewardPanel, OnAlbumShowGetReward);
            GameEvent.AddEventListener<RoleBase, int, int>(GEvent.AlbumShowExchangeRewardPanel, OnAlbumShowExchangeReward);
            GameEvent.AddEventListener<RoleBase, int, int>(GEvent.AlbumShowExchangeSelfRewardPanel, OnAlbumShowSelfExchangeReward);

            //点击小游戏模型
            GameEvent.AddEventListener(GEvent.RaycastHitMiniGame, OnRaycastHitMiniGame);
            //点击传送门本体
            GameEvent.AddEventListener(GEvent.RaycastHitPortal, OnRaycastHitPortal);


            //异世界时空警察相关UI事件
            GameEvent.AddEventListener(GEvent.TimePoliceShowEnterUI, OnTimePoliceShowEnterUI);
            GameEvent.AddEventListener(GEvent.TimePoliceShowResultUI, OnTimePoliceShowResultUI);

            //异世界时空之主相关UI事件
            GameEvent.AddEventListener(GEvent.ShowTimeMasterPanel, OnShowTimeMasterPanel);

            //异世界悬浮道具 - 天气
            GameEvent.AddEventListener<OtherChessFloatWeather>(GEvent.ShowFloatWeatherPanel, OnShowFloatWeatherPanel);
            //异世界悬浮道具 - 遥控骰子
            GameEvent.AddEventListener<OtherChessTileBase>(GEvent.ShowFloatDiceControlPanel, OnShowFloatDiceControlPanel);


            //显示棋盘报告界面
            GameEvent.AddEventListener<bool>(GEvent.ShowReportPanel, OnShowReportPanel);
            //显示角色选择界面
            GameEvent.AddEventListener<bool>(GEvent.ShowAvatarSelectPanel, OnShowAvatarSelectPanel);
            //显示新贴纸开启面板
            GameEvent.AddEventListener<int>(GEvent.ShowNewAlbumPanel, OnShowNewAlbumPanel);
            //显示贴纸礼包Tips面板
            GameEvent.AddEventListener<long>(GEvent.ShowAlbumTipPanel, OnShowAlbumTipPanel);
        }


        /// <summary>
        /// 异世界 - 显示转盘事件
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowWorldTurntablePanel()
        {
            ShowUI<WorldTurntablePanel>();
        }


        /// <summary>
        /// 异世界 - 显示机会卡面板
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="event"></param>
        /// <param name="field"></param>
        private void ShowWorldChanceEventPanel(int nPieceId, RespGo.Types.CardEvent evt, MapField<int, long> lstReward)
        {
            ShowUI<WorldChanceEventPanel>(nPieceId, evt, lstReward);
        }


        /// <summary>
        /// 显示物品的Tips面板
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="lstItems"></param>
        /// <param name="itemCfgId"></param>
        private void ShowRewardTipPanel(CItemTipOption op)
        {
            App.FUI.ShowUI<CItemTipPanel>(op);
        }

        public void ShowCloudPanel(CloudOpenType openType, string faceId, int eventId = 0)
        {
            App.Data.Global.IsShowCloudMask = true;
            App.Data.Global.ShowCloudEvent = eventId;
            ShowUI<CCloudMaskPanel>(openType, faceId);
        }

        public void HideCloudPanel(int eventId = 0)
        {
            App.Data.Global.IsShowCloudMask = false;
            App.Data.Global.HideCloudEvent = eventId;
            GameEvent.Send(GEvent.HiddenCloudMaskPanel, eventId);
        }

        /// <summary>
        /// 显示物品的提示信息面板
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ShowItemTipPanel(long itemCfgId, int timeTipKey = 0, long time = 0, string descExtend = "")
        {
            ItemClass cfg = App.TData.GetItemConfig((int)itemCfgId);
            if (cfg != null)
            {
                TipsInfoEntity tips = new TipsInfoEntity();
                if (cfg.Type == EItemType.WeiJieSuoFanghuzhao ||
                    cfg.Type == EItemType.WeiJieSuoQizi)
                {
                    tips.TitleStr = LMgr.TC(cfg.Name);
                    tips.Desc = LMgr.TC(cfg.Desc);
                    tips.Img = cfg.Icon;
                    tips.TipsType = TipPanelType.Movie;
                    tips.OkStr = LMgr.TC(103902);
                    tips.ItemId = itemCfgId;
                    tips.TimeDescKey = timeTipKey;
                    tips.TimeSec = time;
                    tips.DescExtend = descExtend;
                }
                else
                {
                    tips.TitleStr = LMgr.TC(cfg.Name);
                    tips.Desc = LMgr.TC(cfg.Desc);
                    tips.Img = cfg.Icon;
                    tips.TipsType = TipPanelType.Image;
                }

                ShowUI<CTipPanel>(tips);
            }
        }

        /// <summary>
        /// 显示功能解锁的提示面板
        /// </summary>
        /// <param name="funId"></param>
        private void ShowFunOpenTipPanel(int funId)
        {
            FunctionUnlockClass cfg = App.TData.GetFunctionUnlockConfig(funId);
            if (cfg != null)
            {
                ShowUI<FunOpenInfoPanel>(funId);
            }
        }

        /// <summary>
        /// 显示通用奖励面板
        /// </summary>
        /// <param name="lstReward"></param>
        private void ShowGetRewardPanel(List<long[]> lstReward, bool isAutoClose, string title = "")
        {
            CGetRewardOption op = new CGetRewardOption();
            op.SetNormalOption(lstReward, isAutoClose, title);
            ShowUI<CommonGetRewardPanel>(op);

            //GetRewardPanelType vType = GetRewardPanelType.Normal;

            //string strTitle = (title.Length == 0) ? LMgr.TC(102600) : title; // 恭喜获得奖励
            //ShowUI<CGetRewardPanel>(vType, strTitle, lstReward, "", isAutoClose);
        }

        /// <summary>
        /// 直接根据选项显示获得奖励面板
        /// </summary>
        /// <param name="op"></param>
        private void ShowGetRewardPanel(CGetRewardOption op, float delayTime = 0)
        {
            if (delayTime > 0)
            {
                DelayShowUI<CommonGetRewardPanel>(delayTime, op);
            }
            else
            {
                ShowUI<CommonGetRewardPanel>(op);
            }
        }


        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="nLev">新的玩家等级</param>
        /// <param name="lstReward">获得的奖励列表</param>
        /// <exception cref="NotImplementedException"></exception>
        private void ShowPlayerUpgradePanel(int nLev, RepeatedField<PItem> lstReward)
        {
            DataSystem.Instance.Player.ShowPlayerUpgradePanel(nLev, lstReward);
        }

        private void ShowPlayerUpgradePanel_ClientAsync(int level)
        {
            RepeatedField<PItem> reward = DataSystem.Instance.Player.GetRewardCache(level);
            CGetRewardOption op = DataSystem.Instance.Global.GetRewardOption(reward);
            ShowUI<PlayerLevelUpPanel>(level, op, reward);
        }

        private void ShowPlayerUpgradePanel_Client(int level)
        {
            ShowPlayerUpgradePanel_ClientAsync(level);
        }

        private void OnChessEventComplete()
        {
            if (DataSystem.Instance.Player.ChkUpLevelRewardCount())
            {
                int level = 0;
                foreach (var item in DataSystem.Instance.Player.Dic_UpLevelReward)
                {
                    level = item.Key;
                    break;
                }

                if (level != 0)
                {
                    DataSystem.Instance.Player.UpdatePlayerLevel(level);
                    ShowPlayerUpgradePanel_Client(level);
                }
            }
        }

        /// <summary>
        /// 建造状态下的摄像头对象
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnCameraWatchFinish()
        {
            bool hasExPanelShow = false;
            for (int i = 0; i < m_lstMutexPanel.Count; i++)
            {
                if (HasUI(m_lstMutexPanel[i]))
                {
                    hasExPanelShow = true;
                    break;
                }
            }

            if (!hasExPanelShow && App.Data.Global.IsPlayScreenMovie)
            {
                App.Data.Global.IsPlayScreenMovie = false;
            }
        }

        /// /// <summary>
        /// 显示 罚单(巨额税款) 界面
        /// </summary>
        /// <param name="index">随机的罚单参数</param>
        /// /// <param name="cost">扣除的货币数量</param>
        private void ShowWind_LargeTaxes(int index, int cost, bool bOther)
        {
            //Log.Debug("罚单");
            //GameEvent.Send(GEvent.ChessEvent_ShowWind_IsFinish);
            ShowUI<LargeTaxesEventPanel>(index, cost, bOther);
        }


        /// /// <summary>
        /// 显示 阿拉丁
        /// </summary>
        private void ShowWind_Aladdin()
        {
            //功能未开启
            App.FUI.ShowUI<AladdinEventPanel>();
        }

        private void ShowWind_AladdinReady()
        {
            //功能未开放
            App.FUI.ShowUI<AladdinWaitPanel>();
        }

        private void ShowWind_AladdinEnter()
        {
            App.FUI.ShowUI<CurtainLodingPanel>();
        }

        private void ShowWind_AladdinEnterClose()
        {
            App.FUI.ShowUI<CurtainLodingPanel>(true);
        }


        private void ShowWind_AladdinScene()
        {
            App.FUI.ShowUI<AladdinPanel>();
        }

        private void HideWind_AladdinScene()
        {
            App.FUI.CloseUI<AladdinPanel>();
        }

        private void ShowWind_AladdinResult(Action hidefunc)
        {
            App.FUI.ShowUI<AladdinResultPanel>(hidefunc);
        }

        //打开小游戏规则界面
        private void ShowWind_AladdinFriendReward(Action hidefunc)
        {
            App.FUI.ShowUI<AladdinFriendRewardPanel>(hidefunc);
        }

        // private void ShowWind_DifferentDimensionalSpacePanel(List<Transform> list)
        // {
        //     App.FUI.ShowUI<DifferentDimensionalSpacePanel>(list);
        // }

        // private void OnShowPortalLoading()
        // {
        //     App.FUI.ShowUI<DifferentDimensionalSpaceLoadingPanel>();
        // }

        private void OnShowPortalLoading2()
        {
            App.FUI.ShowUI<DifferentDimensionalSpaceLoading2Panel>();
        }

        /// <summary>
        /// 打开异次元加载界面
        /// </summary>
        /// <param name="EnterOtherWorld">是否进入异次元</param>
        private void OnShowOtherWorldLoading(bool EnterOtherWorld = false)
        {
            if (App.FUI.HasUI<OtherWorldChangePanel>()) return;
            App.FUI.ShowUI<OtherWorldChangePanel>(EnterOtherWorld);
        }


        private void OnShowOtherWorldSettleAccountsPanel()
        {
            App.FUI.ShowUI<OtherWorldSettleAccountsPanel>();
        }

        /// <summary>
        /// 显示主界面
        /// </summary>
        private void OnShowMainHUDEvent()
        {
            App.FUI.ShowUI<HudMainWnd>();
            App.FUI.ShowUI<HudMainTopWnd>();
        }

        /// <summary>
        /// 隐藏主世界主界面
        /// </summary>
        private void OnHideMainHUDEvent()
        {
            App.FUI.CloseUI<HudMainWnd>();
            App.FUI.CloseUI<HudMainTopWnd>();
        }

        /// <summary>
        /// 显示主界面
        /// </summary>
        private void OnShowWorldMainHUDEvent()
        {
            App.FUI.ShowUI<WorldHudMainWnd>();
            App.FUI.ShowUI<WorldHudMainTopWnd>();
        }

        /// <summary>
        /// 隐藏异世界主界面
        /// </summary>
        private void OnHideWorldMainHUDEvent()
        {
            App.FUI.CloseUI<WorldHudMainWnd>();
            App.FUI.CloseUI<WorldHudMainTopWnd>();
        }

        /// <summary>
        /// 显示奖励展示界面
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="lstReward"></param>
        /// <param name="strTitle"></param>
        /// <param name="strDesc"></param>
        private void OnShowActivityBigRewardPanel(Vector2 pos, List<long[]> lstReward, string strTitle, string strDesc)
        {
            App.FUI.ShowUI<CActivityBigRewardPanel>(pos, lstReward, strTitle, strDesc);
        }

        /// <summary>
        /// 显示表情界面
        /// </summary>
        /// <param name="selectItemId">玩家当前选择的表情</param>
        private void OnShowEmojiPanel(int selectItemId)
        {
            if (App.FUI.HasUI<EmojiPanel>()) return;
            App.FUI.ShowUI<EmojiPanel>(selectItemId);
        }



        /// <summary>
        /// 显示万能胶片入口
        /// </summary>
        private void OnShowAllPowerfulFilmEnterPanel(bool isEnter = false)
        {
            if (App.FUI.HasUI<AllPowerfulFilmEnterPanel>()) return;
            App.FUI.ShowUI<AllPowerfulFilmEnterPanel>(isEnter);
        }

        /// <summary>
        /// 显示万能胶片选择
        /// </summary>
        private void OnShowAllPowerfulFilmPanel()
        {
            if (App.FUI.HasUI<AllPowerfulFilmPanel>()) return;
            App.FUI.ShowUI<AllPowerfulFilmPanel>();
        }

        /// <summary>
        /// 显示万能胶片确认兑换
        /// </summary>
        private void OnShowAllPowerfulFilmConfirmPanel(int selectCardId)
        {
            if (App.FUI.HasUI<AllPowerfulFilmConfirmPanel>()) return;
            App.FUI.ShowUI<AllPowerfulFilmConfirmPanel>(selectCardId);
        }

        /// <summary>
        /// 显示万能胶片兑换成功展示
        /// </summary>
        private void OnShowTComGetAloneCardPanel(int selectCardId)
        {
            if (App.FUI.HasUI<TComGetAloneCardPanel>()) return;
            App.FUI.ShowUI<TComGetAloneCardPanel>(selectCardId);
        }

        /// <summary>
        /// 显示彩票屋
        /// </summary>
        private void OnShowLotteryHouseEventPanel()
        {
            if (App.FUI.HasUI<LotteryHouseEventPanel>()) return;
            App.FUI.ShowUI<LotteryHouseEventPanel>();
        }

        /// <summary>
        /// 显示彩票屋
        /// </summary>
        private void OnShowLotteryHouseResultPanel()
        {
            if (App.FUI.HasUI<LotteryHouseResultPanel>()) return;
            App.FUI.ShowUI<LotteryHouseResultPanel>();
        }


        /// <summary>
        /// 显示商城界面
        /// </summary>
        private void OnShowShoppingPanel()
        {
            ShopMsg.Instance.SendGameApiShopPanel();
            if (App.FUI.HasUI<ShoppingPanel>()) return;
            App.FUI.ShowUI<ShoppingPanel>();
        }

        /// <summary>
        /// 显示直购界面
        /// </summary>
        private void OnShowBuyLackPanel(StoreGiftEntity entity)
        {
            if (App.FUI.HasUI<BuyLackPanel>()) return;
            App.FUI.ShowUI<BuyLackPanel>(entity);
        }


        /// <summary>
        /// 限时限时活动
        /// </summary>
        /// <param name="entity"></param>
        private void OnShowLimitTimePanel(ActivityEntity entity)
        {
            if (App.FUI.HasUI<LimitTimeActivityPanel>()) return;
            App.FUI.ShowUI<LimitTimeActivityPanel>(entity);
        }

        /// <summary>
        /// 礼包活动界面
        /// </summary>
        /// <param name="entity"></param>
        private void OnShowStoreGiftActivityPanel(ActivityEntity entity)
        {
            if (App.FUI.HasUI<StoreGiftActivityPanel>()) return;
            App.FUI.ShowUI<StoreGiftActivityPanel>(entity);
        }

        /// <summary>
        /// 锦标赛活动界面
        /// </summary>
        /// <param name="entity"></param>
        private void OnShowRankActivityPanel(ActivityEntity entity)
        {
            if (App.FUI.HasUI<RankActivityPanel>()) return;
            App.FUI.ShowUI<RankActivityPanel>(entity);
        }

        private void OnShowWeekActivityPanel(ActivityEntity entity)
        {
            if (App.FUI.HasUI<DesertTreasureEnterPanel>()) return;
            App.FUI.ShowUI<DesertTreasureEnterPanel>(entity);
        }

        private void OnShowDesertTreasurePanel()
        {
            if (App.FUI.HasUI<DesertTreasurePanel>()) return;
            ActivityEntity entity = DataSystem.Instance.Activity.GetEntityByType(ActivityData.eActivityType.Week);
            App.FUI.ShowUI<DesertTreasurePanel>(entity);
        }

        /// <summary>
        /// 显示主题界面
        /// </summary>
        private void OnShowThemePanel()
        {
            if (App.FUI.HasUI<ThemePanel>()) return;
            App.FUI.ShowUI<ThemePanel>();
        }

        /// <summary>
        /// 显示主题信息界面
        /// </summary>
        private void OnShowThemeInfoPanel()
        {
            if (App.FUI.HasUI<ThemeInfoPanel>()) return;
            App.FUI.ShowUI<ThemeInfoPanel>();
        }


        /// <summary>
        /// 显示陈列室棋格解锁面板
        /// </summary>
        /// <param name="roomType"></param>
        /// <param name="pList"></param>
        private void OnShowRoomUnlockReward(int roomType, int nLevel, RepeatedField<PItem> pList)
        {
            //显示当前的信息
            DelayShowRoomUnlockReward(roomType, pList).Forget();
            //延时1秒后更新数据
            //DelayToUpdateShowRoomData(roomType, nLevel).Forget();
        }

        private async UniTaskVoid DelayShowRoomUnlockReward(int roomType, RepeatedField<PItem> pList)
        {
            await UniTask.WaitForSeconds(0.5f);

            if (App.Data.Player.ChkUpLevelRewardCount())
            {
                //Log.Warning("Delay show ExhibitionUnlockMoviePanel ...");
                await UniTask.WaitForSeconds(3.5f);
            }

            App.FUI.DelayShowUI<ExhibitionUnlockMoviePanel>(0, roomType, pList);
        }

        //private async UniTaskVoid DelayToUpdateShowRoomData(int roomType, int nLevel)
        //{
        //    await UniTask.WaitForSeconds(2.5f);

        //    //更新等级
        //    App.Data.ShowRoom.UpdateLevel(roomType, nLevel);
        //    //清空新装备标识
        //    App.Data.ShowRoom.ClearCacheLastEquip();
        //}

        //打开小游戏规则界面
        private void OnRaycastHitMiniGame()
        {
            App.FUI.ShowUI<CRulePanel>(RuleId.LittleGame);
        }

        /// <summary>
        /// 打开传送门规则界面
        /// </summary>
        private void OnRaycastHitPortal()
        {
            App.FUI.ShowUI<CRulePanel>(RuleId.OtherWorld);
        }

        /// <summary>
        /// 显示时空警察初始的对话面板
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTimePoliceShowEnterUI()
        {
            ShowUI<WorldPrisonEventPanel>();
        }

        /// <summary>
        /// 显示时空警察结算时的对话面板
        /// </summary>
        private void OnTimePoliceShowResultUI()
        {
            ShowUI<WorldPrisonEventPanel>();
        }

        /// <summary>
        /// 显示时空之主主界面
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowTimeMasterPanel()
        {
            ShowUI<TimeMasterPanel>(0);
        }

        /// <summary>
        /// 显示异世界悬浮道具表现面板 - 天气
        /// </summary>
        /// <param name="weather"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowFloatWeatherPanel(OtherChessFloatWeather weather)
        {
            ShowUI<FloatWeatherPanel>(weather);
        }

        /// <summary>
        /// 显示异世界遥控骰子面板
        /// </summary>
        /// <param name="diceUnit"></param>
        private void OnShowFloatDiceControlPanel(OtherChessTileBase diceUnit)
        {
            ShowUI<FloatDiceControlPanel>(diceUnit);
        }


        /// <summary>
        /// 显示报告界面
        /// </summary>
        /// <param name="bLimit">是否只显示离线时发生的报告</param>
        private void OnShowReportPanel(bool bLimit)
        {
            App.FUI.ShowUI<ReportPanel>(bLimit);
        }

        /// <summary>
        /// 显示角色选择面板
        /// </summary>
        /// <param name="showTip"></param>
        private void OnShowAvatarSelectPanel(bool showTip)
        {
            App.FUI.ShowUI<AvatarSelectPanel>(showTip);
        }

        /// <summary>
        /// 显示新画册解锁面板
        /// </summary>
        /// <param name="nAlbumCfgId"></param>
        private void OnShowNewAlbumPanel(int nAlbumCfgId)
        {
            App.FUI.ShowUI<AlbumUnlockPanel>(nAlbumCfgId);
        }

        /// <summary>
        /// 显示贴纸礼包面板
        /// </summary>
        /// <param name="nItemCfgId"></param>
        private void OnShowAlbumTipPanel(long nItemCfgId)
        {
            App.FUI.ShowUI<CAlbumTipPanel>((int)nItemCfgId);
        }

        /// <summary>
        /// 判断当前界面是否
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool ChecHasMutexPanel(Type type)
        {
            bool hasMutexUIVisible = false;

            //如果当前最顶部的界面是互斥界面
            for (int i = 0; i < m_lstMutexPanel.Count; i++)
            {
                if (HasUI(m_lstMutexPanel[i]))
                {
                    hasMutexUIVisible = true;
                    break;
                }
            }

            //如果当前显示出来的界面属于互斥界面，则不允许显示
            if (IsMutexPanel(type) && hasMutexUIVisible)
            {
                return true;
            }

            //如果没有互斥界面需要等候显示，则直接进行显示
            if (m_lstWaitMutexPanel.Count == 0)
            {
                return false;
            }

            //普通界面一直允许显示
            return false;
        }

        /// <summary>
        /// 是否为全局界面
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsGlobalPanel(Type type)
        {
            string typeName = type.Name;
            for (int i = 0; i < m_lstGlobalPanel.Count; i++)
            {
                if (m_lstGlobalPanel[i].Name == typeName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMutexPanel(Type type)
        {
            string typeName = type.Name;
            for (int i = 0; i < m_lstMutexPanel.Count; i++)
            {
                if (m_lstMutexPanel[i].Name == typeName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 添加一个需要互斥显示的界面到列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userDatas"></param>
        private void AddMutexPanel(Type type, params object[] userDatas)
        {
            m_lstWaitMutexPanel.AddLast(new KeyValuePair<Type, object[]>(type, userDatas));
        }

        /// <summary>
        /// 检测并显示下一个互斥显示的界面
        /// </summary>
        /// <returns></returns>
        private bool ChecNextMutexPanel()
        {
            if (m_lstWaitMutexPanel.Count > 0)
            {
                KeyValuePair<Type, object[]> kv = m_lstWaitMutexPanel.First();
                m_lstWaitMutexPanel.RemoveFirst();

                return ShowUI(kv.Key, true, kv.Value);
            }

            //else
            //{
            //    if (App.Data.Global.IsPlayScreenMovie)
            //    {
            //        App.Data.Global.IsPlayScreenMovie = false;
            //    }
            //}
            return false;
        }


        /// <summary>
        /// 添加一个需要延迟显示的界面到列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userDatas"></param>
        private void AddDelayShowPanel(Type type, params object[] userDatas)
        {
            m_lstDelayShowPanel.AddLast(new KeyValuePair<Type, object[]>(type, userDatas));
        }

        /// <summary>
        /// 检测并显示下一个延迟显示的界面
        /// </summary>
        /// <returns></returns>
        private bool ChecNextDelayShowPanel()
        {
            if (m_lstDelayShowPanel.Count > 0 && ChkMainWindIdle())
            {
                KeyValuePair<Type, object[]> kv = m_lstDelayShowPanel.First();
                m_lstDelayShowPanel.RemoveFirst();

                return ShowUI(kv.Key, true, kv.Value);
            }

            //else
            //{
            //    if (App.Data.Global.IsPlayScreenMovie)
            //    {
            //        App.Data.Global.IsPlayScreenMovie = false;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 检测下一个将要显示的UI，如果有则进行显示
        /// </summary>
        private void CheckNextUIShow()
        {
            if (!ChecNextMutexPanel())
            {
                if (!ChecNextDelayShowPanel())
                {
                    if (App.Data.Global.IsPlayScreenMovie)
                    {
                        App.Data.Global.IsPlayScreenMovie = false;
                    }
                }
            }
        }

        //---------------------------------------------------------------

        private void OnShowRandomBoxPanel(OtherChessRandomBox randomBox, RepeatedField<PItem> items)
        {
            App.FUI.ShowUI<RandomBoxEventPanel>(randomBox, items);
        }

        // private void OnShowFloatGodEventPanel(OtherChessFloatGod god)
        // {
        //     App.FUI.ShowUI<FloatGodEventPanel>(god);
        // }
        private void OnShowFloatGodEventPanel(OtherChessFloatGod god, Action HideFunc, bool bOnlyShow = false)
        {
            App.FUI.ShowUI<FloatGodEventPanel>(god, HideFunc, bOnlyShow);
        }

        private void OnShowFloatGodEventPanel(OtherChessFloatGod god, Action HideFunc, bool bOnlyShow = false, RepeatedField<PItem> Reward = null)
        {
            App.FUI.ShowUI<FloatGodEventPanel>(god, HideFunc, bOnlyShow, Reward);
        }

        // private void OnShowFloatGodRewardPanel(RepeatedField<PItem> Reward, Action func)
        // {
        //     App.FUI.ShowUI<FloatGodRewardPanel>(Reward, func);
        // }


        //---------------------------------------------------------------

        /// <summary>
        /// 显示疯狂租金面板
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowCrazyRentEventPanel(int posId, MapField<int, long> items)
        {
            ShowUI<CrazyRentEventPanel>(posId, items);
        }

        /// <summary>
        /// 显示颜色
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowColorTurntableEventPanel(List<int> lstPieceId)
        {
            //var cfg = App.TData.GetChessPieceInfoById(pieceId);
            //if (cfg != null)
            //{
            ShowUI<ColorTurntablePanel>(lstPieceId);
            //}
        }

        /// <summary>
        /// 显示机会面板
        /// </summary>
        private void ShowChanceEventPanel(int nPieceId, RespGo.Types.CardEvent evt, MapField<int, long> lstReward)
        {
            ShowUI<ChanceEventPanel>(nPieceId, evt, lstReward);
        }

        /// <summary>
        /// 显示打气球小游戏面板
        /// </summary>
        /// <param name="nGameId"></param>
        private void ShowMiniGameBalloonFightPanel(int nGameId)
        {
            MiniGameClass cfgGame = App.TData.GetMiniGameConfig(nGameId);
            if (cfgGame != null)
            {
                if (cfgGame.Type == (int)ChessMiniGameType.Balloon)
                {
                    //打气球小游戏
                    ShowUI<MiniGameBalloonFightPanel>(nGameId);
                }
                else if (cfgGame.Type == (int)ChessMiniGameType.FlyMoney)
                {
                    //抛金币小游戏
                    ShowUI<MiniGameFlyMoneyPanel>(nGameId);
                }
            }
        }

        /// <summary>
        /// 显示监狱扔骰子的面板
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnShowPrisonEventPanel()
        {
            ShowUI<PrisonEventPanel>();
        }

        /// <summary>
        /// 显示小游戏界面
        /// </summary>
        private void OnChessMiniGameShowUI()
        {
            int gameId = DataSystem.Instance.MiniGame.MiniGameId;
            int gameType = DataSystem.Instance.MiniGame.MiniGameType;
            int subType = DataSystem.Instance.MiniGame.MiniGameSubType;

            switch (gameType)
            {
                case (int)ChessMiniGameType.ShootGame:
                    //射击类小游戏
                    GameLog.LW("显示射击类小游戏");
                    ShowUI<LittleGame_Shoot_EventPanel>();
                    break;
                case (int)ChessMiniGameType.CashGame:
                    if (subType == (int)CashMiniGameSubType.DefendCity)
                    {
                        //守城杀敌
                        ShowUI<MiniGameCashPanel>(gameId);
                    }
                    break;
                case (int)ChessMiniGameType.ChoiceGame:
                    //三选一
                    ShowUI<MiniGameOneOutOfThreePanel>();
                    break;

            }

        }

        /// <summary>
        /// 小游戏奖励表现显示完成
        /// </summary>
        private void OnChessMiniGameShowReward()
        {
            GameLog.LW("显示小游戏奖励");
            ShowUI<MiniGameResultPanel>();
            ////int gameId = DataSystem.Instance.MiniGame.MiniGameId;
            //int gameType = DataSystem.Instance.MiniGame.MiniGameType;
            ////int subType = DataSystem.Instance.MiniGame.MiniGameSubType;
            //if (gameType == (int)ChessMiniGameType.ShootGame)
            //{
            //    var reward = DataSystem.Instance.MiniGame.Rewards;
            //    Action HideFunc = () =>
            //    {
            //        GameEvent.Send(GEvent.ChessMiniGameShowRewardFinish);
            //        GameEvent.Send(GEvent.HudChangeViewType, HUDViewModel.Main);
            //    };

            //    DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.Normal, reward, HideFunc, false, LMgr.TC(102600), "");
            //}
            ////else if (gameType == (int)ChessMiniGameType.CashGame)
            ////{
            ////    if (subType == (int)CashMiniGameSubType.DefendCity)
            ////    {
            ////        //守城杀敌
            ////    }
            ////}
        }

        /// <summary>
        /// 显示获得护盾的表现
        /// </summary>
        /// <param name="shieldCount">护盾数量</param>
        /// <param name="mapItem">护盾转骰子</param>
        private void OnShowGetShielEventdPanel(int shieldCount, MapField<int, long> mapItem)
        {
            ShowUI<ShieldEventPanel>(shieldCount, mapItem);
        }

        /// <summary>
        /// 显示破坏事件界面
        /// </summary>
        private void OnShowBreakDownEventPanel()
        {
            ShowUI<BreakDownEventPanel>();
        }

        /// <summary>
        /// 显示银行打劫主界面
        /// </summary>
        private void OnShowBankRobberyEventPanel()
        {
            ShowUI<BankRobberyEventPanel>();
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        private void OnShowChessBoardFinishPanel(bool passLoad = false)
        {
            //GameEvent.Send(GEvent.HudChangeViewType, HUDViewModel.None);
            ShowWin_ChessboardFinishPanel(passLoad).Forget();
        }

        /// <summary>
        /// 发送陈列室房间解锁的消息
        /// </summary>
        /// <param name="roomType"></param>
        private void OnSendShowRoomAutoUnlock(int roomType)
        {
            App.MSG.ShowRoom.GetShowRoomUnlockReward(roomType);
        }

        private async UniTaskVoid ShowWin_ChessboardFinishPanel(bool passLoad = false)
        {
            if (!passLoad)
            {
                float time = 0.5f;
                if (DataSystem.Instance.Player.ChkUpLevelRewardCount())
                {
                    time = 3.5f;
                }
                await UniTask.WaitForSeconds(time);
                if (HasUI<PlayerLevelUpPanel>())
                {
                    Action func = () => { GameEvent.Send(GEvent.ShowChessBoardFinishPanel, true); };
                    GameEvent.Send(GEvent.PlayerLevelUpPanel_AddFunc, func);
                    return;
                }
            }

            GameLog.LW("弹出地图切换奖励界面");
            GameEvent.Send(GEvent.HudChangeViewType, HUDViewModel.None);
            App.FUI.ShowUI<ChessboardFinishPanel>();
        }


        /// <summary>
        /// 显示纯文本型
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="strInfo"></param>
        private void ShowTextTipPanelEvent(TipsInfoEntity tipsInfoEntity)
        {
            ShowUI<CTipPanel>(tipsInfoEntity);
        }

        /// <summary>
        /// 显示 黑色渐变LODING
        /// </summary>
        private void OnShowBlackLoding(Action func)
        {
            ShowUI<BlackLodingPanel>(func);
        }

        /// <summary>
        /// 显示 黑色渐变LODING
        /// </summary>
        private void OnShowBlackLoding()
        {
            ShowUI<BlackLodingPanel>();
        }

        /// <summary>
        /// 播放获得贴纸的动画
        /// <param name="rBase"></param>
        /// <param name="card_1"></param>
        /// <param name="emoji"></param>
        /// <param name="isNew"></param>
        private void OnAlbumShowGetReward(RoleBase rBase, int card_1, int emoji, bool isNew)
        {
            App.FUI.ShowUI<AlbumGiftsResultPanel>(rBase, card_1, emoji, isNew);
        }

        /// <summary>
        /// 播放目标对象获得交换贴纸的动画
        /// </summary>
        /// <param name="rBase"></param>+-----------------------\
        /// <param name="card_1"></param>
        /// <param name="card_2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnAlbumShowExchangeReward(RoleBase rBase, int card_1, int card_2)
        {
            App.FUI.ShowUI<AlbumExchangeResultPanel>(rBase, card_1, card_2, card_1);
        }

        /// <summary>
        /// 播放自己获得交换贴纸的动画
        /// </summary>
        /// <param name="rBase"></param>
        /// <param name="card_1"></param>
        /// <param name="card_2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnAlbumShowSelfExchangeReward(RoleBase rBase, int card_1, int card_2)
        {
            App.FUI.ShowUI<AlbumExchangeResultPanel>(rBase, card_1, card_2, card_2);
        }

        #region --- 登陆后显示主界面（在玩家数据拿到后才进行处理）

        private void OnShowHUDWnd()
        {
            //显示底部常驻显示面板
            App.FUI.ShowUI<CBottomEffectWnd>();
            App.FUI.ShowUI<GuideMainWnd>();

            //显示主界面（常驻界面）
            if (DataSystem.Instance.CorePlay.IsOtherWorld)
            {
                App.FUI.ShowUI<WorldHudMainWnd>();
                App.FUI.ShowUI<WorldHudMainTopWnd>();
            }
            else
            {
                App.FUI.ShowUI<HudMainWnd>();
                App.FUI.ShowUI<HudMainTopWnd>();
            }

            //显示动画表现界面 (常驻界面)
            App.FUI.ShowUI<CMovieWnd>();
        }

        #endregion
    }
}
