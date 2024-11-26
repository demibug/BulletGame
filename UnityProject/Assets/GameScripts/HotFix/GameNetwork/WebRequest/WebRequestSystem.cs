using GameBase;
using GameData;
using TEngine;

namespace GameNetwork
{
    public class WebRequestSystem : Singleton<WebRequestSystem>
    {
        private bool m_inited = false;

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

        private void Init()
        {
            if (!m_inited)
            {
                RegisterEvents();
                WebRequestAgentHelper helper = WebRequestAgentHelper.Instance;
                GameModule.WebRequest.AddWebRequestAgentHelper(helper);
                m_inited = true;
            }
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequestProto(string webRequestUri, byte[] postData, object userData)
        {
            Init();
            return GameModule.WebRequest.AddWebRequest(webRequestUri, postData, "protobuf", userData);
        }

        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            Init();

            return GameModule.WebRequest.AddWebRequest(webRequestUri, postData);
        }

        private void RegisterEvents()
        {
            eventMgr.AddEvent<WebRequestStartEventArgs>(WebRequestModule.WebRequestEventStart, OnWebRequestEventStart);
            eventMgr.AddEvent<WebRequestSuccessEventArgs>(WebRequestModule.WebRequestEventSuccess, OnWebRequestEventSuccess);
            eventMgr.AddEvent<WebRequestFailureEventArgs>(WebRequestModule.WebRequestEventFailure, OnWebRequestEventFailure);
        }

        private void OnWebRequestEventStart(WebRequestStartEventArgs args)
        {
            // Log.Debug("OnWebRequestEventStart");
        }

        private void OnWebRequestEventSuccess(WebRequestSuccessEventArgs args)
        {
            // Log.Debug("OnWebRequestEventSuccess");
            GameEvent.Send(GEvent.WebRequestSystemEventSuccess, args);
        }

        private void OnWebRequestEventFailure(WebRequestFailureEventArgs args)
        {
            Log.Debug("OnWebRequestEventFailure");
            GameEvent.Send(GEvent.WebRequestSystemEventFailure, args);
        }
    }
}
