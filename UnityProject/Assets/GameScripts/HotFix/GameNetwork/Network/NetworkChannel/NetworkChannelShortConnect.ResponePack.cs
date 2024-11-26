using System;
using TEngine;

namespace GameNetwork
{
    public partial class NetworkChannelShortConnect
    {
        private static int _mCurrentRuntimeId = 0;

        private sealed class ResponePack<T> : NetworkChannelShortConnect.IResponePack
        {
            public static NetworkChannelShortConnect.ResponePack<T> Create(Action<T> action, Action<object> errorAction)
            {
                NetworkChannelShortConnect.ResponePack<T> responePack = MemoryPool.Acquire<NetworkChannelShortConnect.ResponePack<T>>();
                responePack.Id = ++_mCurrentRuntimeId;
                responePack._mAction = action;
                responePack._mErrorAction = errorAction;
                return responePack;
            }

            public int Id { get; set; }

            /// <summary>
            /// 1成功 0 未使用 -1 失败
            /// </summary>
            public NetworkChannelShortConnect.ResponePackState State { get; set; }

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
                State = NetworkChannelShortConnect.ResponePackState.Unknow;
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

            public abstract NetworkChannelShortConnect.ResponePackState State { get; set; }

            public abstract void Handle();
            public abstract void HandleError();
        }
    }
}
