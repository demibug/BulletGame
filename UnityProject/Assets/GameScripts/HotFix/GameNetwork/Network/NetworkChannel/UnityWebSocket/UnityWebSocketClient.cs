
using GameData;
using GameNetwork.UnityWebSocket;
using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameNetwork
{
    public class UnityWebSocketClient
    {
        public event Action<NetworkChannelStates, string> NetWorkStateChangedEvent;
        private const int DEFAULT_CONNECTION_TIMEOUT = 30;
        private UnityWebSocketEventManager _eventManager;
        private bool _disposed;
        private int _reqUid;
        private Dictionary<uint, Action<string, string>> _requestHandlers;
        private UnityWebSocketProtobufSerializer _protobufSerializer;
        private Dictionary<string, string> _header;
        private string _host;
        private int _port;

        public UnityWebSocketClient(string host, int port)
        {
            Init();
            _host = host;
            _port = port;
            GameEvent.AddEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.AddEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);
        }

        ~UnityWebSocketClient()
        {
            Dispose();
            GameEvent.RemoveEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.RemoveEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);
        }

        private void Init()
        {
            _eventManager = new UnityWebSocketEventManager();
            _protobufSerializer = new UnityWebSocketProtobufSerializer(UnityWebSocketProtobufSerializer.SerializationFormat.Protobuf);
            _header = new Dictionary<string, string>();
        }

        public void Connect(string host, int port, string handshakeOpts = null)
        {
            OnNetworkEvent(NetworkChannelStates.Connected, null);
        }

        public void Disconnect()
        {
            OnNetworkEvent(NetworkChannelStates.Disconnected, null);
        }

        private void OnWebRequestSuccess(WebRequestSuccessEventArgs args)
        {
            if (args != null)
            {
                byte[] data = args.GetWebResponseBytes();
                if (data != null)
                {
                    UnityWebSocketByteBuffer scbb = UnityWebSocketByteBuffer.ResponseByteBuffer(data);
                    int rid = scbb.ReadInt();
                    int pushLen = scbb.ReadInt();
                    int respLen = scbb.ReadInt();
                    int len = data.Length;
                    int headLen = UnityWebSocketDefines.ReqUidLen + UnityWebSocketDefines.PushDataLen + UnityWebSocketDefines.RespDataLen;
                    if (pushLen != 0)
                    {
                        byte[] pushPb = scbb.ReadBytes(len - headLen - respLen, true);
                        if (pushPb != null)
                        {
                            OnPushRespone(pushPb);
                        }

                        UnityWebSocketStreamBufferPool.RecycleBuffer(pushPb);
                    }

                    if (rid != 0)
                    {
                        byte[] respPb = scbb.ReadBytes(len - headLen - pushLen, true);
                        if (respPb != null)
                        {
                            OnRequestResponse(rid, respPb);
                        }

                        UnityWebSocketStreamBufferPool.RecycleBuffer(respPb);
                    }
                }
            }
        }

        private void OnWebRequestFailure(WebRequestFailureEventArgs args)
        {
            if (args != null)
            {
                Log.Warning("web request fail : " + args.ErrorMessage);
            }
        }


        /// <summary cref="Request&lt;TResponse&gt;(int, object, Action&lt;TResponse&gt;, Action&lt;)">
        /// </summary>
        public void Request<TResponse>(string route, object msg, Action<TResponse> action)
        {
            RequestInternal(route, msg, action);
        }


        void RequestInternal<TResponse, TRequest>(string route, TRequest msg, Action<TResponse> action)
        {
            _reqUid++;

            Action<byte[]> responseAction = res => { action(_protobufSerializer.Decode<TResponse>(res)); };

            _eventManager.AddCallBack(_reqUid, responseAction);
            Dictionary<string, string> header = null;
            if (DataSystem.Instance.Login.GameToken != null)
            {
                header = _header;
                if (_header.ContainsKey("token"))
                {
                    _header["token"] = DataSystem.Instance.Login.GameToken;
                }
                else
                {
                    _header.Add("token", DataSystem.Instance.Login.GameToken);
                }
            }

            byte[] data = _protobufSerializer.Encode(msg);
            UnityWebSocketByteBuffer scbb = UnityWebSocketByteBuffer.RequestByteBuffer(_reqUid, data);

            string uri = $"{_host}:{_port}{route}";
            if (Debug.isDebugBuild)
            {
                if (route.CompareTo(GRoute.GateGameSyncTime) != 0)
                {
                    Log.Debug($"Route: {route} | reqId: {_reqUid}");
                }
            }


            WebRequestSystem.Instance.AddWebRequestProto(uri, scbb.buffer, header);
        }

        /// <summary cref="Notify(int, object, int)">
        /// </summary>
        public void Notify(string route, object msg)
        {
            NotifyInternal(route, msg);
        }

        private void NotifyInternal(string route, object msg)
        {
            UnityWebSocketByteBuffer scbb = UnityWebSocketByteBuffer.RequestByteBuffer(_reqUid, _protobufSerializer.Encode(msg));
            Dictionary<string, string> header = null;
            if (DataSystem.Instance.Login.GameToken != null)
            {
                header = _header;
                if (_header.ContainsKey("token"))
                {
                    _header["token"] = DataSystem.Instance.Login.GameToken;
                }
                else
                {
                    _header.Add("token", DataSystem.Instance.Login.GameToken);
                }
            }

            string uri = $"{_host}:{_port}{route}";
            WebRequestSystem.Instance.AddWebRequestProto(uri, scbb.buffer, header);
        }

        /// <summary cref="OnRoute&lt;T&gt;(string, Action&lt;T&gt;)">
        /// </summary>
        public void OnRoute<T>(int route, Action<T> action)
        {
            OnRouteInternal(route, action);
        }

        private void OnRouteInternal<T>(int route, Action<T> action)
        {
            Action<object> responseAction = res => { action((T)res); };
            _eventManager.AddOnRouteEvent(route, responseAction);
        }

        public void OffRoute(int route)
        {
            _eventManager.RemoveOnRouteEvent(route);
        }


        //---------------Listener------------------------//

        public void OnRequestResponse(int rid, byte[] data)
        {
            _eventManager.InvokeCallBack(rid, data);
        }

        private void OnPushRespone(byte[] data)
        {
            //PushMessage pushMsg = _protobufSerializer.Decode<PushMessage>(data);
            //_eventManager.InvokeOnEvent(pushMsg);
        }

        public void OnNetworkEvent(NetworkChannelStates state, string error)
        {
            if (NetWorkStateChangedEvent != null) NetWorkStateChangedEvent.Invoke(state, error);
        }

        public void Dispose()
        {
            Debug.Log("UnityWebSocketClient Disposed");
            if (_disposed)
                return;

            if (_eventManager != null) _eventManager.Dispose();

            _reqUid = 0;
            // _webSocket.Close();
            // _webSocket = null;
            _disposed = true;
        }

        public void ClearAllCallbacks()
        {
            _eventManager.ClearAllCallbacks();
        }

        public void RemoveAllOnRouteEvents()
        {
            _eventManager.RemoveAllOnRouteEvents();
        }
    }
}
