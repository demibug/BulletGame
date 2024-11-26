using System;
using System.Text;
using System.Text.RegularExpressions;
using GameConfig;

namespace GameData
{
    public static class LMgr
    {
        /// <summary>
        /// 设置当前所使用的语言类型
        /// </summary>
        public static GDefine.Language CurLanguage { get; set; }

        /// <summary>
        /// 从配置表中取一个字串，传入的是一个数字的字串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TC(string key)
        {
            int.TryParse(key, out int value);
            if (value >= 0)
            {
                return TC(value);
            }
            return FormatByEmoji(key);
        }

        /// <summary>
        /// 从配置表里面取得一个字串
        /// </summary>
        /// <param name="key">字串的Id</param>
        /// <returns></returns>
        public static string TC(int key)
        {
            if (key == 0) return string.Empty;

            var cfgTable = ConfigSystem.Instance.Tables.GameLanguage;
            if (cfgTable != null)
            {
                cfgTable.DataMap.TryGetValue(key, out GameLanguageClass cfgItem);
                if (cfgItem != null)
                {
                    string strInfo = string.Empty;
                    string strCNBase = cfgItem.CN;
                    switch (CurLanguage)
                    {
                        case GDefine.Language.CN:
                            strInfo = cfgItem.CN;
                            break;

                        case GDefine.Language.EN:
                            strInfo = cfgItem.EN;
                            break;

                        case GDefine.Language.AR:
                            strInfo = cfgItem.AR;
                            break;
                    }

                    //新手引导测试代码（强制中文）
                    //strInfo = cfgItem.CN;

                    if (strInfo.Length > 0)
                    {
                        return FormatByEmoji(strInfo);
                    }
                    else
                    {
                        return FormatByEmoji(key.ToString());
                    }
                }
                else
                {
                    return FormatByEmoji(key.ToString());
                }
            }

            return "";
        }

        public static string TC(int key, params object[] args)
        {
            return Targs(key, args);
        }

        private static string FormatByEmoji(string str)
        {
            string pattern = @"{#(\d+)}";
            string replaced = Regex.Replace(str, pattern, match =>
            {
                // 获取匹配到的数字
                int iconId = 0;
                string url = string.Empty;

                int.TryParse(match.Groups[1].Value, out iconId);
                if (iconId > 0)
                {
                    // 构造图片标签
                    var cfgTable = ConfigSystem.Instance.Tables.TextInsertIcon;
                    if (cfgTable != null)
                    {
                        cfgTable.DataMap.TryGetValue(iconId, out TextInsertIconClass cfgIcon);
                        if (cfgIcon != null)
                        {
                            url = cfgIcon.Resource;
                        }
                    }
                }

                return $"[IMG]{url}[/IMG]";
            });

            return replaced;
        }

        /// <summary>
        /// 从配置表中取到一个字串，并根据传入的参数，将字串中的 {0} {1} 替换为相应的参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Targs(int key, params object[] args)
        {
            var strInfo = TC(key);
            if (string.IsNullOrEmpty(strInfo))
            {
                return "";
            }

            return FormatString(strInfo, args);
        }

        /// <summary>
        /// 将一个字串中的 {0} {1} 替换为后续的参数中提供的值
        /// </summary>
        /// <param name="strInfo"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatString(string strInfo, params object[] args)
        {
            var sb = new StringBuilder(strInfo);
            for (var i = 0; i < args.Length && i < strInfo.Length; i++)
            {
                string strArgs = args[i].ToString();
                int index = strInfo.IndexOf("{" + i + "}", StringComparison.Ordinal);
                if (index >= 0)
                {
                    sb.Replace("{" + i + "}", strArgs);
                }
            }

            sb.Replace("<br>", "\n");

            return FormatByEmoji(sb.ToString());
        }



        public static int GetLanguageLength(string str)
        {
            //switch (CurLanguage)
            //{
            //    case GDefine.Language.AR:
            //        return ArLength(str);

            //    default:
            //        return AllLength(str);
            //}

            return ArLength(str);

            //return AllLength(str);
        }

        private static int AllLength(string str)
        {
            int strLength = 0;
            //中文字符正则
            Regex rx = new Regex("^[\u4e00-\u9fa5]$");
            //全角字符正则
            Regex rx1 = new Regex("^[\uFF00-\uFFFF]$");

            for (int i = 0; i < str.Length; i++)
            {
                if (rx.IsMatch(str[i].ToString()) || rx1.IsMatch(str[i].ToString()))
                {//如果为中文字符或全角字符，字符串长度加2
                    strLength += 2;
                }
                else
                {//否则加1
                    strLength += 1;
                }
            }

            return strLength;
        }


        private static int ArLength(string str)
        {
            //int length = 0;
            //for (int i = 0; i < str.Length; i++)
            //{
            //    length += Encoding.UTF8.GetByteCount(str, i, 1);
            //}
            //return length;

            return str.Length;
        }


        // 模拟函数，直接计算UTF-8字节数组中的Rune数量
        public static int RuneCountInString(string str)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(str);
            int runeCount = 0;
            int i = 0;
            while (i < utf8Bytes.Length)
            {
                int c = utf8Bytes[i];
                if (c < 128) // ASCII字符
                {
                    runeCount++;
                    i++;
                    continue;
                }

                // 注意：这里的代码简化了UTF-8字符解码逻辑
                // 在实际中，您需要处理1-4字节的UTF-8字符
                int size;
                if ((c & 0xE0) == 0xC0) // 2字节字符
                {
                    size = 2;
                }
                else if ((c & 0xF0) == 0xE0) // 3字节字符
                {
                    size = 3;
                }
                else if ((c & 0xF8) == 0xF0) // 4字节字符
                {
                    size = 4;
                }
                else
                {
                    // 无效字符，只计数一个字节
                    runeCount++;
                    i++;
                    continue;
                }

                // 检查是否有足够的字节来形成有效的UTF-8字符
                if (i + size > utf8Bytes.Length)
                {
                    // 不完整的字符，只计数一个字节
                    runeCount++;
                    i++;
                    continue;
                }

                // 假设所有提供的字节都形成了有效的UTF-8字符
                // 在实际中，您应该检查后续字节是否有效
                i += size;
                runeCount++;
            }

            return runeCount;
        }


    }
}
