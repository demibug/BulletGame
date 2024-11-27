/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace Pkg_Login
{
    public class Pkg_LoginBinder
    {
        private static int s_binderId;
        public static int BinderId
        {
            get
            {
                if (s_binderId == 0) s_binderId = TEngine.RuntimeId.ToRuntimeId("Pkg_LoginBinder");
                return s_binderId;
            }
        }

        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_LoginWnd.URL, typeof(UI_LoginWnd));
        }
    }
}