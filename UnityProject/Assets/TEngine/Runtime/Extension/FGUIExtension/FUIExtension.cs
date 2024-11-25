using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    public static class FUIExtension
    {
        private static Dictionary<string, int> _mImgRef = new Dictionary<string, int>();

        private static Dictionary<GLoader, string> _dicImgUrlCache = new();

        /// <summary>
        /// 加载一图外部图片
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="imgName"></param>
        public static void SetImage(GLoader loader, string imgName, bool isFromResources = false)
        {
            if (loader == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(imgName))
            {
                loader.texture = null;

                if (_dicImgUrlCache.ContainsKey(loader))
                {
                    RemoveImgRef(_dicImgUrlCache[loader]);
                }

                _dicImgUrlCache.Remove(loader);
            }
            else
            {
                if (_dicImgUrlCache.ContainsKey(loader))
                {
                    string oldUrl = _dicImgUrlCache[loader];
                    if (oldUrl != imgName)
                    {
                        RemoveImgRef(oldUrl);
                    }

                    _dicImgUrlCache[loader] = imgName;
                }
                else
                {
                    _dicImgUrlCache[loader] = imgName;
                }

                if (imgName.Length > 0)
                {
                    AddImgRef(imgName);
                }

                if (isFromResources)
                {
                    Sprite sp = Resources.Load<Sprite>(imgName);
                    if (sp != null)
                    {
                        //AddImgRef(imgName);
                        loader.texture = new NTexture(sp.texture);
                    }
                }
                else
                {
                    GameModule.Resource.LoadAssetAsync<Sprite>(imgName, operation =>
                    {
                        //如果图片加载完毕后对象已经不存在，则不进行处理
                        if (loader == null || loader.displayObject == null || loader.displayObject.gameObject == null)
                        {
                            operation.Dispose();
                            operation = null;
                            return;
                        }

                        AssetReference assetRef = loader.displayObject.gameObject.GetOrAddComponent<AssetReference>();
                        if (assetRef != null)
                        {
                            assetRef.Reference(operation, imgName);
                        }

                        //将加载好的图片赋值给加载器
                        var spr = operation.AssetObject as Sprite;
                        if (spr != null)
                        {
                            loader.texture = new NTexture(spr.texture);
                        }
                    });
                }
            }
        }


        /// <summary>
        /// 重新设置UI的 zIndex 值
        /// </summary>
        /// <param name="go"></param>
        /// <param name="zIndex"></param>
        public static void SetUIZIndex(GObject go, int zIndex)
        {
            if (go != null)
            {
                go.displayObject.gameObject.transform.localPosition += new Vector3(0, 0, zIndex);
            }
        }

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