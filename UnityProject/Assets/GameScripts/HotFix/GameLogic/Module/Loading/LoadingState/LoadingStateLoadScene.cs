using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoadingSystem>;

namespace GameLogic
{
    public class LoadingStateLoadScene : FsmState<LoadingSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoadingStateLoadScene");
            _mFsm = fsm;
            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadProgress, Constant.LaunchStep.Scene, 0f);
            GameEvent.AddEventListener(GEvent.PreloadSceneReady, OnReady);
            LoadScene();
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoadProgress, Constant.LaunchStep.Scene, 1f);
            GameEvent.RemoveEventListener(GEvent.PreloadSceneReady, OnReady);
        }

        // protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        // {
        // }

        private void LoadScene()
        {
            // int stageId = DataSystem.Instance.CorePlay.Stage;
            // bool isOtherWorld = DataSystem.Instance.CorePlay.IsOtherWorld = CorePlaySystem.Instance.IsOtherWorld(stageId);
            // int flag = CorePlaySystem.InitFlag(true, true, isOtherWorld, true);
            // CorePlaySystem.Instance.PreloadScene(stageId, flag);
        }

        private void OnReady()
        {
            ChangeState<LoadingStateLoadVideo>(_mFsm);
        }
    }
}
