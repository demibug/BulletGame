using System;

namespace TEngine
{
    /// <summary>
    /// UI层级枚举。
    /// </summary>
    public enum FUILayer : int
    {
        Bottom,
        UI,
        UITop,
        Top,
        Tips,
        TouchEffect,//界面触碰特效层，在所有界面之上
        Guide,      //新手引导专用层级
        System,
        SystemTip,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FUIWindowAttribute : Attribute
    {
        /// <summary>
        /// 窗口层级
        /// </summary>
        public readonly int WindowLayer;

        /// <summary>
        /// 全屏窗口标记。
        /// </summary>
        public readonly bool FullScreen;

        /// <summary>
        /// 是否来自本地Resources
        /// </summary>
        public readonly bool FromResources;

        /// <summary>
        /// 包名数组,一个ui有可能包含多个包名
        /// </summary>
        public readonly string[] Packages;

        public FUIWindowAttribute(int windowLayer, bool fullScreen = false,  bool fromResources = false, params string[] packages)
        {
            WindowLayer = windowLayer;
            FullScreen = fullScreen;
            FromResources = fromResources;
            Packages = packages;
        }

        public FUIWindowAttribute(FUILayer windowLayer, bool fullScreen = false,  params string[] packages)
        {
            WindowLayer = (int)windowLayer;
            FullScreen = fullScreen;
            FromResources = false;
            Packages = packages;
        }
    }
}
