using Unity.Entities;
using Unity.Mathematics;

namespace BattleMain
{
    public partial class ManSpawnerControlSystem : SystemBase
    {
        public bool needSpawn;
        public int spawnCount;
        public float3 basePos;
        public float2 xOffset;
        public float2 zOffset;

        protected override void OnUpdate()
        {
            if (needSpawn)
            {
                needSpawn = false;
                Entities.ForEach((ref ManSpawnerUpdateComponent manSpawnerUpdate) =>
                {
                    manSpawnerUpdate.needSpawn = true;
                    manSpawnerUpdate.spawnCount = spawnCount;
                    manSpawnerUpdate.basePos = basePos;
                    manSpawnerUpdate.xOffset = xOffset;
                    manSpawnerUpdate.zOffset = zOffset;
                    manSpawnerUpdate.updateTime = 0.5f;
                }).WithoutBurst().Run();
            }
        }
    }
}