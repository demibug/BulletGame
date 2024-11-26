using System.Collections.Generic;
using GameBase;
using GameData;
using GameData.GDefine;
using Protos;
using TEngine;

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

            //更新当前骰子的恢复时间
            NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.GameRoleSyncLevelUp, OnSyncLevelUp);

            //集齐画册后的推送
            NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.GameSyncAlbum, OnGameSyncAlbum);

            //零点推送（跨天时的推送）
            NetworkSystem.Instance.OnRoute<PushMessageItem>(GRoute.SyncClock0Push, OnSyncClock0Push);
        }

        public override void UnregisterMessage()
        {
            base.UnregisterMessage();

            NetworkSystem.Instance.OffRoute(GRoute.GameRoleSyncLevelUp);
            NetworkSystem.Instance.OffRoute(GRoute.GameSyncAlbum);
        }

        private void OnSyncLevelUp(PushMessageItem msg)
        {
            if (msg != null && msg.SyncLevelUP != null)
            {
                for (int i = 0; i < msg.SyncLevelUP.Rewards.Count; i++)
                {
                    var info = msg.SyncLevelUP.Rewards[i];

                    if (info.Pieces != null && info.Pieces.Count > 0)
                    {
                        GameEvent.Send(GEvent.PlayerUpgradeLevel_Piece, info.Pieces); //地块更新
                    }

                    if (info.DiceTime != 0)
                    {
                        DataSystem.Instance.Player.UpdateDiceReplyTime(info.DiceTime); //骰子的恢复时间
                    }

                    //更新玩家等级
                    DataSystem.Instance.Player.UpdatePlayerLevel(info.Level);
                    //添加物品
                    //DataSystem.Instance.Player.AddBagItems(info.Items);

                    //通知要弹出升级面板
                    //List<long[]> lstReward = FGUIExtension.GetRewardList(info.Items);
                    GameEvent.Send(GEvent.PlayerUpgradeLevel, info.Level, info.Items);
                    //string str = "";
                    //for (int j = 0; j < lstReward.Count; j++)
                    //{
                    //    str += " 物品ID:" + lstReward[j][0] + " 物品数量:" + lstReward[j][1];
                    //}
                    // Log.LW("玩家升级 当前等级:" + info.Level + " 奖励数据：" + str);
                }
            }
        }

        private void OnGameSyncAlbum(PushMessageItem msg)
        {
            //Log.Warning(msg.ToString());
            if (msg != null)
            {
                if (msg.SyncAlbumRewards.SetRewards != null)
                {
                    for (int i = 0; i < msg.SyncAlbumRewards.SetRewards.Count; i++)
                    {                       
                       
                        if (DataSystem.Instance.CorePlay.IsAutoRoll)
                        {
                            //自动模式下，加入到待处理的数据列表
                            DataSystem.Instance.ChessActivityData.AddAlbumCompleteRewardData(msg.SyncAlbumRewards.SetRewards[i]);
                        }
                        else
                        {
                            //图集完成奖励列表
                            DataSystem.Instance.Player.AddBagItems(msg.SyncAlbumRewards.SetRewards[i].Rewards);

                            //显示获得图集的奖励弹框
                            DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.AlbumGroup,
                                msg.SyncAlbumRewards.SetRewards[i].Rewards, null,
                                false, "", "", true, msg.SyncAlbumRewards.SetRewards[i].AlbumID,
                                "", "", "", 0.5f);
                        }
                    }
                }

                //Log.Info("AlbumRewardCount: " + msg.AlbumReward.Count);

                if (msg.SyncAlbumRewards.AlbumReward.Count > 0)
                {
                    if (DataSystem.Instance.CorePlay.IsAutoRoll)
                    {
                        //自动模式下，加入到待处理的数据列表
                        DataSystem.Instance.ChessActivityData.AddAlbumCompleteRewardData(DataSystem.Instance.Album.CurrentAlbumGroupId, msg.SyncAlbumRewards.AlbumReward);
                    }
                    else
                    {
                        //图册完成奖励
                        DataSystem.Instance.Player.AddBagItems(msg.SyncAlbumRewards.AlbumReward);

                        //显示奖励弹框
                        DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.AlbumBook,
                            msg.SyncAlbumRewards.AlbumReward, null,
                            false, "", "", true, DataSystem.Instance.Album.CurrentAlbumGroupId);
                    }
                }
            }
        }

        /// <summary>
        /// 零点推送（部份参数需要重置）
        /// </summary>
        /// <param name="msg"></param>
        private void OnSyncClock0Push(PushMessageItem msg)
        {
            if (msg != null && msg.SyncClock0Push != null)
            {
                //更新零点推送的标识
                DataSystem.Instance.HUD.IsZeroRefresh = true;

                //更新贴纸相关的次数更新
                DataSystem.Instance.Album.UpdateAlbumCount(msg.SyncClock0Push.SendDaily, msg.SyncClock0Push.RequestDaily);
            }
        }

        //-------------------------------------------------

        /// <summary>
        /// 发送获取道具PM
        /// </summary>
        public void SyncGameGmBag(Dictionary<int, long> temp)
        {
            ReqGMBag sendData = new ReqGMBag();
            foreach (var i in temp)
            {
                sendData.Items.Add(i.Key, i.Value);
            }

            NetworkSystem.Instance.Request<RespGMBag>(GRoute.GameGmBag, sendData, OnSyncGameGmBag);
        }

        private void OnSyncGameGmBag(RespGMBag msg)
        {
            if (msg != null)
            {
                //只有一个道具时,宝箱类道具及卡包类道具需要进行表现
                if (msg.Items.Count == 1)
                {
                    PItem item = msg.Items[0];
                    //if (item.Pack != null || item.StickerPack != null)
                    //{
                    DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.Normal, item, null, true);
                    //}
                }
                // Log.LW("<<PM提示>> 添加道具成功！！");

                //添加数据到背包
                DataSystem.Instance.Player.AddBagItems(msg.Items);
            }
        }

        /// <summary>
        /// 发送投掷骰子PM
        /// </summary>
        public void SyncGameGmGo(List<int> temp)
        {
            ReqGMGo sendData = new ReqGMGo
            {
                Dice1 = temp[0],
                Dice2 = temp[1]
            };
            NetworkSystem.Instance.Request<RespGo>(GRoute.GameGmGo, sendData, OnSyncGameGmGo);
        }

        private void OnSyncGameGmGo(RespGo msg)
        {
            if (msg != null)
            {
                if (msg.Dice1 == 0 || msg.Dice2 == 0)
                {
                    // Log.LW("<<PM提示>> 投骰子失败 (Dice1 = " + msg.Dice1 + " Dice2 = " + msg.Dice2 + ")");
                    return;
                }

                // Log.LW("<<PM提示>> 投骰子成功！！(Dice1 = " + msg.Dice1 + " Dice2 = " + msg.Dice2 + ")");
                MsgManage.Instance.Chess.PMRequestRoll(msg);
            }
        }

        public void SyncGameGmMaxpiecelevel()
        {
            ReqEmpty reqEmpty = new ReqEmpty();
            NetworkSystem.Instance.Request<Board>(GRoute.GameGmMaxpiecelevel, reqEmpty, OnSyncGameGmMaxpiecelevel);
        }

        private void OnSyncGameGmMaxpiecelevel(Board msg)
        {
            if (msg != null)
            {
                DataSystem.Instance.Player.UpdatePlayerInfo_Board(msg);
                // Log.LW("<<PM提示>>地块房子满级触发成功！！");
            }
        }

        /// <summary>
        /// PM 添加好友协议
        /// </summary>
        /// <param name="roleId"></param>
        public void SyncSocialGmAddfriend(long roleId)
        {
            ReqFriendAdd sendData = new ReqFriendAdd
            {
                RoleID = roleId
            };
            NetworkSystem.Instance.Request<RespFriendHandle>(GRoute.SocialGmAddfriend, sendData, OnResp_SocialGmAddfriend);
        }

        private void OnResp_SocialGmAddfriend(RespFriendHandle msg)
        {
            if (msg.ErrorCode == 0)
            {
                //发送成功
                // Log.LW("<<PM提示>> 添加好友成功");
            }
            else
            {
                ShowErrorCode(msg.ErrorCode);
            }
        }

        /// <summary>
        /// 发送获取道具PM
        /// </summary>
        public void SendGameManageOrder(string strOrder)
        {
            ReqGm sendData = new ReqGm
            {
                Data = strOrder
            };

            NetworkSystem.Instance.Request<RespGm>(GRoute.GameManageOrder, sendData, OnGetGameManageOrder);
        }

        private void OnGetGameManageOrder(RespGm msg)
        {
            if (msg != null)
            {
                if (msg.ErrorCode > 0)
                {
                    DataSystem.Instance.Global.ShowTextPanel("Error: " + msg.ErrorCode);
                }
                else
                {
                    if(msg.Results != null)
                    {
                        if (msg.Results.GoResult != null)
                        {
                            GameLog.WarringInfo("<<PM提示>> 投骰子成功！！(Dice1 = " + msg.Results.GoResult.Dice1 + " Dice2 = " + msg.Results.GoResult.Dice2 + ")");
                            MsgManage.Instance.Chess.PMRequestRoll(msg.Results.GoResult);
                        }
                        else if (msg.Results.BagResult != null)
                        {
                            GameLog.WarringInfo("<<PM提示>> 添加道具成功！！");

                            ////只有一个道具时,宝箱类道具及卡包类道具需要进行表现
                            if (msg.Results.BagResult.Items.Count > 0)
                            {
                                //    PItem item = msg.Results.BagResult.Items[0];
                                //    DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.Normal, item, null, true);
                                DataSystem.Instance.Global.ShowGetRewardPanel(GetRewardPanelType.Normal, msg.Results.BagResult.Items, null, true);
                            }

                            //添加数据到背包
                            DataSystem.Instance.Player.AddBagItems(msg.Results.BagResult.Items);
                        }
                        else if (msg.Results.AddFriendResult != null)
                        {
                            GameLog.WarringInfo("<<PM提示>> 添加好友成功");
                        }
                        else if (msg.Results.BoardResult != null)
                        {
                            GameLog.WarringInfo("<<PM提示>>地块房子满级触发成功！！");
                            DataSystem.Instance.Player.UpdatePlayerInfo_Board(msg.Results.BoardResult);
                        }
                        else if (msg.Results.EmptyResult != null)
                        {
                            GameLog.WarringInfo("<<PM提示>> GM指令处理成功");
                        }
                    }
                }
            }
        }

        #region --- 新手引导 ---


        /// <summary>
        /// 查询新手引导信息
        /// </summary>
        public void QueryGuideList()
        {
            ReqEmpty sendData = new ReqEmpty();
            NetworkSystem.Instance.Request<RespGuideQuery>(GRoute.GuideQuery, sendData, OnQueryGuideList);
        }

        private void OnQueryGuideList(RespGuideQuery msg)
        {
            if (msg != null)
            {
                DataSystem.Instance.Guide.UpdateGuideList(msg.Data);
            }
        }

        /// <summary>
        /// 保存当前最新的新手引导信息
        /// </summary>
        /// <param name="str"></param>
        public void SaveGuideList(string str)
        {
            ReqGuideSave sendData = new ReqGuideSave
            {
                Data = str
            };
            NetworkSystem.Instance.Request<RespGuideSave>(GRoute.GuideSave, sendData, OnSaveGuideList);
        }

        private void OnSaveGuideList(RespGuideSave msg)
        {
            if (msg != null)
            {

            }
        }

        #endregion
    }
}
