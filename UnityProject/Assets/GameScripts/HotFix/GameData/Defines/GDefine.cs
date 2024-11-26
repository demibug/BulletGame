using TEngine;

namespace GameData
{
    namespace GDefine
    {
        /// <summary>
        /// FUI 中的 branch 名称（默认是使用阿语作为主语言）
        /// </summary>
        public sealed class FUIBranch
        {
            public const string Chinese = "CN";
            public const string English = "EN";
            public const string Arabic = "AR";
        }

        /// <summary>
        /// 外部加载的不同语种图片的前缀（仅部份图片需要增加前缀）
        /// </summary>
        public sealed class IMGHead
        {
            public const string Chinese = "cn_";
            public const string English = "en_";
        }

        /// <summary>
        /// 一些 ISO 639-1 的语言定义，与c#中的对应
        /// C#语法： CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        /// </summary>
        public sealed class ISO_639_1
        {
            public const string English = "en"; //英语
            public const string Chinese = "zh"; //中文（含中文简体及繁体）
            public const string Arabic = "ar"; //阿语
            public const string Japanese = "ja"; //日语
            public const string French = "fr"; //法语
            public const string German = "de"; //德语
        }

        /// <summary>
        /// 当前使用的语言的索引值
        /// </summary>
        public enum Language
        {
            CN = Constant.GameLanguage.CN,
            EN = Constant.GameLanguage.EN,
            AR = Constant.GameLanguage.AR,
        }
    }
}
