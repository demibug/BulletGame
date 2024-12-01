using BattleCore;
using Cysharp.Threading.Tasks;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateFinish : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateFinish");
            _mFsm = fsm;

            CloseLoadingUIAsync().Forget();
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
        }

        private async UniTaskVoid CloseLoadingUIAsync()
        {
            await UniTask.WaitForSeconds(0.5f);

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadFinish);
            FUISystem.Instance.CloseUI<LoginWnd>();
            BattleCoreSystem.Instance.Init();
        }
    }
}
