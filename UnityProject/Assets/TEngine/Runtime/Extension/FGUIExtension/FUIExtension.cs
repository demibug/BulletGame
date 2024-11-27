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
                        gloader.texture = new NTexture(sp.texture);
                        
                        FUISpriteReference fguiSpriteRef = gloader.displayObject.gameObject.GetOrAddComponent<FUISpriteReference>();
                        if (fguiSpriteRef != null)
                        {
                            fguiSpriteRef.Reference(location);
                        }
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
                                gloader.texture = new NTexture(sp.texture);
                                AssetsReference.Ref(sp, gloader.displayObject.gameObject);
                                
                                FUISpriteReference fguiSpriteRef = gloader.displayObject.gameObject.GetOrAddComponent<FUISpriteReference>();
                                if (fguiSpriteRef != null)
                                {
                                    fguiSpriteRef.Reference(location);
                                }
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
                                    gloader.texture = new NTexture(sp.texture);
                                    AssetsReference.Ref(sp, gloader.displayObject.gameObject);
                                    
                                    FUISpriteReference fguiSpriteRef = gloader.displayObject.gameObject.GetOrAddComponent<FUISpriteReference>();
                                    if (fguiSpriteRef != null)
                                    {
                                        fguiSpriteRef.Reference(location);
                                    }
                                }
                            }
                        }, packageName);
                    }
                }
            }
        }
    }
}