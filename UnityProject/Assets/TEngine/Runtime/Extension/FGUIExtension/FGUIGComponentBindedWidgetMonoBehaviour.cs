// using UnityEngine;
//
// namespace TEngine
// {
//     [DisallowMultipleComponent]
//     public class FGUIGComponentBindedWidgetMonoBehaviour : MonoBehaviour
//     {
//         private FairyGUI.GComponent m_fguiBindedGComponent;
//         public T FGUIGetBindedGComponent<T>() where T : FairyGUI.GComponent
//         {
//             return m_fguiBindedGComponent as T;
//         }
//
//         public void FGUISetBindedGComponent(FairyGUI.GComponent comp)
//         {
//             this.m_fguiBindedGComponent = comp;
//         }
//
//         private void OnDestroy()
//         {
//             m_fguiBindedGComponent = null;
//         }
//     }
// }