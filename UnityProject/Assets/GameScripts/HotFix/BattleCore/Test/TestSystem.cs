using Cysharp.Threading.Tasks;
using TEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCore
{
    [Update]
    public class TestSystem : BehaviourSingleton<TestSystem>
    {
        public void Init()
        {
            
        }
        private bool isCall = false;
        public override void Awake()
        {
        }

        public override void Destroy()
        {
        }

        public override void Update()
        {
            if (World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(
                    typeof(ManSpawnerUpdateComponent)).CalculateEntityCount() == 0)
            {
                return;
            }

            if (!isCall)
            {
                isCall = true;
                TestMan();
            }
        }

        public void TestMan()
        {
            ManSpawnerControlSystem manSpawnerControlSystem =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<ManSpawnerControlSystem>();
            manSpawnerControlSystem.needSpawn = true;
            manSpawnerControlSystem.spawnCount = 100000;
            manSpawnerControlSystem.basePos = new float3(0, 0.5f, 0);
            manSpawnerControlSystem.xOffset = new float2(-200, 80);
            manSpawnerControlSystem.zOffset = new float2(-250, 70);
        }
    }
}