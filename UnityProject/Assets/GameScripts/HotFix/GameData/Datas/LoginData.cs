namespace GameData
{
    public class LoginData
    {
        public string EntryHost;
        public int EntryPort;

        /// <summary>
        /// 内网
        /// </summary>
        public string IntranetHostEntry = "http://192.168.110.201"; //内网
        // public string IntraNetHostEntry = "http://175.27.154.72"; //国服

        //public string IntraNetHostEntry = "http://43.157.114.162"; //内网
        public string ExtranetHostEntry = "http://175.27.154.72"; //国服(外网测试)
        public string FormalHostEntry = "http://43.157.114.162"; //外网正式



        /// <summary>
        /// 内网端口
        /// </summary>
        public int IntranetPort = 10000;

        /// <summary>
        /// 外网测试服端口
        /// </summary>
        public int ExtranetPort = 10000;

        /// <summary>
        /// 正式服端口
        /// </summary>
        public int FormalPort = 10000;
        public int ReqIdEntry;

        public string UrlLogin;
        public int ReqIdLogin;

        public string HostHttp;
        public int PortHttp;

        public string DeviceUUID;

        public string Account;
        public string Password;
        public string ServerId;

        public string LoginToken;
        public string GameToken;
    }
}
