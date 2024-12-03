using Unity.Entities;

namespace BattleMain
{
    public static class BattleCoreSystem
    {
       
        public static void InitWorld()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
            {
                // DefaultWorldInitialization.Initialize("Default World", false);
                // var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                // var tempEntity = entityManager.CreateEntity();
            }
        }
    }
}