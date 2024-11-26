using TEngine;

namespace GameNetwork
{
    public class MsgRegisterBase<T> : IMsgSystem where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new T();
                    Log.Assert(_instance != null);
                }

                return _instance;
            }
        }

        /// <summary>
        /// 是否已经注册过了的标识
        /// </summary>
        private bool m_isRegistered = false;

        public virtual void RegisterMessage()
        {
            if (m_isRegistered)
            {
                return;
            }
            m_isRegistered = true;
        }

        public virtual void UnregisterMessage()
        {
            m_isRegistered = false;
        }

        protected virtual void OnError(object data)
        {
            Log.Debug("OnResp error " + data);
            //ShowErrorCode(data);
        }
    }
}
