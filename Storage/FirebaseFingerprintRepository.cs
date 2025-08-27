using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace FingerprintService.Storage
{
    public interface IFingerprintRepository
    {
        Task SaveTemplateAsync(string studentId, string base64Template);
        Task<IReadOnlyList<string>> GetTemplatesByStudentAsync(string studentId);
    }

    public class FirebaseFingerprintRepository : IFingerprintRepository
    {
        private readonly FirestoreDb _db;

        public FirebaseFingerprintRepository(FirestoreDb db)
        {
            _db = db;
        }

        public async Task SaveTemplateAsync(string studentId, string base64Template)
        {
            var doc = _db.Collection("students").Document(studentId).Collection("fingerprints").Document();
            await doc.SetAsync(new Dictionary<string, object>
            {
                { "template", base64Template },
                { "createdAt", FieldValue.ServerTimestamp }
            });
        }

        public async Task<IReadOnlyList<string>> GetTemplatesByStudentAsync(string studentId)
        {
            var snap = await _db.Collection("students").Document(studentId).Collection("fingerprints").GetSnapshotAsync();
            return snap.Documents
                .Select(d => d.ContainsField("template") ? d.GetValue<string>("template") : null)
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();
        }
    }
}


