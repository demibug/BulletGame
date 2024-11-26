using TEngine;

namespace GameData
{
    public static class LauncherSetting
    {
        private static string LauncherSettingStage => GameDefine.LauncherSettingStage;

        public static int CurrentStage
        {
            get
            {
                int stage = 0;
                if (GameModule.Setting.HasSetting(LauncherSettingStage))
                {
                    stage = GameModule.Setting.GetInt(LauncherSettingStage);
                }

                return stage;
            }

            set
            {
                if (value < 0) value = 0;
                if (GameModule.Setting.HasSetting(LauncherSettingStage))
                {
                    int stage = GameModule.Setting.GetInt(LauncherSettingStage);
                    if (stage != value)
                    {
                        GameModule.Setting.SetInt(LauncherSettingStage, value);
                        GameModule.Setting.Save();
                    }
                }
                else
                {
                    GameModule.Setting.SetInt(LauncherSettingStage, value);
                    GameModule.Setting.Save();
                }
            }
        }
    }
}
