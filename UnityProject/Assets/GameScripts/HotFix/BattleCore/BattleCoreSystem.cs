using TEngine;
using Unity.Entities;
using UnityEngine;

namespace BattleCore
{
    public partial class BattleCoreSystem : BehaviourSingleton<BattleCoreSystem>
    {
        public GameObject SceneRoot { get; private set; }

        public override void Awake()
        {
        
        }

        public override void Destroy()
        {
        
        }

        public void Init()
        {
            InitWorld();
            LoadScene("Abasi");   
        }

        private void InitWorld()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
            {
                DefaultWorldInitialization.Initialize("Default World", false);
            }
        }
    }
}
