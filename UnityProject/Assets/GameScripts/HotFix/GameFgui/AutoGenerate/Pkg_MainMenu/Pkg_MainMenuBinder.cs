/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace Pkg_MainMenu
{
    public class Pkg_MainMenuBinder
    {
        private static int s_binderId;
        public static int BinderId
        {
            get
            {
                if (s_binderId == 0) s_binderId = TEngine.RuntimeId.ToRuntimeId("Pkg_MainMenuBinder");
                return s_binderId;
            }
        }

        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_HUD_BoxProgress.URL, typeof(UI_HUD_BoxProgress));
        }
    }
}