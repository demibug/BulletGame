using System;
using System.Collections.Generic;
using TEngine;

namespace GameLogic
{
    public partial class FUISystem : BehaviourSingleton<FUISystem>
    {
        private Dictionary<string, Type> m_dicUIRegister = new();

        private bool m_isUIRegister = false;

        /// <summary>
        /// 注册一个UI到列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RegUI<T>()
        {
            Type type = typeof(T);
            if (!m_dicUIRegister.ContainsKey(type.Name))
            {
                m_dicUIRegister.Add(type.Name, type);
            }
        }

        //------------------------------------------------------

        /// <summary>
        /// 根据UI名显示一个UI
        /// </summary>
        /// <param name="uiName"></param>
        public void ShowUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                ShowUI(m_dicUIRegister[uiName], true, null);
            }
        }

        /// <summary>
        /// 根据UI名判断UI是否在显示中
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public bool HasUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                return HasUI(m_dicUIRegister[uiName]);
            }
            return false;
        }

        /// <summary>
        /// 根据UI名取得窗口对象
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public FUIWindow GetUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                return GameModule.FUI.GetWindow(m_dicUIRegister[uiName].FullName);
            }
            return null;
        }

        /// <summary>
        /// 注册UI（UI类名不允许重名）
        /// </summary>
        public void RegisterUI()
        {
            if (m_isUIRegister) return;
            m_isUIRegister = true;

            m_dicUIRegister.Clear();

            RegUI<AladdinEventPanel>();
            RegUI<AladdinFriendRewardPanel>();
            RegUI<AladdinPanel>();
            RegUI<AladdinResultPanel>();
            RegUI<AladdinWaitPanel>();
            RegUI<AlbumExchangePanel>();
            RegUI<AlbumExchangeResultPanel>();
            RegUI<AlbumExchangeTipPanel>();
            RegUI<AlbumFriendExChangePanel>();
            RegUI<AlbumFriendGetPanel>();
            RegUI<AlbumGiftsResultPanel>();
            RegUI<AlbumGiftsTipPanel>();
            RegUI<AlbumImageInfoPanel>();
            RegUI<AlbumInfoMainWnd>();
            RegUI<AlbumMainWnd>();
            RegUI<AlbumSalePanel>();
            RegUI<AlbumUnlockPanel>();
            RegUI<AvatarSelectPanel>();
            RegUI<BankRobberyEventPanel>();
            RegUI<BankRobberyInfoPanel>();
            RegUI<BlackLodingPanel>();
            RegUI<BreakDownEventPanel>();
            RegUI<CActivityBigRewardPanel>();
            RegUI<CBottomEffectWnd>();
            RegUI<CClickTipPanel>();
            RegUI<CCloudMaskPanel>();
            RegUI<CDebugPanel>();
            RegUI<ChanceEventPanel>();
            RegUI<ChangeNamePanel>();
            RegUI<ChessboardChangePanel>();
            RegUI<ChessboardFinishPanel>();
            RegUI<CItemTipPanel>();
            RegUI<CMovieWnd>();
            RegUI<CNPCTalkTipPanel>();
            RegUI<ColorTurntablePanel>();
            RegUI<CommonGetRewardPanel>();
            RegUI<ComOpenAlbumBoxPanel>();
            RegUI<ComOpenBoxPanel>();
            RegUI<ComOpenSpecialItemPanel>();
            RegUI<CrazyRentEventPanel>();
            RegUI<CRulePanel>();
            RegUI<CTextTipPanel>();
            RegUI<CTipPanel>();
            RegUI<CTouchEffectWind>();
            RegUI<CurtainLodingPanel>();
            RegUI<DailyTaskMainWnd>();
            RegUI<DesertTreasureEnterPanel>();
            RegUI<DesertTreasurePanel>();
            RegUI<DifferentDimensionalSpaceLoading2Panel>();
            RegUI<EmojiPanel>();
            RegUI<ExhibitionRoomMainWnd>();
            RegUI<ExhibitionSelectPanel>();
            RegUI<ExhibitionUnlockMoviePanel>();
            RegUI<FloatDiceControlPanel>();
            RegUI<FloatGodEventPanel>();
            RegUI<FloatGodRewardPanel>();
            RegUI<FloatWeatherPanel>();
            RegUI<FriendMsgBoxPanel>();
            RegUI<FriendPanel>();
            RegUI<FunOpenInfoPanel>();
            RegUI<GuideMainWnd>();
            RegUI<HudMainTopWnd>();
            RegUI<HudMainWnd>();
            RegUI<LargeTaxesEventPanel>();
            RegUI<LimitTimeActivityGoldDoubleShowPanel>();
            RegUI<LimitTimeActivityPanel>();
            RegUI<LittleGame_Shoot_EventPanel>();
            RegUI<LoginWnd>();
            RegUI<MainMenuPanel>();
            RegUI<MiniGameBalloonFightPanel>();
            RegUI<MiniGameCashPanel>();
            RegUI<MiniGameFlyMoneyPanel>();
            RegUI<MiniGameResultPanel>();
            RegUI<NetAssetsPanel>();
            RegUI<OptionLanguagePanel>();
            RegUI<OptionMainPanel>();
            RegUI<OtherWorldSettleAccountsPanel>();
            RegUI<PlayerInfoPanel>();
            RegUI<PlayerLevelUpPanel>();
            RegUI<PMPanel>();
            RegUI<PrisonEventPanel>();
            RegUI<RandomBoxEventPanel>();
            RegUI<RankActivityPanel>();
            RegUI<RankActivityRulePanel>();
            RegUI<ReportPanel>();
            RegUI<ScoreActivityPanel>();
            RegUI<SevenDayMainWnd>();
            RegUI<ShieldEventPanel>();
            RegUI<ShoppingPanel>();
            RegUI<StoreGiftActivityPanel>();
            RegUI<ThemeInfoPanel>();
            RegUI<ThemePanel>();
            RegUI<TimeMasterPanel>();
            RegUI<TimeMasterRankPanel>();
            RegUI<WorldChanceEventPanel>();
            RegUI<WorldHudMainTopWnd>();
            RegUI<WorldHudMainWnd>();
            RegUI<WorldPrisonEventPanel>();
            RegUI<WorldSceneTaskPanel>();
            RegUI<WorldTurntablePanel>();
        }
    }
}
