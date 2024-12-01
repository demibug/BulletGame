using Unity.Entities;
using Unity.Mathematics;

namespace BattleCore
{
    public struct ManSpawnerComponent : IComponentData
    {
        public bool needSpawn;
        public int spawnCount;
    }

    public struct ManSpawnerUpdateComponent : IComponentData
    {
        public bool needSpawn;
        public int spawnCount;
        public float3 basePos;
        public float2 xOffset;
        public float2 zOffset;
        public Random random;
        public float updateTime;
    }

    public struct ManSpawnerAnimatorPrefabBufferElement : IBufferElementData
    {
        public Entity gpuEcsAnimatorPrefab;
    }

    public struct ManSpawnerAnimatorBufferElement : IBufferElementData
    {
        public Entity gpuEcsAnimator;
    }
    
}