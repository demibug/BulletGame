using UnityEngine;

namespace TEngine
{
    public sealed class WWWFormInfo : IReference
    {
        private WWWForm m_WWWForm;
        private object m_UserData;
        private bool m_UseProtobuf;

        public WWWFormInfo()
        {
            m_WWWForm = null;
            m_UserData = null;
        }

        public WWWForm WWWForm
        {
            get
            {
                return m_WWWForm;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public bool UseProtobuf
        {
            get
            {
                return m_UseProtobuf;
            }
        }

        public static WWWFormInfo Create(WWWForm wwwForm, object userData)
        {
            WWWFormInfo wwwFormInfo = ReferencePool.Acquire<WWWFormInfo>();
            wwwFormInfo.m_WWWForm = wwwForm;
            wwwFormInfo.m_UserData = userData;
            return wwwFormInfo;
        }

        public static WWWFormInfo Create(WWWForm wwwForm, object userData, bool useProtobuf)
        {
            WWWFormInfo wwwFormInfo = ReferencePool.Acquire<WWWFormInfo>();
            wwwFormInfo.m_WWWForm = wwwForm;
            wwwFormInfo.m_UserData = userData;
            wwwFormInfo.m_UseProtobuf = useProtobuf;
            return wwwFormInfo;
        }

        public void Clear()
        {
            m_WWWForm = null;
            m_UserData = null;
        }
    }
}