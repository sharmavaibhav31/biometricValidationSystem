namespace FingerprintService.Models
{
    public class FingerprintMatchRequest
    {
        public string ReferenceTemplate { get; set; }
        public string ClaimedTemplate { get; set; }
    }

    public class EnrollRequest
    {
        public string StudentId { get; set; }
    }
}


