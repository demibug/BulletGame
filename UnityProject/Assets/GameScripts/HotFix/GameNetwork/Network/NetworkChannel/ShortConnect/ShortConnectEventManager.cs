//using Protos;
using System;
using System.Collections.Generic;

namespace GameNetwork
{
    public class ShortConnectEventManager : IDisposable
    {
        // 回包
        private readonly Dictionary<int, Action<byte[]>> _responeMap;

        // 推送
        private readonly Dictionary<int, List<Action<object>>> _pushMap;

        public ShortConnectEventManager()
        {
            _responeMap = new Dictionary<int, Action<byte[]>>();
            _pushMap = new Dictionary<int, List<Action<object>>>();
        }

        //Adds callback to callBackMap by id.
        public void AddCallBack(int id, Action<byte[]> callback)
        {
            if (id > 0 && callback != null)
            {
                _responeMap.Add(id, callback);
            }
        }

        public void InvokeCallBack(int id, byte[] data)
        {
            Action<byte[]> action = null;
            var foundAction = _responeMap.TryGetValue(id, out action);

            ClearCallbacks(id);

            if (foundAction) action.Invoke(data);
        }

        private void ClearCallbacks(int id)
        {
            _responeMap.Remove(id);
        }

        public void ClearAllCallbacks()
        {
            _responeMap.Clear();
        }

        // Adds the event to eventMap by name.
        public void AddOnRouteEvent(int routeId, Action<object> callback)
        {
            List<Action<object>> list;
            if (_pushMap.TryGetValue(routeId, out list))
            {
                list.Add(callback);
            }
            else
            {
                list = new List<Action<object>> { callback };
                _pushMap.Add(routeId, list);
            }
        }

        public void RemoveOnRouteEvent(int routeId)
        {
            _pushMap.Remove(routeId);
        }

        public void RemoveAllOnRouteEvents()
        {
            _pushMap.Clear();
        }

        /// <summary>
        /// If the event exists,invoke the event when server return messge.
        /// </summary>
        /// <returns></returns>
        ///
        //public void InvokeOnEvent(PushMessage msg)
        //{
        //    if (msg != null && msg.Messages != null)
        //    {
        //        for (int i = 0; i < msg.Messages.Count; i++)
        //        {
        //            PushMessageItem item = msg.Messages[i];
        //            if (item == null) continue;
        //            int routeId = item.Cmd;
        //            Log.Debug($"收到推送: {routeId}");
        //            if (!_pushMap.ContainsKey(routeId)) continue;
        //            var list = _pushMap[routeId];
        //            foreach (var action in list)
        //            {
        //                action.Invoke(item);
        //            }
        //        }
        //    }
        //}

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        // ReSharper disable once UnusedParameter.Local
        private void Dispose(bool disposing)
        {
            _responeMap.Clear();
            _pushMap.Clear();
        }
    }
}
