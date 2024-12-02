using TEngine;
using UnityEngine;

namespace BattleMain
{
    [Update]
    public partial class BattleMainSystem : BehaviourSingleton<BattleMainSystem>
    {
        public GameObject SceneRoot { get; private set; }

        public override void Awake()
        {
        
        }

        public override void Destroy()
        {
        
        }

        public override void Update()
        {
            TestSystem.Update();
        }

        public void Init()
        {
            BattleCoreSystem.InitWorld();
            LoadScene("Abasi");   
        }
    }
}
