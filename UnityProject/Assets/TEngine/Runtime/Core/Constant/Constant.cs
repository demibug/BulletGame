namespace TEngine
{
    /// <summary>
    /// 资源相关常量。
    /// </summary>
    public static partial class Constant
    {
        /// <summary>
        /// 默认资源加载优先级。
        /// </summary>
        internal const int DefaultPriority = 0;

        public const int DesignWidth = 1920;
        public const int DesignHeight = 1080;

    }

    /// <summary>
    /// 常用设置相关常量。
    /// </summary>
    public static partial class Constant
    {
        public static class Setting
        {
            public const string Language = "Setting.Language";
            public const string SoundGroupMuted = "Setting.{0}Muted";
            public const string SoundGroupVolume = "Setting.{0}Volume";
            public const string MusicMuted = "Setting.MusicMuted";
            public const string MusicVolume = "Setting.MusicVolume";
            public const string SoundMuted = "Setting.SoundMuted";
            public const string SoundVolume = "Setting.SoundVolume";
            public const string UISoundMuted = "Setting.UISoundMuted";
            public const string UISoundVolume = "Setting.UISoundVolume";
        }


        public enum LaunchStep
        {
            None,

            /// <summary>
            /// 游戏启动
            /// </summary>
            Launch,

            /// <summary>
            /// 闪屏
            /// </summary>
            Splash,

            /// <summary>
            /// 初始化资源包
            /// </summary>
            InitPackage,

            /// <summary>
            /// 更新静态版本文件
            /// </summary>
            UpdateVersion, // host,
            InitResources, // offline,host(无网络)

            /// <summary>
            /// 更新资源清单
            /// </summary>
            UpdateManifest, // host,

            /// <summary>
            /// 创建补丁下载器
            /// </summary>
            CreateDownloader, // host,

            /// <summary>
            /// 下载热更新文件
            /// </summary>
            DownloadFile, // host,

            /// <summary>
            /// 下载热更新文件结束
            /// </summary>
            DownloadOver, // host,

            /// <summary>
            /// 清理未使用的缓存文件
            /// </summary>
            ClearCache, // host,
            Preload, // editor, offline

            /// <summary>
            /// 加载热更新代码
            /// </summary>
            Assembly, // editor, offline, host

            /// <summary>
            /// 热更新完成
            /// </summary>
            StartGame, // editor, offline, host

            /// <summary>
            /// 预加载场景
            /// </summary>
            Scene = 100,

            /// <summary>
            /// 预加载开场动画
            /// </summary>
            Video,

            /// <summary>
            /// 关闭loading界面,展示登陆界面
            /// </summary>
            CloseLoading,
        }

        /// <summary>
        /// 当前使用的语言的索引值
        /// </summary>
        public enum GameLanguage
        {
            CN,
            EN,
            AR,
        }

        public const string LauncherSettingLanguage = "LauncherSettingLanguage"; //登录语言语言

        public const string LauncherSettingStage = "LauncherSettingStage"; // 登录场景
    }
}