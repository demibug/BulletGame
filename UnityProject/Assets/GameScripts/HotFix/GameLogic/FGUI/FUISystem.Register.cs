using System;
using System.Collections.Generic;
using TEngine;

namespace GameLogic
{
    public partial class FUISystem : BehaviourSingleton<FUISystem>
    {
        private Dictionary<string, Type> m_dicUIRegister = new();

        private bool m_isUIRegister = false;

        /// <summary>
        /// 注册一个UI到列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RegUI<T>()
        {
            Type type = typeof(T);
            if (!m_dicUIRegister.ContainsKey(type.Name))
            {
                m_dicUIRegister.Add(type.Name, type);
            }
        }

        //------------------------------------------------------

        /// <summary>
        /// 根据UI名显示一个UI
        /// </summary>
        /// <param name="uiName"></param>
        public void ShowUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                ShowUI(m_dicUIRegister[uiName], true, null);
            }
        }

        /// <summary>
        /// 根据UI名判断UI是否在显示中
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public bool HasUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                return HasUI(m_dicUIRegister[uiName]);
            }
            return false;
        }

        /// <summary>
        /// 根据UI名取得窗口对象
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public FUIWindow GetUIByName(string uiName)
        {
            if (m_dicUIRegister.ContainsKey(uiName))
            {
                return GameModule.FUI.GetWindow(m_dicUIRegister[uiName].FullName);
            }
            return null;
        }

        /// <summary>
        /// 注册UI（UI类名不允许重名）
        /// </summary>
        public void RegisterUI()
        {
            if (m_isUIRegister) return;
            m_isUIRegister = true;

            m_dicUIRegister.Clear();

            RegUI<LoginWnd>();
        }
    }
}
