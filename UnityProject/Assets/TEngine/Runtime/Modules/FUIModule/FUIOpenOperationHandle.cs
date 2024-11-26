namespace TEngine
{
    internal class FUIOpenOperationHandle : IMemory
    {
        private enum EStatus
        {
            None,
            Waiting,
            Succeed,
            Failed
        }
        private EStatus m_status;


        public bool IsDone
        {
            get
            {
                return m_status == EStatus.Succeed || m_status == EStatus.Failed;
            }
        }

        public bool IsValid
        {
            get
            {
                return m_status != EStatus.Failed;
            }

        }

        public void OpenSucceed()
        {
            m_status = EStatus.Succeed;
        }

        public void OpenFailed()
        {
            m_status = EStatus.Failed;
        }

        public void Clear()
        {
            m_status = EStatus.None;
        }
    }
}
