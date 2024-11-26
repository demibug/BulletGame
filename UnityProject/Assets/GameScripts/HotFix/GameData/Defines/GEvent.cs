using TEngine;

namespace GameData
{
    public static class GEvent
    {
        #region --- 登陆相关 ---

        /// <summary>
        /// 获取账号成功
        /// </summary>
        public static readonly int GetAccountSuccess = RuntimeId.ToRuntimeId("GetAccountSuccess");

        /// <summary>
        /// WebRequest请求成功
        /// </summary>
        public static readonly int WebRequestSystemEventSuccess = RuntimeId.ToRuntimeId("WebRequestSystemEventSuccess");

        /// <summary>
        /// WebRequest请求失败
        /// </summary>
        public static readonly int WebRequestSystemEventFailure = RuntimeId.ToRuntimeId("WebRequestSystemEventFailure");

        #endregion

        #region --- 全局 ---

        /// <summary>
        /// 屏幕点击事件（显示屏幕点击特效）
        /// </summary>
        public static readonly int WindTouchEffectEvent = RuntimeId.ToRuntimeId("WindTouchEffectEvent");

        /// <summary>
        /// 屏幕点击事件（显示屏幕点击特效）
        /// </summary>
        public static readonly int WindTouchEffectOpen = RuntimeId.ToRuntimeId("WindTouchEffectOpen");

        /// <summary>
        /// 屏幕点击事件（关闭屏幕点击特效）
        /// </summary>
        public static readonly int WindTouchEffectClose = RuntimeId.ToRuntimeId("WindTouchEffectClose");

        /// <summary>
        /// 每帧会抛出的事件
        /// </summary>
        public static readonly int EnterFrameEvent = RuntimeId.ToRuntimeId("EnterFrameEvent");

        /// <summary>
        /// 更新当前玩家的信息
        /// </summary>
        public static readonly int SyncPlayerInfo = RuntimeId.ToRuntimeId("SyncPlayerInfo");

        /// <summary>
        /// 同步当前的服务器时间
        /// </summary>
        public static readonly int SyncServerTime = RuntimeId.ToRuntimeId("SyncServerTime");

        /// <summary>
        /// 更新背包中的物品
        /// </summary>
        public static readonly int SyncBagItems = RuntimeId.ToRuntimeId("SyncBagItems");

        /// <summary>
        /// 关闭遮照云动画
        /// </summary>
        public static readonly int HiddenCloudMaskPanel = RuntimeId.ToRuntimeId("HiddenCloudMaskPanel");

        /// <summary>
        /// 显示奖励提示信息弹框
        /// </summary>
        public static readonly int ShowRewardTipPanel = RuntimeId.ToRuntimeId("ShowRewardTipPanel");

        /// <summary>
        /// 显示物品的描述信息面板
        /// </summary>
        public static readonly int ShowItemTipPanel = RuntimeId.ToRuntimeId("ShowItemTipPanel");

        /// <summary>
        /// 显示功能相关的信息面板
        /// </summary>
        public static readonly int ShowFunOpenTipPanel = RuntimeId.ToRuntimeId("ShowItemTipPanel");

        /// <summary>
        /// 显示商城界面
        /// </summary>
        public static readonly int ShowShoppingPanel = RuntimeId.ToRuntimeId("ShowShoppingPanel");

        /// <summary>
        /// 显示直购界面
        /// </summary>
        public static readonly int ShowBuyLackPanel = RuntimeId.ToRuntimeId("ShowBuyLackPanel");
        /// <summary>
        /// 关闭直购界面
        /// </summary>
        public static readonly int CloseBuyLackPanel = RuntimeId.ToRuntimeId("CloseBuyLackPanel");



        /// <summary>
        /// 通用弹出获得奖励面板
        /// </summary>
        public static readonly int ShowGetRewardPaneel = RuntimeId.ToRuntimeId("ShowGetRewardPaneel");

        /// <summary>
        /// 根据数据类型直接进行显示
        /// </summary>
        public static readonly int ShowGetRewardPanelByOption = RuntimeId.ToRuntimeId("ShowGetRewardPanelByOption");

        /// <summary>
        /// 玩家升级事件
        /// </summary>
        public static readonly int PlayerUpgradeLevel = RuntimeId.ToRuntimeId("PlayerUpgradeLevel");

        /// <summary>
        /// 玩家升级 跟新地块信息
        /// </summary>
        public static readonly int PlayerUpgradeLevel_Piece = RuntimeId.ToRuntimeId("PlayerUpgradeLevel_Piece");

        /// <summary>
        /// 显示玩家升级表现
        /// </summary>
        public static readonly int PlayerUpgradeLevel_Client = RuntimeId.ToRuntimeId("PlayerUpgradeLevel_Client");

        /// <summary>
        /// 显示进入下张地图的界面
        /// </summary>
        public static readonly int ShowChessBoardFinishPanel = RuntimeId.ToRuntimeId("ShowChessBoardFinishPanel");

        /// <summary>
        /// 进入下张地图异常
        /// </summary>
        public static readonly int SyncMovemapError = RuntimeId.ToRuntimeId("SyncMovemapError");


        /// <summary>
        /// 升级界面关闭后
        /// </summary>
        public static readonly int PlayerLevelUpPanel_Close = RuntimeId.ToRuntimeId("PlayerLevelUpPanel_Close");

        /// <summary>
        /// 给玩家升级界面添加委托
        /// </summary>
        public static readonly int PlayerLevelUpPanel_AddFunc = RuntimeId.ToRuntimeId("PlayerLevelUpPanel_AddFunc");


        /// <summary>
        /// 播放物品飞行的动画
        /// </summary>
        public static readonly int PlayItemFlyMovie = RuntimeId.ToRuntimeId("PlayItemFlyMovie");

        public static readonly int PlayItemFlyMovieComplete = RuntimeId.ToRuntimeId("PlayItemFlyMOvieComplete");

        /// <summary>
        /// 获得活动道具时的表现
        /// </summary>
        public static readonly int GetActivityItemMove = RuntimeId.ToRuntimeId("GetActivityItemMove");

        public static readonly int GetActivityItemMoveComplete = RuntimeId.ToRuntimeId("GetActivityItemMoveComplete");
        public static readonly int GetActivityItemMoveFinish = RuntimeId.ToRuntimeId("GetActivityItemMoveFinish");


        /// <summary>
        /// 打开PM界面
        /// </summary>
        public static readonly int OpenPmView = RuntimeId.ToRuntimeId("OpenPmView");

        /// <summary>
        /// 播放获得经验表现
        /// </summary>
        public static readonly int Main_PlayEffect_Exp = RuntimeId.ToRuntimeId("Main_PlayEffect_Exp");

        /// <summary>
        /// 播放获得小房子表现
        /// </summary>
        public static readonly int Main_PlayEffect_LittleHouse = RuntimeId.ToRuntimeId("Main_PlayEffect_LittleHouse");

        /// <summary>
        /// 小房子动效表现完成后
        /// </summary>
        public static readonly int HouseMoveEnd = RuntimeId.ToRuntimeId("HouseMoveEnd");

        /// <summary>
        /// 小房子合成经验的表现
        /// </summary>
        public static readonly int LittleHouseChangeExpEvent = RuntimeId.ToRuntimeId("LittleHouseChangeExpEvent");


        /// <summary>
        /// 清空一些 pop 界面的显示
        /// </summary>
        public static readonly int ClearPopUI = RuntimeId.ToRuntimeId("ClearPopUI");

        public static readonly int TouchScreen = RuntimeId.ToRuntimeId("TouchScreen");

        /// <summary>
        /// UI界面显示及隐藏的通用事件
        /// </summary>
        public static readonly int UIShowEvent = RuntimeId.ToRuntimeId("UIShowEvent");

        public static readonly int UICloseEvent = RuntimeId.ToRuntimeId("UICloseEvent");


        /// <summary>
        /// 界面完成了显示（由界面内部抛出）
        /// </summary>
        public static readonly int UIShowComplete = RuntimeId.ToRuntimeId("UIShowComplete");

        /// <summary>
        /// 检测UI打开后的弹出界面逻辑
        /// </summary>
        public static readonly int CheckUIAction = RuntimeId.ToRuntimeId("CheckUIAction");

        /// <summary>
        /// 显示通用文本Tip面板
        /// </summary>
        public static readonly int ShowTextTipPanel = RuntimeId.ToRuntimeId("ShowTextTipPanel");

        /// <summary>
        /// 刷新属性
        /// </summary>
        public static readonly int SyncAttrib = RuntimeId.ToRuntimeId("SyncAttrib");

        /// <summary>
        /// 登陆后显示主界面
        /// </summary>
        public static readonly int LoginAndShowHUD = RuntimeId.ToRuntimeId("LoginAndShowHUD");

        /// <summary>
        /// 更新好友列表状态
        /// </summary>
        public static readonly int SyncFriendList = RuntimeId.ToRuntimeId("SyncFriendList");

        /// <summary>
        /// 设置默认的形象成功
        /// </summary>
        public static readonly int SelectAvatarComplete = RuntimeId.ToRuntimeId("SelectAvatarComplete");

        /// <summary>
        /// 播放通用获得奖励的面板动画完毕
        /// </summary>
        public static readonly int PlayGetRewardMovieComplete = RuntimeId.ToRuntimeId("PlayGetRewardMovieComplete");

        /// <summary>
        /// 刷新建筑相关的红点逻辑
        /// </summary>
        public static readonly int SyncBuildRedHot = RuntimeId.ToRuntimeId("SyncBuildRedHot");

        /// <summary>
        /// 通用表现界面打开时，隐藏/恢复非固定显示的UI界面
        /// </summary>
        public static readonly int RewardMovieHidenUI = RuntimeId.ToRuntimeId("RewardMovieHidenUI");
        public static readonly int RewardMovieReshowUI = RuntimeId.ToRuntimeId("RewardMovieReshowUI");

        /// <summary>
        /// 显示贴纸礼包 Tip 面板
        /// </summary>
        public static readonly int ShowAlbumTipPanel = RuntimeId.ToRuntimeId("ShowAlbumTipPanel");

        #endregion

        #region --- 主界面 ---

        /// <summary>
        /// 主界面打开时
        /// </summary>
        public static readonly int HudMainWndShow = RuntimeId.ToRuntimeId("HudMainWndShow");

        /// <summary>
        /// 主界面更新界面显示模式
        /// </summary>
        public static readonly int HudChangeViewType = RuntimeId.ToRuntimeId("HudChangeViewType");

        /// <summary>
        /// 主界面抖动表现
        /// </summary>
        public static readonly int HudMainWndShake = RuntimeId.ToRuntimeId("HudMainWndShake");

        /// <summary>
        /// 切换到HUD飞房子分页
        /// </summary>
        public static readonly int HudChangeToMiniHouse = RuntimeId.ToRuntimeId("HudChangeToMiniHouse");

        /// <summary>
        /// 切换到HUD主界面
        /// </summary>
        public static readonly int HudChangeToMain = RuntimeId.ToRuntimeId("HudChangeToMain");

        /// <summary>
        /// 主界面保存当前的布局分页，强行切换到空白分页
        /// </summary>
        public static readonly int HudClearScreen = RuntimeId.ToRuntimeId("HudClearScreen");

        /// <summary>
        /// 主界面还原当前的界面设置
        /// </summary>
        public static readonly int HudResetScreen = RuntimeId.ToRuntimeId("HudResetScreen");

        /// <summary>
        /// 更新当前棋子的倍率
        /// </summary>
        public static readonly int SyncDiceMultiplier = RuntimeId.ToRuntimeId("SyncDiceMultiplier");

        /// <summary>
        /// 更新当前棋子的形象
        /// </summary>
        public static readonly int SyncBoardToken = RuntimeId.ToRuntimeId("SyncBoardToken");

        /// <summary>
        /// 更新下一个打劫目标对象
        /// </summary>
        public static readonly int SyncHeistInfo = RuntimeId.ToRuntimeId("SyncHeistInfo");

        /// <summary>
        /// 更新建筑信息
        /// </summary>
        public static readonly int UpdateLandMarkInfo = RuntimeId.ToRuntimeId("UpdateLandMarkInfo");

        /// <summary>
        /// 更新骰子相关的信息（数量，倒计时时间等）
        /// </summary>
        public static readonly int UpdateDiceInfo = RuntimeId.ToRuntimeId("UpdateDiceInfo");

        /// <summary>
        /// 更新体力相关的信息
        /// </summary>
        public static readonly int UpdateStrengthInfo = RuntimeId.ToRuntimeId("UpdateStrengthInfo");

        /// <summary>
        /// 点击了主界面上的建筑按钮
        /// </summary>
        public static readonly int ClickBuildIcon = RuntimeId.ToRuntimeId("ClickBuildIcon");

        /// <summary>
        /// 通知更新主界面各坐标的信息
        /// </summary>
        public static readonly int SyncHUDPos = RuntimeId.ToRuntimeId("SyncHUDPos");

        /// <summary>
        /// 主界面坐标位置更新完毕
        /// </summary>
        public static readonly int SyncHUDPosComplete = RuntimeId.ToRuntimeId("SyncHUDPosComplete");

        /// <summary>
        /// 停止自动掷骰子动作
        /// </summary>
        public static readonly int HUDStopAutoDice = RuntimeId.ToRuntimeId("HUDStopAutoDice");

        /// <summary>
        /// 主界面点击锁定或解锁点击状态
        /// </summary>
        public static readonly int HUDLockedTouch = RuntimeId.ToRuntimeId("HUDLockedTouch");


        /// <summary>
        /// 主界面投骰子按钮获得双倍金币活动的表现
        /// </summary>
        public static readonly int HUDBtnActivityGoldDouble = RuntimeId.ToRuntimeId("HUDBtnActivityGoldDouble");

        /// <summary>
        /// 小房子动效减少
        /// </summary>
        public static readonly int HUDHouseFlySub = RuntimeId.ToRuntimeId("HUDHouseFlySub");

        /// <summary>
        /// 小房子动效增加
        /// </summary>
        public static readonly int HUDHouseFlyAdd = RuntimeId.ToRuntimeId("HUDHouseFlyAdd");

        /// <summary>
        /// 净资产动效增加
        /// </summary>
        public static readonly int HUDNetAssetsFlyAdd = RuntimeId.ToRuntimeId("HUDNetAssetsFlyAdd");

        /// <summary>
        /// 净资产动效增加
        /// </summary>
        public static readonly int HUDNetAssetsFlyAddFinish = RuntimeId.ToRuntimeId("HUDNetAssetsFlyAddFinish");

        /// <summary>
        /// 使用小房子协议回调
        /// </summary>
        public static readonly int SyncGameRoleBoardscene = RuntimeId.ToRuntimeId("SyncGameRoleBoardscene");

        /// <summary>
        /// 主界面事件，在当前护盾的基础上添加一个新的护盾（加到上限就停止）
        /// </summary>
        public static readonly int HUDAddOneShieldObject = RuntimeId.ToRuntimeId("HUDAddOneShieldObject");

        /// <summary>
        /// 添加物品的飞行终点坐标（用于播放飞行动画时处理）
        /// </summary>
        public static readonly int AddItemFlyUIPos = RuntimeId.ToRuntimeId("AddItemFlyUIPos");

        public static readonly int DelItemFlyUIPos = RuntimeId.ToRuntimeId("DelItemFlyUIPos");

        /// <summary>
        /// 显示黑色渐变loding
        /// </summary>
        public static readonly int ShowBlackLoding = RuntimeId.ToRuntimeId("ShowBlackLoding");

        /// <summary>
        /// 隐藏黑色渐变loding
        /// </summary>
        public static readonly int HideBlackLoding = RuntimeId.ToRuntimeId("HideBlackLoding");

        /// <summary>
        /// 系统菜单界面操作
        /// </summary>
        public static readonly int MainMenuEvent = RuntimeId.ToRuntimeId("MainMenuEvent");

        /// <summary>
        /// 通知显示主体的界面
        /// </summary>
        public static readonly int ShowMainHUD = RuntimeId.ToRuntimeId("ShowMainHUD");

        /// <summary>
        /// 隐藏主HUD
        /// </summary>
        public static readonly int HideMainHUD = RuntimeId.ToRuntimeId("HideMainHUD");

        /// <summary>
        /// 通知显示异世界的主界面
        /// </summary>
        public static readonly int ShowWorldMainHUD = RuntimeId.ToRuntimeId("ShowWorldMainHUD");

        /// <summary>
        /// 隐藏异世界主HUD
        /// </summary>
        public static readonly int HideWorldMainHUD = RuntimeId.ToRuntimeId("HideWorldMainHUD");

        /// <summary>
        /// 异世界的主界面打开后的通知
        /// </summary>
        public static readonly int WorldHudMainWndShow = RuntimeId.ToRuntimeId("WorldHudMainWndShow");

        /// <summary>
        /// 通知开始检测功能解锁相关的动画表现
        /// </summary>
        public static readonly int CheckPlayFunUnlockMovie = RuntimeId.ToRuntimeId("CheckPlayFunUnlockMovie");

        /// <summary>
        /// 复位操作按钮
        /// </summary>
        public static readonly int HUDResetRollButton = RuntimeId.ToRuntimeId("HUDResetRollButton");

        #endregion

        #region --- 核心玩法 ---

        /// <summary>
        /// 预加载场景完成
        /// </summary>
        public static readonly int PreloadSceneReady = RuntimeId.ToRuntimeId("PlayerSceneReady");

        /// <summary>
        /// 重新加载场景完成
        /// </summary>
        public static readonly int ReloadSceneReady = RuntimeId.ToRuntimeId("ReloadSceneReady");

        /// <summary>
        /// 加载场景完成
        /// </summary>
        public static readonly int LoadSceneReady = RuntimeId.ToRuntimeId("LoadSceneReady");

        /// <summary>
        /// 玩家数据准备完成
        /// </summary>
        public static readonly int PlayerDataReady = RuntimeId.ToRuntimeId("PlayerDataReady");

        /// <summary>
        /// 核心玩法已开始
        /// </summary>
        public static readonly int CorePlayStart = RuntimeId.ToRuntimeId("CorePlayStart");

        /// <summary>
        /// 主相机FOV
        /// </summary>
        public static readonly int CameraFOV = RuntimeId.ToRuntimeId("CameraFOV");

        /// <summary>
        /// 摄像机移动到角色
        /// </summary>
        public static readonly int CameraMoveToChar = RuntimeId.ToRuntimeId("CameraMoveToChar");

        /// <summary>
        /// 摄像机自由拖动
        /// </summary>
        public static readonly int CameraMoveFree = RuntimeId.ToRuntimeId("CameraMoveFree");

        /// <summary>
        /// 摄像机状态:飞房子
        /// </summary>
        public static readonly int CameraToHouseFly = RuntimeId.ToRuntimeId("CameraToHouseFly");

        /// <summary>
        /// 相机飞房子状态移动完成
        /// </summary>
        public static readonly int CameraHouseFlyFinish = RuntimeId.ToRuntimeId("CameraHouseFlyFinish");

        /// <summary>
        /// 镜头运动:建筑总览
        /// </summary>
        public static readonly int CameraBuildingOverview = RuntimeId.ToRuntimeId("CameraBuildingOverview");

        /// <summary>
        /// 镜头运动:建筑总览 结束
        /// </summary>
        public static readonly int CameraBuildingOverviewFinish = RuntimeId.ToRuntimeId("CameraBuildingOverviewFinish");

        /// <summary>
        /// 镜头运动:观察特定建筑
        /// </summary>
        public static readonly int CameraBuildingWatch = RuntimeId.ToRuntimeId("CameraBuildingWatch");

        /// <summary>
        /// 镜头运动:观察特定建筑结束
        /// </summary>
        public static readonly int CameraBuildingWatchFinish = RuntimeId.ToRuntimeId("CameraBuildingWatchFinish");


        /// <summary>
        /// 摄像机:攻击总览
        /// </summary>
        public static readonly int CameraAttackOverview = RuntimeId.ToRuntimeId("CameraAttackOverview");

        /// <summary>
        /// 摄像机移动到时空警察
        /// </summary>
        public static readonly int CameraToTimePolice = RuntimeId.ToRuntimeId("CameraToTimePolice");

        /// <summary>
        /// 摄像机移动到看向指定位置为中心的位置
        /// </summary>
        public static readonly int CameraMoveToPos = RuntimeId.ToRuntimeId("CameraMoveToPos");

        /// <summary>
        /// 摄像机移动到看向指定位置为中心的位置 完成
        /// </summary>
        public static readonly int CameraMoveToPosFinish = RuntimeId.ToRuntimeId("CameraMoveToPosFinish");

        /// <summary>
        /// 摄像机移动到阿拉丁
        /// </summary>
        public static readonly int CameraMoveToAladin = RuntimeId.ToRuntimeId("CameraMoveToAladin");

        /// <summary>
        /// 摄像机移动到阿拉丁结束
        /// </summary>
        public static readonly int CameraMoveToAladinFinish = RuntimeId.ToRuntimeId("CameraMoveToAladinFinish");

        /// <summary>
        /// 摄像机移动到护盾
        /// </summary>
        public static readonly int CameraMoveToShield = RuntimeId.ToRuntimeId("CameraMoveToShield");

        /// <summary>
        /// 摄像机移动到护盾结束
        /// </summary>
        public static readonly int CameraMoveToShieldFinish = RuntimeId.ToRuntimeId("CameraMoveToShieldFinish");

        /// <summary>
        /// 摄像机移动到棋子
        /// </summary>
        public static readonly int CameraMoveToToken = RuntimeId.ToRuntimeId("CameraMoveToToken");

        /// <summary>
        /// 摄像机移动到棋子结束
        /// </summary>
        public static readonly int CameraMoveToTokenFinish = RuntimeId.ToRuntimeId("CameraMoveToTokenFinish");

        /// <summary>
        /// 摄像机: 镜头移动到时空门
        /// </summary>
        public static readonly int CameraUnlockPortal = RuntimeId.ToRuntimeId("CameraUnlockPortal");

        /// <summary>
        /// 摄像机:镜头移动到时空门完成
        /// </summary>
        public static readonly int CameraUnlockPortalFinish = RuntimeId.ToRuntimeId("CameraUnlockPortalFinish");

        /// <summary>
        /// 摄像机移动到小游戏位置
        /// </summary>
        public static readonly int CameraToMiniGame = RuntimeId.ToRuntimeId("CameraToMiniGame");

        /// <summary>
        /// 遥控色子镜头
        /// </summary>
        public static readonly int CameraControlDice = RuntimeId.ToRuntimeId("CameraControlDice");

        /// <summary>
        /// 摄像机移动到小游戏位置完成
        /// </summary>
        public static readonly int CameraToMiniGameFinish = RuntimeId.ToRuntimeId("CameraToMiniGameFinish");

        /// <summary>
        /// 摄像机移动到异世界总览
        /// </summary>
        public static readonly int CameraToOtherWorldOverview = RuntimeId.ToRuntimeId("CameraToOtherWorldOverview");

        /// <summary>
        /// 摄像机移动到异世界总览
        /// </summary>
        public static readonly int CameraToOtherWorldOverviewFinish = RuntimeId.ToRuntimeId("CameraToOtherWorldOverviewFinish");

        /// <summary>
        /// 摄像机到传送门三选一镜头
        /// </summary>
        public static readonly int CameraPortalSelect = RuntimeId.ToRuntimeId("CameraPortalSelect");

        /// <summary>
        /// 摄像机动画到传送门三选一镜头结束
        /// </summary>
        public static readonly int CameraPortalSelectFinish = RuntimeId.ToRuntimeId("CameraPortalSelectFinish");

        /// <summary>
        /// 摄像机: 目标居中
        /// </summary>
        public static readonly int CameraAttackAim = RuntimeId.ToRuntimeId("CameraAttackAim");

        /// <summary>
        /// 摄像机: 目标居中 完成
        /// </summary>
        public static readonly int CameraAttackAimFinish = RuntimeId.ToRuntimeId("CameraAttackAimFinish");

        /// <summary>
        /// 摄像机:活动默认相机
        /// </summary>
        public static readonly int CameraActivityDefault = RuntimeId.ToRuntimeId("CameraActivityDefault");


        /// <summary>
        /// 摇色子按钮按下
        /// </summary>
        public static readonly int RollBtnPressed = RuntimeId.ToRuntimeId("RollBtnPressed");

        /// <summary>
        /// 摇色子按钮弹起
        /// </summary>
        public static readonly int RollBtnReleased = RuntimeId.ToRuntimeId("RollBtnReleased");

        /// <summary>
        /// 云加载显示
        /// </summary>
        public static readonly int CloudLoadingShow = RuntimeId.ToRuntimeId("CloudLoadingShow");

        /// <summary>
        /// 云加载隐藏
        /// </summary>
        public static readonly int CloudLoadingHide = RuntimeId.ToRuntimeId("CloudLoadingHide");

        /// <summary>
        /// 云加载动画完成
        /// </summary>
        public static readonly int CloudLoadingReady = RuntimeId.ToRuntimeId("CloudLoadingReady");

        /// <summary>
        /// 云界面关闭时
        /// </summary>
        public static readonly int CloudLoadingClose = RuntimeId.ToRuntimeId("CloudLoadingClose");


        /// <summary>
        /// 黑色加载界面显示
        /// </summary>
        public static readonly int DarkLoadingShow = RuntimeId.ToRuntimeId("DarkLoadingShow");

        /// <summary>
        /// 黑色加载界面隐藏
        /// </summary>
        public static readonly int DarkLoadingHide = RuntimeId.ToRuntimeId("DarkLoadingHide");

        /// <summary>
        /// 黑色加载界面准备加载
        /// </summary>
        public static readonly int DarkLoadingReady = RuntimeId.ToRuntimeId("DarkLoadingReady");

        /// <summary>
        /// 丢色子成功
        /// </summary>
        public static readonly int RollDiceMsgSuccess = RuntimeId.ToRuntimeId("RollDiceMsgSuccess");

        /// <summary>
        /// 丢色子失败
        /// </summary>
        public static readonly int RollDiceMsgFail = RuntimeId.ToRuntimeId("RollDiceMsgFail");

        /// <summary>
        /// 色子加载并且丢色子完成
        /// </summary>
        public static readonly int LoadAndRollDiceComplete = RuntimeId.ToRuntimeId("LoadAndRollDiceComplete");

        /// <summary>
        /// 经过格子的触发事件
        /// </summary>
        public static readonly int TilePassEvent = RuntimeId.ToRuntimeId("TilePassEvent");

        /// <summary>
        /// 终点格子的触发事件
        /// </summary>
        public static readonly int TileEndEvent = RuntimeId.ToRuntimeId("TileEndEvent");

        /// <summary>
        ///  处理上次登录的地块事件
        /// </summary>
        public static readonly int ProcessLastTileEvent = RuntimeId.ToRuntimeId("ProcessLastTileEvent");

        /// <summary>
        /// 建筑总览
        /// </summary>
        public static readonly int BuildingOverview = RuntimeId.ToRuntimeId("BuildingOverview");

        /// <summary>
        /// 观察特定建筑
        /// </summary>
        public static readonly int BuildingWatch = RuntimeId.ToRuntimeId("BuildingWatch");

        /// <summary>
        /// 隐藏建筑功能
        /// </summary>
        public static readonly int BuildingHide = RuntimeId.ToRuntimeId("BuildingHide");

        /// <summary>
        /// 主场景加载完毕
        /// </summary>
        public static readonly int WorldMapLoadComplete = RuntimeId.ToRuntimeId("WorldMapLoadComplete");

        /// <summary>
        /// 射线击中棋格
        /// </summary>
        public static readonly int RaycastHitTile = RuntimeId.ToRuntimeId("RaycastHitTile");

        /// <summary>
        /// 射线击中棋子
        /// </summary>
        public static readonly int RaycastHitToken = RuntimeId.ToRuntimeId("RaycastHitToken");

        /// <summary>
        /// 射线击中阿拉丁
        /// </summary>
        public static readonly int RaycastHitAladin = RuntimeId.ToRuntimeId("RaycastHitAladin");

        /// <summary>
        /// 射线击中行人
        /// </summary>
        public static readonly int RaycastHitWalker = RuntimeId.ToRuntimeId("RaycastHitWalker");

        /// <summary>
        /// 射线击中主世界中的传送门
        /// </summary>
        public static readonly int RaycastHitMainWorldPortal = RuntimeId.ToRuntimeId("RaycastHitMainWorldPortal");

        /// <summary>
        /// 射线击中传送门模型本体
        /// </summary>
        public static readonly int RaycastHitPortal = RuntimeId.ToRuntimeId("RaycastHitPortal");


        /// <summary>
        /// 射线击中异世界中的传送门
        /// </summary>
        public static readonly int RaycastHitOtherWorldPortal = RuntimeId.ToRuntimeId("RaycastHitOtherWorldPortal");

        /// <summary>
        /// 射线击中小游戏
        /// </summary>
        public static readonly int RaycastHitMiniGame = RuntimeId.ToRuntimeId("RaycastHitMiniGame");

        /// <summary>
        /// 射线击中小游戏
        /// </summary>
        public static readonly int RaycastHitAttackHeadEmoji = RuntimeId.ToRuntimeId("RaycastHitAttackHeadEmoji");

        /// <summary>
        /// 进入新场景表现开始(新手,或者过关表现)
        /// </summary>
        public static readonly int EnterNewStageEvent = RuntimeId.ToRuntimeId("EnterNewStageEvent");

        /// <summary>
        /// 进入新场景表现完成(新手,或者过关表现)
        /// </summary>
        public static readonly int EnterNewStageComplete = RuntimeId.ToRuntimeId("EnterNewStageComplete");

        /// <summary>
        /// 切换场景的数据准备完成
        /// </summary>
        public static readonly int SwitchStageDataReady = RuntimeId.ToRuntimeId("SwitchStageDataReady");

        /// <summary>
        /// 切换到下一关场景
        /// </summary>
        public static readonly int SwitchToNextStage = RuntimeId.ToRuntimeId("SwitchToNextStage");

        /// <summary>
        /// 切换到下一关场景 加载完成
        /// </summary>
        public static readonly int SwitchToNextStageLoadComplete = RuntimeId.ToRuntimeId("SwitchToNextStageLoadComplete");

        /// <summary>
        /// 切换到下一关场景 准备进入
        /// </summary>
        public static readonly int SwitchToNextStageReadyEnter = RuntimeId.ToRuntimeId("SwitchToNextStageReadyEnter");

        /// <summary>
        /// 切换到下一关场景 开始进入表现
        /// </summary>
        public static readonly int SwitchToNextStageBeginEnter = RuntimeId.ToRuntimeId("SwitchToNextStageBeginEnter");

        /// <summary>
        /// 切换到下一关场景 显示欢迎界面
        /// </summary>
        public static readonly int SwitchToNextStageShowWelcomeUI = RuntimeId.ToRuntimeId("SwitchToNextStageShowWelcomeUI");

        /// <summary>
        /// 切换到下一关场景 欢迎界面完成
        /// </summary>
        public static readonly int SwitchToNextStageWelcomeUIComplete = RuntimeId.ToRuntimeId("SwitchToNextStageWelcomeUIComplete");

        /// <summary>
        /// 切换到下一关场景 小房子飞入完成
        /// </summary>
        public static readonly int SwitchToNextStageHouseFlyComplete = RuntimeId.ToRuntimeId("SwitchToNextStageHouseFlyComplete");

        /// <summary>
        /// 切换到旧关卡场景
        /// </summary>
        public static readonly int SwitchToOldStage = RuntimeId.ToRuntimeId("SwitchToOldStage");

        /// <summary>
        /// 切换到旧关卡场景 加载完成
        /// </summary>
        public static readonly int SwitchToOldStageLoadComplete = RuntimeId.ToRuntimeId("SwitchToOldStageLoadComplete");

        /// <summary>
        /// 切换到当前关场景
        /// </summary>
        public static readonly int SwitchToCurrentStage = RuntimeId.ToRuntimeId("SwitchToCurrentStage");

        /// <summary>
        /// 切换到当前关场景 加载完成
        /// </summary>
        public static readonly int SwitchToCurrentStageLoadComplete = RuntimeId.ToRuntimeId("SwitchToCurrentStageLoadComplete");

        /// <summary>
        /// 隐藏棋盘
        /// </summary>
        public static readonly int ChessBoardHide = RuntimeId.ToRuntimeId("ChessBoardHide");


        /// <summary>
        /// 通知飞金币到主界面
        /// </summary>
        public static readonly int FlyItemsToHUD = RuntimeId.ToRuntimeId("FlyItemsToHUD");

        #endregion

        #region --- 棋盘事件 ---

        /// <summary>
        /// 当前事件处理完毕
        /// 空闲,没有待处理事件
        /// </summary>
        public static readonly int ChessEventComplete = RuntimeId.ToRuntimeId("ChessEventComplete");

        /// <summary>
        /// 事件的活动奖励表现开始
        /// </summary>
        public static readonly int ChessEventRewardStart = RuntimeId.ToRuntimeId("ChessEventRewardStart");

        /// <summary>
        /// 事件的活动奖励完成
        /// </summary>
        public static readonly int ChessEventRewardComplete = RuntimeId.ToRuntimeId(("ChessEventRewardComplete"));

        /// <summary>
        /// 棋盘悬浮道具表现开始
        /// </summary>
        public static readonly int ChessEventLevitationStart = RuntimeId.ToRuntimeId("ChessEventLevitationStart");

        /// <summary>
        /// 棋盘悬浮道具表现完成
        /// </summary>
        public static readonly int ChessEventLevitationComplete = RuntimeId.ToRuntimeId("ChessEventLevitationComplete");

        /// <summary>
        /// 悬浮道具 传送门 表现完毕
        /// </summary>
        public static readonly int ChessEventFloatRiftComplete = RuntimeId.ToRuntimeId("ChessEventFloatRiftComplete");

        /// <summary>
        /// 活动表现开始
        /// </summary>
        public static readonly int ChessActivityStart = RuntimeId.ToRuntimeId("ChessActivityStart");

        /// <summary>
        /// 活动表现结束
        /// </summary>
        public static readonly int ChessActivityComplete = RuntimeId.ToRuntimeId("ChessActivityComplete");


        //巨额税款界面显示
        public static readonly int ChessEvent_ShowWind_LargeTaxes = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_LargeTaxes");

        //阿拉丁相关界面
        public static readonly int ChessEvent_ShowWind_Aladdin = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_Aladdin");
        public static readonly int ChessEvent_ShowWind_AladdinReady = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinReady");
        public static readonly int ChessEvent_ShowWind_AladdinEnter = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinEnter");
        public static readonly int ChessEvent_ShowWind_AladdinScene = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinScene");
        public static readonly int ChessEvent_HideWind_AladdinScene = RuntimeId.ToRuntimeId("ChessEvent_HideWind_AladdinScene");
        public static readonly int ChessEvent_ShowWind_AladdinResult = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinResult");
        public static readonly int ChessEvent_ShowWind_AladdinFriendReward = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinFriendReward");

        public static readonly int ChessEvent_ShowWind_AladdinSceneStep1 = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinSceneStep1");
        public static readonly int ChessEvent_ShowWind_AladdinSceneStep2 = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinSceneStep2");
        public static readonly int ChessEvent_ShowWind_AladdinLightToIndex = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinLightToIndex");
        public static readonly int ChessEvent_ShowWind_AladdinToNext = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinToNext");
        public static readonly int ChessEvent_ShowWind_AladdinToSelect = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinToSelect");
        public static readonly int ChessEvent_ShowWind_AladdinSelected = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinSelected");
        public static readonly int ChessEvent_ShowWind_AladdinAlphaHide = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_AladdinAlphaHide");

        public static readonly int ChessEvent_ShowWind_IsFinish = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_IsFinish");
        public static readonly int ChessEvent_ShowWind_ShieldGet = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_ShieldGet");


        /// <summary>
        /// 显示/隐藏 疯狂租金面板
        /// </summary>
        public static readonly int ShowCrazyRentEventPanel = RuntimeId.ToRuntimeId("ShowCrazyRentEventPanel");

        public static readonly int HideCrazyRentEventPanel = RuntimeId.ToRuntimeId("HideCrazyRentEventPanel");

        /// <summary>
        /// 显示/隐藏 颜色转盘面板
        /// </summary>
        public static readonly int ShowColorTurntableEventPanel = RuntimeId.ToRuntimeId("ShowColorTurntableEventPanel");

        public static readonly int HideColorTurntableEventPanel = RuntimeId.ToRuntimeId("HideColorTurntableEventPanel");

        //异世界
        public static readonly int ChessEvent_ShowWind_DifferentDimensionalSpacePanel = RuntimeId.ToRuntimeId("ChessEvent_ShowWind_DifferentDimensionalSpacePanel");


        /// <summary>
        /// 显示/隐藏 机会面板
        /// </summary>
        public static readonly int ShowChanceEventPanel = RuntimeId.ToRuntimeId("ShowChanceEventPanel");

        public static readonly int HideChanceEventPanel = RuntimeId.ToRuntimeId("HideChanceEventPanel");

        /// <summary>
        /// 显示/隐藏 打气球小游戏面板
        /// </summary>
        public static readonly int ShowMiniGameBalloonFightPanel = RuntimeId.ToRuntimeId("ShowMiniGameBalloonFightPanel");

        public static readonly int HideMiniGameBalloonFightPanel = RuntimeId.ToRuntimeId("HideMiniGameBalloonFightPanel");

        /// <summary>
        /// 显示/隐藏 抛金币小游戏面板
        /// </summary>
        public static readonly int ShowMiniGameFlyMoneyPanel = RuntimeId.ToRuntimeId("ShowMiniGameFlyMoneyPanel");

        public static readonly int HideMiniGameFlyMoneyPanel = RuntimeId.ToRuntimeId("HideMiniGameFlyMoneyPanel");


        /// <summary>
        /// 显示/隐藏 监狱扔骰子面板
        /// </summary>
        public static readonly int ShowPrisonEventPanel = RuntimeId.ToRuntimeId("ShowPrisonEventPanel");

        public static readonly int HidePrisonEventPanel = RuntimeId.ToRuntimeId("HidePrisonEventPanel");
        public static readonly int PlayPrisonOpenMovie = RuntimeId.ToRuntimeId("PlayPrisonOpenMovie");
        public static readonly int PlayPrisonCloseMovie = RuntimeId.ToRuntimeId("PlayPrisonCloseMovie");


        /// <summary>
        /// 显示/隐藏 监狱扔骰子面板
        /// </summary>
        public static readonly int ShowBreakDownEventPanel = RuntimeId.ToRuntimeId("ShowBreakDownEventPanel");

        public static readonly int HideBreakDownEventPanel = RuntimeId.ToRuntimeId("HideBreakDownEventPanel");


        /// <summary>
        /// 转盘结束（需要停止表现及展示结果）
        /// </summary>
        public static readonly int BoardWheelComplete = RuntimeId.ToRuntimeId("BoardWheelComplete");

        /// <summary>
        /// 刷新小房子列表的信息
        /// </summary>
        public static readonly int SyncBuildPieceList = RuntimeId.ToRuntimeId("SyncBuildPieceList");

        /// <summary>
        /// 播放小房子飞到台面的动画
        /// </summary>
        public static readonly int PlayHousePieceMovie = RuntimeId.ToRuntimeId("PlayHousePieceMovie");

        /// <summary>
        /// 监狱扔骰子事件结束
        /// </summary>
        public static readonly int PrisonEventRollComplete = RuntimeId.ToRuntimeId("PrisonEventRollComplete");

        /// <summary>
        /// 破坏事件UI准备完成
        /// </summary>
        public static readonly int ChessEventAttackDataReady = RuntimeId.ToRuntimeId("ChessEventAttackUIReady");

        /// <summary>
        /// 破坏事件-选择目标
        /// </summary>
        public static readonly int ChessEventAttackSelectTarget = RuntimeId.ToRuntimeId("ChessEventAttackSelectTarget");

        /// <summary>
        /// 破坏事件,加载进攻场景
        /// </summary>
        public static readonly int ChessEventAttackLoadAtkScene = RuntimeId.ToRuntimeId("ChessEventAttackLoadAtkScene");

        /// <summary>
        /// 破坏事件,进攻场景准备完成
        /// </summary>
        public static readonly int ChessEventLoadAtttackSceneComplete = RuntimeId.ToRuntimeId("ChessEventLoadAtttackSceneComplete");

        /// <summary>
        /// 破坏事件,加载玩家场景
        /// </summary>
        public static readonly int ChessEventAttackLoadPlayerScene = RuntimeId.ToRuntimeId("ChessEventAttackLoadPlayerScene");

        /// <summary>
        /// 破坏事件,加载玩家场景完成
        /// </summary>
        public static readonly int ChessEventAttackLoadPlayerSceneComplete = RuntimeId.ToRuntimeId("ChessEventAttackLoadPlayerSceneComplete");


        /// <summary>
        /// 破坏事件-直升机上升结束
        /// </summary>
        public static readonly int ChessEventAttackFlyUpFinish = RuntimeId.ToRuntimeId("ChessEventAttackFlyUpFinish");

        /// <summary>
        /// 破坏事件完成
        /// </summary>
        public static readonly int ChessEventAttackFinish = RuntimeId.ToRuntimeId("ChessEventAttackFinish");

        /// <summary>
        /// 破坏事件-飞弹发射
        /// </summary>
        public static readonly int ChessEventAttackMissleLaunch = RuntimeId.ToRuntimeId("ChessEventAttackMissleLaunch");

        /// <summary>
        /// 破坏事件-飞弹命中
        /// </summary>
        public static readonly int ChessEventAttackMissleHit = RuntimeId.ToRuntimeId("ChessEventAttackMissleHit");

        /// <summary>
        /// 破坏事件-飞弹阻挡
        /// </summary>
        public static readonly int ChessEventAttackMissleBlock = RuntimeId.ToRuntimeId("ChessEventAttackMissleBlock");


        /// <summary>
        /// 阿拉丁事件 加载场景
        /// </summary>
        public static readonly int AladinLoadScene = RuntimeId.ToRuntimeId("AladinLoadScene");

        /// <summary>
        /// 阿拉丁加载界面 显示加载界面
        /// </summary>
        public static readonly int AladinLoadingShow = RuntimeId.ToRuntimeId("AladinLoadingShow");

        /// <summary>
        /// 阿拉丁加载界面 准备完成
        /// </summary>
        public static readonly int AladinLoadingReady = RuntimeId.ToRuntimeId("AladinLoadingReady");

        /// <summary>
        /// 阿拉丁加载界面 隐藏
        /// </summary>
        public static readonly int AladinLoadingHide = RuntimeId.ToRuntimeId("AladinLoadingHide");


        /// <summary>
        /// 阿拉丁金币飞向神灯结束
        /// </summary>
        public static readonly int ChessEventAladinMoneyFlyFinish = RuntimeId.ToRuntimeId("ChessEventAladinMoneyFlyFinish");

        /// <summary>
        /// 阿拉丁事件 翻牌摄像机运动完成
        /// </summary>
        public static readonly int AladinFanpaiCameraFinish = RuntimeId.ToRuntimeId("AladinFanpaiCameraFinish");

        /// <summary>
        /// 阿拉丁事件 选中玩家头像
        /// </summary>
        public static readonly int AladinRaycastHitFaceIcon = RuntimeId.ToRuntimeId("AladinRaycastHitFaceIcon");

        /// <summary>
        /// 阿拉丁事件 加载场景完成
        /// </summary>
        public static readonly int AladinLoadSceneComplete = RuntimeId.ToRuntimeId("AladinLoadSceneComplete");

        /// <summary>
        /// 阿拉丁事件 结束
        /// </summary>
        public static readonly int AladinFinish = RuntimeId.ToRuntimeId("AladinFinish");

        /// <summary>
        /// 时空警察事件
        /// </summary>
        public static readonly int TimePoliceEvent = RuntimeId.ToRuntimeId("TimePoliceEvent");

        /// <summary>
        /// 时空警察事件回合开始
        /// </summary>
        public static readonly int TimePoliceEventRoundStart = RuntimeId.ToRuntimeId("TimePoliceEventRoundStart");

        /// <summary>
        /// 时空警察Hud显示
        /// </summary>
        public static readonly int TimePoliceShowHud = RuntimeId.ToRuntimeId("TimePoliceHudShow");

        /// <summary>
        /// 时空警察进入UI展示
        /// </summary>
        public static readonly int TimePoliceShowEnterUI = RuntimeId.ToRuntimeId("TimePoliceShowEnterUI");

        /// <summary>
        /// 时空警察结算UI展示
        /// </summary>
        public static readonly int TimePoliceShowResultUI = RuntimeId.ToRuntimeId("TimePoliceShowResultUI");

        /// <summary>
        /// 时空警察事件结算UI关闭
        /// </summary>
        public static readonly int TimePoliceEventResultUIClose = RuntimeId.ToRuntimeId("TimePoliceEventResultUIClose");

        /// <summary>
        /// 时空警察事件回合完成
        /// </summary>
        public static readonly int TimePoliceEventRoundComplete = RuntimeId.ToRuntimeId("TimePoliceEventRoundComplete");

        /// <summary>
        /// 时空警察讲话
        /// </summary>
        public static readonly int TimePoliceDialog = RuntimeId.ToRuntimeId("TimePoliceDialog");

        #endregion

        #region --- 活动相关 ---

        /// <summary>
        /// 显示七日签到的奖励
        /// </summary>
        public static readonly int SevenDaySignInComplete = RuntimeId.ToRuntimeId("SevenDaySignInComplete");

        /// <summary>
        /// 七日签到更新数据
        /// </summary>
        public static readonly int SevenDaySyncData = RuntimeId.ToRuntimeId("SevenDaySyncData");

        /// <summary>
        /// 积分活动获得积分动画播放
        /// </summary>
        public static readonly int PlayScoreActivityGetItems = RuntimeId.ToRuntimeId("PlayScoreActivityGetItems");

        /// <summary>
        /// 触发棋盘积分活动悬浮物
        /// </summary>
        public static readonly int ChessEventStateScoreActivityItemTouch = RuntimeId.ToRuntimeId("ChessEventStateScoreActivityItemTouch");
        /// <summary>
        /// 触发棋盘周活动悬浮物
        /// </summary>
        public static readonly int ChessEventStateWeekActivityItemTouch = RuntimeId.ToRuntimeId("ChessEventStateWeekActivityItemTouch");



        /// <summary>
        /// 场景任务获得积分动画播放
        /// </summary>
        public static readonly int PlaySceneTaskGetItems = RuntimeId.ToRuntimeId("PlaySceneTaskGetItems");

        /// <summary>
        /// 更新积分活动数据
        /// </summary>
        public static readonly int SyncScoreActivityInfo = RuntimeId.ToRuntimeId("SyncScoreActivityInfo");

        /// <summary>
        /// 更新场景任务活动数据
        /// </summary>
        public static readonly int SyncSceneTaskInfo = RuntimeId.ToRuntimeId("SyncSceneTaskInfo");

        #endregion

        #region --- 小游戏 ---

        /// <summary>
        /// 小游戏开始
        /// </summary>
        public static readonly int ChessMiniGameStart = RuntimeId.ToRuntimeId("ChessMiniGameStart");

        /// <summary>
        /// 显示小游戏界面
        /// </summary>
        public static readonly int ChessMiniGameShowUI = RuntimeId.ToRuntimeId("ChessMiniGameShowUI");

        /// <summary>
        /// 显示小游戏奖励界面
        /// </summary>
        public static readonly int ChessMiniGameShowReward = RuntimeId.ToRuntimeId("ChessMiniGameShowReward");

        /// <summary>
        /// 小游戏奖励表现显示完成
        /// </summary>
        public static readonly int ChessMiniGameShowRewardFinish = RuntimeId.ToRuntimeId("ChessMiniGameShowRewardFinish");

        /// <summary>
        /// 小游戏全结束
        /// </summary>
        public static readonly int ChessMiniGameAllFinish = RuntimeId.ToRuntimeId("ChessMiniGameAllFinish");

        /// <summary>
        /// 小游戏全结束
        /// </summary>
        public static readonly int ChessMiniGameResult = RuntimeId.ToRuntimeId("ChessMiniGameResult");


        /// <summary>
        /// 三选一小游戏选择表现
        /// </summary>
        public static readonly int ChessMiniGameChoiceGameShowSelect = RuntimeId.ToRuntimeId("ChessMiniGameChoiceGameShowSelect");

        #endregion

        #region --- 棋盘报告 ---

        public static readonly int Client_EventReport_PushNew = RuntimeId.ToRuntimeId("Client_EventReport_PushNew");
        public static readonly int ShowReportPanel = RuntimeId.ToRuntimeId("ShowReportPanel");

        #endregion

        #region --- 表情相关 ---

        public static readonly int ShowEmojiPanel = RuntimeId.ToRuntimeId("ShowEmojiPanel");
        public static readonly int EmojiSelectCallBack = RuntimeId.ToRuntimeId("EmojiSelectCallBack");

        #endregion

        #region --- 活动相关 ---

        public static readonly int ShowLimitTimePanel = RuntimeId.ToRuntimeId("ShowLimitTimePanel");
        public static readonly int ShowRankActivityPanel = RuntimeId.ToRuntimeId("ShowRankActivityPanel");
        public static readonly int RankActivityProgressAction = RuntimeId.ToRuntimeId("RankActivityProgressAction");
        public static readonly int RankActivityProgressActionNext = RuntimeId.ToRuntimeId("RankActivityProgressActionNext");
        public static readonly int ComRankActivityProgressEnd = RuntimeId.ToRuntimeId("ComRankActivityProgressEnd");

        public static readonly int ShowWeekActivityPanel = RuntimeId.ToRuntimeId("ShowWeekActivityPanel");
        public static readonly int ShowDesertTreasurePanel = RuntimeId.ToRuntimeId("ShowDesertTreasurePanel");
        public static readonly int HideDesertTreasurePanel = RuntimeId.ToRuntimeId("HideDesertTreasurePanel");
        public static readonly int NextDesertTreasurePanel = RuntimeId.ToRuntimeId("NextDesertTreasurePanel");
        public static readonly int SendQuitDesertTreasure = RuntimeId.ToRuntimeId("SendQuitDesertTreasure");

        public static readonly int ShowDesertTreasureDig = RuntimeId.ToRuntimeId("ShowDesertTreasureDig");
        public static readonly int ShowDesertTreasureReward = RuntimeId.ToRuntimeId("ShowDesertTreasureReward");
        public static readonly int ShowDesertTreasureRewardAll = RuntimeId.ToRuntimeId("ShowDesertTreasureReward");
        public static readonly int ShowDesertTreasureRewardFinish = RuntimeId.ToRuntimeId("ShowDesertTreasureRewardFinish");



        public static readonly int ShowStoreGiftActivityPanel = RuntimeId.ToRuntimeId("ShowStoreGiftActivityPanel");


        /// <summary>
        /// 沙漠宝藏挖宝道具不足
        /// </summary>
        public static readonly int ShowDesertTreasureItemNotEnough = RuntimeId.ToRuntimeId("ShowDesertTreasureItemNotEnough");

        /// <summary>
        /// 周活动悬浮物
        /// </summary>
        public static readonly int WeekActivityItemTouch = RuntimeId.ToRuntimeId("WeekActivityItemTouch");




        public static readonly int DiceMaxLimitUp_Add = RuntimeId.ToRuntimeId("DiceMaxLimitUp_Add");
        public static readonly int DiceMaxLimitUp_Def = RuntimeId.ToRuntimeId("DiceMaxLimitUp_Def");

        // public static readonly int CrazeRentHead_Add = RuntimeId.ToRuntimeId("CrazeRentHead_Add");

        public static readonly int ActivityPushAddChessItem = RuntimeId.ToRuntimeId("ActivityPushAddChessItem");



        public static readonly int BreakTime_Add = RuntimeId.ToRuntimeId("BreakTime_Add");


        public static readonly int NewActivityGetAction = RuntimeId.ToRuntimeId("NewActivityGetAction");

        public static readonly int ActivityActionNewChange = RuntimeId.ToRuntimeId("ActivityActionNewChange");
        public static readonly int ActivityShowDoubleMoneyAction = RuntimeId.ToRuntimeId("ActivityShowDoubleMoneyAction");


        public static readonly int ActivityEntityDel = RuntimeId.ToRuntimeId("ActivityEntityDel");

        /// <summary>
        /// 通知开始超级打劫活动
        /// </summary>
        public static readonly int SuperRobberyStart = RuntimeId.ToRuntimeId("SuperRobberyStart");


        /// <summary>
        /// 活动表现完成
        /// </summary>
        public static readonly int ShowActivityComplete = RuntimeId.ToRuntimeId("ShowActivityComplete");

        /// <summary>
        /// 活动返回主世界事件
        /// </summary>
        public static readonly int ActivityBackToMainWorld = RuntimeId.ToRuntimeId("ActivityBackToMainWorld");

        /// <summary>
        /// 活动返回异世界事件
        /// </summary>
        public static readonly int ActivityBackToOtherWorld = RuntimeId.ToRuntimeId("ActivityBackToOtherWorld");

        /// <summary>
        /// 周活动 沙漠宝藏
        /// </summary>
        public static readonly int ShowWeeklyActivityShamobaozang = RuntimeId.ToRuntimeId("ShowWeeklyActivityShamobaozang");

        /// <summary>
        /// ActivityStateIdle 切换状态通知
        /// </summary>
        public static readonly int ShowNormalActivity = RuntimeId.ToRuntimeId("ShowNormalActivity");


        /// <summary>
        /// 更新积分活动数据
        /// </summary>
        public static readonly int UpdateScoreActivityData = RuntimeId.ToRuntimeId("UpdateScoreActivityData");

        /// <summary>
        /// 积分活动关闭
        /// </summary>
        public static readonly int ScoreActivityClose = RuntimeId.ToRuntimeId("ScoreActivityClose");

        /// <summary>
        /// 显示奖励展示界面
        /// </summary>
        public static readonly int ShowActivityBigRewardPanel = RuntimeId.ToRuntimeId("ShowActivityBigRewardPanel");

        /// <summary>
        /// 更新异世界场景任务的数据
        /// </summary>
        public static readonly int UpdateSceneTaskData = RuntimeId.ToRuntimeId("UpdateSceneTaskData");

        #endregion

        #region --- 异世界 事件 ---

        /// <summary>
        /// 显示随机宝箱面板
        /// </summary>
        public static readonly int ShowRandomBoxPanel = RuntimeId.ToRuntimeId("ShowRandomBoxPanel");

        /// <summary>
        /// 显示神灵界面表现
        /// </summary>
        public static readonly int ShowFloatGodEventPanel = RuntimeId.ToRuntimeId("ShowFloatGodEventPanel");

        public static readonly int CloseFloatGodEventPanel = RuntimeId.ToRuntimeId("CloseFloatGodEventPanel");
        public static readonly int TouchFloatGod = RuntimeId.ToRuntimeId("TouchFloatGod");
        public static readonly int DestroyFloatGod = RuntimeId.ToRuntimeId("DestroyFloatGod");

        /// <summary>
        /// 神灵展示界面关闭时触发
        /// </summary>
        public static readonly int CloseFloatGodEventUI = RuntimeId.ToRuntimeId("ClseFloatGodEventUI");


        public static readonly int ShowFloatGodRewardPanel = RuntimeId.ToRuntimeId("ShowFloatGodRewardPanel");


        public static readonly int CloseRandomBoxPanel = RuntimeId.ToRuntimeId("CloseRandomBoxPanel");
        public static readonly int DestroyRandomBox = RuntimeId.ToRuntimeId("DestroyRandomBox");



        /// <summary>
        /// 显示万能胶片入口
        /// </summary>
        public static readonly int ShowAllPowerfulFilmEnterPanel = RuntimeId.ToRuntimeId("ShowAllPowerfulFilmEnterPanel");

        /// <summary>
        /// 显示万能胶片选择
        /// </summary>
        public static readonly int ShowAllPowerfulFilmPanel = RuntimeId.ToRuntimeId("ShowAllPowerfulFilmPanel");

        /// <summary>
        /// 显示万能胶片确认兑换
        /// </summary>
        public static readonly int ShowAllPowerfulFilmConfirmPanel = RuntimeId.ToRuntimeId("ShowAllPowerfulFilmConfirmPanel");

        /// <summary>
        /// 显示万能胶片奖励展示（卡片）
        /// </summary>
        public static readonly int ShowTComGetAloneCardPanel = RuntimeId.ToRuntimeId("ShowTComGetAloneCardPanel");

        /// <summary>
        /// 显示彩票屋
        /// </summary>
        public static readonly int ShowLotteryHouseEventPanel = RuntimeId.ToRuntimeId("ShowLotteryHouseEventPanel");

        /// <summary>
        /// 关闭彩票屋
        /// </summary>
        public static readonly int CloseLotteryHouseEventPanel = RuntimeId.ToRuntimeId("CloseLotteryHouseEventPanel");


        /// <summary>
        /// 显示彩票屋开奖界面
        /// </summary>
        public static readonly int ShowLotteryHouseResultPanel = RuntimeId.ToRuntimeId("ShowLotteryHouseResultPanel");

        /// <summary>
        /// 显示异世界转盘面板
        /// </summary>
        public static readonly int ShowWorldTurntablePanel = RuntimeId.ToRuntimeId("ShowWorldTurntablePanel");

        /// <summary>
        /// 显示异世界机会卡面板
        /// </summary>
        public static readonly int ShowWorldChanceEventPanel = RuntimeId.ToRuntimeId("ShowWorldChanceEventPanel");

        /// <summary>
        /// 异世界警察对话面板关闭
        /// </summary>
        public static readonly int WorldPrisonEventPanelClose = RuntimeId.ToRuntimeId("WorldPrisonEventPanelClose");

        /// <summary>
        /// 显示异世界结算界面
        /// </summary>
        public static readonly int ShowOtherWorldSettleAccountsPanel = RuntimeId.ToRuntimeId("ShowOtherWorldSettleAccountsPanel");

        /// <summary>
        /// 显示或隐藏时空之主界面
        /// </summary>
        public static readonly int ShowTimeMasterPanel = RuntimeId.ToRuntimeId("ShowTimeMasterPanel");

        public static readonly int HideTimeMasterPanel = RuntimeId.ToRuntimeId("HideTimeMasterPanel");

        /// <summary>
        /// 时空之主单回合结算
        /// </summary>
        public static readonly int WorldTimeMasterRoundResult = RuntimeId.ToRuntimeId("WorldTimeMasterRoundResult");

        /// <summary>
        /// 时空之主领取当前的回合奖励
        /// </summary>
        public static readonly int WorldTimeMasterGetReward = RuntimeId.ToRuntimeId("WorldTimeMasterGetReward");

        /// <summary>
        /// 更新异世界排名信息
        /// </summary>
        public static readonly int UpdateWorldRankList = RuntimeId.ToRuntimeId("UpdateWorldRankList");

        /// <summary>
        /// 更新悬浮BUFF列表
        /// </summary>
        public static readonly int SyncFloatBuffList = RuntimeId.ToRuntimeId("SyncFloatBuffList");

        /// <summary>
        /// 显示悬浮道具-天气 表现面板
        /// </summary>
        public static readonly int ShowFloatWeatherPanel = RuntimeId.ToRuntimeId("ShowFloatWeatherPanel");

        public static readonly int HideFloatWeatherPanel = RuntimeId.ToRuntimeId("HideFloatWeatherPanel");

        /// <summary>
        /// 显示隐藏遥控骰子表现面板
        /// </summary>
        public static readonly int ShowFloatDiceControlPanel = RuntimeId.ToRuntimeId("ShowFloatDiceControlPanel");

        public static readonly int HideFloatDiceControlPanel = RuntimeId.ToRuntimeId("HideFloatDiceControlPanel");

        /// <summary>
        /// 遥控色子成功
        /// </summary>
        public static readonly int ControlDiceRollSuccess = RuntimeId.ToRuntimeId("ControlDiceRollSuccess");

        #endregion

        #region --- 异世界 传送门 ---

        /// <summary>
        /// 进入传送门去异世界请求通过
        /// </summary>
        public static readonly int PortalToOtherWorldRequestPass = RuntimeId.ToRuntimeId("PortalToOtherWorldRequestPass");

        /// <summary>
        /// 进入传送门去异世界请求未通过
        /// </summary>
        public static readonly int PortalToOtherWorldRequestError = RuntimeId.ToRuntimeId("PortalToOtherWorldRequestError");


        /// <summary>
        /// 玩家主动退出异世界
        /// </summary>
        public static readonly int PlayerWantQuitOtherWorld = RuntimeId.ToRuntimeId("PlayerWantQuitOtherWorld");

        /// <summary>
        /// 传送门选择阶段
        /// </summary>
        public static readonly int PortalShowSelect = RuntimeId.ToRuntimeId("PortalShowSelect");

        /// <summary>
        /// 退出传送门选择
        /// </summary>
        public static readonly int PortalSelectQuit = RuntimeId.ToRuntimeId("PortalSelectQuit");

        /// <summary>
        /// 显示传送门选择界面
        /// </summary>
        public static readonly int PortalSelectUIShow = RuntimeId.ToRuntimeId("PortalSelectUIShow");

        /// <summary>
        /// 隐藏传送门选择界面
        /// </summary>
        public static readonly int PortalSelectUIClose = RuntimeId.ToRuntimeId("PortalSelectUIClose");

        /// <summary>
        /// 传送门去主世界
        /// </summary>
        public static readonly int PortalToMainWorld = RuntimeId.ToRuntimeId("PortalToMainWorld");

        /// <summary>
        /// 进入传送门回主世界请求通过
        /// </summary>
        public static readonly int PortalToMainWorldRequestPass = RuntimeId.ToRuntimeId("PortalToMainWorldRequestPass");

        /// <summary>
        /// 显示传送门的加载界面
        /// </summary>
        public static readonly int ShowPortalLoading = RuntimeId.ToRuntimeId("ShowPortalLoading");

        /// <summary>
        /// 显示主题传送门界面
        /// </summary>
        public static readonly int ShowThemePanel = RuntimeId.ToRuntimeId("ShowThemePanel");

        /// <summary>
        /// 显示主题信息界面
        /// </summary>
        public static readonly int ShowThemeInfoPanel = RuntimeId.ToRuntimeId("ShowThemeInfoPanel");

        /// <summary>
        /// 关闭主题信息界面
        /// </summary>
        public static readonly int CloseThemeInfoPanel = RuntimeId.ToRuntimeId("CloseThemeInfoPanel");

        /// <summary>
        /// 显示时空隧道加载界面
        /// </summary>
        public static readonly int ShowPortalLoading2 = RuntimeId.ToRuntimeId("ShowPortalLoading2");


        /// <summary>
        /// 显示异次元转场loading界面
        /// </summary>
        public static readonly int ShowOtherWorldLoading = RuntimeId.ToRuntimeId("ShowOtherWorldLoading");

        /// <summary>
        /// 关闭异次元转场loading界面
        /// </summary>
        public static readonly int CloseOtherWorldLoading = RuntimeId.ToRuntimeId("CloseOtherWorldLoading");

        /// <summary>
        /// 异次元转场loading界面动效播放完毕
        /// </summary>
        public static readonly int WaitingOtherWorldLoading = RuntimeId.ToRuntimeId("WaitingOtherWorldLoading");

        /// <summary>
        /// 异次元转场loading界面更新进度
        /// </summary>
        public static readonly int UpdataOtherWorldLoading = RuntimeId.ToRuntimeId("UpdataOtherWorldLoading");



        /// <summary>
        /// 传送门加载界面准备加载
        /// </summary>
        public static readonly int PortalLoadingReady = RuntimeId.ToRuntimeId("PortalLoadingReady");

        /// <summary>
        /// 时空隧道加载界面准备加载
        /// </summary>
        public static readonly int PortalLoadingReady2 = RuntimeId.ToRuntimeId("PortalLoadingReady2");

        /// <summary>
        /// 关闭传送门加载界面
        /// </summary>
        public static readonly int ClosePortalLoading = RuntimeId.ToRuntimeId("ClosePortalLoading");

        /// <summary>
        /// 关闭时空隧道加载界面
        /// </summary>
        public static readonly int ClosePortalLoading2 = RuntimeId.ToRuntimeId("ClosePortalLoading2");

        /// <summary>
        /// 传送门数据OK
        /// </summary>
        public static readonly int PortalDataReady = RuntimeId.ToRuntimeId("PortalDataReady");

        /// <summary>
        /// 选择传送门
        /// </summary>
        public static readonly int PortalSelect = RuntimeId.ToRuntimeId("PortalSelect");


        /// <summary>
        /// 进入传送睡到
        /// </summary>
        public static readonly int PortalEnterTunnel = RuntimeId.ToRuntimeId("PortalEnterTunnel");

        /// <summary>
        /// 更新传送门阶段
        /// </summary>
        public static readonly int UpdatePortalPhase = RuntimeId.ToRuntimeId("UpdatePortalPhase");


        /// <summary>
        /// 加载异世界场景完成
        /// </summary>
        public static readonly int LoadOtherWorldSceneComplete = RuntimeId.ToRuntimeId("LoadOtherWorldSceneComplete");

        /// <summary>
        /// 加载主世界场景
        /// </summary>
        public static readonly int LoadMainWorldScene = RuntimeId.ToRuntimeId("LoadMainWorldScene");

        /// <summary>
        /// 加载主世界场景完成
        /// </summary>
        public static readonly int LoadMainWorldSceneComplete = RuntimeId.ToRuntimeId("LoadMainWorldSceneComplete");

        #endregion

        #region --- 服务端回调 ---

        public static readonly int OnSyncGameSyncHelicopterdestroy = RuntimeId.ToRuntimeId("OnSyncGameSyncHelicopterdestroy");
        public static readonly int OnSyncGameSyncBehelicopterdestroy = RuntimeId.ToRuntimeId("OnSyncGameSyncBehelicopterdestroy");
        public static readonly int OnSyncGameHelicopterRevenges = RuntimeId.ToRuntimeId("OnSyncGameHelicopterRevenges");
        public static readonly int OnSyncSocialSyncNotify = RuntimeId.ToRuntimeId("OnSyncSocialSyncNotify");

        public static readonly int OnResp_SocialFriendFriends = RuntimeId.ToRuntimeId("OnResp_SocialFriendFriends");
        public static readonly int OnResp_SocialFriendRecommend = RuntimeId.ToRuntimeId("OnResp_SocialFriendRecommend");
        public static readonly int OnResp_SocialFriendAdd = RuntimeId.ToRuntimeId("OnResp_SocialFriendAdd");
        public static readonly int OnResp_SocialFriendHandle = RuntimeId.ToRuntimeId("OnResp_SocialFriendHandle");
        public static readonly int OnResp_SocialFriendRemove = RuntimeId.ToRuntimeId("OnResp_SocialFriendRemove");

        public static readonly int OnResp_GameAladdinData = RuntimeId.ToRuntimeId("OnResp_GameAladdinData");
        public static readonly int OnResp_GameAladdinStart = RuntimeId.ToRuntimeId("OnResp_GameAladdinStart");
        public static readonly int OnResp_GameAladdinTest = RuntimeId.ToRuntimeId("OnResp_GameAladdinTest");
        public static readonly int OnResp_GameAladdinManual = RuntimeId.ToRuntimeId("OnResp_GameAladdinManual");

        public static readonly int OnRespUpgradeBuild = RuntimeId.ToRuntimeId("OnRespUpgradeBuild");
        public static readonly int OnRespCreateBuild = RuntimeId.ToRuntimeId("OnRespCreateBuild");
        public static readonly int OnRespFixBuild = RuntimeId.ToRuntimeId("OnRespFixBuild");


        public static readonly int OnGameRoleRolename = RuntimeId.ToRuntimeId("OnGameRoleRolename");
        public static readonly int OnGameRoleRoleavatar = RuntimeId.ToRuntimeId("OnGameRoleRoleavatar");
        public static readonly int OnGameApiFilterWord = RuntimeId.ToRuntimeId("OnGameApiFilterWord");

        public static readonly int OnResp_GameRoleTimelineemoji = RuntimeId.ToRuntimeId("OnResp_GameRoleTimelineemoji");
        public static readonly int OnResp_GameRoleReporterreward = RuntimeId.ToRuntimeId("OnResp_GameRoleReporterreward");
        public static readonly int OnSyncGameSyncReporter = RuntimeId.ToRuntimeId("OnSyncGameSyncReporter");
        public static readonly int OnRespAlbumReporterReward = RuntimeId.ToRuntimeId("OnRespAlbumReporterReward");

        public static readonly int OnRespShowRoomEquip = RuntimeId.ToRuntimeId("OnRespShowRoomEquip");

        public static readonly int OnSyncEventActivityPush = RuntimeId.ToRuntimeId("OnSyncEventActivityPush");
        public static readonly int OnSyncActivityClosePush = RuntimeId.ToRuntimeId("OnSyncActivityClosePush");
        public static readonly int OnSyncActivityClosePush_CrazyRent = RuntimeId.ToRuntimeId("OnSyncActivityClosePush_CrazyRent");
        public static readonly int OnSyncActivityClosePush_Score = RuntimeId.ToRuntimeId("OnSyncActivityClosePush_Score");
        public static readonly int OnSyncActivityClosePush_Week = RuntimeId.ToRuntimeId("OnSyncActivityClosePush_Week");


        public static readonly int OnGiftPackActivityPush = RuntimeId.ToRuntimeId("OnGiftPackActivityPush");
        public static readonly int OnGameApiGiftPackBuy = RuntimeId.ToRuntimeId("OnGameApiGiftPackBuy");

        public static readonly int OnRespGameApiShopPanel = RuntimeId.ToRuntimeId("OnRespGameApiShopPanel");
        public static readonly int OnRespGameApiShopBuy = RuntimeId.ToRuntimeId("OnRespGameApiShopBuy");

        public static readonly int OnEventActivityRewardPush = RuntimeId.ToRuntimeId("OnEventActivityRewardPush");
        public static readonly int OnRankActivityPush = RuntimeId.ToRuntimeId("OnRankActivityPush");
        public static readonly int OnGameApiRankActivityPanel = RuntimeId.ToRuntimeId("OnGameApiRankActivityPanel");
        public static readonly int OnGameApiweeklyActivityDig = RuntimeId.ToRuntimeId("OnGameApiweeklyActivityDig");

        public static readonly int OnResp_FriendRank = RuntimeId.ToRuntimeId("OnResp_FriendRank");

        public static readonly int OnResp_BreakTimeAdd = RuntimeId.ToRuntimeId("OnResp_BreakTimeAdd");

        public static readonly int OnResp_ReqBagStickerExchange = RuntimeId.ToRuntimeId("OnResp_ReqBagStickerExchange");

        public static readonly int OnWeekActivityPush = RuntimeId.ToRuntimeId("OnWeekActivityPush");
        public static readonly int OnNextDayZeroRefreshTimePush = RuntimeId.ToRuntimeId("OnNextDayZeroRefreshTimePush");

        public static readonly int OnRespLotteryBuy = RuntimeId.ToRuntimeId("OnRespLotteryBuy");
        public static readonly int OnRespLotteryBuyError = RuntimeId.ToRuntimeId("OnRespLotteryBuyError");
        public static readonly int OnGameLotteryData = RuntimeId.ToRuntimeId("OnGameLotteryData");






        #endregion

        #region --- 任务相关 ---

        /// <summary>
        /// 更新每日任务
        /// </summary>
        public static readonly int UpdateDailyTaskInfo = RuntimeId.ToRuntimeId("UpdateDailyTaskInfo");

        #endregion

        #region --- 银行打劫相关 ---

        /// <summary>
        /// 显示银行打劫主界面
        /// </summary>
        public static readonly int ShowBankRobberyEventPanel = RuntimeId.ToRuntimeId("ShowBankRobberyEventPanel");

        /// <summary>
        /// 隐藏银行打劫主界面
        /// </summary>
        public static readonly int HideBankRobberyEventPanel = RuntimeId.ToRuntimeId("HideBankRobberyEventPanel");

        /// <summary>
        /// 开始加载银行打劫场景
        /// </summary>
        public static readonly int StartLoadRobberyScene = RuntimeId.ToRuntimeId("StartLoadRobberyScene");

        /// <summary>
        /// 银行打劫场景加载完毕
        /// </summary>
        public static readonly int RobberySceneLoadComplete = RuntimeId.ToRuntimeId("RobberySceneLoadComplete");

        /// <summary>
        /// 更新银行打劫各宝箱的数据
        /// </summary>
        public static readonly int SyncBankRobberyBoxData = RuntimeId.ToRuntimeId("SyncBankRobberyBoxData");

        /// <summary>
        /// 银行打劫锁定点击
        /// </summary>
        public static readonly int BankRobberyLockedBox = RuntimeId.ToRuntimeId("BankRobberyLockedBox");

        /// <summary>
        /// 银行打劫完成
        /// </summary>
        public static readonly int ChessEventBankRobberyFinish = RuntimeId.ToRuntimeId("ChessEventBankRobberyFinish");

        /// <summary>
        /// 银行打劫完成，需要进行表现和结算
        /// </summary>
        public static readonly int BankRobberyComplete = RuntimeId.ToRuntimeId("BankRobberyComplete");

        /// <summary>
        /// 更新当前的银行打劫宝箱的状态
        /// </summary>
        public static readonly int SyncBankRobberySelectInfo = RuntimeId.ToRuntimeId("SyncBankRobberySelectInfo");

        /// <summary>
        /// 点击宝箱后触发的事件
        /// </summary>
        public static readonly int RobberyClickObject = RuntimeId.ToRuntimeId("RobberyClickObject");

        /// <summary>
        /// 显示宝箱流光
        /// </summary>
        public static readonly int RobberyShowBoxLight = RuntimeId.ToRuntimeId("RobberyShowBoxLight");

        /// <summary>
        /// 自动打开指定的宝箱
        /// </summary>
        public static readonly int RobberyAutoOpen = RuntimeId.ToRuntimeId("RobberyAutoOpen");

        /// <summary>
        /// 播放银行打劫选中对象的光效表现
        /// </summary>
        public static readonly int PlayRobberyResultMovie = RuntimeId.ToRuntimeId("PlayRobberyResultMovie");

        /// <summary>
        /// 通知银行打劫场景切换为结算视角
        /// </summary>
        public static readonly int ChangeRobberyResultCamera = RuntimeId.ToRuntimeId("ChangeRobberyResultCamera");

        /// <summary>
        /// 退出银行打劫场景
        /// </summary>
        public static readonly int ExitRobberyScene = RuntimeId.ToRuntimeId("ExitRobberyScene");

        #endregion

        #region --- 陈列室 ---

        /// <summary>
        /// 改变陈列室当前穿戴的装备
        /// </summary>
        public static readonly int ChangeChessToken = RuntimeId.ToRuntimeId("ChangeChessToken");

        public static readonly int ChangeShieldToken = RuntimeId.ToRuntimeId("ChangeShieldToken");
        public static readonly int ChangeEmojiSlotSet = RuntimeId.ToRuntimeId("ChangeEmojiSlotSet");

        /// <summary>
        /// 更新陈列室信息
        /// </summary>
        public static readonly int SyncShowRoomInfo = RuntimeId.ToRuntimeId("SyncShowRoomInfo");

        /// <summary>
        /// 自动解锁陈列室中的格子
        /// </summary>
        public static readonly int SendRoomAutoUnlock = RuntimeId.ToRuntimeId("ShowRoomAutoUnlock");

        /// <summary>
        /// 显示陈列室解锁时的动画及奖励
        /// </summary>
        public static readonly int PlayShowRoomUnlockReward = RuntimeId.ToRuntimeId("PlayShowRoomUnlockReward");

        /// <summary>
        /// 修改指定位置的表情
        /// </summary>
        public static readonly int ShowEmojiChangeBySlot = RuntimeId.ToRuntimeId("ShowChangeSlotEmoji");

        /// <summary>
        /// 修改指定表情的位置
        /// </summary>
        public static readonly int ShowEmojiChangeBySelect = RuntimeId.ToRuntimeId("ShowEmojiChangeBySelect");

        /// <summary>
        /// 退出表情编辑模式
        /// </summary>
        public static readonly int HideEmojiChangeView = RuntimeId.ToRuntimeId("HideEmojiChangeView");

        /// <summary>
        /// 旋转模型
        /// </summary>
        public static readonly int RMChangeRotation = RuntimeId.ToRuntimeId("RMChangeRotation");

        /// <summary>
        /// 播放解锁动画期间穿戴装备时的缓存处理
        /// </summary>
        public static readonly int CacheEquipItem = RuntimeId.ToRuntimeId("CacheEquipItem");

        #endregion

        #region --- 贴纸 ---

        /// <summary>
        /// 刷新贴纸信息
        /// </summary>
        public static readonly int SyncAlbumList = RuntimeId.ToRuntimeId("SyncAlbumList");

        /// <summary>
        /// 刷新贴纸商店
        /// </summary>
        public static readonly int SyncAlbumShop = RuntimeId.ToRuntimeId("SyncAlbumShop");

        /// <summary>
        /// 向好友索要贴纸操作成功
        /// </summary>
        public static readonly int AlbumAskForComplete = RuntimeId.ToRuntimeId("AlbumAskForComplete");

        /// <summary>
        /// 赠送贴纸给好友成功
        /// </summary>
        public static readonly int AlbumGiftComplete = RuntimeId.ToRuntimeId("AlbumGiftComplete");

        /// <summary>
        /// 向好友发送交换贴纸操作成功
        /// </summary>
        public static readonly int AlbumExchangeComplete = RuntimeId.ToRuntimeId("AlbumExchangeComplete");

        public static readonly int AlbumExchangeReturnComplete = RuntimeId.ToRuntimeId("AlbumExchangeReturnComplete");

        /// <summary>
        /// 变更贴纸交换时的星数
        /// </summary>
        public static readonly int AlbumChangeSaleAmount = RuntimeId.ToRuntimeId("AlbumChangeSaleAmount");

        /// <summary>
        /// 播放获得贴纸的动画 (被赠送,索要被同意)
        /// </summary>
        public static readonly int AlbumShowGetRewardPanel = RuntimeId.ToRuntimeId("AlbumShowGetRewardPanel");

        /// <summary>
        /// 播放贴纸交换动画(自己是目标对象)
        /// </summary>
        public static readonly int AlbumShowExchangeRewardPanel = RuntimeId.ToRuntimeId("AlbumShowExchangeRewardPanel");

        /// <summary>
        /// 播放贴纸交换动画(自己是发起方)
        /// </summary>
        public static readonly int AlbumShowExchangeSelfRewardPanel = RuntimeId.ToRuntimeId("AlbumShowExchangeSelfRewardPanel");

        /// <summary>
        /// 刷新今日使用的次数
        /// </summary>
        public static readonly int AlbumSyncDayUseCount = RuntimeId.ToRuntimeId("AlbumSyncDayUseCount");

        /// <summary>
        /// 改就当前兑换星星时的选中对象
        /// </summary>
        public static readonly int AlbumChangeSaleSelectObject = RuntimeId.ToRuntimeId("AlbumChangeSaleSelectObject");

        /// <summary>
        /// 万能胶片检测刷新
        /// </summary>
        public static readonly int AllPowerfulFilmChkReset = RuntimeId.ToRuntimeId("AllPowerfulFilmChkReset");

        public static readonly int OnRespAlbumOpenChest = RuntimeId.ToRuntimeId("OnRespAlbumOpenChest");
        public static readonly int OnRespAlbumAskForReturn = RuntimeId.ToRuntimeId("OnRespAlbumAskForReturn");
        public static readonly int OnRespAlbumExchangerComplete = RuntimeId.ToRuntimeId("OnRespAlbumExchangerComplete");
        public static readonly int OnSyncStickerPushEvent = RuntimeId.ToRuntimeId("OnSyncStickerPushEvent");

        /// <summary>
        /// 显示新贴纸面板
        /// </summary>
        public static readonly int ShowNewAlbumPanel = RuntimeId.ToRuntimeId("ShowNewAlbumPanel");

        #endregion

        #region --- 小游戏 ---

        /// <summary>
        /// 小游戏结束
        /// </summary>
        public static readonly int MiniGameCashComplete = RuntimeId.ToRuntimeId("MiniGameCashComplete");

        /// <summary>
        /// 打气球小游戏结束
        /// </summary>
        public static readonly int MiniGameBalloonsComplete = RuntimeId.ToRuntimeId("MiniGameBalloonsComplete");

        /// <summary>
        /// 打气球小游戏结算
        /// </summary>
        public static readonly int MiniGameBalloonsResult = RuntimeId.ToRuntimeId("MiniGameBalloonsResult");

        #endregion

        #region --- 新手引导 ---

        /// <summary>
        /// 执行新手引导的信息
        /// </summary>
        public static readonly int PlayGuideInfo = RuntimeId.ToRuntimeId("PlayGuideInfo");

        /// <summary>
        /// 快进新手引导到指定的位置
        /// </summary>
        public static readonly int FastPlayGuideInfo = RuntimeId.ToRuntimeId("FastPlayGuideInfo");

        /// <summary>
        /// 更新当前的新手引导信息
        /// </summary>
        public static readonly int UpdateGuideList = RuntimeId.ToRuntimeId("UpdateGuideList");

        /// <summary>
        /// 清空新手引导UI
        /// </summary>
        public static readonly int ResetGuideUI = RuntimeId.ToRuntimeId("ResetGuideUI");

        /// <summary>
        /// 对象被点击后的回馈事件处理
        /// </summary>
        public static readonly int UITouchEvent = RuntimeId.ToRuntimeId("UITouchEvent");

        /// <summary>
        /// （新手期间）云动画停止时的事件
        /// </summary>
        public static readonly int CloudMaskMovieStop = RuntimeId.ToRuntimeId("CloudMaskMovieStop");

        /// <summary>
        /// （新手期间）继续播放云层动画
        /// </summary>
        public static readonly int CloudMaskMoviePlay = RuntimeId.ToRuntimeId("CloudMaskMoviePlay");

        /// <summary>
        /// （新手期间）点击了破坏建筑事件
        /// </summary>
        public static readonly int HelicopterBuildTouch = RuntimeId.ToRuntimeId("HelicopterBuildTouch");

        /// <summary>
        /// （新手期间）破坏小游戏移动到了中间的区域
        /// </summary>
        public static readonly int HelicopterAttackGameDouble = RuntimeId.ToRuntimeId("HelicopterAttackGameDouble");

        /// <summary>
        /// （新手期间）破坏小游戏显示结算面板
        /// </summary>
        public static readonly int HelicopterAttackShowResult = RuntimeId.ToRuntimeId("HelicopterAttackShowResult");

        /// <summary>
        /// 打劫/破坏事件结算消息
        /// </summary>
        public static readonly int HelicopterEventComplete = RuntimeId.ToRuntimeId("HelicopterEventComplete");

        /// <summary>
        /// 播放防护罩对象第一次显示里的动画表现
        /// </summary>
        public static readonly int PlayShieldFirtShowMovie = RuntimeId.ToRuntimeId("PlayShieldFirtShowMovie");

        /// <summary>
        /// 玩家升级动画播放控制
        /// </summary>
        public static readonly int PlayerLevelUpMovieStop = RuntimeId.ToRuntimeId("PlayerLevelUpMovieStop");
        public static readonly int PlayerLevelUpMovieContiue = RuntimeId.ToRuntimeId("PlayerLevelUpMovieContiue");

        /// <summary>
        /// 开启自动掷骰子功能成功
        /// </summary>
        public static readonly int OpenAutoDiceComplete = RuntimeId.ToRuntimeId("OpenAutoDiceComplete");

        /// <summary>
        /// 新手专用事件 - 摄像机恢复到空闲状态
        /// </summary>
        public static readonly int GuideCameraStateFree = RuntimeId.ToRuntimeId("GuideCameraStateFree");

        /// <summary>
        /// 检测主界面相关的引导
        /// </summary>
        public static readonly int CheckHUDGuide = RuntimeId.ToRuntimeId("CheckHUDGuide");

        /// <summary>
        /// 小房子飞行动画表现结束
        /// </summary>
        public static readonly int HouserFlyMovieComplete = RuntimeId.ToRuntimeId("HouserFlyMovieComplete");

        /// <summary>
        /// 第一次展示角色动画播放完毕
        /// </summary>
        public static readonly int FirstAvatarMovieComplete = RuntimeId.ToRuntimeId("FirstAvatarMovieComplete");

        /// <summary>
        /// 显示角色选择面板
        /// </summary>
        public static readonly int ShowAvatarSelectPanel = RuntimeId.ToRuntimeId("ShowAvatarSelectPanel");

        #endregion

        #region --- 红点 ---

        /// <summary>
        /// 每日任务红点
        /// </summary>
        public static readonly int HandleDailyTaskRedNote = RuntimeId.ToRuntimeId("HandleDailyTaskRedNote");

        public static readonly int HandleRedNoteActivity = RuntimeId.ToRuntimeId("HandleRedNoteActivity");
        public static readonly int HandleRedNoteActivityBack = RuntimeId.ToRuntimeId("HandleRedNoteActivityBack");

        #endregion
    }
}
