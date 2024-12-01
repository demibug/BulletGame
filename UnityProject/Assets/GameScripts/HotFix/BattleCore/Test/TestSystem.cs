using Cysharp.Threading.Tasks;
using TEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleCore
{
    public class TestSystem : BehaviourSingleton<TestSystem>
    {
        public override void Awake()
        {
        }

        public override void Destroy()
        {
        }

        public async UniTaskVoid TestMan()
        {
            int testBench = 1000;
            int testCount = 100;
            Vector3 pos = new Vector3();
            for (int j = 0; j < testBench; j++)
            {
                ManSpawnerControlSystem manSpawnerControlSystem =
                    World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<ManSpawnerControlSystem>();
                manSpawnerControlSystem.needSpawn = true;
                manSpawnerControlSystem.spawnCount = testCount;
                manSpawnerControlSystem.basePos = new float3(0, 0.5f, 0);
                manSpawnerControlSystem.xOffset = new float2(-200, 80);
                manSpawnerControlSystem.zOffset = new float2(-250, 70);
                // for (int i = 0; i < testCount; i++)
                // {
                //     int x = Random.Range(-200, 80);
                //     int z = Random.Range(-250, 70);
                //     float y = 0.5f;
                //
                //     pos.Set(x, y, z);
                //     
                //     GameObject go = ResSystem.Instance.LoadGameObject("man_prefab", BattleCoreSystem.Instance.SceneRoot.transform);
                //     go.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
                //     go.transform.localScale = Vector3.one;
                //
                // }

                await UniTask.Yield();
            }
        }
    }
}