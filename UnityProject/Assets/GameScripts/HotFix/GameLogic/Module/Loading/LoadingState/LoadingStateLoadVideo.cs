using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoadingSystem>;

namespace GameLogic
{
    public class LoadingStateLoadVideo : FsmState<LoadingSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoadingStateLoadVideo");
            _mFsm = fsm;

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadProgress, Constant.LaunchStep.Video, 0f);

            // AudioSystem.Instance.PlaySound_BeforeBackGround(); //背景音效
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadProgress, Constant.LaunchStep.Video, 1f);
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            ChangeState<LoadingStateFinish>(fsm);
        }
    }
}
