using TEngine;

namespace GameLogic
{
    public class LoadingSystem : BehaviourSingleton<LoadingSystem>
    {
        private IFsm<LoadingSystem> m_fsm;

        private void CreateFsm()
        {
            if (m_fsm == null)
            {
                // 先预加载,然后获取账号,再获取入口entry,再login,最后验证Auth
                FsmState<LoadingSystem>[] fsmStates =
                {
                    new LoadingStateInit(),
                    new LoadingStateLoadCommonUI(),
                    new LoadingStateLoadScene(),
                    new LoadingStateLoadVideo(),
                    new LoadingStateFinish(),
                };

                m_fsm = GameModule.Get<FsmModule>().CreateFsm(this, fsmStates);
            }
        }

        public void StartLoading()
        {
            CreateFsm();
            m_fsm.Start<LoadingStateInit>();
        }

        /// <summary>
        /// 开始登录
        /// </summary>
        public void ToLogin()
        {
            LoginSystem.Instance.StartLogin();
        }
    }
}
