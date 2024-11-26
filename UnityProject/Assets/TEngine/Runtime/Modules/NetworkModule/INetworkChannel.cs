using System;

namespace TEngine
{
    public interface INetworkChannel
    {
        /// <summary>
        /// 获取网络频道名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取是否已连接。
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 获取累计发送的消息包数量。
        /// </summary>
        int SentPacketCount { get; }

        /// <summary>
        /// 获取已接收未处理的消息包数量。
        /// </summary>
        int ReceivePacketCount { get; }

        /// <summary>
        /// 获取累计已接收的消息包数量。
        /// </summary>
        int ReceivedPacketCount { get; }

        /// <summary>
        /// 获取或设置当收到消息包时是否重置心跳流逝时间。
        /// </summary>
        bool ResetHeartBeatElapseSecondsWhenReceivePacket { get; set; }

        /// <summary>
        /// 获取丢失心跳的次数。
        /// </summary>
        int MissHeartBeatCount { get; }

        /// <summary>
        /// 获取或设置心跳间隔时长，以秒为单位。
        /// </summary>
        float HeartBeatInterval { get; set; }

        /// <summary>
        /// 获取心跳等待时长，以秒为单位。
        /// </summary>
        float HeartBeatElapseSeconds { get; }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        void Connect(string ipAddress, int port);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Connect(string ipAddress, int port, object userData);

        /// <summary>
        /// 关闭网络频道。
        /// </summary>
        void Close();

        /// <summary>
        /// 网络请求
        /// </summary>
        /// <param name="route"></param>
        /// <param name="msg"></param>
        /// <param name="action"></param>
        /// <param name="errorAction"></param>
        void Request<TRespone>(string route, object msg, Action<TRespone> action, Action<object> errAction);

        /// <summary>
        /// 网络通知
        /// </summary>
        /// <param name="route"></param>
        /// <param name="msg"></param>
        void Notify(string route, object msg);

        /// <summary>
        /// 网络监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <param name="action"></param>
        void OnRoute<T>(string route, Action<T> action);

        /// <summary>
        /// 取消网络监听
        /// </summary>
        /// <param name="route"></param>
        void OffRoute(string route);
    }
}
