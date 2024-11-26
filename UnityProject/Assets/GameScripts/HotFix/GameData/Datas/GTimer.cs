using System;
using GameBase;
using TEngine;

namespace GameData
{
    public class GTimer : Singleton<GTimer>
    {
        private long m_serverTime = 0; //当前服务器的时间戳(毫秒)
        private long m_serverSyncTime = 0; //记录服务器时间时的当前游戏运行时间
        private long m_NextDayZeroFrefeshTime = 0; //每日重置时间（所有的每日重置都可以用这个做倒计时）

        public void SyncServerTime(long time)
        {
            SetServerTime(time);

            //通知同步当前的服务器时间
            GameEvent.Send(GEvent.SyncServerTime);
        }

        /**设置服务器时间 */
        public void SetServerTime(long time)
        {
            m_serverTime = time;
            m_serverSyncTime = UtcTimeSec();
            // TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            // m_serverOffset = UtcTimeMs();
        }

        /** 设置隔天0点的时间（服务器基准时间） **/
        public void SetZeroFreshTime(long time)
        {
            m_NextDayZeroFrefeshTime = time;
        }

        /// <summary>
        /// 获取隔日的刷新时间
        /// </summary>
        public long GetZeroFreshTime()
        {
            long timePass = UtcTimeSec() - m_serverSyncTime;
            return m_NextDayZeroFrefeshTime + timePass;
        }

        /// <summary>
        /// 获取每日重置的剩余时间时间（秒）
        /// </summary>
        public long ToZeroTime
        {
            get
            {
                return Math.Max(0,m_NextDayZeroFrefeshTime - ServerTime);
            }
        }

        /// <summary>
        /// 取得与服务器时间相比剩余的时间
        /// </summary>
        /// <param name="time">传入的时间戳</param>
        /// <param name="isZeomMin">是否以0秒为最小值</param>
        /// <returns></returns>
        public long GetLastSec(long time, bool isZeomMin = true)
        {
            long nTime = time - ServerTime;
            if (isZeomMin)
            {
                return (nTime < 0) ? 0 : nTime;
            }
            return nTime;
        }

        /**
         * 取得服务器时间的时间戳（秒单位）
         */
        public long ServerTime
        {
            get
            {
                var time = GetServerTimeSec();
                return time;
            }
        }

        public long GetServerTimeSec()
        {
            // TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            long timePass = UtcTimeSec() - m_serverSyncTime;
            return m_serverTime + timePass;
        }

        /// <summary>
        /// 获取当前UTC时间戳(毫秒)
        /// </summary>
        public long UtcTimeMs()
        {
            //DateTime.Ticks 属性返回自 0001 年 1 月 1 日午夜以来经过的时间间隔，以 100 纳秒为单位
            //621355968000000000 是从 0001 年 1 月 1 日至 1970 年 1 月 1 日的间隔所对应的 Tick 数。
            //将当前时间的 Tick 数减去这个值，得到的是从 1970 年 1 月 1 日至今的 Tick 数，然后再除以 10000，将单位转换为毫秒
            return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 获取当前UTC时间戳(秒)
        /// </summary>
        public long UtcTimeSec()
        {
            //DateTime.Ticks 属性返回自 0001 年 1 月 1 日午夜以来经过的时间间隔，以 100 纳秒为单位
            //621355968000000000 是从 0001 年 1 月 1 日至 1970 年 1 月 1 日的间隔所对应的 Tick 数。
            //将当前时间的 Tick 数减去这个值，得到的是从 1970 年 1 月 1 日至今的 Tick 数，然后再除以 10000000，将单位转换为秒
            return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000;
        }

        /**
         * 格式代秒数
         * @param sec           要转换的秒数
         * @param shortModel    短格式，默认格式为 HH:MM:SS，短模式为 MM:SS
         * @param sp            分隔符，默认为 :
         */
        public string FormatSec(long sec, bool shortModel = false, string sp = ":")
        {
            long h = sec / 3600;
            long m = (sec % 3600) / 60;
            long s = sec % 60;

            string strH = (h < 10) ? "0" + h : h.ToString();
            string strM = (m < 10) ? "0" + m : m.ToString();
            string strS = (s < 10) ? "0" + s : s.ToString();

            if (shortModel && h < 10)
            {
                if (h > 0)
                {
                    return h + sp + strM + sp + strS;
                }

                return strM + sp + strS;
            }

            return strH + sp + strM + sp + strS;
        }

        /// <summary>
        /// 获取时间转换
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public int FormatTimeRemaining(long sec)
        {
            if (sec >= 3600)
            {
                return (int)(Math.Floor((double)sec / 3600));
            }
            else if (sec >= 60)
            {
                return (int)(Math.Floor((double)sec / 60));
            }
            return (int)sec;
        }

        /// <summary>
        /// 转换特殊显示的字串文本
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetTimeString(long time)
        {
            long sec = time - ServerTime;
            if (sec < 0)
            {
                return GDefine.TimeDefine.GetHowLongTimeStr(-sec);
            }
            else
            {
                return GDefine.TimeDefine.GetLastTimeStr(sec);
            }
        }
    }
}
