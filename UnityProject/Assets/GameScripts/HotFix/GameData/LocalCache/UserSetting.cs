using System.Collections.Generic;
using GameData.GDefine;

namespace GameData
{
    /// <summary>
    /// 玩家设置的数据信息
    /// </summary>
    public sealed class UserSetting
    {
        public string accountId;                //玩家账号ID
        public string userName;                 //玩家名称
        public string serverId;                 //默认登陆的服务器的ID（仅限内网测试用）
        public Language language;               //玩家当前选择的语言
        public string branch;                   //玩家当前UI的类型
        public bool hasSound;                   //是否打开音效
        public bool hasMusic;                   //是否打开音乐
        public bool hasEmoji;                   //是否显示动态表情
        public int LastEmojiItemId;             //最后一次使用的表情
        public Dictionary<int,long> RedNoteDayDic;        //红点提示的天数列表
        //public long DoubleActivityStarTime;   //双倍活动起始时间缓存(用来检验活动是否是新的)

        private UserSetting() { }

        public static UserSetting CreateInstance()
        {
            var r = new UserSetting
            {
                accountId = string.Empty,
                userName = string.Empty,
                serverId = "1",
                language = Language.AR,
                branch = FUIBranch.Arabic,
                hasSound = true,
                hasMusic = true,
                hasEmoji = true,
                LastEmojiItemId = 0,
                RedNoteDayDic = new Dictionary<int,long>(),
                //DoubleActivityStarTime = 0,
            };

            return r;
        }
    }
}
