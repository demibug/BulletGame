using System;
using System.Text;
using GameBase;
using GameData;
using TEngine;
using UnityEngine;

namespace GameNetwork
{
    public class NetworkSystem : Singleton<NetworkSystem>
    {
        private bool m_inited = false;
        private StringBuilder _mStringBuilder = new StringBuilder();

        // private NetworkChannelPitaya m_mainChannel;

        private NetworkChannelShortConnect m_mainChannel;
        private Action m_onConnected;

        #region 事件管理器

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

        #endregion

        public void Connect(string host, int port, Action connectedAction)
        {
            if (!m_inited)
            {
                init();
            }

            //IPAddress adress = IPAddress.Parse(host);
            string channelName = $"{host}:{port}";
            if (m_mainChannel == null)
            {
                if (!GameModule.Network.HasNetworkChannel(channelName))
                {
                    m_mainChannel = new NetworkChannelShortConnect(channelName);
                    // m_mainChannel = new NetworkChannelPitaya(channelName);
                    GameModule.Network.CreateNetworkChannel(channelName, m_mainChannel);
                }
                else
                {
                    m_mainChannel = GameModule.Network.GetNetworkChannel(channelName) as NetworkChannelShortConnect;
                    // m_mainChannel = GameModule.Network.GetNetworkChannel(channelName) as NetworkChannelPitaya;
                }
            }


            if (m_mainChannel != null)
            {
                if (m_mainChannel.Connected)
                {
                    connectedAction();
                }
                else
                {
                    m_onConnected = connectedAction;
                    m_mainChannel.Connect(host, port);
                }
            }
        }

        // /// <summary>
        // /// C <-> S 请求(有返回)
        // /// </summary>
        public void Request<TRespone>(string route, object msg, Action<TRespone> action, Action<object> errAction = null)
        {
            //m_mainChannel?.Request<TRespone>(route, msg, action, errorAction);
            _mStringBuilder.Clear();
            _mStringBuilder.Append("Request ");
            _mStringBuilder.Append(route);
            _mStringBuilder.Append(" : ");
            GameLog.LogColor(_mStringBuilder.ToString(), msg, Color.green, Color.white);
            long sendTime = GTimer.Instance.UtcTimeSec();
            m_mainChannel?.Request<TRespone>(route, msg, (TRespone) =>
            {
                _mStringBuilder.Clear();
                _mStringBuilder.Append("Respone ");
                _mStringBuilder.Append(route);
                _mStringBuilder.Append(" : ");
                GameLog.LogColor(_mStringBuilder.ToString(), TRespone, Color.cyan, Color.white);
                action(TRespone);
                long useTime = GTimer.Instance.UtcTimeSec() - sendTime;
                if (useTime > 2)
                {
                    _mStringBuilder.Clear();
                    _mStringBuilder.Append("Long time warning : Route ");
                    _mStringBuilder.Append(route);
                    _mStringBuilder.Append(" take ");
                    _mStringBuilder.Append(useTime);
                    _mStringBuilder.Append(" seconds! ");
                    Log.Warning(_mStringBuilder.ToString());
                }
            },
                errAction == null ? null : (err) =>
            {
                errAction(err);
            });
        }

        /// <summary>
        /// C <-> S 请求(有返回)
        /// </summary>
        public void Request<TRespone>(int route, object msg, Action<TRespone> action)
        {
            //m_mainChannel?.Request<TRespone>(route, msg, action, errorAction);

            // GameLog.LogColor($"Request {route} : ", msg, Color.green, Color.white);
            // long sendTime = GTimer.Instance.ServerTime;
            // m_mainChannel?.Request<TRespone>(route, msg, (TRespone) =>
            // {
            //     GameLog.LogColor($"Respone {route} : ", TRespone, Color.cyan, Color.white);
            //     action(TRespone);
            //     long useTime = GTimer.Instance.ServerTime - sendTime;
            //     if (useTime > 2)
            //     {
            //         Log.Warning($"Long time warning : Route [{route}] take {useTime} seconds! ");
            //     }
            // });
        }

        // /// <summary>
        // /// C -> S 通知(无返回)
        // /// </summary>
        public void Notify(string route, object msg)
        {
            _mStringBuilder.Clear();
            _mStringBuilder.Append("Notify ");
            _mStringBuilder.Append(route);
            _mStringBuilder.Append(" : ");
            GameLog.LogColor(_mStringBuilder.ToString(), msg, Color.green, Color.white);
            m_mainChannel?.Notify(route, msg);
        }

        /// <summary>
        /// C -> S 通知(无返回)
        /// </summary>
        public void Notify(int route, object msg)
        {
            // GameLog.LogColor($"Notify {route} : ", msg, Color.green, Color.white);
            // m_mainChannel?.Notify(route, msg);
        }

        // /// <summary>
        // /// C <- S 监听
        // /// </summary>
        // public void OnRoute<T>(string route, Action<T> action)
        // {
        //     //m_mainChannel?.OnRoute<T>(route, action);
        //     m_mainChannel?.OnRoute<T>(route, (T data) =>
        //     {
        //         GameLog.LogColor($"OnRoute {route} : ", data.ToString(), Color.cyan, Color.white);
        //         action(data);
        //     });
        // }

        // /// <summary>
        // /// C <- S 取消监听
        // /// </summary>
        // public void OffRoute(string route)
        // {
        //     m_mainChannel?.OffRoute(route);
        // }


        /// <summary>
        /// C <- S 监听
        /// </summary>
        public void OnRoute<T>(int route, Action<T> action)
        {
            //m_mainChannel?.OnRoute<T>(route, action);
            m_mainChannel?.OnRoute<T>(route, (T data) =>
            {
                _mStringBuilder.Clear();
                _mStringBuilder.Append("OnRoute ");
                _mStringBuilder.Append(route.ToString());
                _mStringBuilder.Append(" : ");
                GameLog.LogColor(_mStringBuilder.ToString(), data, Color.cyan, Color.white);
                action(data);
            });
        }

        /// <summary>
        /// C <- S 取消监听
        /// </summary>
        public void OffRoute(int route)
        {
            m_mainChannel?.OffRoute(route);
        }

        public void SyncServerTime()
        {
            m_mainChannel?.SyncServerTime();
        }


        private void init()
        {
            RegisterEvents();
            m_inited = true;
        }

        private void RegisterEvents()
        {
            eventMgr.AddEvent<NetworkConnectedEventArgs>(NetworkModule.NetworkEventConnected, OnNetworkEventConnected);
            eventMgr.AddEvent<NetworkClosedEventArgs>(NetworkModule.NetworkEventClosed, OnNetworkEventClosed);
            eventMgr.AddEvent<NetworkMissHeartBeatEventArgs>(NetworkModule.NetworkEventMissHeartBeat, OnNetworkEventMissHeartBeatId);
            eventMgr.AddEvent<NetworkErrorEventArgs>(NetworkModule.NetworkEventError, OnNetworkEventErrorId);
            eventMgr.AddEvent<NetworkCustomErrorEventArgs>(NetworkModule.NetworkEventCustomError, OnNetworkEventCustomErrorId);
        }

        private void OnNetworkEventConnected(NetworkConnectedEventArgs args)
        {
            if (m_onConnected != null)
            {
                m_onConnected();
                m_onConnected = null;
            }
        }

        private void OnNetworkEventClosed(NetworkClosedEventArgs args)
        {
            if (m_mainChannel != null)
            {
                m_mainChannel.Dispose();
            }

            m_mainChannel = null;
        }

        private void OnNetworkEventMissHeartBeatId(NetworkMissHeartBeatEventArgs args)
        {
            Log.Debug("OnNetworkEventMissHeartBeatId");
        }

        private void OnNetworkEventErrorId(NetworkErrorEventArgs args)
        {
            Log.Error("OnNetworkEventError " + args.ErrorMessage);
        }

        private void OnNetworkEventCustomErrorId(NetworkCustomErrorEventArgs args)
        {
            Log.Error("OnNetworkEventCustomErrorId");
        }
    }
}
