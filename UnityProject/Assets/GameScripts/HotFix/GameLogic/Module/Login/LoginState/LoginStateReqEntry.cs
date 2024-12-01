using System;
using Cysharp.Threading.Tasks;
using GameData;
using TEngine;
using UnityEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.LoginSystem>;

namespace GameLogic
{
    public class LoginStateReqEntry : FsmState<LoginSystem>
    {
        private ProcedureOwner _mFsm;

        protected override void OnEnter(ProcedureOwner fsm)
        {
            Log.Debug("LoginStateReqEntry");
            _mFsm = fsm;
            GameEvent.AddEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.AddEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);

            RequestEntry();

            GameEvent.Send(Constant.LauncherEvent.LauncherEventLoginProgress, LoginSystem.LoginProgressEntry);
            
            ChangeState<LoginStateFinish>(_mFsm);
        }

        protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
        {
            _mFsm = null;
            DataSystem.Instance.Login.ReqIdEntry = 0;
            GameEvent.RemoveEventListener<WebRequestSuccessEventArgs>(GEvent.WebRequestSystemEventSuccess, OnWebRequestSuccess);
            GameEvent.RemoveEventListener<WebRequestFailureEventArgs>(GEvent.WebRequestSystemEventFailure, OnWebRequestFailure);
        }

        private void RequestEntry()
        {
            LoginData loginData = DataSystem.Instance.Login;
            loginData.DeviceUUID = SystemInfo.deviceUniqueIdentifier;
//             ReqEntry req = new ReqEntry()
//             {
//                 UUID = loginData.DeviceUUID,
// #if UNITY_ANDROID
//                 Platform = Protos.Platform.Android,
// #elif UNITY_IOS
//                 Platform = Protos.Platform.Ios,
// #elif UNITY_EDITOR
//                 Platform = Platform.Win,
// #else
//                 Platform = Protos.Platform.Win,
// #endif
//                 ResVersion = GameModule.Resource.InternalResourceVersion.ToString(),
//                 GameVersion = GameModule.Resource.ApplicableGameVersion,
//                 Channel = 0,
//             };
            
            if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Intranet)
            {
                loginData.EntryHost = loginData.IntranetHostEntry;
                loginData.EntryPort = loginData.IntranetPort;
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Extranet)
            {
                loginData.EntryHost = loginData.ExtranetHostEntry;
                loginData.EntryPort = loginData.ExtranetPort;
            }
            else if (SettingsUtils.ResourcesArea.ServerType == ServerTypeEnum.Formal)
            {
                loginData.EntryHost = loginData.FormalHostEntry;
                loginData.EntryPort = loginData.FormalPort;
            }


            // string uri = $"{loginData.EntryHost}:{loginData.EntryPort}/gate/entry";
            // var protobufSerializer = new ShortConnectProtobufSerializer(ShortConnectProtobufSerializer.SerializationFormat.Protobuf);
            // ShortConnectByteBuffer scbb = ShortConnectByteBuffer.GateRequestByteBuffer(protobufSerializer.Encode(req));
            //
            // loginData.ReqIdEntry = WebRequestSystem.Instance.AddWebRequestProto(uri, scbb.buffer, null);
        }

        private void OnWebRequestSuccess(WebRequestSuccessEventArgs args)
        {
            if (args != null && args.SerialId == DataSystem.Instance.Login.ReqIdEntry)
            {
                if (args.GetWebResponseBytes() != null)
                {
                    byte[] datas = args.GetWebResponseBytes();
                    // ShortConnectByteBuffer scbb = ShortConnectByteBuffer.ResponseByteBuffer(datas);
                    // byte[] pb = scbb.ReadBytes(datas.Length, true);
                    //
                    // RespEntry res = RespEntry.Parser.ParseFrom(pb);
                    // if (res != null)
                    // {
                    //     Log.Debug("OnWebRequestSuccess " + res.ErrCode);
                    //     if (res.ErrCode == ErrorCode.Ok)
                    //     {
                    //         // 成功
                    //         LoginData loginData = DataSystem.Instance.Login;
                    //         loginData.UrlLogin = res.LoginUrl;
                    //         ChangeState<LoginStateLogin>(_mFsm);
                    //     }
                    //     else
                    //     {
                    //         ChangeState<LoginStateGetAccount>(_mFsm);
                    //         // RetryRequestEntry().Forget();
                    //     }
                    // }
                }
                else
                {
                    ChangeState<LoginStateGetAccount>(_mFsm);
                }
            }
            else
            {
                ChangeState<LoginStateGetAccount>(_mFsm);
            }
        }

        private void OnWebRequestFailure(WebRequestFailureEventArgs args)
        {
            if (args != null && args.SerialId == DataSystem.Instance.Login.ReqIdEntry)
            {
                ChangeState<LoginStateGetAccount>(_mFsm);
                // RetryRequestEntry().Forget();
            }
        }

        private async UniTaskVoid RetryRequestEntry()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            RequestEntry();
        }
    }
}
