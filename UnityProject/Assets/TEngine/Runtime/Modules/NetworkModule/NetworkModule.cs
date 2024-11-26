using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// 网络模块
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class NetworkModule : Module
    {
        private INetworkManager m_networkManager;

        #region 事件id
        public static readonly int NetworkEventConnected = RuntimeId.ToRuntimeId("NetworkEventConnected");
        public static readonly int NetworkEventClosed = RuntimeId.ToRuntimeId("NetworkEventClosed");
        public static readonly int NetworkEventMissHeartBeat = RuntimeId.ToRuntimeId("NetworkEventMissHeartBeat");
        public static readonly int NetworkEventError = RuntimeId.ToRuntimeId("NetworkEventError");
        public static readonly int NetworkEventCustomError = RuntimeId.ToRuntimeId("NetworkEventCustomError");

        #endregion

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_networkManager.NetworkChannelCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        private void Start()
        {
            m_networkManager = ModuleImpSystem.GetModule<INetworkManager>();
            if (m_networkManager == null)
            {
                Log.Fatal("Network manager is invalid.");
                return;
            }

            m_networkManager.NetworkConnected += OnNetworkConnected;
            m_networkManager.NetworkClosed += OnNetworkClosed;
            m_networkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_networkManager.NetworkError += OnNetworkError;
            m_networkManager.NetworkCustomError += OnNetworkCustomError;
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_networkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return m_networkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_networkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        public void GetAllNetworkChannels(List<INetworkChannel> results)
        {
            m_networkManager.GetAllNetworkChannels(results);
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="serviceType">网络服务类型。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name, NetworkChannelBase networkChannel)
        {
            return m_networkManager.CreateNetworkChannel(name, networkChannel);
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return m_networkManager.DestroyNetworkChannel(name);
        }

        private void OnNetworkConnected(object sender, NetworkConnectedEventArgs e)
        {
            GameEvent.Send(NetworkEventConnected, e);
        }

        private void OnNetworkClosed(object sender, NetworkClosedEventArgs e)
        {
            GameEvent.Send(NetworkEventClosed, e);
        }

        private void OnNetworkMissHeartBeat(object sender, NetworkMissHeartBeatEventArgs e)
        {
            GameEvent.Send(NetworkEventMissHeartBeat, e);
        }

        private void OnNetworkError(object sender, NetworkErrorEventArgs e)
        {
            GameEvent.Send(NetworkEventError, e);
        }

        private void OnNetworkCustomError(object sender, NetworkCustomErrorEventArgs e)
        {
            GameEvent.Send(NetworkEventCustomError, e);
        }
    }
}
