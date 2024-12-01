using TEngine;
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
            LoadScene("Abasi");   
        }
    }
}
