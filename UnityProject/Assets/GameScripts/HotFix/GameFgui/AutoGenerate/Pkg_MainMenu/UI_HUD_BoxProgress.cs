/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using Cysharp.Threading.Tasks;
using FairyGUI;
using FairyGUI.Utils;

namespace Pkg_MainMenu
{
    public class UI_HUD_BoxProgress : GProgressBar, TEngine.IFUIWidget
    {
        public const string UIPackageName = "Pkg_MainMenu";
        public const string UIResName = "HUD_BoxProgress";

        public GTextField txtTitle;
        public const string URL = "ui://hdzsud0le16i1a";

        public static UI_HUD_BoxProgress CreateInstance()
        {
            return (UI_HUD_BoxProgress)UIPackage.CreateObject(UIPackageName, UIResName);
        }

        public static UniTask<UI_HUD_BoxProgress> CreateInstanceAsync()
        {
            UniTaskCompletionSource<UI_HUD_BoxProgress> tcs = new UniTaskCompletionSource<UI_HUD_BoxProgress>();

            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult((UI_HUD_BoxProgress)go);
            }
            );

            return tcs.Task;
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback callback)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, callback);
        }

        public void CreateFromGComp(GComponent rootComp)
        {
            txtTitle = (GTextField)rootComp?.GetChildAt(2);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            CreateFromGComp(this);
        }
    }
}