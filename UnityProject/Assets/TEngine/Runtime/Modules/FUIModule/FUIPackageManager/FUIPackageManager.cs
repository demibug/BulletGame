using System.Collections.Generic;
using FairyGUI;

namespace TEngine
{
    internal sealed partial class FUIPackageManager : IMemory
    {
        /// <summary>
        /// 加载UIPackage列表
        /// 图集纹理数量不能超过一个
        /// </summary>
        /// <param name="fuiPackageNames">包名列表</param>
        /// <param name="onAddPackage">加载完成回调代理</param>
        public static void AddPackage(GObject uiObject, string[] fuiPackageNames, System.Action<bool> onAddPackage, string assetsPackageName)
        {
            FUIPackageManager mgr = MemoryPool.Acquire<FUIPackageManager>();
            mgr?.LoadPackages(uiObject, fuiPackageNames, onAddPackage, assetsPackageName);
        }

        /// <summary>
        /// 包是否已加载
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static bool IsLoadedPackage(string packageName)
        {
            return UIPackage.GetByName(packageName) != null;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            m_fuiPackageNames.Clear();
            m_onPackageLoaded = null;
            m_needRelease = false;
        }

        #region Loade package list

        private List<string> m_fuiPackageNames = new List<string>();
        private bool m_needRelease;
        private System.Action<bool> m_onPackageLoaded;

        public void LoadPackages(GObject uiObject, string[] fuiPackages, System.Action<bool> onAddPackages, string assetsPackageName)
        {
            m_needRelease = true;
            m_onPackageLoaded = onAddPackages;
            if (fuiPackages.Length != 0)
            {
                for (int i = 0; i < fuiPackages.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fuiPackages[i]))
                    {
                        m_fuiPackageNames.Add(fuiPackages[i]);
                    }

                }

                for (int i = 0; i < fuiPackages.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fuiPackages[i]))
                    {
                        // 实际加载
                        FUIPackageLoader.AddPackage(uiObject, fuiPackages[i], OnLoadPackage, assetsPackageName);
                    }
                }
            }
            else
            {
                OnLoadPackageComplete(true);
            }
        }

        private void OnLoadPackage(string packageName, bool isComplete)
        {
            // 是否已完成
            if (isComplete)
            {
                // 是否有包名
                if (!string.IsNullOrEmpty(packageName))
                {
                    // 是否已加载
                    if (IsLoadedPackage(packageName))
                    {
                        if (m_fuiPackageNames.IndexOf(packageName) != -1)
                        {
                            m_fuiPackageNames.Remove(packageName);
                        }
                    }
                }

                if (m_fuiPackageNames.Count == 0)
                {
                    OnLoadPackageComplete(true);
                }
            }
            else
            {
                OnLoadPackageComplete(false);
            }

        }

        private void OnLoadPackageComplete(bool isSucceed)
        {
            System.Action<bool> onAddAction = m_onPackageLoaded;
            if (onAddAction != null)
            {
                onAddAction(isSucceed);
            }

            if (m_needRelease)
            {
                MemoryPool.Release(this);
            }
        }

        #endregion

    }
}
