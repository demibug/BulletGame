using BattleMain;
using TEngine;
using Unity.Entities;
using Unity.Mathematics;

public static class TestSystem
{
    private static bool isCall = false;

    public static void Update()
    {
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            Log.Error("World.DefaultGameObjectInjectionWorld is null");
            return;
        }
        else
        {
            var entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(
                typeof(ManSpawnerUpdateComponent));
        
            if (entityQuery.CalculateEntityCount() == 0)
            {
                Log.Error("entityQuery CalculateEntityCount is 0");
                return;
            }
        }

        if (!isCall)
        {
            isCall = true;
            TestMan();
        }
    }


    public static void TestMan()
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