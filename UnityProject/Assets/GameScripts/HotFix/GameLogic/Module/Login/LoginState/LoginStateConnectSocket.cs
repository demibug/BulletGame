using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateConnectSocket : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateConnectSocket");
            _mFsm = fsm;

            ConnectSocket();


            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoginProgress, LoginSystem.LoginProgressConnect);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
        }

        private void ConnectSocket()
        {
            LoginData loginData = DataSystem.Instance.Login;
            // NetworkSystem.Instance.Connect(loginData.HostHttp, loginData.PortHttp, OnConnect);
            Log.Debug("Connect to Socket " + loginData.HostHttp + " : " + loginData.PortHttp);
        }

        private void OnConnect()
        {
            ChangeState<LoginStateAuth>(_mFsm);
        }
    }
}
