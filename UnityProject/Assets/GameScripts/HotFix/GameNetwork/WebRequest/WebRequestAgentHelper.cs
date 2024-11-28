using GameNetwork.UnityWebSocket;
using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine.Networking;

namespace GameNetwork
{
    /// <summary>
    /// Web 请求代理辅助器基类。
    /// </summary>
    [Update]
    public class WebRequestAgentHelper : BehaviourSingleton<WebRequestAgentHelper>, IWebRequestAgentHelper
    {
        private UnityWebRequest m_UnityWebRequest = null;
        private bool m_Disposed = false;

        private EventHandler<WebRequestAgentHelperCompleteEventArgs> m_WebRequestAgentHelperCompleteEventHandler = null;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> m_WebRequestAgentHelperErrorEventHandler = null;

        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add { m_WebRequestAgentHelperCompleteEventHandler += value; }
            remove { m_WebRequestAgentHelperCompleteEventHandler -= value; }
        }

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        public event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add { m_WebRequestAgentHelperErrorEventHandler += value; }
            remove { m_WebRequestAgentHelperErrorEventHandler -= value; }
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void Request(string webRequestUri, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            WWWFormInfo wwwFormInfo = (WWWFormInfo)userData;
            if (wwwFormInfo.WWWForm == null)
            {
                m_UnityWebRequest = UnityWebRequest.Get(webRequestUri);
            }
            else
            {
                m_UnityWebRequest = UnityWebRequest.Post(webRequestUri, wwwFormInfo.WWWForm);
            }

            if (wwwFormInfo.UserData != null && wwwFormInfo.UserData is Dictionary<string, string> headers)
            {
                string head = "headers:";
                foreach (KeyValuePair<string, string> item in headers)
                {
                    head += $"{item.Key}+{item.Value}\n";
                    m_UnityWebRequest.SetRequestHeader(item.Key, item.Value);
                }

                Log.Debug(head);
            }

            m_UnityWebRequest.certificateHandler = new WebRequestCert();
            m_UnityWebRequest.SendWebRequest();
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void Request(string webRequestUri, byte[] postData, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            // 创建 UnityWebRequest 对象
            UnityWebRequest request = new UnityWebRequest(webRequestUri, "POST");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(postData); // 直接使用 postData 构造 UploadHandlerRaw
            request.uploadHandler.contentType = "application/json";
            m_UnityWebRequest = request;

            // 设置请求头部
            if (userData != null && userData is WWWFormInfo wwwFormInfo && wwwFormInfo.UserData is Dictionary<string, string> headers)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    m_UnityWebRequest.SetRequestHeader(item.Key, item.Value);
                }
            }

            // 发送请求
            m_UnityWebRequest.SendWebRequest();
            // 回收buffer
            UnityWebSocketStreamBufferPool.RecycleBuffer(postData);
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RequestProtobuf(string webRequestUri, byte[] postData, object userData)
        {
            if (m_WebRequestAgentHelperCompleteEventHandler == null || m_WebRequestAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }
            //string jsonData = Encoding.UTF8.GetString(postData);
            //m_UnityWebRequest = UnityWebRequest.Post(webRequestUri, jsonData, "application/x-protobuf");

            if (postData == null || postData.Length == 0)
            {
                Log.Fatal("Post data is invalid.");
                return;
            }

            UnityWebRequest request = new UnityWebRequest(webRequestUri, "POST");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.uploadHandler.contentType = "application/x-protobuf";
            m_UnityWebRequest = request;

            WWWFormInfo wwwFormInfo = (WWWFormInfo)userData;
            if (wwwFormInfo.UserData != null && wwwFormInfo.UserData is Dictionary<string, string> headers)
            {
                string head = "headers:";
                foreach (KeyValuePair<string, string> item in headers)
                {
                    head += $"{item.Key}+{item.Value}\n";
                    m_UnityWebRequest.SetRequestHeader(item.Key, item.Value);
                }

                // Log.Debug(head);
            }

            m_UnityWebRequest.SendWebRequest();
            // 回收buffer
            UnityWebSocketStreamBufferPool.RecycleBuffer(postData);
        }

        /// <summary>
        /// 重置 Web 请求代理辅助器。
        /// </summary>
        public void Reset()
        {
            if (m_UnityWebRequest != null)
            {
                m_UnityWebRequest.Dispose();
                m_UnityWebRequest = null;
            }
        }

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
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_UnityWebRequest != null)
                {
                    m_UnityWebRequest.Dispose();
                    m_UnityWebRequest = null;
                }
            }

            m_Disposed = true;
        }

        public override void Update()
        {
            if (m_UnityWebRequest == null || !m_UnityWebRequest.isDone)
            {
                return;
            }

            bool isError = false;
#if UNITY_2020_2_OR_NEWER
            isError = m_UnityWebRequest.result != UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
            isError = m_UnityWebRequest.isNetworkError || m_UnityWebRequest.isHttpError;
#else
            isError = m_UnityWebRequest.isError;
#endif
            string errorStr = m_UnityWebRequest.error;
            //if (!isError)
            //{
            //    string dataString = m_UnityWebRequest.downloadHandler.text;
            //    if (!string.IsNullOrEmpty(dataString) && dataString.Contains("code") && dataString.Contains("msg"))
            //    {
            //        JSONNode kvp = JSON.Parse(dataString);
            //        if (kvp != null)
            //        {
            //            int code = kvp["code"].AsInt;
            //            string msg = kvp["msg"];
            //            if (code != 200)
            //            {
            //                errorStr = $"code:{code} msg:{msg}";
            //                isError = true;
            //            }
            //        }
            //    }
            //}

            if (isError)
            {
                WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = WebRequestAgentHelperErrorEventArgs.Create(errorStr);
                m_WebRequestAgentHelperErrorEventHandler(this, webRequestAgentHelperErrorEventArgs);
                ReferencePool.Release(webRequestAgentHelperErrorEventArgs);
            }
            else if (m_UnityWebRequest.downloadHandler.isDone)
            {
                WebRequestAgentHelperCompleteEventArgs webRequestAgentHelperCompleteEventArgs =
                    WebRequestAgentHelperCompleteEventArgs.Create(m_UnityWebRequest.downloadHandler.data);
                m_WebRequestAgentHelperCompleteEventHandler(this, webRequestAgentHelperCompleteEventArgs);
                ReferencePool.Release(webRequestAgentHelperCompleteEventArgs);
            }
        }
    }
}
