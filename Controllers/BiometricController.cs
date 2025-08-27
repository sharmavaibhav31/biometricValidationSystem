using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FingerprintService.Services;
using FingerprintService.Storage;
using FingerprintService.Models;

namespace FingerprintService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BiometricController : ControllerBase
    {
        private readonly ITatvikFingerprintService _fingerprintService;
        private readonly IFingerprintRepository _repository;

        public BiometricController(ITatvikFingerprintService fingerprintService, IFingerprintRepository repository)
        {
            _fingerprintService = fingerprintService;
            _repository = repository;
        }

        [HttpGet("check-device")]
        public IActionResult CheckDevice()
        {
            bool isConnected = _fingerprintService.IsDeviceConnected();
            return Ok(new { connected = isConnected });
        }

        [HttpGet("device-info")]
        public IActionResult GetDeviceInfo()
        {
            var info = _fingerprintService.GetDeviceInfo();
            return Ok(info);
        }

        [HttpGet("capture")]
        public IActionResult Capture()
        {
            var templateBytes = _fingerprintService.CaptureTemplate(10000);
            string template = Convert.ToBase64String(templateBytes);
            return Ok(new { success = true, template });
        }

        [HttpPost("match")]
        public IActionResult Match([FromBody] FingerprintMatchRequest req)
        {
            byte[] refTpl = Convert.FromBase64String(req.ReferenceTemplate);
            byte[] claimTpl = Convert.FromBase64String(req.ClaimedTemplate);
            bool matched = _fingerprintService.MatchIsoTemplates(refTpl, claimTpl);
            return Ok(new { matched });
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollRequest request)
        {
            var templateBytes = _fingerprintService.CaptureTemplate(15000);
            var base64 = Convert.ToBase64String(templateBytes);
            await _repository.SaveTemplateAsync(request.StudentId, base64);
            return Ok(new { success = true });
        }

        [HttpPost("verify/{studentId}")]
        public async Task<IActionResult> Verify(string studentId)
        {
            var claim = _fingerprintService.CaptureTemplate(15000);
            var allTemplates = await _repository.GetTemplatesByStudentAsync(studentId);
            foreach (var tplBase64 in allTemplates)
            {
                var refTpl = Convert.FromBase64String(tplBase64);
                if (_fingerprintService.MatchIsoTemplates(refTpl, claim))
                {
                    return Ok(new { matched = true });
                }
            }
            return Ok(new { matched = false });
        }
    }
}


