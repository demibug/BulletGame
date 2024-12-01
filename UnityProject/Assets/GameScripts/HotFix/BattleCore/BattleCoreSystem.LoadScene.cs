using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace BattleCore
{
    public partial class BattleCoreSystem : BehaviourSingleton<BattleCoreSystem>
    {
        private string m_sceneRes;
        public void LoadScene(string res)
        {
            m_sceneRes = res;
            DoLoadScene();
        }

        private void DoLoadScene()
        {
            GameScene.SceneSystem.Instance.LoadScene(m_sceneRes, OnLoadscene);
        }

        private void OnLoadscene(string scene, bool complete)
        {
            if (!complete)
            {
                Log.Error("Load scene fail : " + scene);
            }
            
            SceneRoot = GameObject.Find("scene_root");
            TestSystem.Instance.TestMan().Forget();
        }
    }
}

