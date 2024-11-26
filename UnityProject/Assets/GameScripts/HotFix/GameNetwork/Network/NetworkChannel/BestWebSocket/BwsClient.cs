// using System;
// using System.Collections.Generic;
// using Best.HTTP.Shared.PlatformSupport.Memory;
// using Best.WebSockets;
// using Best.WebSockets.Implementations;
// using UnityEngine;
//
// namespace GameNetwork.BestWebSocket
// {
//     public class BwsClient
//     {
//         public event Action<NetworkChannelStates, string> NetWorkStateChangedEvent;
//
//         private WebSocket _webSocket;
//
//         private const int DEFAULT_CONNECTION_TIMEOUT = 30;
//         private BwsEventManager _eventManager;
//         private bool _disposed;
//         private uint _reqUid;
//         private Dictionary<uint, Action<string, string>> _requestHandlers;
//         private BwsProtobufSerializer _protobufSerializer;
//
//         public BwsClient(string certificateName = null, int connectionTimeout = DEFAULT_CONNECTION_TIMEOUT)
//         {
//             Init(certificateName, certificateName != null);
//         }
//
//         ~BwsClient()
//         {
//             Dispose();
//         }
//
//         private void Init(string certificateName, bool enableTlS)
//         {
//             _eventManager = new BwsEventManager();
//             _protobufSerializer = new BwsProtobufSerializer(BwsProtobufSerializer.SerializationFormat.Protobuf);
//             if (certificateName != null)
//             {
// #if UNITY_EDITOR
//                 // if (File.Exists(certificateName))
//                 //     _binding.SetCertificatePath(certificateName);
//                 // else
//                 //     _binding.SetCertificateName(certificateName);
// #else
//                 // _binding.SetCertificateName(certificateName);
// #endif
//             }
//         }
//
//         public WebSocketStates State
//         {
//             get { return _webSocket.State; }
//         }
//
//         public void Connect(string host, int port, string handshakeOpts = null)
//         {
//             if (_webSocket != null && _webSocket.IsOpen)
//             {
//                 throw new Exception("Websocket already connected");
//             }
//
//             string uri = $"ws://{host}:{port}";
//             _webSocket = new WebSocket(new Uri(uri));
//             _webSocket.OnOpen += OnOpen;
//             _webSocket.OnMessage += OnMessageReceived;
//             _webSocket.OnBinary += OnBinaryReceived;
//             _webSocket.OnClosed += OnClosed;
//             _webSocket.Open();
//         }
//
//         void OnOpen(WebSocket ws)
//         {
//             if (ws.State == WebSocketStates.Open)
//             {
//                 OnNetworkEvent(NetworkChannelStates.Connected, null);
//             }
//         }
//
//         void OnMessageReceived(WebSocket ws, string message)
//         {
//             Debug.Log("receive message");
//         }
//
//         void OnBinaryReceived(WebSocket ws, BufferSegment data)
//         {
//             BwsByteBuffer bbb = BwsByteBuffer.ResponseByteBuffer(data.Data);
//             int route = bbb.ReadInt();
//             int len = bbb.ReadInt();
//             uint rid = bbb.ReadUint();
//             byte[] pb = bbb.ReadBytes(len - 4, true);
//             if (rid != 0)
//             {
//                 OnRequestResponse(rid, pb);
//             }
//             else
//             {
//                 OnUserDefinedPush(route, pb);
//             }
//
//             BufferPool.Release(pb);
//         }
//
//         void OnClosed(WebSocket ws, WebSocketStatusCodes code, string message)
//         {
//             NetworkChannelStates state = NetworkChannelStates.Closed;
//             switch (code)
//             {
//                 case WebSocketStatusCodes.ClosedAbnormally:
//                     state = NetworkChannelStates.Disconnected;
//                     break;
//                 case WebSocketStatusCodes.NormalClosure:
//                     break;
//             }
//
//             OnNetworkEvent(state, message);
//         }
//
//
//         /// <summary cref="Request&lt;TResponse&gt;(int, object, Action&lt;TResponse&gt;, Action&lt;)">
//         /// </summary>
//         public void Request<TResponse>(int route, object msg, Action<TResponse> action)
//         {
//             RequestInternal(route, msg, action);
//         }
//
//
//         void RequestInternal<TResponse, TRequest>(int route, TRequest msg, Action<TResponse> action)
//         {
//             _reqUid++;
//
//             Action<byte[]> responseAction = res => { action(_protobufSerializer.Decode<TResponse>(res)); };
//
//             _eventManager.AddCallBack(_reqUid, responseAction);
//
//             BwsByteBuffer bbb = BwsByteBuffer.RequestByteBuffer(route, _reqUid, _protobufSerializer.Encode(msg));
//             _webSocket.Send(bbb.buffer);
//             bbb.Dispose();
//         }
//
//         /// <summary cref="Notify(int, object, int)">
//         /// </summary>
//         public void Notify(int route, object msg)
//         {
//             NotifyInternal(route, msg);
//         }
//
//         private void NotifyInternal(int route, object msg)
//         {
//             BwsByteBuffer bbb = BwsByteBuffer.RequestByteBuffer(route, 0, _protobufSerializer.Encode(msg));
//             _webSocket.Send(bbb.buffer);
//             bbb.Dispose();
//         }
//
//         /// <summary cref="OnRoute&lt;T&gt;(string, Action&lt;T&gt;)">
//         /// </summary>
//         public void OnRoute<T>(int route, Action<T> action)
//         {
//             OnRouteInternal(route, action);
//         }
//
//         private void OnRouteInternal<T>(int route, Action<T> action)
//         {
//             Action<byte[]> responseAction = res => { action(_protobufSerializer.Decode<T>(res)); };
//             _eventManager.AddOnRouteEvent(route, responseAction);
//         }
//
//         public void OffRoute(int route)
//         {
//             _eventManager.RemoveOnRouteEvent(route);
//         }
//
//         public void Disconnect()
//         {
//             _webSocket.Close();
//         }
//
//         //---------------Listener------------------------//
//
//         public void OnRequestResponse(uint rid, byte[] data)
//         {
//             _eventManager.InvokeCallBack(rid, data);
//         }
//
//         public void OnNetworkEvent(NetworkChannelStates state, string error)
//         {
//             if (NetWorkStateChangedEvent != null) NetWorkStateChangedEvent.Invoke(state, error);
//         }
//
//         public void OnUserDefinedPush(int route, byte[] serializedBody)
//         {
//             _eventManager.InvokeOnEvent(route, serializedBody);
//         }
//
//         public void Dispose()
//         {
//             Debug.Log(string.Format("BwsClient Disposed {0}", _webSocket));
//             if (_disposed)
//                 return;
//
//             if (_eventManager != null) _eventManager.Dispose();
//
//             _reqUid = 0;
//             _webSocket.Close();
//             _webSocket = null;
//             _disposed = true;
//         }
//
//         public void ClearAllCallbacks()
//         {
//             _eventManager.ClearAllCallbacks();
//         }
//
//         public void RemoveAllOnRouteEvents()
//         {
//             _eventManager.RemoveAllOnRouteEvents();
//         }
//     }
// }
