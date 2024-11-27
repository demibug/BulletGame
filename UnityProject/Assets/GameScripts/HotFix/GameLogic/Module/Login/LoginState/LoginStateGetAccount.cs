using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateGetAccount : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateGetAccount");
            _mFsm = fsm;

            GameEvent.AddEventListener<string, string, string>(GEvent.GetAccountSuccess, OnGetAccountSuccess);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            GameEvent.RemoveEventListener<string, string, string>(GEvent.GetAccountSuccess, OnGetAccountSuccess);
        }

        private void ToReqEntry()
        {
            ChangeState<LoginStateReqEntry>(_mFsm);
        }

        private void OnGetAccountSuccess(string account, string password, string serverId)
        {
            DataSystem.Instance.Login.Account = account;
            DataSystem.Instance.Login.Password = password;
            DataSystem.Instance.Login.ServerId = serverId;

            ToReqEntry();
        }
    }
}
