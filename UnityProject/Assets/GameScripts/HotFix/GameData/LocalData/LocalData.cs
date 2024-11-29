using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GameData.GDefine;
using TEngine;
using UnityEngine;
using Language = GameData.GDefine.Language;

namespace GameData
{
    public static class LocalData
    {
        private const string KEY_PC_LOGIN_DATA = "pcLoginData"; //当前登陆的用户信息默认信息
        private const string KEY_ALL_USER = "allUserKeys";
        private const string KEY_USER_HEAD = "user_";

        private static Dictionary<string, UserSetting> dicSetting = new Dictionary<string, UserSetting>();
        private static UserSetting defSetting = null;

        private static string m_currentUser = string.Empty; //当前的用户

        public static void InitCacheData()
        {
            //取得本地所有用户的数据信息
            string allUsers = GetData(KEY_ALL_USER);
            if (allUsers.Length > 0)
            {
                string[] lstUsers = allUsers.Split(',');
                foreach (var user in lstUsers)
                {
                    UserSetting setInfo = GetObject<UserSetting>(user);
                    if (setInfo == null)
                    {
                        setInfo = UserSetting.CreateInstance();
                    }

                    dicSetting.Add(user, setInfo);
                }
            }

            //Debug模式下，读取当前本地的缓存信息
            defSetting = GetObject<UserSetting>(KEY_PC_LOGIN_DATA);
            if (defSetting == null)
            {
                //如果本地数据不存在，则创建新的数据项
                defSetting = UserSetting.CreateInstance();
                defSetting.language = GetDefaultLanguage(defSetting.language);

                WriteObject(KEY_PC_LOGIN_DATA, defSetting);
            }

            //测试部份赋值，OK
            //UpdateUserSetting("abc1000", hasMusiic: true);
        }


        public static UserSetting GetDefaultSetting()
        {
            return defSetting;
        }

        public static void UpdateDefaultSetting()
        {
            if (defSetting != null)
            {
                WriteObject(KEY_PC_LOGIN_DATA, defSetting);
            }
        }

        /// <summary>
        /// 设置当前成功登陆的用户ID
        /// </summary>
        /// <param name="userAccount"></param>
        public static void SetCurrentAccount(string userAccount)
        {
            m_currentUser = userAccount;
        }

        /// <summary>
        /// 取得当前登陆的用户设置
        /// </summary>
        /// <returns></returns>
        public static UserSetting GetCurrentSetting()
        {
            UserSetting setting = GetUserSetting(m_currentUser);
            if (setting == null)
            {
                return defSetting;
            }
            return setting;
        }

        /// <summary>
        /// 设置表情
        /// </summary>
        /// <param name="itemId"></param>
        public static void SetData_LastEmoji(int itemId)
        {
            defSetting.LastEmojiItemId = itemId;
            UpdateDefaultSetting();
        }

        /// <summary>
        /// 获取表情
        /// </summary>
        /// <returns></returns>
        public static int GetData_LastEmoji()
        {
            return defSetting.LastEmojiItemId;
        }

        /// <summary>
        /// 设置每日红点本地缓存
        /// </summary>
        public static void SetData_RedNoteDay(int RedNoteNotify)
        {
            if (defSetting.RedNoteDayDic == null) defSetting.RedNoteDayDic = new Dictionary<int, long>();
            if (!defSetting.RedNoteDayDic.ContainsKey(RedNoteNotify))
            {
                defSetting.RedNoteDayDic.Add(RedNoteNotify, GTimer.Instance.ServerTime);
                UpdateDefaultSetting();
                UpdateUserSetting();
            }
        }

        /// <summary>
        /// 获取每日红点本地缓存
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, long> GetData_RedNoteDay()
        {
            if (defSetting.RedNoteDayDic == null) return new Dictionary<int, long>();
            return defSetting.RedNoteDayDic;
        }

        /// <summary>
        /// 更新默认的配置
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strServerId"></param>
        /// <param name="language"></param>
        /// <param name="hasMusiic"></param>
        /// <param name="hasSound"></param>
        public static void UpdateDefaultSetting(string strUserName,
            string strServerId,
            Language language = Language.AR,
            bool? hasMusiic = null,
            bool? hasSound = null,
            bool? hasEmoji = null,
            int LastEmojiItemId = 0,
            Dictionary<int, long> RedNoteDayDic = null
            )
        {
            if (defSetting != null)
            {
                defSetting.accountId = strUserName;
                defSetting.userName = strUserName;
                defSetting.serverId = strServerId;
                defSetting.language = language;
                defSetting.hasMusic = hasMusiic ?? defSetting.hasMusic;
                defSetting.hasSound = hasSound ?? defSetting.hasSound;
                defSetting.hasEmoji = hasEmoji ?? defSetting.hasEmoji;
                defSetting.branch = GetFUIBrance(language);
                defSetting.LastEmojiItemId = LastEmojiItemId;
                defSetting.RedNoteDayDic = RedNoteDayDic ?? defSetting.RedNoteDayDic;

                //更新默认信息
                WriteObject(KEY_PC_LOGIN_DATA, defSetting);

                //更新记载的玩家信息
                UpdateUserSetting(strUserName, strUserName, strServerId, language, hasMusiic, hasSound, hasEmoji, LastEmojiItemId, RedNoteDayDic);
            }
        }

        public static string GetFUIBrance(Language language)
        {
            switch (language)
            {
                case Language.AR: return FUIBranch.Arabic;
                case Language.EN: return FUIBranch.English;
            }

            return FUIBranch.Arabic;
        }

        /// <summary>
        /// 取得当前手机系统中默认的语言
        /// </summary>
        /// <returns></returns>
        public static Language GetDefaultLanguage()
        {
            string languageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            switch (languageCode)
            {
                case ISO_639_1.English:
                    return Language.EN;

                case ISO_639_1.Chinese:
                    return Language.CN;

                default:
                    return Language.AR;
            }
        }

        /// <summary>
        /// 根据传入的语言，读取本地配置，并判断该语言是否可用，能过状态返回真实可用的默认语言（默认为阿语）
        /// </summary>
        /// <param name="lan"></param>
        /// <returns></returns>
        public static Language GetDefaultLanguage(Language lan)
        {
            string strKey = GetFUIBrance(lan);
            if (strKey.Length > 0)
            {
                //if (TableDataSystem.Instance.TablesData.IsLanguageOpen(strKey))
                //{
                //    return lan;
                //}
            }
            return Language.AR;
        }

        public static void LogAppLanguage()
        {
            // 获取当前UI文化的名称，通常是系统的首选语言
            string currentUICultureName = CultureInfo.CurrentUICulture.Name;
            // 或者获取当前系统的文化信息
            string currentCultureName = CultureInfo.CurrentCulture.Name;

            // 打印结果
            Log.Info("当前UI文化名称: " + currentUICultureName);
            Log.Info("当前系统文化名称: " + currentCultureName);

            // 根据需要，你可能只想获取语言代码（例如"zh-CN", "en-US"）
            string languageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            Log.Info("当前语言代码: " + languageCode);

            // 如果需要更详细的区域信息，也可以使用Name属性
            // 注意，这个会包含国家和地区的代码（例如"zh-CN", "en-US"）
            string detailedCultureName = CultureInfo.CurrentCulture.Name;
            Log.Info("详细的当前文化名称: " + detailedCultureName);
        }

        public static UserSetting UpdateUserSetting(string userAccountId,
#nullable enable
            string? userName = null,
            string? serverId = null,
#nullable disable
            Language language = Language.AR,
            bool? hasMusiic = null,
            bool? hasSound = null,
            bool? hasEmoji = null,
            int LastEmojiItemId = 0,
            Dictionary<int, long> RedNoteDayDic = null
            )
        {
            UserSetting setting = GetUserSetting(userAccountId);
            setting.accountId = userAccountId;
            setting.userName = userName ?? setting.userName;
            setting.serverId = serverId ?? setting.serverId;
            setting.hasMusic = hasMusiic ?? setting.hasMusic;
            setting.hasSound = hasSound ?? setting.hasSound;
            setting.hasEmoji = hasEmoji ?? setting.hasEmoji;
            setting.LastEmojiItemId = LastEmojiItemId;
            setting.RedNoteDayDic = RedNoteDayDic ?? setting.RedNoteDayDic;
            string userKey = KEY_USER_HEAD + userAccountId;
            WriteObject(userKey, setting);

            return setting;
        }

        public static void UpdateUserSetting()
        {
            UserSetting setting = GetUserSetting(DataSystem.Instance.Player.accountId);
            setting.userName = defSetting.userName;
            setting.serverId = defSetting.serverId;
            setting.hasMusic = defSetting.hasMusic;
            setting.hasSound = defSetting.hasSound;
            setting.hasEmoji = defSetting.hasEmoji;
            setting.LastEmojiItemId = defSetting.LastEmojiItemId;
            setting.RedNoteDayDic = defSetting.RedNoteDayDic;
            string userKey = KEY_USER_HEAD + DataSystem.Instance.Player.accountId;
            WriteObject(userKey, setting);
        }



        public static UserSetting GetUserSetting(string userAccountId)
        {
            string userKey = KEY_USER_HEAD + userAccountId;
            if (dicSetting.ContainsKey(userKey))
            {
                return dicSetting[userKey];
            }
            else
            {
                //生成一个新的设置对象
                UserSetting setting = UserSetting.CreateInstance();
                if (defSetting != null)
                {
                    setting.hasMusic = defSetting.hasMusic;
                    setting.hasSound = defSetting.hasSound;
                    setting.hasEmoji = defSetting.hasEmoji;
                    setting.language = GetDefaultLanguage(defSetting.language);
                    setting.branch = GetFUIBrance(setting.language);
                }

                //根据 Setting 表中的项，为新用户设置初始值
                //List<SettingClass> lstSetting = TableDataSystem.Instance.TablesData.GetAllSettingConfigList();
                //if (lstSetting != null)
                //{
                //    for (int i = 0; i < lstSetting.Count; i++)
                //    {
                //        var cfg = lstSetting[i];
                //        if (cfg.Id == (int)OptionSettingType.Music)
                //        {
                //            setting.hasMusic = (cfg.DefaultOpen == 1);
                //        }
                //        else if (cfg.Id == (int)OptionSettingType.Sound)
                //        {
                //            setting.hasSound = (cfg.DefaultOpen == 1);
                //        }
                //        else if (cfg.Id == (int)OptionSettingType.Emoji)
                //        {
                //            setting.hasEmoji = (cfg.DefaultOpen == 1);
                //        }
                //    }
                //}

                dicSetting.Add(userKey, setting);

                //写入本地缓存
                WriteObject(userKey, setting);

                //更新总数据记录
                UpdateAllUserKeys();

                return setting;
            }
        }

        /// <summary>
        /// 保存用户设置,要求用户的 accountId 必须不为空值
        /// </summary>
        /// <param name="setting"></param>
        public static void SaveSetting(UserSetting setting)
        {
            if (setting != null && setting.accountId.Length > 0)
            {
                string userKey = KEY_USER_HEAD + setting.accountId;
                WriteObject(userKey, setting);
            }
        }

        /// <summary>
        /// 更新背景音乐开关
        /// </summary>
        /// <param name="flag"></param>
        public static void SetMusicFlag(bool flag)
        {
            if (defSetting != null)
            {
                defSetting.hasMusic = flag;

                WriteObject(KEY_PC_LOGIN_DATA, defSetting);

                var uSetting = GetUserSetting(defSetting.accountId);
                if (uSetting != null)
                {
                    uSetting.hasMusic = flag;

                    string userKey = KEY_USER_HEAD + defSetting.accountId;
                    WriteObject(userKey, uSetting);
                }
            }
        }

        /// <summary>
        /// 更新音效音乐开关
        /// </summary>
        /// <param name="flag"></param>
        public static void SetSoundFlag(bool flag)
        {
            if (defSetting != null)
            {
                defSetting.hasSound = flag;

                WriteObject(KEY_PC_LOGIN_DATA, defSetting);

                var uSetting = GetUserSetting(defSetting.accountId);
                if (uSetting != null)
                {
                    uSetting.hasSound = flag;

                    string userKey = KEY_USER_HEAD + defSetting.accountId;
                    WriteObject(userKey, uSetting);
                }
            }
        }

        /// <summary>
        /// 更新表情显示开关
        /// </summary>
        /// <param name="flag"></param>
        public static void SetEmojiFlag(bool flag)
        {
            if (defSetting != null)
            {
                defSetting.hasEmoji = flag;

                WriteObject(KEY_PC_LOGIN_DATA, defSetting);

                var uSetting = GetUserSetting(defSetting.accountId);
                if (uSetting != null)
                {
                    uSetting.hasEmoji = flag;

                    string userKey = KEY_USER_HEAD + defSetting.accountId;
                    WriteObject(userKey, uSetting);
                }
            }
        }

        /// <summary>
        /// 更新多语言
        /// </summary>
        /// <param name="flag"></param>
        public static void SetLanguage(Language lan)
        {
            if (defSetting != null)
            {
                defSetting.language = lan;
                defSetting.branch = GetFUIBrance(lan);

                WriteObject(KEY_PC_LOGIN_DATA, defSetting);
                WriteData(Constant.LauncherSettingLanguage, (int)lan);

                var uSetting = GetUserSetting(defSetting.accountId);
                if (uSetting != null)
                {
                    uSetting.language = lan;
                    uSetting.branch = defSetting.branch;

                    string userKey = KEY_USER_HEAD + defSetting.accountId;
                    WriteObject(userKey, uSetting);
                }
            }
        }


        private static void UpdateAllUserKeys()
        {
            StringBuilder sb = new();
            foreach (var key in dicSetting.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append(key);
            }

            string allUsers = sb.ToString();
            WriteData(KEY_ALL_USER, allUsers);
        }

        #region 一些读取本地配置参数的方法

        /// <summary>
        /// 是否可以播放背景音乐
        /// </summary>
        /// <returns></returns>
        public static bool IsHasMusic()
        {
            return (defSetting != null) ? defSetting.hasMusic : true;
        }

        /// <summary>
        /// 是否可以播放音效
        /// </summary>
        /// <returns></returns>
        public static bool IsHasSound()
        {
            return (defSetting != null) ? defSetting.hasSound : true;
        }

        /// <summary>
        /// 是否可以播放陈列室表情
        /// </summary>
        /// <returns></returns>
        public static bool IsHasEmoji()
        {
            return (defSetting != null) ? defSetting.hasEmoji : true;
        }

        #endregion


        #region 一些读写本地数据的方法

        public static void WriteData(string key, string value)
        {
            GameModule.Setting.SetString(key, value);
        }

        public static void WriteData(string key, int data)
        {
            GameModule.Setting.SetInt(key, data);
        }

        public static string GetData(string key)
        {
            if (GameModule.Setting.HasSetting(key))
            {
                return GameModule.Setting.GetString(key);
            }

            return string.Empty;
        }

        public static void WriteObject<T>(string key, T obj)
        {
            GameModule.Setting.SetObject(key, obj);
            GameModule.Setting.Save();
        }

        public static T GetObject<T>(string key)
        {
            if (GameModule.Setting.HasSetting(key))
            {
                return GameModule.Setting.GetObject<T>(key);
            }

            return default(T);
        }


        /// <summary>
        /// 给LOG添加颜色
        /// </summary>
        /// <param name="str"></param>
        /// <param name="_color"></param>
        /// <returns></returns>
        public static string Log_UbbColor(string str, string _color = "00FFFF")
        {
            return $"<color=#{_color}>{str}</color>";
        }

        /// <summary>
        /// 修改文本颜色
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <param name="_color">16位颜色标识</param>
        /// <returns></returns>
        public static string UbbColor(string str, string _color)
        {
            return $"[color=#{_color}]{str}[/color]";
        }

        /// <summary>
        /// 修改文本的渐变颜色
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <param name="_color1">16位颜色标识 左上</param>
        /// <param name="_color2">16位颜色标识 左下</param>
        /// <param name="_color3">16位颜色标识 右上</param>
        /// <param name="_color4">16位颜色标识 右下</param>
        /// <returns></returns>
        public static string UbbGradualColor(string str, string _color1, string _color2, string _color3, string _color4)
        {
            return $"[color=#{_color1},#{_color2},#{_color3},#{_color4}]{str}[/color]";
        }


        /// <summary>
        /// 修改文本为粗体
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <returns></returns>
        public static string UbbBold(string str)
        {
            return $"[b]{str}[/b]";
        }

        /// <summary>
        /// 修改文本为斜体
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <returns></returns>
        public static string UbbItalic(string str)
        {
            return $"[i]{str}[/i]";
        }

        /// <summary>
        /// 修改文本颜色与粗体
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <param name="_color">16位颜色标识</param>
        /// <returns></returns>
        public static string UbbColorAndBold(string str, string _color)
        {
            return $"[color=#{_color}][b]{str}[/b][/color]";
        }


        //修改物体下的所有子物体层级
        public static void ChangeChildrenLayer(Transform _tr, int _layer)
        {
            // 遍历所有直接子物体
            for (int i = 0; i < _tr.childCount; i++)
            {
                Transform childTransform = _tr.GetChild(i);
                // 设置当前子物体的层级
                childTransform.gameObject.layer = _layer;
                // 如果当前子物体还有它自己的子物体，递归调用此方法
                if (childTransform.childCount > 0)
                {
                    ChangeChildrenLayer(childTransform, _layer);
                }
            }
        }


        /// <summary>
        /// 修改物体的透明度
        /// </summary>
        /// <param name="_tr"></param>
        /// <param name="alphaValue"></param>
        public static void SetGameObjectAlpha(Transform _tr, float alphaValue)
        {
            // 遍历所有直接子物体
            for (int i = 0; i < _tr.childCount; i++)
            {
                Transform childTransform = _tr.GetChild(i);
                Renderer render = childTransform.GetComponent<Renderer>();
                if (render != null)
                {
                    Material material = render.material;
                    if (material != null)
                    {
                        if (material.HasProperty("_Opacity"))
                        {
                            material.SetFloat("_Opacity", alphaValue);
                        }
                        else if (material.HasProperty("_Alpha"))
                        {
                            material.SetFloat("_Alpha", alphaValue);
                        }
                    }
                }

                // 如果当前子物体还有它自己的子物体，递归调用此方法
                if (childTransform.childCount > 0)
                {
                    SetGameObjectAlpha(childTransform, alphaValue);
                }
            }
        }

        #endregion
    }
}
