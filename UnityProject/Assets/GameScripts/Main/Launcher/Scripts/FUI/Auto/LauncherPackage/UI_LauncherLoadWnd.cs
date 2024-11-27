/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using Cysharp.Threading.Tasks;
using FairyGUI;
using FairyGUI.Utils;

namespace LauncherPackage
{
    public class UI_LauncherLoadWnd : GComponent
    {
        public const string UIPackageName = "LauncherPackage";
        public const string UIResName = "LauncherLoadWnd";

        public GLoader imgLoader;
        public GProgressBar progress;
        public GMovieClip camel;
        public const string URL = "ui://piqhg590ewgu0";

        public static UI_LauncherLoadWnd CreateInstance()
        {
            return (UI_LauncherLoadWnd)UIPackage.CreateObject(UIPackageName, UIResName);
        }

        public static UniTask<UI_LauncherLoadWnd> CreateInstanceAsync()
        {
            UniTaskCompletionSource<UI_LauncherLoadWnd> tcs = new UniTaskCompletionSource<UI_LauncherLoadWnd>();

            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult((UI_LauncherLoadWnd)go);
            }
            );

            return tcs.Task;
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback callback)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, callback);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            imgLoader = (GLoader)GetChildAt(1);
            progress = (GProgressBar)GetChildAt(2);
            camel = (GMovieClip)GetChildAt(3);
        }
    }
}