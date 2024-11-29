using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using GameData;
using TEngine;

namespace GameNetwork
{
    public partial class NetworkChannelShortConnect : NetworkChannelBase
    {
        private ShortConnectClient m_client;
        private ConcurrentQueue<IResponePack> m_pendingPacks;
        private ConcurrentDictionary<int, IResponePack> m_packPool;
        private Dictionary<int, int> _mRouteIdMap;

        public NetworkChannelShortConnect(string name) : base(name)
        {
            m_pendingPacks = new ConcurrentQueue<IResponePack>();
            m_packPool = new ConcurrentDictionary<int, IResponePack>();
            _mRouteIdMap = new Dictionary<int, int>();
        }

        /// <summary>
        /// 获取已接收未处理的消息包数量。
        /// </summary>
        public override int ReceivePacketCount
        {
            get { return m_pendingPacks.Count; }
        }

        public override void Connect(string ipAddress, int port, object userData)
        {
            if (m_client != null)
            {
                Close();
                m_client = null;
            }

            m_SendState.Reset();

            m_client = new ShortConnectClient(ipAddress, port);
            if (m_client == null)
            {
                string errorMessage = "Initialize network channel failure.";
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkManagerErrorCode.SocketError, SocketError.Success, errorMessage);
                    return;
                }
            }

            m_client.NetWorkStateChangedEvent += (ev, err) => { NetworkStateChangeEvent(ev, err); };

            m_client.Connect(ipAddress, port);
        }

        private void NetworkStateChangeEvent(NetworkChannelStates state, string error)
        {
            switch (state)
            {
                case NetworkChannelStates.Closed:
                    Log.Warning("NetworkChannelBestWebSocket Closed");
                    Connected = false;
                    m_Active = false;
                    Close();
                    break;
                case NetworkChannelStates.Disconnected:
                    Log.Warning("NetworkChannelBestWebSocket Disconnected");
                    Connected = false;
                    m_Active = false;
                    Close();
                    ConnectErrorCallback("Connection disconnected");
                    break;
                case NetworkChannelStates.Connected:
                    Log.Debug("NetworkChannelBestWebSocket Successfully Connected!");
                    Connected = true;
                    m_Active = true;
                    ConnectCallback();
                    break;
            }
        }

        public override void Close()
        {
            lock (this)
            {
                if (m_client == null)
                {
                    return;
                }

                m_Active = false;

                if (Connected)
                {
                    m_client?.Disconnect();
                    Connected = false;
                }

                m_client?.Dispose();
                m_client = null;

                m_SentPacketCount = 0;
                m_ReceivedPacketCount = 0;

                m_packPool.Clear();
                m_pendingPacks.Clear();

                //m_ReceivePacketPool.Clear();

                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(true);
                }

                ResponePack<object> responePack = ResponePack<object>.Create((object data) =>
                {
                    if (NetworkChannelClosed != null)
                    {
                        NetworkChannelClosed(this);
                    }
                }, null);
                responePack.State = ResponePackState.Succeed;
                m_pendingPacks.Enqueue(responePack);
            }
        }

        /// <summary>
        /// 只是通知服务端,没有回包
        /// </summary>
        /// <param name="route"></param>
        /// <param name="msg"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Notify(string route, object msg)
        {
            if (ClientIsValid())
            {
                m_client.Notify(route, msg);
            }
        }


        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="route"></param>
        /// <param name="msg"></param>
        /// <param name="action"></param>
        /// <param name="errorAction"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Request<TRespone>(string route, object msg, Action<TRespone> action, Action<object> errAction = null)
        {
            if (!ClientIsValid())
            {
                return;
            }

            ResponePack<TRespone> responePack = ResponePack<TRespone>.Create(action, errAction);
            int packId = responePack.Id;
            m_packPool.TryAdd(packId, (IResponePack)responePack);
            m_client.Request(route, msg, (TRespone data) =>
            {
                m_packPool.TryRemove(packId, out IResponePack pack);
                if (pack != null)
                {
                    pack.State = ResponePackState.Succeed;
                    ResponePack<TRespone> resPack = pack as ResponePack<TRespone>;
                    if (resPack != null)
                    {
                        if (data != null)
                        {
                            resPack.Data = data;
                        }
                    }

                    // 加入待处理队列
                    m_pendingPacks.Enqueue(pack);
                }

                ProcessPacket();
            });
        }

        /// <summary>
        /// 网络通知
        /// </summary>
        /// <param name="route"></param>
        /// <param name="msg"></param>
        public override void OnRoute<T>(int route, Action<T> action)
        {
            if (ClientIsValid())
            {
                ResponePack<T> responePack = ResponePack<T>.Create(action, null);
                int packId = responePack.Id;

                if (_mRouteIdMap.ContainsKey(route))
                {
                    if (_mRouteIdMap.TryGetValue(route, out int lastId))
                    {
                        m_packPool.TryRemove(lastId, out IResponePack pack);
                    }

                    _mRouteIdMap[route] = packId;
                }
                else
                {
                    _mRouteIdMap.Add(route, packId);
                }

                m_packPool.TryAdd(packId, (IResponePack)responePack);

                m_client.OnRoute(route, (T data) =>
                {
                    if (_mRouteIdMap.TryGetValue(route, out int oldId))
                    {
                        m_packPool.TryRemove(oldId, out IResponePack pack);
                        if (pack != null)
                        {
                            pack.State = ResponePackState.Succeed;
                            ResponePack<T> resPack = pack as ResponePack<T>;
                            if (resPack != null)
                            {
                                if (data != null)
                                {
                                    resPack.Data = data;
                                }
                            }

                            // 加入待处理队列
                            m_pendingPacks.Enqueue(pack);
                        }

                        // 新建回包
                        ResponePack<T> responePack = ResponePack<T>.Create(action, null);
                        // 新回包id
                        int newId = responePack.Id;
                        _mRouteIdMap[route] = newId;
                        m_packPool.TryAdd(newId, (IResponePack)responePack);
                    }

                    ProcessPacket();
                });
            }
        }

        /// <summary>
        /// 取消网络监听
        /// </summary>
        /// <param name="route"></param>
        public override void OffRoute(int routeId)
        {
            if (!ClientIsValid())
            {
                m_client.OffRoute(routeId);
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            ProcessReceive();

            if (m_client == null || !m_Active)
            {
                return;
            }

            //m_ReceivePacketPool.Update(elapseSeconds, realElapseSeconds);

            if (m_HeartBeatInterval > 0f)
            {
                bool sendHeartBeat = false;
                int missHeartBeatCount = 0;
                lock (m_HeartBeatState)
                {
                    if (m_client == null || !m_Active || DataSystem.Instance.Login.GameToken == null)
                    {
                        return;
                    }

                    m_HeartBeatState.HeartBeatElapseSeconds += realElapseSeconds;
                    if (m_HeartBeatState.HeartBeatElapseSeconds >= m_HeartBeatInterval && m_HeartBeatState.MissHeartBeatCount <= 3)
                    {
                        sendHeartBeat = true;
                        missHeartBeatCount = m_HeartBeatState.MissHeartBeatCount;
                        m_HeartBeatState.HeartBeatElapseSeconds = 0f;
                        m_HeartBeatState.MissHeartBeatCount++;
                    }
                }

                if (sendHeartBeat && SendHeartBeat())
                {
                    if (missHeartBeatCount > 0 && NetworkChannelMissHeartBeat != null)
                    {
                        ResponePack<object> responePack = ResponePack<object>.Create((object data) =>
                        {
                            if (NetworkChannelMissHeartBeat != null)
                            {
                                NetworkChannelMissHeartBeat(this, missHeartBeatCount);
                            }
                        }, null);

                        responePack.State = ResponePackState.Succeed;
                        m_pendingPacks.Enqueue(responePack);
                    }
                }
            }
        }

        /// <summary>
        /// 收到回包后是否重置心跳时间
        /// </summary>
        protected override bool ProcessPacket()
        {
            // 收到回包后是否重置心跳时间
            lock (m_HeartBeatState)
            {
                m_HeartBeatState.Reset(m_ResetHeartBeatElapseSecondsWhenReceivePacket);
            }

            return true;
        }

        protected override void ProcessReceive()
        {
            while (m_pendingPacks.Count > 0)
            {
                m_pendingPacks.TryDequeue(out IResponePack pack);
                if (pack != null)
                {
                    switch (pack.State)
                    {
                        case ResponePackState.Succeed:
                            pack.Handle();
                            break;
                        case ResponePackState.Error:
                            pack.HandleError();
                            break;
                    }

                    MemoryPool.Release(pack);
                }
            }
        }

        private bool ClientIsValid()
        {
            if (m_client == null)
            {
                string errorMessage = "You must connect first.";
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkManagerErrorCode.SendError, SocketError.Success, errorMessage);
                    return false;
                }
            }

            return true;
        }

        private bool SendHeartBeat()
        {
            SyncServerTime();
            return true;
        }

        public void SyncServerTime()
        {
            //ReqEmpty req = new ReqEmpty();
            //Request<RespSyncTime>(GRoute.GateGameSyncTime, req, OnSyncServerTime);
        }

        //private void OnSyncServerTime(RespSyncTime msg)
        //{
        //    if (msg != null)
        //    {
        //        GTimer.Instance.SetServerTime(msg.NowTime);
        //    }
        //}

        private void ConnectCallback()
        {
            m_pendingPacks.Clear();
            m_packPool.Clear();
            lock (m_HeartBeatState)
            {
                m_HeartBeatState.Reset(true);
            }

            ResponePack<object> responePack = ResponePack<object>.Create((object data) =>
            {
                if (NetworkChannelConnected != null)
                {
                    NetworkChannelConnected(this, null);
                }
            }, null);

            responePack.State = ResponePackState.Succeed;
            m_pendingPacks.Enqueue(responePack);
        }

        private void ConnectErrorCallback(string error)
        {
            NetworkManagerErrorCode code = NetworkManagerErrorCode.ConnectError;

            string errorMsg = string.Empty;
            if (error != null)
            {
                errorMsg = $"ConnectErrorCallback error : {error}";
                Log.Warning(errorMsg);

                // TipsInfoEntity tips = new TipsInfoEntity();
                // tips.TitleStr = LMgr.TC(103900);
                // tips.Desc = LMgr.TC(113100);
                // tips.CanNotClose = true;
                // GameEvent.Send(GEvent.ShowTextTipPanel, tips); //LW测试
            }

            ResponePack<object> responePack = ResponePack<object>.Create((object data) =>
            {
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, code, SocketError.Success, errorMsg);
                }
            }, null);

            responePack.State = ResponePackState.Succeed;
            m_pendingPacks.Enqueue(responePack);
        }

        // private void KickCallback(WebSocketStates state, string error)
        // {
        //     TipsInfoEntity tips = new TipsInfoEntity();
        //     tips.TitleStr = "提示";
        //     tips.Desc = "账号已在其他地方登录";
        //     tips.CanNotClose = true;
        //     GameEvent.Send(GEvent.ShowTextTipPanel, tips); //LW测试
        //
        //     ResponePack<object> responePack = ResponePack<object>.Create((object data) =>
        //     {
        //         if (NetworkChannelCustomError != null)
        //         {
        //             NetworkChannelCustomError(this, NetworkStats.Kicked);
        //         }
        //     }, null);
        //
        //     responePack.State = NetworkChannelBestWebSocket.ResponePackState.Succeed;
        //     m_pendingPacks.Enqueue(responePack);
        // }

        public override void Request<TRespone>(int route, object msg, Action<TRespone> action)
        {
        }

        public override void Notify(int route, object msg)
        {
        }

        public override void OnRoute<T>(string route, Action<T> action)
        {
        }

        public override void OffRoute(string route)
        {
        }
    }
}
