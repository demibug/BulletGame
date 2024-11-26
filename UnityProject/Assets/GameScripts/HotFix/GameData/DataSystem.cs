using GameBase;

namespace GameData
{
    public class DataSystem : Singleton<DataSystem>
    {
        private LoginData _mLoginData;
        private PlayerData _mPlayerData;

        /// <summary>
        ///     登录数据
        /// </summary>
        public LoginData Login => _mLoginData ??= new LoginData();

        /// <summary>
        ///     玩家数据
        /// </summary>
        public PlayerData Player => _mPlayerData ??= new PlayerData();
    }
}
