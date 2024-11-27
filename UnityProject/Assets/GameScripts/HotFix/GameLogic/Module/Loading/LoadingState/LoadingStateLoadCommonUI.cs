using FairyGUI;
using Pkg_Common;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoadingSystem>;

namespace GameLogic
{
    public class LoadingStateLoadCommonUI : FsmState<LoadingSystem>
    {
        private ProcedureOwner _mFsm;

        private bool _mLoadComplete;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoadingStateLoadCommonUI");
            _mFsm = fsm;

            if (!_mLoadComplete)
            {
                LoadFUICommnPackage();
            }
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
        }

        protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
        {
            if (_mLoadComplete)
            {
                // 加载common ui 完成,显示登录界面
                LoadingSystem.Instance.ToLogin();
                ChangeState<LoadingStateLoadScene>(fsm);
            }
        }

        private void LoadFUICommnPackage()
        {
            string[] lstRes =
            {
                FUIPackageDefine.Pkg_Common
            };

            GComponent gcomp = GameModule.FUI.UIRoot.GetChild("LauncherLoadWnd") as GComponent;
            if (gcomp != null)
            {
                Log.Info("LoadFUICommnPackage AddPackage");
                GameModule.FUI.AddPackage(gcomp, lstRes, OnCommonPackageLoadComplete);
            }
        }

        private void OnCommonPackageLoadComplete(bool success)
        {
            Log.Info("LoadFUICommnPackage OnCommonPackageLoadComplete " + success);
            if (success)
            {
                //一些通用资源类进行注册
                FUISystem.Instance.CheckBindAll(Pkg_CommonBinder.BinderId, Pkg_CommonBinder.BindAll);
                Log.Info("LoadFUICommnPackage CheckBindAll");
            }
            else
            {
                Log.Error("通用资源加载失败");
            }

            _mLoadComplete = true;
        }
    }
}
