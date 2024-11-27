/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace LauncherPackage
{
    public class LauncherPackageBinder
    {
        private static int s_binderId;
        public static int BinderId
        {
            get
            {
                if (s_binderId == 0) s_binderId = TEngine.RuntimeId.ToRuntimeId("LauncherPackageBinder");
                return s_binderId;
            }
        }

        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_LauncherLoadWnd.URL, typeof(UI_LauncherLoadWnd));
        }
    }
}