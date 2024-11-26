using UnityEngine.Networking;

namespace GameNetwork
{
    public class WebRequestCert : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}