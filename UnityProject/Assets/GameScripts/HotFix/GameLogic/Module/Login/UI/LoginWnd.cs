using Cysharp.Threading.Tasks;
using FairyGUI;
using GameAudio;
using GameData;
using Pkg_Login;
using System.Collections.Generic;
using System.Threading;
using TEngine;
using UnityEngine;
using Language = GameData.GDefine.Language;

namespace GameLogic
{
    [FUIWindow(FUILayer.Top, fullScreen: true, FUIPackageDefine.Res_Common, FUIPackageDefine.Pkg_Login)]
    public class LoginWnd : FUIWindow
    {
        private UI_LoginWnd m_view;

        private CancellationToken funcCancel;

        //默认的语言为阿语
        private Language m_curLanguage = Language.AR;

        public override void CheckBindAll()
        {
            FUISystem.Instance.CheckBindAll(Pkg_LoginBinder.BinderId, Pkg_LoginBinder.BindAll);
        }

        protected override GObject FUICreateInstance()
        {
            m_view = UI_LoginWnd.CreateInstance();
            return m_view;
        }

        protected override async UniTask<GObject> FUICreateInstanceAsync()
        {
            m_view = await UI_LoginWnd.CreateInstanceAsync();
            return m_view;
        }

        public override void RegisterEvent()
        {
            AddUIEvent(GEvent.WorldMapLoadComplete, OnLoadWorldComplete);
        }

        public override void OnCreate()
        {
            FUIExtension.SetFuiTexture(m_view.imgBg, "AssetLoad/ImageBg/loading_bg", false, "", true);
            // FUIExtension.SetFuiTexture(m_view.imgLoginBg, "login_bg");

            funcCancel = m_view.displayObject.gameObject.GetCancellationTokenOnDestroy();

            m_view.txtServerInput.text = "1";
            m_view.txtErrorTip.text = "";

            if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Intranet)
            {
                UserSetting setInfo = LocalData.GetDefaultSetting();
                if (setInfo != null)
                {
                    m_view.txtServerInput.text = setInfo.serverId;
                    m_view.txtUserInput.text = setInfo.userName;
                    AudioSystem.Instance.IsCanPlayBgMusic = setInfo.hasMusic;
                    AudioSystem.Instance.IsCanPlaySound = setInfo.hasSound;

                    m_curLanguage = setInfo.language;

                    // LMgr.CurLanguage = setInfo.language; // GameData.GDefine.Language.EN;
                    UIPackage.branch = LocalData.GetFUIBrance(setInfo.language);
                }
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Extranet)
            {
                UserSetting setInfo = LocalData.GetDefaultSetting();
                if (setInfo != null)
                {
                    m_view.txtServerInput.text = setInfo.serverId;
                    m_view.txtUserInput.text = setInfo.userName;

                    AudioSystem.Instance.IsCanPlayBgMusic = setInfo.hasMusic;
                    AudioSystem.Instance.IsCanPlaySound = setInfo.hasSound;

                    m_curLanguage = setInfo.language;

                    // LMgr.CurLanguage = setInfo.language; // GameData.GDefine.Language.EN;
                    UIPackage.branch = LocalData.GetFUIBrance(setInfo.language);
                }
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Formal)
            {
                UserSetting setInfo = LocalData.GetDefaultSetting();
                if (setInfo != null)
                {
                    m_view.txtServerInput.text = setInfo.serverId;
                    m_view.txtUserInput.text = setInfo.userName;
                    //
                    AudioSystem.Instance.IsCanPlayBgMusic = setInfo.hasMusic;
                    AudioSystem.Instance.IsCanPlaySound = setInfo.hasSound;

                    m_curLanguage = setInfo.language;

                    // LMgr.CurLanguage = setInfo.language; // GameData.GDefine.Language.EN;
                    UIPackage.branch = LocalData.GetFUIBrance(setInfo.language);
                }
            }


            // m_view.txtServerTitle.text = LMgr.TC(103601); // "Please enter the server number";
            // m_view.txtUserTitle.text = LMgr.TC(103602); // "Please enter the user name";

            if (Version.InternalGameVersion.Length > 0)
            {
                m_view.txtVersion.text = Version.GameVersion + "." + Version.InternalGameVersion; //版本：游戏版本.内部版本
            }
            else
            {
                m_view.txtVersion.text = Version.GameVersion; //版本：XXXX
            }

            m_view.txtServerInput.promptText = "[color=#888888]enter server number[/color]";
            m_view.txtUserInput.promptText = "[color=#888888]enter user name[/color]";

            // m_view.btnLogin.text = LMgr.TC(103603); // "Login";
            m_view.btnLogin.onClick.Set(OnBtnLoginClick);
#if UNITY_EDITOR || DEBUG
            LocalData.LogAppLanguage();
#endif

        }

        private void LoadDefaultFonts()
        {
            string fontName = "";
            if (m_curLanguage == Language.AR)
            {
                fontName = "DejaVuSansCondensed-Bold";
            }
            //else if (m_curLanguage == Language.EN)
            //{
            //    fontName = "DinNextLTArabicBlack";
            //}
            //else
            //{
            //    fontName = "msyhbd_1";
            //}

            if (fontName.Length == 0) return;

            //var fonts = ResSystem.Instance.LoadAsset<Font>("DejaVuSansCondensed-Bold", true, true, "");
            var fonts = Resources.Load<Font>("AssetLoad/Font/" + fontName);
            if (FontManager.sFontFactory.TryGetValue(fontName, out BaseFont font))
            {
                FontManager.RegisterFont(font, fontName);
            }
            else
            {
                //FontManager.RegisterFont(FontManager.GetFont(fontName), fontName);
                FontManager.RegisterFont(new DynamicFont(fontName, fonts));
            }
            UIConfig.defaultFont = fontName;
        }

        private void OnBtnLoginClick()
        {
            string strServerId = m_view.txtServerInput.text.Trim();
            string strUserName = m_view.txtUserInput.text.Trim();
            string strUserPass = "123";

            if (strServerId.Length == 0)
            {
                m_view.txtErrorTip.text = "need server id";
                m_view.txtServerInput.SetSelection(0, 1);
                return;
            }

            if (strUserName.Length == 0)
            {
                m_view.txtErrorTip.text = "need user name";
                m_view.txtUserInput.SetSelection(0, 1);
                return;
            }

            int LastEmojiItemId = 0;
            Dictionary<int, long> RedNoteDayDic = null;
            DataSystem.Instance.Player.accountId = strUserName;
            //根据当前的用户，取得本地缓存的配置，并进行处理
            UserSetting uSetting = LocalData.GetUserSetting(strUserName);
            if (uSetting != null)
            {
                AudioSystem.Instance.IsCanPlayBgMusic = uSetting.hasMusic;
                AudioSystem.Instance.IsCanPlaySound = uSetting.hasSound;

                m_curLanguage = uSetting.language;
                LastEmojiItemId = uSetting.LastEmojiItemId;
                RedNoteDayDic = uSetting.RedNoteDayDic;
                // LMgr.CurLanguage = uSetting.language; // GameData.GDefine.Language.EN;
                UIPackage.branch = LocalData.GetFUIBrance(uSetting.language);
            }

            LoadDefaultFonts();

            m_view.txtErrorTip.text = "";
            if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Intranet)
            {
                LocalData.UpdateDefaultSetting(strUserName, strServerId, m_curLanguage, AudioSystem.Instance.IsCanPlayBgMusic, AudioSystem.Instance.IsCanPlaySound, null, LastEmojiItemId, RedNoteDayDic);
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Extranet)
            {
                LocalData.UpdateDefaultSetting(strUserName, strServerId, m_curLanguage, AudioSystem.Instance.IsCanPlayBgMusic, AudioSystem.Instance.IsCanPlaySound, null, LastEmojiItemId, RedNoteDayDic);
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Formal)
            {
                LocalData.UpdateDefaultSetting(strUserName, strServerId, m_curLanguage, AudioSystem.Instance.IsCanPlayBgMusic, AudioSystem.Instance.IsCanPlaySound, null, LastEmojiItemId, RedNoteDayDic);
            }

            //抛出事件通知登陆
            GameEvent.Send(GEvent.GetAccountSuccess, strUserName, strUserPass, strServerId);
        }

        private void OnBtnResetCacheClick()
        {
            m_curLanguage = Language.AR;
            UserSetting setInfo = LocalData.GetDefaultSetting();
            if (setInfo != null)
            {
                setInfo.language = m_curLanguage;
                setInfo.hasMusic = true;
                setInfo.hasSound = true;

                AudioSystem.Instance.IsCanPlayBgMusic = setInfo.hasMusic;
                AudioSystem.Instance.IsCanPlaySound = setInfo.hasSound;

                LocalData.UpdateDefaultSetting();
            }
        }

        private void OnLoadWorldComplete()
        {
            FUISystem.Instance.CloseUI<LoginWnd>();
        }
    }
}
