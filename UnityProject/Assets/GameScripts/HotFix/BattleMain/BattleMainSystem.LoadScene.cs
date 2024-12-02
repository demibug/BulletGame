using GameScene;
using TEngine;
using UnityEngine;

namespace BattleMain
{
    public partial class BattleMainSystem : BehaviourSingleton<BattleMainSystem>
    {
        private string m_sceneRes;

        public void LoadScene(string res)
        {
            m_sceneRes = res;
            DoLoadScene();
        }

        private void DoLoadScene()
        {
            SceneSystem.Instance.LoadScene(m_sceneRes, OnLoadscene);
        }

        private void OnLoadscene(string scene, bool complete)
        {
            if (!complete)
            {
                Log.Error("Load scene fail : " + scene);
            }

            SceneRoot = GameObject.Find("scene_root");
            // TestSystem.Instance.Init();
        }
    }
}