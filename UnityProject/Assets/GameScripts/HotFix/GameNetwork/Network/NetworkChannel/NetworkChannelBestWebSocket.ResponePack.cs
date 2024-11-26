using System;
using TEngine;

namespace GameNetwork
{
    public partial class NetworkChannelBestWebSocket
    {
        private static int _mCurrentRuntimeId = 0;

        private sealed class ResponePack<T> : NetworkChannelBestWebSocket.IResponePack
        {
            public static NetworkChannelBestWebSocket.ResponePack<T> Create(Action<T> action, Action<object> errorAction)
            {
                NetworkChannelBestWebSocket.ResponePack<T> responePack = MemoryPool.Acquire<NetworkChannelBestWebSocket.ResponePack<T>>();
                responePack.Id = ++_mCurrentRuntimeId;
                responePack._mAction = action;
                responePack._mErrorAction = errorAction;
                return responePack;
            }

            public int Id { get; set; }

            /// <summary>
            /// 1成功 0 未使用 -1 失败
            /// </summary>
            public NetworkChannelBestWebSocket.ResponePackState State { get; set; }

            public T Data { get; set; }
            public object Error { get; set; }

            private Action<T> _mAction;
            private Action<object> _mErrorAction;


            public void Handle()
            {
                if (_mAction != null)
                {
                    _mAction(Data);
                }
            }

            public void HandleError()
            {
                if (_mErrorAction != null)
                {
                    _mErrorAction(Error);
                }
            }

            public void Clear()
            {
                Id = 0;
                State = NetworkChannelBestWebSocket.ResponePackState.Unknow;
                _mAction = null;
                _mErrorAction = null;
                Data = default(T);
                Error = null;
            }
        }

        public enum ResponePackState
        {
            Succeed,
            Error,
            Unknow,
        }

        public interface IResponePack : IMemory
        {
            public abstract int Id { get; set; }

            public abstract NetworkChannelBestWebSocket.ResponePackState State { get; set; }

            public abstract void Handle();
            public abstract void HandleError();
        }
    }
}
