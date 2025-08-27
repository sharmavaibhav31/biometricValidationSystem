using System;
using TMF20SDK; // Provided by TMF20SDK.dll

namespace FingerprintService.Services
{
    public interface ITatvikFingerprintService
    {
        bool IsDeviceConnected();
        object GetDeviceInfo();
        byte[] CaptureTemplate(int timeoutMs);
        bool MatchIsoTemplates(byte[] referenceTemplate, byte[] claimedTemplate);
    }

    public class TatvikFingerprintService : ITatvikFingerprintService
    {
        private readonly TMF20FPLibrary _api;

        public TatvikFingerprintService()
        {
            _api = new TMF20FPLibrary();
        }

        public bool IsDeviceConnected()
        {
            return _api.isDeviceConnected();
        }

        public object GetDeviceInfo()
        {
            var info = new DeviceInfo();
            var code = _api.getDeviceInfo(info);
            if (code != 0)
            {
                throw new InvalidOperationException($"Tatvik device error code {code}");
            }
            return info;
        }

        public byte[] CaptureTemplate(int timeoutMs)
        {
            var result = new CaptureResult();
            var code = _api.captureFingerprint(result, timeoutMs);
            if (code != 0)
            {
                throw new InvalidOperationException($"Capture failed with code {code}: {result.errorString}");
            }
            return result.fmrBytes ?? result.TemplateData;
        }

        public bool MatchIsoTemplates(byte[] referenceTemplate, byte[] claimedTemplate)
        {
            return _api.matchIsoTemplates(referenceTemplate, claimedTemplate);
        }
    }
}


