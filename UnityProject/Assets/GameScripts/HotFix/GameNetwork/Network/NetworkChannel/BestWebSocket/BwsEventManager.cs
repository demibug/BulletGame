using System;
using System.Collections.Generic;

namespace GameNetwork.BestWebSocket
{
    public class BwsEventManager : IDisposable
    {
        private readonly Dictionary<uint, Action<byte[]>> _callBackMap;

        private readonly Dictionary<int, List<Action<byte[]>>> _eventMap;

        public BwsEventManager()
        {
            _callBackMap = new Dictionary<uint, Action<byte[]>>();
            _eventMap = new Dictionary<int, List<Action<byte[]>>>();
        }

        //Adds callback to callBackMap by id.
        public void AddCallBack(uint id, Action<byte[]> callback)
        {
            if (id > 0 && callback != null)
            {
                _callBackMap.Add(id, callback);
            }
        }

        public void InvokeCallBack(uint id, byte[] data)
        {
            Action<byte[]> action = null;
            var foundAction = _callBackMap.TryGetValue(id, out action);

            ClearCallbacks(id);

            if (foundAction) action.Invoke(data);
        }

        private void ClearCallbacks(uint id)
        {
            _callBackMap.Remove(id);
        }

        public void ClearAllCallbacks()
        {
            _callBackMap.Clear();
        }

        // Adds the event to eventMap by name.
        public void AddOnRouteEvent(int routeId, Action<byte[]> callback)
        {
            List<Action<byte[]>> list;
            if (_eventMap.TryGetValue(routeId, out list))
            {
                list.Add(callback);
            }
            else
            {
                list = new List<Action<byte[]>> { callback };
                _eventMap.Add(routeId, list);
            }
        }

        public void RemoveOnRouteEvent(int routeId)
        {
            _eventMap.Remove(routeId);
        }

        public void RemoveAllOnRouteEvents()
        {
            _eventMap.Clear();
        }

        /// <summary>
        /// If the event exists,invoke the event when server return messge.
        /// </summary>
        /// <returns></returns>
        ///
        public void InvokeOnEvent(int routeId, byte[] msg)
        {
            if (!_eventMap.ContainsKey(routeId)) return;

            var list = _eventMap[routeId];
            foreach (var action in list) action.Invoke(msg);
        }

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
            _callBackMap.Clear();
            _eventMap.Clear();
        }
    }
}
