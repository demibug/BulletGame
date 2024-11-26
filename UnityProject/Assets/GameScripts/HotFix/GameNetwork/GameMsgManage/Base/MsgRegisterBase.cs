using Protos;
using TEngine;

namespace GameNetwork
{
    public class MsgRegisterBase<T> : IMsgSystem where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new T();
                    Log.Assert(_instance != null);
                }

                return _instance;
            }
        }

        /// <summary>
        /// 是否已经注册过了的标识
        /// </summary>
        private bool m_isRegistered = false;

        public virtual void RegisterMessage()
        {
            if (m_isRegistered)
            {
                return;
            }
            m_isRegistered = true;
        }

        public virtual void UnregisterMessage()
        {
            m_isRegistered = false;
        }

        protected virtual void OnError(object data)
        {
            Log.Debug("OnResp error " + data);
            //ShowErrorCode(data);
        }


        protected void ShowErrorCode(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.Cancelled:   //操作已取消
                case ErrorCode.InvalidArgument:  // 参数错误
                case ErrorCode.Unavailable:  // 服务不可用
                case ErrorCode.Unauthenticated:  // 请求没有相应操作的有效身份验证凭据
                case ErrorCode.Forbidden:  // 请求被禁止
                case ErrorCode.ServerOffline:  // 游戏服下线
                case ErrorCode.WrongGameServer:  // 游戏服下线
                case ErrorCode.SessionData:  // 会话数据异常
                case ErrorCode.Rpcerror:  // RPC调用异常
                case ErrorCode.RoleNotExists:  // 角色数据丢失
                case ErrorCode.ItemNotEnough:  // 物品数量不足

                /* web，基本错误 */
                case ErrorCode.WebAccountError:  // 账号错误
                case ErrorCode.WebPasswordWrong:  // 密码错误
                case ErrorCode.WebAccountAlreadyExists:  // 账号已存在
                case ErrorCode.WebAccountIsForbidden:  // 账号被禁用
                case ErrorCode.WebAccountRegisterException:  // 账号注册异常
                case ErrorCode.WebAuthorizationFailed:  // 授权失败
                case ErrorCode.WebServerUnavailable:  // 服务不可用
                case ErrorCode.WebServerIsBusy:  // 服务器繁忙
                case ErrorCode.WebNoRoles:  // 当前服务器无角色
                case ErrorCode.WebGetRoleException:  // 查询角色失败
                case ErrorCode.WebCreateRoleException:  // 创建角色失败
                case ErrorCode.WebRoleNotExists:  // 角色不存在

                /* game模块,基本错误 */
                //    GameServerWrong     = 130000;  // 请求错误游戏服
                //    GameRoleNotExist    = 130001;
                //    GameProfileNotExist = 131001;  // game进程serverID没有设置(这情况一般不存在)
                case ErrorCode.GameItemNotEnough:  // 道具数量不足
                case ErrorCode.GameBoardMulti:  // 设置骰子倍数错误

                default:
                    Log.Error("Message Error: " +  code);
                    break;
            }
        }
    }
}
