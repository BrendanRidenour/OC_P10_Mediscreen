using System.Net.Http.Json;

namespace Mediscreen.Data
{
    public class PatientDiabetesAssessmentService : IPatientDiabetesAssessmentService
    {
        readonly HttpClient _http;

        public PatientDiabetesAssessmentService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<string?> GenerateDiabetesReport(Guid patientId)
        {
            var response = await _http.GetAsync($"/diabetesassessment/{patientId}");

            response.EnsureSuccessStatusCode();

            var report = await response.Content.ReadAsStringAsync();

            return report;
        }

        public async Task<string?> GenerateDiabetesReport(string patientFamilyName)
        {
            var response = await _http.GetAsync($"/diabetesassessment/{patientFamilyName}");

            response.EnsureSuccessStatusCode();

            var report = await response.Content.ReadAsStringAsync();

            return report;
        }
    }
}