// using Cysharp.Threading.Tasks;
// using FairyGUI;
// using GameData;
// using Pkg_MainMenu;
// using TEngine;
//
// namespace GameLogic
// {
//     [FUIWidget()]
//     public class TestProgressWgt : FUIWidget
//     {
//         private UI_HUD_BoxProgress m_view;
//         public UI_HUD_BoxProgress View { get { return m_view; } }
//
//         public override void CheckBindAll()
//         {
//             FUISystem.Instance.CheckBindAll(Pkg_MainMenuBinder.BinderId, Pkg_MainMenuBinder.BindAll);
//         }
//
//         protected override IFUIWidget FUICreateWidget()
//         {
//             m_view = UI_HUD_BoxProgress.CreateInstance();
//             return m_view;
//         }
//
//         protected override async UniTask<GObject> FUICreateInstanceAsync()
//         {
//             m_view = await UI_HUD_BoxProgress.CreateInstanceAsync();
//             return m_view;
//         }
//
//         public override void RegisterEvent()
//         {
//             AddUIEvent(GEvent.WorldMapLoadComplete, OnLoadWorldComplete);
//         }
//
//         public override void OnCreate()
//         {
//         }
//
//
//         private void OnLoadWorldComplete()
//         {
//             FUISystem.Instance.CloseUI<LoginWnd>();
//         }
//     }
// }