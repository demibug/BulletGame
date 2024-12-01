using GPUECSAnimationBaker.Engine.AnimatorSystem;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BattleCore
{
    [BurstCompile]
    public partial struct ManSpawnerSystem : ISystem
    {
        private BufferLookup<GpuEcsAnimationDataBufferElement> gpuEcsAnimationDataBufferLookup;
        private ComponentLookup<LocalTransform> localTransformLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            gpuEcsAnimationDataBufferLookup = state.GetBufferLookup<GpuEcsAnimationDataBufferElement>(isReadOnly: true);
            localTransformLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            gpuEcsAnimationDataBufferLookup.Update(ref state);
            localTransformLookup.Update(ref state);
            EndSimulationEntityCommandBufferSystem.Singleton ecbSystem =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer.ParallelWriter ecb
                = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            float deltaTime = SystemAPI.Time.DeltaTime;

            state.Dependency = new ManSpawnerJob()
            {
                ecb = ecb,
                deltaTime = deltaTime,
                gpuEcsAnimationDataBufferLookup = gpuEcsAnimationDataBufferLookup,
                localTransformLookup = localTransformLookup
            }.ScheduleParallel(state.Dependency);
        }


        [BurstCompile]
        private partial struct ManSpawnerJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            [ReadOnly] public float deltaTime;
            [ReadOnly] public BufferLookup<GpuEcsAnimationDataBufferElement> gpuEcsAnimationDataBufferLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> localTransformLookup;

            public void Execute(
                ref ManSpawnerUpdateComponent manSpawnerUpdate,
                in ManSpawnerComponent manSpawner,
                in DynamicBuffer<ManSpawnerAnimatorBufferElement> manSpawnerAnimators,
                in DynamicBuffer<ManSpawnerAnimatorPrefabBufferElement> manSpawnerAnimatorPrefabs,
                Entity manSpawnerEntity, [ChunkIndexInQuery] int sortKey)
            {
                if (manSpawner.needSpawn != manSpawnerUpdate.needSpawn)
                {
                    manSpawnerUpdate.updateTime -= deltaTime;
                    if (manSpawnerUpdate.updateTime <= 0)
                    {
                        // First delete all existing entities
                        // foreach (ManSpawnerAnimatorBufferElement crowdSpawnerAnimator in manSpawnerAnimators)
                        //     ecb.DestroyEntity(sortKey, crowdSpawnerAnimator.gpuEcsAnimator);

                        DynamicBuffer<ManSpawnerAnimatorBufferElement> newManSpawnerAnimators =
                            ecb.SetBuffer<ManSpawnerAnimatorBufferElement>(sortKey, manSpawnerEntity);
                        newManSpawnerAnimators.Clear();

                        // Calculate the base offset so that the square of entities is centered around the origin
                        float3 pos = manSpawnerUpdate.basePos;
                        float2 xOffset = manSpawnerUpdate.xOffset;
                        float2 zOffset = manSpawnerUpdate.xOffset;
                        int total = manSpawnerUpdate.spawnCount;
                        for (int i = 0; i < total; i++)
                        {
                            newManSpawnerAnimators.Add(new ManSpawnerAnimatorBufferElement()
                            {
                                gpuEcsAnimator = CreateNewAnimator(ref manSpawnerUpdate, manSpawner, sortKey, 
                                    pos, xOffset, zOffset,
                                    manSpawnerAnimatorPrefabs)
                            });
                        }
                    
                        ecb.SetComponent<ManSpawnerComponent>(sortKey, manSpawnerEntity, new ManSpawnerComponent()
                        {
                            needSpawn = manSpawnerUpdate.needSpawn,
                            spawnCount = manSpawnerUpdate.spawnCount,
                        });
                    }
                }
            }

            private Entity CreateNewAnimator(ref ManSpawnerUpdateComponent manSpawnerUpdate,
                ManSpawnerComponent manSpawner,
                int sortKey, float3 pos, float2 xOffset, float2 zOffset,
                in DynamicBuffer<ManSpawnerAnimatorPrefabBufferElement> manSpawnerAnimatorPrefabs)
            {
                // Select a random prefab from the available buffer
                // Entity gpuEcsAnimatorPrefab = manSpawnerAnimatorPrefabs[
                //     manSpawnerUpdate.random.NextInt(0, manSpawnerAnimatorPrefabs.Length)].gpuEcsAnimatorPrefab;
                Entity gpuEcsAnimatorPrefab = manSpawnerAnimatorPrefabs[0].gpuEcsAnimatorPrefab;
                // Spawn a character
                Entity gpuEcsAnimator = ecb.Instantiate(sortKey, gpuEcsAnimatorPrefab);

                // set the position according to column, row & spacing values
                // Preserve the scale that was set in the prefab
                ecb.SetComponent(sortKey, gpuEcsAnimator, new LocalTransform()
                {
                    Position = pos + new float3( manSpawnerUpdate.random.NextFloat(xOffset.x, xOffset.y), 
                        0, manSpawnerUpdate.random.NextFloat(zOffset.x, zOffset.y)
                        ),
                    Rotation = Quaternion.identity,
                    Scale = localTransformLookup[gpuEcsAnimatorPrefab].Scale
                });

                // Pick a random animation ID from the available animations
                DynamicBuffer<GpuEcsAnimationDataBufferElement> animationDataBuffer =
                    gpuEcsAnimationDataBufferLookup[gpuEcsAnimatorPrefab];
                int animationID = (int)TestManAnimationId.stand;

                // Kick off the correct animation with a random time offset so to avoid synchronized animations
                ecb.SetComponent(sortKey, gpuEcsAnimator, new GpuEcsAnimatorControlComponent()
                {
                    animatorInfo = new AnimatorInfo()
                    {
                        animationID = animationID,
                        blendFactor = 0,
                        speedFactor = 1f
                    },
                    startNormalizedTime = manSpawnerUpdate.random.NextFloat(0f, 1f),
                    transitionSpeed = 0
                });
                return gpuEcsAnimator;
            }
        }
    }
}