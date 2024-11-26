using System;

namespace TEngine
{

    [AttributeUsage(AttributeTargets.Class)]
    public class FUIWidgetAttribute : Attribute
    {

        /// <summary>
        /// 包名数组,一个ui有可能包含多个包名
        /// </summary>
        public readonly string[] Packages;

        public FUIWidgetAttribute(params string[] packages)
        {
            Packages = packages;
        }
    }
}