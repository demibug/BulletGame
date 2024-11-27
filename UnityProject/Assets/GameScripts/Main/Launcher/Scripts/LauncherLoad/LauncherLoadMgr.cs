using System;
using System.Collections.Generic;
using GameMain.LauncherDefine;
using TEngine;

namespace GameMain
{
    public static class LauncherLoadMgr
    {
        private static bool _sIsInit = false;
        private static Dictionary<Constant.LaunchStep, int> _sLaunchSteps = new Dictionary<Constant.LaunchStep, int>();
        private static Dictionary<Constant.LaunchStep, int> _sNewbieSteps = new Dictionary<Constant.LaunchStep, int>();
        private static Dictionary<string, Type> _sUIMap = new Dictionary<string, Type>();
        private static OpenType _sOpenType;
        private static Constant.LaunchStep _sNowStep;

        private static void Init()
        {
            if (!_sIsInit)
            {
                _sIsInit = true;
                InitSteps();
                InitLanguage();

                GameEvent.AddEventListener<int, float>(Constant.LauncherEvent.LauncherEventLoadProgress, OnLauncherLoadProgress);
                GameEvent.AddEventListener(Constant.LauncherEvent.LauncherEventLoadFinish, OnLauncherLoadFinish);
                GameEvent.AddEventListener<int>(Constant.LauncherEvent.LauncherEventLoginProgress, OnLauncherEventLoginProgress);
            }
        }

        public static void FirstLoad()
        {
            Init();
            _sOpenType = IsNewbie() ? OpenType.NewPlayer : OpenType.OldPlayer;
            _sNowStep = Constant.LaunchStep.None;
            Show(LauncherUIDefine.LauncherLoadWnd, Constant.LaunchStep.Launch, 0);
        }

        public static void SwitchLoad()
        {
            _sOpenType = OpenType.OldPlayerNewDevice;
            _sNowStep = Constant.LaunchStep.None;
        }

        public static bool IsNewbie()
        {
            int currentStage = 0;
            if (GameModule.Setting.HasSetting(Constant.LauncherSettingStage))
            {
                currentStage = GameModule.Setting.GetInt(Constant.LauncherSettingStage);
            }

            if (currentStage == 0)
            {
                return true;
            }

            return false;
        }


        public static void Show(string uiInfo, Constant.LaunchStep step = Constant.LaunchStep.None, float percent = 0f)
        {
            if (string.IsNullOrEmpty(uiInfo)) return;

            if (_sUIMap.ContainsKey(uiInfo))
            {
                if (_sNowStep < step)
                {
                    _sNowStep = step;
                }

                float nowProgress = GetProgress(_sNowStep);
                Constant.LaunchStep nextStep = _sNowStep + 1;
                if (_sNowStep == Constant.LaunchStep.StartGame)
                {
                    nextStep = Constant.LaunchStep.Scene;
                }
                else if (_sNowStep == Constant.LaunchStep.CloseLoading)
                {
                    nextStep = Constant.LaunchStep.CloseLoading;
                }

                float nextProgress = GetProgress(nextStep);

                float offset = nextProgress - nowProgress;

                float progress = nowProgress + (offset * percent);

                GameModule.FUI.ShowUI(_sUIMap[uiInfo], progress);
            }
        }

        public static void HideAll()
        {
            GameModule.FUI.CloseUI<LauncherLoadWnd>();
        }

        private static float GetProgress(Constant.LaunchStep step)
        {
            float progress = 0;
            if (_sOpenType == OpenType.NewPlayer)
            {
                progress = _sNewbieSteps[step];
            }
            else
            {
                progress = _sLaunchSteps[step];
            }

            return progress;
        }


        private static void InitSteps()
        {
            // init launch steps
            _sLaunchSteps.Add(Constant.LaunchStep.None, 0);
            _sLaunchSteps.Add(Constant.LaunchStep.Launch, 1);
            _sLaunchSteps.Add(Constant.LaunchStep.Splash, 2);
            _sLaunchSteps.Add(Constant.LaunchStep.InitPackage, 3);
            _sLaunchSteps.Add(Constant.LaunchStep.UpdateVersion, 4);
            _sLaunchSteps.Add(Constant.LaunchStep.InitResources, 10);
            _sLaunchSteps.Add(Constant.LaunchStep.UpdateManifest, 11);
            _sLaunchSteps.Add(Constant.LaunchStep.CreateDownloader, 20);
            _sLaunchSteps.Add(Constant.LaunchStep.DownloadFile, 21);
            _sLaunchSteps.Add(Constant.LaunchStep.DownloadOver, 60);
            _sLaunchSteps.Add(Constant.LaunchStep.ClearCache, 65);
            _sLaunchSteps.Add(Constant.LaunchStep.Preload, 70);
            _sLaunchSteps.Add(Constant.LaunchStep.Assembly, 75);
            _sLaunchSteps.Add(Constant.LaunchStep.StartGame, 80);
            _sLaunchSteps.Add(Constant.LaunchStep.Scene, 81);
            _sLaunchSteps.Add(Constant.LaunchStep.Video, 95);
            _sLaunchSteps.Add(Constant.LaunchStep.CloseLoading, 100);

            // init newbie steps
            _sNewbieSteps.Add(Constant.LaunchStep.None, 0);
            _sNewbieSteps.Add(Constant.LaunchStep.Launch, 1);
            _sNewbieSteps.Add(Constant.LaunchStep.Splash, 2);
            _sNewbieSteps.Add(Constant.LaunchStep.InitPackage, 3);
            _sNewbieSteps.Add(Constant.LaunchStep.UpdateVersion, 4);
            _sNewbieSteps.Add(Constant.LaunchStep.InitResources, 10);
            _sNewbieSteps.Add(Constant.LaunchStep.UpdateManifest, 11);
            _sNewbieSteps.Add(Constant.LaunchStep.CreateDownloader, 20);
            _sNewbieSteps.Add(Constant.LaunchStep.DownloadFile, 21);
            _sNewbieSteps.Add(Constant.LaunchStep.DownloadOver, 60);
            _sNewbieSteps.Add(Constant.LaunchStep.ClearCache, 65);
            _sNewbieSteps.Add(Constant.LaunchStep.Preload, 66);
            _sNewbieSteps.Add(Constant.LaunchStep.Assembly, 70);
            _sNewbieSteps.Add(Constant.LaunchStep.StartGame, 75);
            _sNewbieSteps.Add(Constant.LaunchStep.Scene, 76);
            _sNewbieSteps.Add(Constant.LaunchStep.Video, 90);
            _sNewbieSteps.Add(Constant.LaunchStep.CloseLoading, 100);

            // init UIs
            _sUIMap.Add(LauncherUIDefine.LauncherLoadWnd, typeof(LauncherLoadWnd));
        }

        private static void InitLanguage()
        {
            int lang = 0;
            if (GameModule.Setting.HasSetting(Constant.LauncherSettingLanguage))
            {
                lang = GameModule.Setting.GetInt(Constant.LauncherSettingLanguage);
            }

            FairyGUI.RTLSupport.BaseDirection = FairyGUI.RTLSupport.DirectionType.LTR;
            Constant.GameLanguage launage = (Constant.GameLanguage)lang;
            switch (launage)
            {
                case Constant.GameLanguage.AR:
                    FairyGUI.RTLSupport.BaseDirection = FairyGUI.RTLSupport.DirectionType.RTL;
                    break;
            }
        }

        private static void OnLauncherLoadFinish()
        {
            HideAll();
        }

        private static void OnLauncherLoadProgress(int step, float progress)
        {
            Constant.LaunchStep eStep = (Constant.LaunchStep)step;

            Show(LauncherUIDefine.LauncherLoadWnd, eStep, progress);
        }

        private static void OnLauncherEventLoginProgress(int progress)
        {
            GameModule.FUI.ShowUI(typeof(LauncherLoadWnd), (float)progress);
        }
    }
}
