/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using Cysharp.Threading.Tasks;
using FairyGUI;
using FairyGUI.Utils;

namespace Pkg_Login
{
    public partial class UI_LoginWnd : GComponent
    {
        public const string UIPackageName = "Pkg_Login";
        public const string UIResName = "LoginWnd";

        public GLoader imgBg;
        public GButton btnLogin;
        public GLoader imgLoginBg;
        public GLoader imgLogo;
        public GTextField txtUserTitle;
        public GTextField txtServerTitle;
        public GTextField txtErrorTip;
        public GTextInput txtServerInput;
        public GTextInput txtUserInput;
        public GTextField txtVersion;
        public const string URL = "ui://y3d6pccbjjos0";

        public static UI_LoginWnd CreateInstance()
        {
            return (UI_LoginWnd)UIPackage.CreateObject(UIPackageName, UIResName);
        }

        public static UniTask<UI_LoginWnd> CreateInstanceAsync()
        {
            UniTaskCompletionSource<UI_LoginWnd> tcs = new UniTaskCompletionSource<UI_LoginWnd>();

            CreateGObjectAsync((go) =>
            {
                tcs.TrySetResult((UI_LoginWnd)go);
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

            imgBg = (GLoader)GetChildAt(0);
            btnLogin = (GButton)GetChildAt(1);
            imgLoginBg = (GLoader)GetChildAt(2);
            imgLogo = (GLoader)GetChildAt(3);
            txtUserTitle = (GTextField)GetChildAt(4);
            txtServerTitle = (GTextField)GetChildAt(5);
            txtErrorTip = (GTextField)GetChildAt(6);
            txtServerInput = (GTextInput)GetChildAt(7);
            txtUserInput = (GTextInput)GetChildAt(8);
            txtVersion = (GTextField)GetChildAt(9);
        }
    }
}