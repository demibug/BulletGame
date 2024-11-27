using GameBase;
using GameConfig.General;
using TEngine;
using UnityEngine;

namespace GameConfig
{
    public class JSONConfig : Singleton<JSONConfig>
    {
        private bool m_isInited = false;

        /// <summary>
        /// 全局表
        /// </summary>
        public GeneralConfig General { get; private set; } = null;


        public void InitGlobal()
        {
            if (m_isInited) return;

            m_isInited = true;

            TextAsset ta = GameModule.Resource.LoadAsset<TextAsset>("General");
            OnGlobalLoadComplete(ta);
        }

        private void OnGlobalLoadComplete(TextAsset ta)
        {
            var assetName = ta.name;
            Log.Debug($"LoadAssetSuccess, assetName: [ {assetName} ]");

            var textAsset = ta;
            if (textAsset == null)
            {
                Log.Warning($"Load text asset [ {assetName} ] failed.");
                return;
            }

            //全局表赋值
            General = Utility.Json.ToObject<GeneralConfig>(textAsset.text);
        }
    }
}
