using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using TEngine;
using UnityEngine;

namespace GameRes
{
    public class ResSystem : BehaviourSingleton<ResSystem>
    {
        public const int LAYER_DEFAULT = 0; // Default
        public const int LAYER_TRANSPARENT_FX = 1; // TransparentFX
        public const int LAYER_HIDE = 2; // Ignore Raycast
        public const int LAYER_WATER = 4; // Water
        public const int LAYER_UI = 5; // FUI

        private Dictionary<string, bool> _mLoadedAsset = new Dictionary<string, bool>();

        /// <summary>
        /// 同步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        public T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            return GameModule.Resource.LoadAsset<T>(location, packageName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="cancellationToken">取消操作Token。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>异步资源实例。</returns>
        public async UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default,
            string packageName = "") where T : UnityEngine.Object
        {

            return await GameModule.Resource.LoadAssetAsync<T>(location, cancellationToken, packageName);
        }

        // private void LoadMaterials(GameObject go)
        // {
        //     MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        //
        //     foreach (var renderer in renderers)
        //     {
        //         if (renderer != null)
        //         {
        //             Material mat = renderer.sharedMaterial;
        //             if (mat != null)
        //             {
        //                 string matName = mat.name.Replace(" (Instance)", "");
        //                 if (!_mLoadedMaterials.ContainsKey(matName))
        //                 {
        //                     _mLoadedMaterials.Add(matName, true);
        //                     GameModule.Resource.LoadAsset<Material>(matName);
        //                 }
        //             }
        //         }
        //     }
        // }

        public static void ForceChangeLayers(GameObject go, int layer)
        {
            if (go != null && go.layer != layer)
            {
                SetLayerIteratively(go, layer);
            }
        }

        public static void SetLayerIteratively(GameObject obj, int newLayer)
        {
            if (obj == null) return;

            // Use a stack to avoid deep recursion which can lead to stack overflow for very large hierarchies
            Stack<Transform> stack = new Stack<Transform>();
            stack.Push(obj.transform);

            while (stack.Count > 0)
            {
                Transform current = stack.Pop();
                GameObject currentGameObject = current.gameObject;

                // Only set the layer if it is different from the target layer
                if (currentGameObject.layer != newLayer)
                {
                    currentGameObject.layer = newLayer;
                }

                // Push all children to the stack
                foreach (Transform child in current)
                {
                    stack.Push(child);
                }
            }
        }
    }
}
