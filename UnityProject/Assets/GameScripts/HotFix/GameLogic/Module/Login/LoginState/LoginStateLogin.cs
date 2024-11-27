using System;
using Cysharp.Threading.Tasks;
using GameData;
using TEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateLogin : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateLogin");
            _mFsm = fsm;
            GameEvent.AddEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.AddEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);

            RequestLogin();

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoginProgress, LoginSystem.LoginProgressLogin);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            DataSystem.Instance.Login.ReqIdLogin = 0;
            GameEvent.RemoveEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.RemoveEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);
        }

        private void RequestLogin()
        {
            LoginData loginData = DataSystem.Instance.Login;
            // ReqLogin req = new ReqLogin()
            // {
            //     UUID = loginData.DeviceUUID,
            //     Account = loginData.Account,
            //     Password = loginData.Password,
            //     ServerID = loginData.ServerId,
            // };

            string uri;
            if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Intranet)
            {
                uri = $"{loginData.EntryHost}:{loginData.EntryPort}/gate/debugl";
            }
            else
            {
                if (string.IsNullOrEmpty(loginData.UrlLogin))
                {
                    uri = loginData.UrlLogin;
                }
                else
                {
                    uri = $"{loginData.EntryHost}:{loginData.EntryPort}/gate/login";
                }
            }

            // Log.Debug("uri: " + uri + ", req " + req);

            // var protobufSerializer = new ShortConnectProtobufSerializer(ShortConnectProtobufSerializer.SerializationFormat.Protobuf);
            // ShortConnectByteBuffer scbb = ShortConnectByteBuffer.GateRequestByteBuffer(protobufSerializer.Encode(req));
            //
            // loginData.ReqIdLogin = WebRequestSystem.Instance.AddWebRequestProto(uri, scbb.buffer, null);
        }

        private void OnWebRequestSuccess(WebRequestSuccessEventArgs args)
        {
            if (args != null && args.SerialId == DataSystem.Instance.Login.ReqIdLogin)
            {
                if (args.GetWebResponseBytes() != null)
                {

                    byte[] datas = args.GetWebResponseBytes();
                    // ShortConnectByteBuffer scbb = ShortConnectByteBuffer.ResponseByteBuffer(datas);
                    // byte[] pb = scbb.ReadBytes(datas.Length, true);
                    // RespLogin res = RespLogin.Parser.ParseFrom(pb);
                    // if (res != null)
                    // {
                    //     if (res.ErrCode == ErrorCode.Ok)
                    //     {
                    //         // 成功
                    //         LoginData loginData = DataSystem.Instance.Login;
                    //         loginData.LoginToken = res.Token;
                    //
                    //         // 缓存当前成功登陆的用户名
                    //         GLocalCache.SetCurrentAccount(loginData.Account);
                    //
                    //         if (res.WSAddr != null)
                    //         {
                    //             loginData.HostHttp = $"{res.WSAddr.Protocol}{res.WSAddr.Host}";
                    //             loginData.PortHttp = res.WSAddr.Port;
                    //         }
                    //
                    //         ChangeState<LoginStateConnectSocket>(_mFsm);
                    //     }
                    //     else
                    //     {
                    //         Log.Warning(res.ErrCode + " " + res.Token);
                    //         ChangeState<LoginStateGetAccount>(_mFsm);
                    //         // RetryRequestLogin().Forget();
                    //     }
                    // }
                }
                else
                {
                    ChangeState<LoginStateGetAccount>(_mFsm);
                    // RetryRequestLogin().Forget();
                }
            }
        }

        private void OnWebRequestFailure(WebRequestFailureEventArgs args)
        {
            if (args != null && args.SerialId == DataSystem.Instance.Login.ReqIdEntry)
            {
                // RetryRequestLogin().Forget();
            }
        }

        private async UniTaskVoid RetryRequestLogin()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            RequestLogin();
        }
    }
}
