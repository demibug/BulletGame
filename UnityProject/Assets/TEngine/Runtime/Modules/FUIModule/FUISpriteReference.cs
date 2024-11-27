using UnityEngine;
using System.Collections.Generic;

namespace TEngine
{
    /// <summary>
    /// 资源引用。
    /// </summary>
    /// <remarks>用于绑定资源引用关系。</remarks>
    [DisallowMultipleComponent, AddComponentMenu("")]
    internal sealed class FUISpriteReference : MonoBehaviour
    {
        private static Dictionary<string, int> m_locationRefCnt = new Dictionary<string, int>();
        private string m_location;

        private static void AddSpriteRef(string location)
        {
            if (m_locationRefCnt.ContainsKey(location))
            {
                int cnt = m_locationRefCnt[location];
                m_locationRefCnt[location] = ++cnt;
            }
            else
            {
                m_locationRefCnt.Add(location, 1);
            }
        }

        /// <summary>
        /// 卸载从Resources加载的资源
        /// </summary>
        public static void RemoveSpriteRef(string location)
        {
            if (m_locationRefCnt.ContainsKey(location))
            {
                int cnt = m_locationRefCnt[location];
                cnt -= 1;
                if (cnt <= 0)
                {
                    m_locationRefCnt.Remove(location);
                    Sprite sp = Resources.Load<Sprite>(location);
                    if (sp != null)
                    {
                        Resources.UnloadAsset(sp);
                    }
                }
                else
                {
                    m_locationRefCnt[location] = cnt;
                }
            }
        }

        public void Reference(string location)
        {
            if (m_location != location)
            {
                if (!string.IsNullOrEmpty(m_location))
                {
                    if (m_locationRefCnt.ContainsKey(m_location))
                    {
                        RemoveSpriteRef(m_location);
                    }
                }
                m_location = location;

                if (!string.IsNullOrEmpty(m_location))
                {
                    AddSpriteRef(m_location);
                }
            }

        }
        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(m_location))
            {
                RemoveSpriteRef(m_location);
                m_location = null;
            }
        }
    }
}
