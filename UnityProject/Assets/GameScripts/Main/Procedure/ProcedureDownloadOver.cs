using TEngine;
using ProcedureOwner = TEngine.IFsm<TEngine.IProcedureManager>;

namespace GameMain
{
    public class ProcedureDownloadOver:ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private bool _needClearCache;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("ProcedureDownloadOver");
            Log.Info("下载完成!!!");

            UILoadMgr.Show(UIDefine.UILoadUpdate, $"下载完成...");
            LauncherLoadMgr.Show(LauncherUIDefine.LauncherLoadWnd, Constant.LaunchStep.DownloadOver);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            if (_needClearCache)
            {
                ChangeState<ProcedureClearCache>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureLoadAssembly>(procedureOwner);
            }
        }
    }
}