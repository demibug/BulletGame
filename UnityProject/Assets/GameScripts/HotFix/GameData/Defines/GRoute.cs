namespace GameData
{
    public static class GRoute
    {
        //登陆游戏
        public const string Auth = "/gameApi/auth";
        // public const int Auth = 101;

        //查询用户信息
        public const string GameRoleProfile = "/gameApi/profile";
        // public const int GameRoleProfile = 103;

        //每日重置时间戳更新
        public const int NextDayZeroRefreshTimePush = 529;

        //更新功能开放状态
        public const string FunOpennSave = "/gameApi/functionOpenSave";
        // public const int FunOpennSave = 113;

        //扔骰子
        public const string GameBoardGo = "/gameApi/go";
        // public const int GameBoardGo = 301;

        //使用遥控骰子
        public const string GameDiceControl = "/gameApi/dimensionDiceControl";

        //设置骰子结算倍数
        public const string GameBoardMultiplier = "/gameApi/multiplier";
        // public const int GameBoardMultiplier = 303;

        //----- 玩家信息 ------

        //修改玩家头像
        public const string GameRoleRoleavatar = "/gameApi/changeAvatar";
        // public const int GameRoleRoleavatar = 107;

        //修改玩家名称
        public const string GameRoleRolename = "/gameApi/changeName";
        // public const int GameRoleRolename = 109;

        //选择人物形象
        public const string SelectAvatar = "/gameApi/diceSelect";
        // public const int SelectAvatar = 111;

        //检测敏感字
        public const string GameApiFilterWord = "/gameApi/filterWord";


        //----- 商店相关 ------

        //获取商店面板数据
        public const string GameApiShopPanel = "/gameApi/shopPanel";
        //商店物品购买
        public const string GameApiShopBuy = "/gameApi/shopBuy";


        //----- 建筑相关 ------
        //建造建筑
        public const string GameLandmarkBuild = "/gameApi/buildingBuild";
        // public const int GameLandmarkBuild = 801;

        //升级建筑
        public const string GameLandmarkUpgrade = "/gameApi/buildingUpgrade";
        // public const int GameLandmarkUpgrade = 803;

        //修复建筑
        public const string GameLandmarkFix = "/gameApi/buildingFix";
        // public const int GameLandmarkFix = 805;


        //更新骰子的恢复时间
        // public const string GameBagSyncDicetime = "game.bag.sync_dicetime";
        public const int GameBagSyncDicetime = 506;

        //同步当前服务器时间
        public const string GateGameSyncTime = "/gameApi/syncTime";
        // public const int GateGameSyncTime = 105;

        //玩家等级升级
        // public const string GameRoleSyncLevelUp = "game.role.synclevelup";
        public const int GameRoleSyncLevelUp = 502;

        //转动转盘消息
        public const string GameRoleWheel = "/gameApi/wheel";
        // public const int GameRoleWheel = 601;

        //监狱投骰子
        public const string GameRoleJailRoll = "/gameApi/jailRoll";
        // public const int GameRoleJailRoll = 605;

        //使用小房子
        public const string GameRoleBoardscene = "/gameApi/boardScene";
        // public const int GameRoleBoardscene = 603;


        //好友数据状态更新
        // public const string SocialSyncNotify = "social.sync.notify";
        public const int SocialSyncNotify = 518;

        //获取好友列表
        public const string SocialFriendFriends = "/gameApi/friendList";
        // public const int SocialFriendFriends = 201;

        //通知new
        public const string SocialFriendNewlist = "/gameApi/friendNewList";
        // public const int SocialFriendNewlist = 203;

        //获取推荐玩家
        public const string SocialFriendRecommend = "/gameApi/friendRecommend";
        // public const int SocialFriendRecommend = 205;

        //添加好友
        public const string SocialFriendAdd = "/gameApi/friendAdd";
        // public const int SocialFriendAdd = 207;

        //处理好友请求
        public const string SocialFriendHandle = "/gameApi/friendApply";
        // public const int SocialFriendHandle = 211;

        //删除好友
        public const string SocialFriendRemove = "/gameApi/friendRemove";
        // public const int SocialFriendRemove = 213;

        //获取排行榜数据
        public const string GameApiFriendRank = "/gameApi/friendRank";
        //获取异世界排行榜数据
        public const string GameApiDimensionRank = "/gameApi/dimensionRank";


        //请求破坏目标建筑信息
        public const string GameHelicopterHdbuildings = "/gameApi/hDBuildings";
        // public const int GameHelicopterHdbuildings = 315;

        //破坏目标建筑
        public const string GameHelicopterHelicopterdestroy = "/gameApi/helicopterDestroy";
        // public const int GameHelicopterHelicopterdestroy = 339;

        //返回目标建筑信息
        // public const string GameSyncHdbuildings = "game.sync.hdbuildings";
        // public const int GameSyncHdbuildings = 516;

        //响应破坏消息
        // public const string GameSyncHelicopterdestroy = "game.sync.helicopterdestroy";
        // public const int GameSyncHelicopterdestroy = 514;

        //推送被破坏消息
        // public const string GameSyncBehelicopterdestroy = "game.sync.behelicopterdestroy";
        public const int GameSyncBehelicopterdestroy = 312;

        //切换破坏目标
        //public const string GameHelicopterReselecttarget = "game.helicopter.reselecttarget";

        //响应切换破坏目标
        //public const string GameSyncHdreselect = "game.sync.hdreselect";

        //请求破坏复仇列表
        public const string GameHelicopterRevenges = "/gameApi/helicopterRevenges";
        // public const int GameHelicopterRevenges = 313;

        //请求获得银行打劫数据
        public const string GameHelicopterHeistvaults = "/gameApi/heistVaults";
        // public const int GameHelicopterHeistvaults = 307;

        //进行银行打劫
        public const string GameHalicopterHeist = "/gameApi/heist";
        // public const int GameHalicopterHeist = 341;

        //银行打劫返回
        // public const string GameSyncHeist = "game.sync.heist";
        // public const int GameSyncHeist = 310;

        //保存银行打劫的数据记录
        public const string GameHelicopterSave = "/gameApi/helicopterSave";
        // public const int GameHelicopterSave = 347;


        /// <summary>
        /// 彩票屋
        /// </summary>
        public const string GameApiLotteryBuy = "/gameApi/lotteryBuy";

        /// <summary>
        /// 彩票屋结算
        /// </summary>
        public const int GameLotteryData = 1013;


        //----- 地图相关 ------

        public const string GameLandmarkMovemap = "/gameApi/buildingMoveMap";
        // public const int GameLandmarkMovemap = 807;



        //----- 棋盘报告 ------

        public const string GameReporterEmoji = "/gameApi/reportEmoji";
        // public const int GameReporterEmoji = 321;

        public const string GameReporterReward_100 = "/gameApi/reward100";
        // public const int GameReporterReward_100 = 335;

        public const string GameReporterRewardAlbum = "/gameApi/rewardAlbum";
        // public const int GameReporterRewardAlbum = 337;

        //万能胶片兑换
        public const string GameApiBagStickerExchange = "/gameApi/bagStickerExchange";

        // public const string GameSyncReporter = "game.sync.reporter";
        public const int GameSyncReporter = 504;


        //----- 阿拉丁相关 ------

        public const string GameAladdinData = "/gameApi/aladingData";
        // public const int GameAladdinData = 901;

        public const string GameAladdinStart = "/gameApi/aladingStart";
        // public const int GameAladdinStart = 903;

        public const string GameAladdinManual = "/gameApi/aladingManual";
        // public const int GameAladdinManual = 905;

        //----- 小游戏相关 ------

        public const string GameBoardMinigame = "/gameApi/miniGame";
        // public const int GameBoardMinigame = 305;


        //----- 任务相关 ------

        // [Push] 玩家任务更新
        // public const string TaskSyncData = "task.sync.data";
        public const int TaskSyncData = 512;

        // [rpc] 请求获得日常任务信息
        public const string TaskDailyData = "/gameApi/dailyTaskList";
        // public const int TaskDailyData = 401;

        // [rpc] 领取日常任务奖励
        public const string TaskDailyDtobtain = "/gameApi/dailyTaskReward";
        // public const int TaskDailyDtobtain = 403;

        // [rpc] 领取周积分奖励
        public const string TaskDailyDailyPointReward = "/gameApi/dailyTaskPointReward";
        // public const int TaskDailyDailyPointReward = 405;


        //---- 画册相关

        // [call] 集齐画册推送的通知
        // public const string GameSyncAlbum = "game.sync.album";
        public const int GameSyncAlbum = 508;

        // [call] 贴纸赠送
        public const string GameReporterAlbumGive = "/gameApi/albumGive";
        // public const int GameReporterAlbumGive = 323;

        // [call] 贴纸交换（发起方）
        public const string GameReporterAlbumExchange = "/gameApi/albumExchange";
        // public const int GameReporterAlbumExchange = 325;

        // [call] 贴纸交换处理（接收方处理逻辑）
        public const string GameReporterAlbumExchange1 = "/gameApi/albumExchange1";
        // public const int GameReporterAlbumExchange1 = 331;

        // [call] 贴纸交换处理（发送方二次处理逻辑）
        public const string GameReporterAlbumExchange2 = "/gameApi/albumExchange2";
        // public const int GameReporterAlbumExchange2 = 333;

        // [call] 贴纸索要
        public const string GameReporterAlbumAskFor = "/gameApi/albumAskFor";
        // public const int GameReporterAlbumAskFor = 327;

        // [call] 贴纸索要处理（接收方处理逻辑）
        public const string GameReporterAlbumAskForReturn = "/gameApi/albumAskFor1";
        // public const int GameReporterAlbumAskForReturn = 329;

        // [call] 贴纸宝箱兑换
        public const string GameRoleAlbumOpenChest = "/gameApi/albumOpenChest";
        // public const int GameRoleAlbumOpenChest = 613;

        // 画册变更（新画册开启）
        public const int SyncAlbumChange = 1014;


        // --------------- 生成随机异次元主题 ---------------[call] game.dimension.themes
        public const string GameDimesionThemes = "/gameApi/dimensionThemes";
        // public const int GameDimesionThemes = 317;

        // --------------- 进入选择的异次元主题 --------------- [call] game.dimension.enter
        public const string GameDimesionEnter = "/gameApi/dimensionEnter";
        // public const int GameDimesionEnter = 319;

        // --------------- 验证退出 --------------- [call] game.dimension.canexit
        public const string GameDimensionCanexit = "/gameApi/dimensionCanExit";
        // public const int GameDimensionCanexit = 343;

        //【异世界】设置体力消息的倍率
        public const string GameDimensionMultiplier = "/gameApi/dimensionMultiplier";
        // public const int GameDimensionMultiplier = 345;

        //----- 陈列室相关

        // [push] 陈列室解锁后的数据推送
        // public const string GameSyncShowRoom = "game.sync.showroom";
        public const int GameSyncShowRoom = 510;

        

        // [call] 陈列室穿戴更换（棋子，护盾样式）
        public const string GameRoleShowRoomEquip = "/gameApi/showRoomEquip";
        // public const int GameRoleShowRoomEquip = 609;

        // [call] 领取解锁陈列室奖励
        public const string GameRoleShowRoomReward = "/gameApi/showroomReward";
        // public const int GameRoleShowRoomReward = 611;


        //----- GM 相关 -----

        //GM 添加物品
        public const string GameGmBag = "/gameApi/gmBag";
        // public const int GameGmBag = 701;

        //GM 仍骰子
        public const string GameGmGo = "/gameApi/gmGo";
        // public const int GameGmGo = 703;

        //GM 地块房子全满级
        public const string GameGmMaxpiecelevel = "/gameApi/gmPieceLevelMax";
        // public const int GameGmMaxpiecelevel = 707;

        //GM 添加好友
        public const string SocialGmAddfriend = "/gameApi/gmFriend";
        // public const int SocialGmAddfriend = 705;


        /// <summary>
        /// GM 指令，格式为：指令 参数1 参数2 ....  不同指令附带的参数量不一样
        /// </summary>
        public const string GameManageOrder = "/gameApi/gm";


        //----- 游戏时长
        public const string GameRolePlayTime = "/gameApi/playTime";
        // public const int GameRolePlayTime = 607;


        /// <summary>
        /// 贴纸推送
        /// </summary>
        public const int SyncStickerPush = 520;

        /// <summary>
        /// 异世界场景任务推送
        /// </summary>
        public const int SyncDimensionSceneTask = 524;
        /// <summary>
        /// 异世界查询排行榜信息
        /// </summary>
        public const string QueryDimensionRank = "/gameApi/dimensionRank";


        #region --- 积分活动/限时活动/七日签到 ---

        /// <summary>
        /// 七日签到 - 领取奖励
        /// </summary>
        public const string SevenDaySignIn = "/gameApi/sign7TakeHandle";
        // public const int SevenDaySignIn = 1001;

        /// <summary>
        /// 七日签到 - 数据更新
        /// </summary>
        public const int SyncSevenDayInfo = 1004;

        /// <summary>
        /// 积分活动信息推送
        /// </summary>
        public const int SyncScoreActivityPush = 1006;

        /// <summary>
        /// 事件活动推送
        /// </summary>
        public const int SyncEventActivityPush = 1008;

        /// <summary>
        /// 活动关闭推送
        /// </summary>
        public const int SyncActivityClosePush = 1010;

        /// <summary>
        /// 事件活动奖励推送
        /// </summary>
        public const int EventActivityRewardPush = 522;


        /// <summary>
        /// 打气球小游戏结算
        /// </summary>
        public const string MiniGameBalloonsResult = "/gameApi/happyBalloonsBalance";
        // public const int MiniGameBalloonsResult = 1011;


        /// <summary>
        /// 锦标赛排行榜面板
        /// </summary>
        public const string gameApiRankActivityPanel = "/gameApi/rankActivityPanel";

        /// <summary>
        /// 弹脸奖励领取
        /// </summary>
        public const string gameApiAlertRewardTake = "/gameApi/alertRewardTake";

        /// <summary>
        /// 挖宝活动 挖掘
        /// </summary>
        public const string gameApiWeeklyActivityDig = "/gameApi/weeklyActivityDig";

        /// <summary>
        /// 活动礼包购买
        /// </summary>
        public const string GameApiGiftPackBuy = "/gameApi/giftPackBuy";

        /// <summary>
        /// 锦标赛积分变化推送
        /// </summary>
        public const int RankActivityPush = 526;

        /// <summary>
        /// 挖宝活动推送
        /// </summary>
        public const int WeeklyActivityPush = 528;

        /// <summary>
        /// 礼包活动推送
        /// </summary>
        public const int GiftPackActivityPush = 1012;

        #endregion

        //时空之主猜拳
        public const string DismensionRps = "/gameApi/dimensionRps";

        //时空之主猜拳结束
        public const string DismensionRpsOver = "/gameApi/dimensionRpsOver";


        //新手引导相关
        public const string GuideQuery = "/gameApi/guideQuery";
        public const string GuideSave = "/gameApi/guideSave";

        //零点推送
        public const int SyncClock0Push = 1015;


        // 测试
        public const int CmdTestReq = 115;

        // 测试回包
        public const int CmdTestResp = 116;


    }
}
