using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateInit : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateInit");
            _mFsm = fsm;
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            ChangeState<LoginStateShowUI>(fsm);
        }
    }
}