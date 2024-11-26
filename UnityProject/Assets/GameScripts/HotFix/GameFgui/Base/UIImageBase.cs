using FairyGUI;
using System.Collections.Generic;
using TEngine;

namespace PlayCore
{
    public abstract class UIImageBase
    {
        private Dictionary<GLoader, string> _dicImgUrlCache = new();

        protected void SetImage(GLoader loader, string imgName, bool isFromResources = false)
        {
            if (loader != null)
            {
                if (string.IsNullOrEmpty(imgName))
                {
                    if (_dicImgUrlCache.ContainsKey(loader))
                    {
                        FUIExtension.ReleaseImage(_dicImgUrlCache[loader]);
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
                            FUIExtension.ReleaseImage(oldUrl);
                        }
                        _dicImgUrlCache[loader] = imgName;
                    }
                    else
                    {
                        _dicImgUrlCache[loader] = imgName;
                    }
                }

                loader.SetFuiTexture(imgName, isFromResources);
            }
        }

        protected void ReleaseFuiImage()
        {
            foreach (KeyValuePair<GLoader, string> keyValuePair in _dicImgUrlCache)
            {
                string res = keyValuePair.Value;
                FUIExtension.ReleaseImage(res);
            }
        }

        public virtual void Dispose()
        {
            ReleaseFuiImage();
        }
    }
}
