namespace GameNetwork
{
    /// <summary>
    /// 全局的一些通用协议
    /// </summary>
    public class GlobalMsg : MsgRegisterBase<GlobalMsg>
    {
        public override void RegisterMessage()
        {
            base.RegisterMessage();

            ////更新当前骰子的恢复时间
            //NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.GameRoleSyncLevelUp, OnSyncLevelUp);

            ////集齐画册后的推送
            //NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.GameSyncAlbum, OnGameSyncAlbum);

            ////零点推送（跨天时的推送）
            //NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.SyncClock0Push, OnSyncClock0Push);
        }

        public override void UnregisterMessage()
        {
            base.UnregisterMessage();

            //NetworkSystem.Instance.OffRoute(GRoute.GameRoleSyncLevelUp);
            //NetworkSystem.Instance.OffRoute(GRoute.GameSyncAlbum);
        }
    }
}
