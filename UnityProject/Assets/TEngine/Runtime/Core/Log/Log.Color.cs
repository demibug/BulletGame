using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// 日志工具集。
    /// </summary>
    public static partial class Log
    {

        private static StringBuilder _colorBuilder = new StringBuilder();
        private static StringBuilder _logBuilder = new StringBuilder();

        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void ColorLog(string title, object content, string titleColor, string contentColor)
        {
            _logBuilder.Clear();
            if (!string.IsNullOrEmpty(title))
            {
                if (string.IsNullOrEmpty(titleColor))
                {
                    _logBuilder.Append(title);
                    _logBuilder.Append("    ");
                }
                else
                {
                    _logBuilder.Append("<color=");
                    _logBuilder.Append(titleColor);
                    _logBuilder.Append(">");
                    _logBuilder.Append(title);
                    _logBuilder.Append("</color>    ");
                }
            }

            string strContent = content.ToString();
            if (!string.IsNullOrEmpty(strContent))
            {
                if (string.IsNullOrEmpty(contentColor))
                {
                    _logBuilder.Append(strContent);
                }
                else
                {
                    _logBuilder.Append("<color=");
                    _logBuilder.Append(contentColor);
                    _logBuilder.Append(">");
                    _logBuilder.Append(strContent);
                    _logBuilder.Append("</color>");
                }
            }

            Log.Info(_logBuilder.ToString());
        }

        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void ColorLog(string title, object content, Color titleColor, Color contentColor)
        {
            string titleColorStr = titleColor == Color.white ? null : ToHexString(titleColor);
            string contentColorStr = contentColor == Color.white ? null : ToHexString(contentColor);
            ColorLog(title, content.ToString(), titleColorStr, contentColorStr);
        }

        private static string ToHexString(Color color)
        {
            _colorBuilder.Clear();
            _colorBuilder.Append("#");
            _colorBuilder.Append(Mathf.RoundToInt(color.r * 255).ToString("X2"));
            _colorBuilder.Append(Mathf.RoundToInt(color.g * 255).ToString("X2"));
            _colorBuilder.Append(Mathf.RoundToInt(color.b * 255).ToString("X2"));
            return _colorBuilder.ToString();
        }
    }
}