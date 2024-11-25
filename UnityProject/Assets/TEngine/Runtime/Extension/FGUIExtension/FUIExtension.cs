using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public static class FUIExtension
    {
        private static Dictionary<string, int> _mImgRef = new Dictionary<string, int>();

        /// <summary>
        /// 卸载从Resources加载的资源
        /// </summary>
        public static void ReleaseImage(string imgName)
        {
            RemoveImgRef(imgName);
        }

        private static void AddImgRef(string imgName)
        {
            if (_mImgRef.ContainsKey(imgName))
            {
                int cnt = _mImgRef[imgName];
                _mImgRef[imgName] = ++cnt;
            }
            else
            {
                _mImgRef.Add(imgName, 1);
            }
        }

        private static void RemoveImgRef(string imgName)
        {
            if (_mImgRef.ContainsKey(imgName))
            {
                int cnt = _mImgRef[imgName];
                cnt -= 1;
                if (cnt <= 0)
                {
                    _mImgRef.Remove(imgName);
                    Sprite sp = Resources.Load<Sprite>(imgName);
                    if (sp != null)
                    {
                        Resources.UnloadAsset(sp);
                    }
                }
                else
                {
                    _mImgRef[imgName] = cnt;
                }
            }
        }
    }
}