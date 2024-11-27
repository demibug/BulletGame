using Cysharp.Threading.Tasks;
using FairyGUI;
using LauncherPackage;
using TEngine;
using UnityEngine;

namespace GameMain
{
    [FUIWindow((int)FUILayer.System, fullScreen: true, fromResources: true, "AssetLoad/FUI/Auto/cn/LauncherPackage")]
    public class LauncherLoadWnd : FUIWindow
    {
        private UI_LauncherLoadWnd _mView;

        private float _mTargetProgress;
        private float _mCurrentProgress;
        private float _mSpeed = 10f;

        public override void CheckBindAll()
        {
            GameModule.FUI.CheckBindAll(LauncherPackageBinder.BinderId, LauncherPackageBinder.BindAll);
        }

        protected override GObject FUICreateInstance()
        {
            _mView = UI_LauncherLoadWnd.CreateInstance();
            return _mView;
        }

        protected override async UniTask<GObject> FUICreateInstanceAsync()
        {
            _mView = await UI_LauncherLoadWnd.CreateInstanceAsync();
            return _mView;
        }

        public override void OnCreate()
        {
            if (RTLSupport.BaseDirection == RTLSupport.DirectionType.RTL)
            {
                _mView.camel.scaleX = -1;
                GImage bar = _mView.progress.GetChild("bar") as GImage;
                if (bar != null)
                {
                    bar.fillOrigin = 1;
                }
            }
            else
            {
                _mView.camel.scaleX = 1;
                GImage bar = _mView.progress.GetChild("bar") as GImage;
                if (bar != null)
                {
                    bar.fillOrigin = 0;
                }
            }

            float progress = GetProgress();
            ResetProgress(progress);

            FUIExtension.SetFuiTexture(_mView.imgLoader, "AssetLoad/ImageBg/loading_bg", false, "", true);
        }

        public override void OnRefresh()
        {
            float progress = GetProgress();
            SetProgress(progress);
        }

        public override void OnUpdate()
        {
            UpdateProgress();
        }

        private float GetProgress()
        {
            if (m_userDatas != null && m_userDatas.Length > 0)
            {
                return (float)m_userDatas[0];
            }

            return 0f;
        }

        private void ResetProgress(float progress)
        {
            _mCurrentProgress = progress;
            _mTargetProgress = progress;
            _mView.progress.value = progress;
        }

        private void SetProgress(float newTargetProgress)
        {
            if (newTargetProgress != _mTargetProgress)
            {
                _mCurrentProgress = _mTargetProgress;
                _mTargetProgress = newTargetProgress;
            }
        }

        private void UpdateProgress()
        {
            if (_mCurrentProgress != _mTargetProgress && _mView != null)
            {
                float progress = _mCurrentProgress + _mSpeed * Time.deltaTime;
                if (progress > _mTargetProgress)
                {
                    progress = _mTargetProgress;
                }

                _mCurrentProgress = progress;
                _mView.progress.value = progress;
            }
        }
    }
}
