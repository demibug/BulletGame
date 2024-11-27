using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateShowUI : FsmState<LoginSystem>
    {
        private ProcedureOwner m_fsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateShowUI");
            m_fsm = fsm;

            ShowGetAccount();
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            m_fsm = null;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            ChangeState<LoginStateGetAccount>(fsm);
        }

        private void ShowGetAccount()
        {
            if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Intranet)
            {
                //显示获取账号界面
                FUISystem.Instance.ShowUI<LoginWnd>();
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Extranet)
            {
                //显示游客或者自动登录
                FUISystem.Instance.ShowUI<LoginWnd>();
                if (FUISystem.EnabledLogPanel)
                {
                    // App.FUI.ShowUI<CDebugPanel>();
                }
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Formal)
            {
                //显示游客或者自动登录
                FUISystem.Instance.ShowUI<LoginWnd>();
            }


        }
    }
}
