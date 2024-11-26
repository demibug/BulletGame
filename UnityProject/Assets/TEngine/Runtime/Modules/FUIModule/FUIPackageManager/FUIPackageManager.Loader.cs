using System;
using FairyGUI;
using UnityEngine;

namespace TEngine
{
    internal sealed partial class FUIPackageManager
    {
        private sealed class FUIPackageLoader : IMemory
        {
            /// <summary>
            /// 加载一个UIPackage
            /// 图集纹理数量不能超过一个
            /// </summary>
            /// <param name="fuiPackageName">包名</param>
            /// <param name="onAddPackage">加载完成回调代理</param>
            public static void AddPackage(GObject uiObject, string fuiPackageName, Action<string, bool> onAddPackage, string assetsPackageName = "")
            {
                FUIPackageLoader loader = MemoryPool.Acquire<FUIPackageLoader>();
                loader?.LoadPackage(uiObject, fuiPackageName, onAddPackage, assetsPackageName);
                // Log.Debug("AddPackage : " + packageName);
            }

            private GObject m_uiObject;
            private string m_fuiPackageName;
            private string m_assetsPackageName;
            private Action<string, bool> m_onAddPackage;

            private string m_descName;

            /// <summary>
            /// 加载UIPackage
            /// </summary>
            /// <param name="fuiPackageName">UIPackage name</param>
            /// <param name="onAddPackage">成功或失败回调</param>
            public void LoadPackage(GObject uiObject, string fuiPackageName, Action<string, bool> onAddPackage, string assetsPackageName)
            {
                m_uiObject = uiObject;
                m_fuiPackageName = fuiPackageName;
                m_onAddPackage = onAddPackage;
                m_assetsPackageName = assetsPackageName;


                bool isLoadFail = false;

                if (m_uiObject != null && !m_uiObject.isDisposed && m_uiObject.displayObject != null && m_uiObject.displayObject.gameObject != null)
                {
                    // 先加载UI描述文件
                    m_descName = $"{fuiPackageName}_fui";
                    GameModule.Resource.LoadAsset<TextAsset>(m_descName, textAsset => { HandleTextAssetCompleted(textAsset); });
                }
                else
                {
                    Log.Error($"Load UIPackage {m_fuiPackageName} Failed because uiObject is null");
                    isLoadFail = true;
                }

                if (isLoadFail)
                {
                    LoadFailed();
                }
            }

            /// <summary>
            /// 加载描述文件完成
            /// </summary>
            /// <param name="handle"></param>
            private void HandleTextAssetCompleted(TextAsset textAsset)
            {
                bool isLoadFail = false;
                if (textAsset == null)
                {
                    Log.Error($"Load UIPackage: {m_fuiPackageName} Failed because TextAsset AssetOperationHandle is null");
                    isLoadFail = true;
                }
                else
                {
                    if (m_uiObject != null && !m_uiObject.isDisposed && m_uiObject.displayObject != null && m_uiObject.displayObject.gameObject != null)
                    {
                        AssetsReference.Ref(textAsset, m_uiObject.displayObject.gameObject);

                        // 如果已加载包
                        if (UIPackage.GetByName(m_fuiPackageName) != null)
                        {
                            // 加载成功
                            LoadSucceed();
                        }
                        else
                        {
                            // 再加载UI图集文件
                            UIPackage.AddPackage(textAsset.bytes, m_fuiPackageName, LoadTextureAsset);
                            // 加载成功
                            LoadSucceed();
                        }
                    }
                    else
                    {
                        Log.Error($"Load UIPackage {m_fuiPackageName} Failed because uiObject is null");
                        isLoadFail = true;
                    }
                }

                if (isLoadFail)
                {
                    LoadFailed();
                }
            }

            /// <summary>
            /// 异步加载纹理
            /// </summary>
            /// <param name="location">注意，这个name是FGUI内部组装的纹理全名，例如FUILogin_atlas0</param>
            /// <param name="extension"></param>
            /// <param name="type"></param>
            /// <param name="item"></param>
            private void LoadTextureAsset(string location, string extension, Type type, PackageItem item)
            {
                GameModule.Resource.LoadAsset<Texture>(location, texture =>
                {
                    if (texture == null)
                    {
                        Log.Error($"Load {location} Failed, Texture is null");
                    }
                    else
                    {
                        item.owner.SetItemAsset(item, texture, DestroyMethod.Unload);
                    }
                }, m_assetsPackageName);
            }


            /// <summary>
            /// 加载失败
            /// </summary>
            private void LoadFailed()
            {
                var onLoad = m_onAddPackage;
                if (onLoad != null)
                {
                    onLoad(m_fuiPackageName, false);
                }

                // 回收
                MemoryPool.Release(this);
            }

            /// <summary>
            /// 加载成功
            /// </summary>
            private void LoadSucceed()
            {
                var onLoad = m_onAddPackage;
                if (onLoad != null)
                {
                    onLoad(m_fuiPackageName, true);
                }

                MemoryPool.Release(this);
            }

            public void Clear()
            {
                m_onAddPackage = null;
                m_fuiPackageName = null;
                m_descName = null;
            }
        }
    }
}