using System;
using System.Net.Sockets;

namespace TEngine
{
    /// <summary>
    /// 网络频道基类。
    /// </summary>
    public abstract class NetworkChannelBase : INetworkChannel, IDisposable
    {
        private const float DefaultHeartBeatInterval = 5f;

        private readonly string m_Name;

        //protected readonly Queue<Packet> m_SendPacketPool;
        //protected readonly EventPool<Packet> m_ReceivePacketPool;
        protected bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
        protected float m_HeartBeatInterval;

        protected readonly NetworkManagerSendState m_SendState;

        //protected readonly NetworkManagerReceiveState m_ReceiveState;
        protected readonly NetworkManagerHeartBeatState m_HeartBeatState;
        protected int m_SentPacketCount;
        protected int m_ReceivedPacketCount;
        protected bool m_Active;
        private bool m_Disposed;

        public Action<NetworkChannelBase, object> NetworkChannelConnected;
        public Action<NetworkChannelBase> NetworkChannelClosed;
        public Action<NetworkChannelBase, int> NetworkChannelMissHeartBeat;
        public Action<NetworkChannelBase, NetworkManagerErrorCode, SocketError, string> NetworkChannelError;
        public Action<NetworkChannelBase, object> NetworkChannelCustomError;

        /// <summary>
        /// 初始化网络频道基类的新实例。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        public NetworkChannelBase(string name)
        {
            m_Name = name ?? string.Empty;
            //m_SendPacketPool = new Queue<Packet>();
            //m_ReceivePacketPool = new EventPool<Packet>(EventPoolMode.Default);
            m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
            m_HeartBeatInterval = DefaultHeartBeatInterval;
            m_SendState = new NetworkManagerSendState();
            //m_ReceiveState = new NetworkManagerReceiveState();
            m_HeartBeatState = new NetworkManagerHeartBeatState();
            m_SentPacketCount = 0;
            m_ReceivedPacketCount = 0;
            m_Active = false;
            m_Disposed = false;

            NetworkChannelConnected = null;
            NetworkChannelClosed = null;
            NetworkChannelMissHeartBeat = null;
            NetworkChannelError = null;
            NetworkChannelCustomError = null;
        }

        /// <summary>
        /// 获取网络频道名称。
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        /// <summary>
        /// 获取是否已连接。
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// 获取累计发送的消息包数量。
        /// </summary>
        public int SentPacketCount
        {
            get { return m_SentPacketCount; }
        }

        /// <summary>
        /// 获取已接收未处理的消息包数量。
        /// </summary>
        public abstract int ReceivePacketCount { get; }

        /// <summary>
        /// 获取累计已接收的消息包数量。
        /// </summary>
        public int ReceivedPacketCount
        {
            get { return m_ReceivedPacketCount; }
        }

        /// <summary>
        /// 获取或设置当收到消息包时是否重置心跳流逝时间。
        /// </summary>
        public bool ResetHeartBeatElapseSecondsWhenReceivePacket
        {
            get { return m_ResetHeartBeatElapseSecondsWhenReceivePacket; }
            set { m_ResetHeartBeatElapseSecondsWhenReceivePacket = value; }
        }

        /// <summary>
        /// 获取丢失心跳的次数。
        /// </summary>
        public int MissHeartBeatCount
        {
            get { return m_HeartBeatState.MissHeartBeatCount; }
        }

        /// <summary>
        /// 获取或设置心跳间隔时长，以秒为单位。
        /// </summary>
        public float HeartBeatInterval
        {
            get { return m_HeartBeatInterval; }
            set { m_HeartBeatInterval = value; }
        }

        /// <summary>
        /// 获取心跳等待时长，以秒为单位。
        /// </summary>
        public float HeartBeatElapseSeconds
        {
            get { return m_HeartBeatState.HeartBeatElapseSeconds; }
        }

        /// <summary>
        /// 网络频道轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭网络频道。
        /// </summary>
        public virtual void Shutdown()
        {
            Close();
            //m_ReceivePacketPool.Shutdown();
        }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        public void Connect(string ipAddress, int port)
        {
            Connect(ipAddress, port, null);
        }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public abstract void Connect(string ipAddress, int port, object userData);

        /// <summary>
        /// 关闭连接并释放所有相关资源。
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                Close();
                m_SendState.Dispose();
                //m_ReceiveState.Dispose();
            }

            m_Disposed = true;
        }

        protected abstract void ProcessReceive();

        protected abstract bool ProcessPacket();

        public abstract void Request<TRespone>(string route, object msg, Action<TRespone> action, Action<object> errAction);
        public abstract void Notify(string route, object msg);
        public abstract void OnRoute<T>(string route, Action<T> action);
        public abstract void OffRoute(string route);


        public abstract void Request<TRespone>(int route, object msg, Action<TRespone> action);
        public abstract void Notify(int route, object msg);
        public abstract void OnRoute<T>(int route, Action<T> action);
        public abstract void OffRoute(int route);
    }
}
