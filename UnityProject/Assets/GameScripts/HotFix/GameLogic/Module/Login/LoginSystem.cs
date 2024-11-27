using TEngine;

namespace GameLogic
{
    public class LoginSystem : BehaviourSingleton<LoginSystem>
    {
        public static int LoginProgressEntry = 25;
        public static int LoginProgressLogin = 50;
        public static int LoginProgressConnect = 75;
        public static int LoginProgressAuth = 100;
        private IFsm<LoginSystem> m_fsm;

        private void CreateFsm()
        {
            if (m_fsm == null)
            {
                // 先预加载,然后获取账号,再获取入口entry,再login,最后验证Auth
                FsmState<LoginSystem>[] fsmStates =
                {
                    new LoginStateInit(),
                    new LoginStateShowUI(),
                    new LoginStateGetAccount(),
                    new LoginStateReqEntry(),
                    new LoginStateLogin(),
                    new LoginStateConnectSocket(),
                    new LoginStateAuth(),
                    new LoginStateFinish(),
                };

                m_fsm = GameModule.Get<FsmModule>().CreateFsm(this, fsmStates);
            }
        }

        // 先加载
        public void StartLogin()
        {
            CreateFsm();
            Init();

            // test
            //ToPlayCore();
        }

        // 初始化核心玩法
        // public void EndLoginSystem()
        // {
        //     GameModule.Get<FsmModule>().DestroyFsm<LoginSystem>();
        //     m_fsm = null;
        // }

        private void Init()
        {
            m_fsm.Start<LoginStateInit>();
        }
    }
}
