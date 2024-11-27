using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoadingSystem>;

namespace GameLogic
{
    public class LoadingStateInit : FsmState<LoadingSystem>
    {
        private ProcedureOwner _mFsm;

        private bool _mIsInit;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoadingStateInit");
            _mFsm = fsm;

            if (!_mIsInit)
            {
                _mIsInit = true;
                //注册全局的UI事件
                FUISystem.Instance.RegisterUI();
                FUISystem.Instance.InitUISystem();

                //JSON相关配置初始化
                GameConfig.JSONConfig.Instance.InitGlobal();
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            ChangeState<LoadingStateLoadCommonUI>(fsm);
        }

        private int GetCurrentStage()
        {
            int stage = GameData.LauncherSetting.CurrentStage;
            if (stage == 0)
            {
                stage = GameConfig.JSONConfig.Instance.General.InitialMap;
            }

            return stage;
        }
    }
}
