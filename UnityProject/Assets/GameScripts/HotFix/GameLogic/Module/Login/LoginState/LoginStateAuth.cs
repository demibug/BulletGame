using System;
using Cysharp.Threading.Tasks;
using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateAuth : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateAuth");
            _mFsm = fsm;

            RegisterMessage();
            RequestAuth();

            GameEvent.AddEventListener(GEvent.WorldMapLoadComplete, OnLoadWorldComplete);

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoginProgress, LoginSystem.LoginProgressAuth);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            GameEvent.RemoveEventListener(GEvent.WorldMapLoadComplete, OnLoadWorldComplete);
        }

        private void RequestAuth()
        {
            // LoginData loginData = DataSystem.Instance.Login;
            // ReqAuth req = new ReqAuth()
            // {
            //     Token = loginData.LoginToken,
            // };
            //
            // NetworkSystem.Instance.Request<RespAuth>(GRoute.Auth, req, OnRespAuth);
        }

        // private void OnRespAuth(RespAuth data)
        // {
        //     if (data != null && data.ErrCode == ErrorCode.Ok)
        //     {
        //         Log.Debug("OnRespAuth success");
        //         DataSystem.Instance.Login.GameToken = data.GameToken;
        //
        //         // if (Debug.isDebugBuild)
        //         // {
        //         //     App.FUI.CloseUI<LoginWnd>();
        //         // }
        //         // else
        //         // {
        //         //     App.FUI.CloseUI<LoginWnd>();
        //         // }
        //
        //         //同步更新服务器时间
        //         GTimer.Instance.SetServerTime(data.ServerTime);
        //
        //         //初始化部份数据类
        //         InitBaseData();
        //
        //         // 初始化红点管理器
        //         RedNoteMgr.Instance.Init();
        //
        //         //初始化登陆后的消息查询
        //         //查询当前玩家的信息
        //         MsgManage.Instance.Hud.QueryPlayerInfo();
        //     }
        //     else
        //     {
        //         Log.Debug("OnRespAuth error");
        //         ChangeState<LoginStateGetAccount>(_mFsm);
        //         // RetryRequestAuth().Forget();
        //     }
        // }

        //注册本地的协议
        private void RegisterMessage()
        {
            // MsgManage.Instance.RegisterMessage();
        }

        private void InitBaseData()
        {
            //初始化贴纸相关的数据项
        }

        private async UniTaskVoid RetryRequestAuth()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            RequestAuth();
        }

        private void OnLoadWorldComplete()
        {
            ChangeState<LoginStateFinish>(_mFsm);
        }
    }
}
