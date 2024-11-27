using Cysharp.Threading.Tasks;
using TEngine;

namespace GameMain
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            StartGame().Forget();
            Log.Info("ProcedureStartGame");
        }

        private async UniTaskVoid StartGame()
        {
            await UniTask.Yield();
            UILoadMgr.HideAll();
            LauncherLoadMgr.Show(LauncherUIDefine.LauncherLoadWnd, Constant.LaunchStep.StartGame);
        }
    }
}