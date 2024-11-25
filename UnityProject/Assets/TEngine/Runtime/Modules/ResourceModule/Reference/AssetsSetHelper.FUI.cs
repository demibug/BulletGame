using FairyGUI;

namespace TEngine
{
    public static partial class AssetsSetHelper
    {

        public static void SetTexture(this GLoader gloader, string location, bool isAsync = false, string packageName = "", bool isFromResources = false)
        {
            if (gloader == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(location))
            {
                gloader.texture = null;


            }
        }




        ///// <summary>
        ///// 加载一图外部图片
        ///// </summary>
        ///// <param name="loader"></param>
        ///// <param name="imageRes"></param>
        //public static void SetImage(GLoader loader, string imageRes, string packageName = "", bool isFromResources = false)
        //{
        //    if (loader == null)
        //    {
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(imageRes))
        //    {
        //        loader.texture = null;

        //        if (_dicImgUrlCache.ContainsKey(loader))
        //        {
        //            RemoveImgRef(_dicImgUrlCache[loader]);
        //        }

        //        _dicImgUrlCache.Remove(loader);
        //    }
        //    else
        //    {
        //        if (_dicImgUrlCache.ContainsKey(loader))
        //        {
        //            string oldUrl = _dicImgUrlCache[loader];
        //            if (oldUrl != imageRes)
        //            {
        //                RemoveImgRef(oldUrl);
        //            }

        //            _dicImgUrlCache[loader] = imageRes;
        //        }
        //        else
        //        {
        //            _dicImgUrlCache[loader] = imageRes;
        //        }

        //        if (imageRes.Length > 0)
        //        {
        //            AddImgRef(imageRes);
        //        }

        //        if (isFromResources)
        //        {
        //            Sprite sp = Resources.Load<Sprite>(imageRes);
        //            if (sp != null)
        //            {
        //                //AddImgRef(imgName);
        //                loader.texture = new NTexture(sp.texture);
        //            }
        //        }
        //        else
        //        {
        //            if (loader != null && loader.displayObject != null && loader.displayObject.gameObject != null)
        //            {
        //                Sprite sp = GameModule.Resource.LoadAsset<Sprite>(imageRes, packageName);
        //                if (sp != null)
        //                {
        //                    loader.texture = new NTexture(sp.texture);
        //                }

        //            }


        //            GameModule.Resource.LoadAssetAsync<Sprite>(imageRes, operation =>
        //            {
        //                //如果图片加载完毕后对象已经不存在，则不进行处理
        //                if (loader == null || loader.displayObject == null || loader.displayObject.gameObject == null)
        //                {
        //                    operation.Dispose();
        //                    operation = null;
        //                    return;
        //                }

        //                AssetReference

        //                AssetReference assetRef = loader.displayObject.gameObject.GetOrAddComponent<AssetReference>();
        //                if (assetRef != null)
        //                {
        //                    assetRef.Reference(operation, imageRes);
        //                }

        //                //将加载好的图片赋值给加载器
        //                var spr = operation.AssetObject as Sprite;
        //                if (spr != null)
        //                {
        //                    loader.texture = new NTexture(spr.texture);
        //                }
        //            });
        //        }
        //    }
        //}
        //#region SetMaterial

        //public static void SetMaterial(this Image image, string location, bool isAsync = false, string packageName = "")
        //{
        //    if (image == null)
        //    {
        //        throw new GameFrameworkException($"SetSprite failed. Because image is null.");
        //    }

        //    CheckResourceManager();

        //    if (!isAsync)
        //    {
        //        Material material = _resourceManager.LoadAsset<Material>(location, packageName);
        //        image.material = material;
        //        AssetsReference.Ref(material, image.gameObject);
        //    }
        //    else
        //    {
        //        _resourceManager.LoadAsset<Material>(location, material =>
        //        {
        //            if (image == null || image.gameObject == null)
        //            {
        //                _resourceManager.UnloadAsset(material);
        //                material = null;
        //                return;
        //            }

        //            image.material = material;
        //            AssetsReference.Ref(material, image.gameObject);
        //        }, packageName);
        //    }
        //}

        //public static void SetMaterial(this SpriteRenderer spriteRenderer, string location, bool isAsync = false, string packageName = "")
        //{
        //    if (spriteRenderer == null)
        //    {
        //        throw new GameFrameworkException($"SetSprite failed. Because image is null.");
        //    }

        //    CheckResourceManager();

        //    if (!isAsync)
        //    {
        //        Material material = _resourceManager.LoadAsset<Material>(location, packageName);
        //        spriteRenderer.material = material;
        //        AssetsReference.Ref(material, spriteRenderer.gameObject);
        //    }
        //    else
        //    {
        //        _resourceManager.LoadAsset<Material>(location, material =>
        //        {
        //            if (spriteRenderer == null || spriteRenderer.gameObject == null)
        //            {
        //                _resourceManager.UnloadAsset(material);
        //                material = null;
        //                return;
        //            }

        //            spriteRenderer.material = material;
        //            AssetsReference.Ref(material, spriteRenderer.gameObject);
        //        }, packageName);
        //    }
        //}

        //public static void SetMaterial(this MeshRenderer meshRenderer, string location, bool needInstance = true, bool isAsync = false, string packageName = "")
        //{
        //    if (meshRenderer == null)
        //    {
        //        throw new GameFrameworkException($"SetSprite failed. Because image is null.");
        //    }

        //    CheckResourceManager();

        //    if (!isAsync)
        //    {
        //        Material material = _resourceManager.LoadAsset<Material>(location, packageName);
        //        meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
        //        AssetsReference.Ref(material, meshRenderer.gameObject);
        //    }
        //    else
        //    {
        //        _resourceManager.LoadAsset<Material>(location, material =>
        //        {
        //            if (meshRenderer == null || meshRenderer.gameObject == null)
        //            {
        //                _resourceManager.UnloadAsset(material);
        //                material = null;
        //                return;
        //            }

        //            meshRenderer.material = needInstance ? Object.Instantiate(material) : material;
        //            AssetsReference.Ref(material, meshRenderer.gameObject);
        //        }, packageName);
        //    }
        //}

        //public static void SetSharedMaterial(this MeshRenderer meshRenderer, string location, bool isAsync = false, string packageName = "")
        //{
        //    if (meshRenderer == null)
        //    {
        //        throw new GameFrameworkException($"SetSprite failed. Because image is null.");
        //    }

        //    CheckResourceManager();

        //    if (!isAsync)
        //    {
        //        Material material = _resourceManager.LoadAsset<Material>(location, packageName);
        //        meshRenderer.sharedMaterial = material;
        //        AssetsReference.Ref(material, meshRenderer.gameObject);
        //    }
        //    else
        //    {
        //        _resourceManager.LoadAsset<Material>(location, material =>
        //        {
        //            if (meshRenderer == null || meshRenderer.gameObject == null)
        //            {
        //                _resourceManager.UnloadAsset(material);
        //                material = null;
        //                return;
        //            }

        //            meshRenderer.sharedMaterial = material;
        //            AssetsReference.Ref(material, meshRenderer.gameObject);
        //        }, packageName);
        //    }
        //}

        //#endregion
    }
}