using System.Collections.Generic;

namespace TEngine
{
    internal sealed partial class FUIPackageManager
    {
        private static Dictionary<int, int> s_bindedPackages = new Dictionary<int, int>();

        /// <summary>
        /// 检测并进行包绑定
        /// </summary>
        /// <param name="binderId"></param>
        /// <param name="bindFunc"></param>
        public static void CheckBindAll(int binderId, System.Action bindFunc)
        {
            if (!s_bindedPackages.ContainsKey(binderId))
            {
                if (bindFunc != null)
                {
                    s_bindedPackages.Add(binderId, 0);
                    bindFunc();
                }
            }
        }
    }
}
