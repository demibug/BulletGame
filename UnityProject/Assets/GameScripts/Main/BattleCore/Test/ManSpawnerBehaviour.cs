using GPUECSAnimationBaker.Engine.AnimatorSystem;
using TEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace BattleMain
{
    public class ManSpawnerBehaviour : MonoBehaviour
    {
        public GameObject[] gpuEcsAnimatorPrefabs;
    }

    public class ManSpawnerBaker : Baker<ManSpawnerBehaviour>
    {
        public override void Bake(ManSpawnerBehaviour authoring)
        {
            Log.Warning("Bake");
            Entity entity = GetEntity(TransformUsageFlags.None);
            Log.Warning("entity is " + entity);
            AddComponent(entity, new ManSpawnerComponent()
            {
                needSpawn = false,
                spawnCount =  0,
            });

            AddComponent(entity, new ManSpawnerUpdateComponent()
            {
                needSpawn = false,
                spawnCount =  0,
                basePos = float3.zero,
                xOffset = float2.zero,
                zOffset = float2.zero,
                updateTime = 0,
                random = Random.CreateFromIndex((uint)Mathf.RoundToInt(Time.time))
            });

            DynamicBuffer<ManSpawnerAnimatorPrefabBufferElement> manSpawnerAnimatorPrefabs
                = AddBuffer<ManSpawnerAnimatorPrefabBufferElement>(entity);
            foreach (GameObject gpuEcsAnimatorPrefab in authoring.gpuEcsAnimatorPrefabs)
            {
                manSpawnerAnimatorPrefabs.Add(new ManSpawnerAnimatorPrefabBufferElement()
                {
                    gpuEcsAnimatorPrefab = GetEntity(gpuEcsAnimatorPrefab,
                        gpuEcsAnimatorPrefab.GetComponent<GpuEcsAnimatorBehaviour>().transformUsageFlags)
                });
            }

            AddBuffer<ManSpawnerAnimatorBufferElement>(entity);
        }
    }
}