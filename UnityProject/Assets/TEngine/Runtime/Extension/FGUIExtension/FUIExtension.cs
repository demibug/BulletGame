using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    public static class FUIExtension
    {
        private static Dictionary<string, int> _mImgRef = new Dictionary<string, int>();

        /// <summary>
        /// 加载一张外部图片
        /// </summary>
        /// <param name="gloader"></param>
        /// <param name="location"></param>
        /// <param name="isAsync"></param>
        /// <param name="packageName"></param>
        /// <param name="isFromResources"></param>
        /// <returns></returns>
        public static void SetFuiTexture(GLoader gloader, string location, bool isAsync = false, string packageName = "", bool isFromResources = false)
        {
            if (gloader == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(location))
            {
                gloader.texture = null;
            }
            else
            {
                if (isFromResources)
                {
                    Sprite sp = Resources.Load<Sprite>(location);
                    if (sp != null)
                    {
                        AddImgRef(location);
                        gloader.texture = new NTexture(sp.texture);
                    }
                }
                else
                {
                    if (!isAsync)
                    {
                        if (gloader != null && gloader.displayObject != null && gloader.displayObject.gameObject != null)
                        {
                            Sprite sp = GameModule.Resource.LoadAsset<Sprite>(location, packageName);
                            if (sp != null)
                            {
                                AddImgRef(location);
                                gloader.texture = new NTexture(sp.texture);
                                AssetsReference.Ref(sp, gloader.displayObject.gameObject);
                            }
                        }
                    }
                    else
                    {
                        GameModule.Resource.LoadAsset<Sprite>(location, sp =>
                        {
                            if (sp != null)
                            {
                                //如果图片加载完毕后对象已经不存在，则不进行处理
                                if (gloader == null || gloader.displayObject == null || gloader.displayObject.gameObject == null)
                                {
                                    GameModule.Resource.UnloadAsset(sp);
                                    sp = null;
                                    return;
                                }
                                else
                                {
                                    AddImgRef(location);
                                    gloader.texture = new NTexture(sp.texture);
                                    AssetsReference.Ref(sp, gloader.displayObject.gameObject);
                                }
                            }
                        }, packageName);
                    }
                }
            }
        }

        /// <summary>
        /// 卸载从Resources加载的资源
        /// </summary>
        public static void ReleaseImage(string location)
        {
            RemoveImgRef(location);
        }

        private static void AddImgRef(string location)
        {
            if (!string.IsNullOrEmpty(location))
            {
                if (_mImgRef.ContainsKey(location))
                {
                    int cnt = _mImgRef[location];
                    _mImgRef[location] = ++cnt;
                }
                else
                {
                    _mImgRef.Add(location, 1);
                }
            }
        }

        private static void RemoveImgRef(string location)
        {
            if (_mImgRef.ContainsKey(location))
            {
                int cnt = _mImgRef[location];
                cnt -= 1;
                if (cnt <= 0)
                {
                    _mImgRef.Remove(location);
                    Sprite sp = Resources.Load<Sprite>(location);
                    if (sp != null)
                    {
                        Resources.UnloadAsset(sp);
                    }
                }
                else
                {
                    _mImgRef[location] = cnt;
                }
            }
        }
    }
}