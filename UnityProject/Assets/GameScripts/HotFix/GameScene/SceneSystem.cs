using System;
using TEngine;
using UnityEngine;
using YooAsset;

namespace GameScene
{
    public class SceneSystem : BehaviourSingleton<SceneSystem>
    {
        private string _mSceneRes;
        private Action<string, bool> _mOnLoadSceneAction;

        public void LoadScene(string res, Action<string, bool> onLoadScene)
        {
            if (string.IsNullOrEmpty(res))
            {
                if (onLoadScene != null)
                {
                    onLoadScene(res, false);
                    return;
                }
            }

            _mSceneRes = res;
            _mOnLoadSceneAction = onLoadScene;
            SceneOperationHandle handle = GameModule.Scene.LoadScene("EmptyScene");
            handle.Completed += OnLoadEmptyComplete;
        }

        private void OnLoadEmptyComplete(SceneOperationHandle handle)
        {
            SceneOperationHandle sceneOperationHandle = GameModule.Scene.LoadScene(_mSceneRes);
            sceneOperationHandle.Completed += OnLoadSceneComplete;
        }

        private void OnLoadSceneComplete(SceneOperationHandle handle)
        {
            bool complete = false;
            if (handle == null || !handle.IsDone)
            {
                Log.Error($"Load Scene: {_mSceneRes} Failed because SceneOperationHandle is null or handle IsDone is false");
            }
            else if (!handle.SceneObject.isLoaded)
            {
                Log.Error($"Load Scene: {_mSceneRes} Failed because SceneObject isLoaded is false");
            }
            else
            {
                complete = true;
            }

            if (_mOnLoadSceneAction != null)
            {
                _mOnLoadSceneAction(_mSceneRes, complete);
                _mOnLoadSceneAction = null;
            }
        }


        public void ApplyEnvSetting(SceneEnvSetting setting)
        {
            RenderSettings.ambientMode = setting.AmbientMode;
            RenderSettings.ambientLight = setting.AmbientLight;
            RenderSettings.ambientSkyColor = setting.AmbientSkyColor;
            RenderSettings.ambientEquatorColor = setting.AmbientEquatorColor;
            RenderSettings.ambientGroundColor = setting.AmbientGroundColor;

            RenderSettings.fog = setting.FogEnable;
            RenderSettings.fogColor = setting.FogColor;
            RenderSettings.fogMode = setting.FogMode;
            RenderSettings.fogStartDistance = setting.FogStartDistance;
            RenderSettings.fogEndDistance = setting.FogEndDistance;
        }
    }
}
