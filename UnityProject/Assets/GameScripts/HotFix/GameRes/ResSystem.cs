using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        /// <param name="needInstance">是否需要实例化。</param>
        /// <param name="needCache">是否需要缓存。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        public T LoadAsset<T>(string location, bool needInstance = true, bool needCache = false,
            string customPackageName = "") where T : UnityEngine.Object
        {
            return GameModule.Resource.LoadAsset<T>(location, needInstance, needCache, customPackageName);
        }

        /// <summary>
        /// 同步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="parent">父节点位置。</param>
        /// <param name="needInstance">是否需要实例化。</param>
        /// <param name="needCache">是否需要缓存。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        public T LoadAsset<T>(string location, Transform parent, bool needInstance = true, bool needCache = false,
            string customPackageName = "") where T : UnityEngine.Object
        {
            return GameModule.Resource.LoadAsset<T>(location, parent, needInstance, needCache, customPackageName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="cancellationToken">取消操作Token。</param>
        /// <param name="needInstance">是否需要实例化。</param>
        /// <param name="needCache">是否需要缓存。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>异步资源实例。</returns>
        public async UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default,
            bool needInstance = true, bool needCache = false, string customPackageName = "", Transform parent = null)
            where T : UnityEngine.Object
        {
            // Log.Debug("async load : " + location);
            T t = await GameModule.Resource.LoadAssetAsync<T>(location, cancellationToken, needInstance, needCache, customPackageName, parent);
            if (t != null)
            {
                if (!_mLoadedAsset.ContainsKey(location))
                {
                    _mLoadedAsset.Add(location, true);
                    bool isNeedDelay = false;
                    GameObject go = null;
                    if (typeof(T) == typeof(GameObject))
                    {
                        go = t as GameObject;
                        isNeedDelay = true;
                        // LoadMaterials(go);
                    }
                    else if (typeof(T) == typeof(Transform))
                    {
                        Transform trans = t as Transform;
                        go = trans.gameObject;
                        isNeedDelay = true;
                        // LoadMaterials(trans.gameObject);
                    }

                    if (isNeedDelay)
                    {
                        Transform trans = go.transform;
                        Animator animator = trans.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.enabled = false;
                        }

                        Animator[] anis = trans.GetComponentsInChildren<Animator>();
                        if (anis != null)
                        {
                            foreach (var ani in anis)
                            {
                                ani.enabled = false;
                            }
                        }

                        ParticleSystem[] pss = trans.GetComponentsInChildren<ParticleSystem>();
                        if (pss != null)
                        {
                            foreach (var ps in pss)
                            {
                                if (ps.gameObject.activeInHierarchy)
                                {
                                    ps.Stop();
                                }
                            }
                        }

                        trans.localScale = Vector3.zero;
                        await UniTask.WaitForSeconds(0.1f);
                        trans.localScale = Vector3.zero;
                        await UniTask.Yield();
                        if (cancellationToken.IsCancellationRequested)
                        {
                            GameObject.Destroy(t);
                            t = null;
                        }
                        else
                        {
                            if (trans != null)
                            {
                                if (animator != null)
                                {
                                    animator.enabled = true;
                                }

                                if (anis != null)
                                {
                                    foreach (var ani in anis)
                                    {
                                        if (ani != null)
                                        {
                                            ani.enabled = true;
                                        }
                                    }
                                }

                                if (pss != null)
                                {
                                    foreach (var ps in pss)
                                    {
                                        if (ps.gameObject.activeInHierarchy)
                                        {
                                            ps.Play();
                                        }
                                    }
                                }

                                trans.localScale = Vector3.one;
                            }
                        }
                    }
                }
            }


            return t;
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
