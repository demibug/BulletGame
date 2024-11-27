using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoadingSystem>;

namespace GameLogic
{
    public class LoadingStateFinish : FsmState<LoadingSystem>
    {
        // private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoadingStateFinish");

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadFinish);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            // _mFsm = null;
        }

        // protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        // {
        // }
    }
}
