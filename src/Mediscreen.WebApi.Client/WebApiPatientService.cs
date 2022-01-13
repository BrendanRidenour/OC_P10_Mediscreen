using System.Net.Http.Json;

namespace Mediscreen.Data
{
    public class WebApiPatientService : IPatientService
    {
        readonly HttpClient _http;

        public WebApiPatientService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<PatientEntity> Create(PatientData patient)
        {
            var response = await _http.PostAsJsonAsync("/patients", patient);

            response.EnsureSuccessStatusCode();

            var entity = await response.Content.ReadFromJsonAsync<PatientEntity>();

            return entity!;
        }

        public async Task<IEnumerable<PatientEntity>> Read()
        {
            var response = await _http.GetAsync("/patients");

            response.EnsureSuccessStatusCode();

            var entities = await response.Content.ReadFromJsonAsync<IEnumerable<PatientEntity>>();

            return entities ?? Array.Empty<PatientEntity>();
        }

        public async Task<PatientEntity?> Read(Guid id)
        {
            var response = await _http.GetAsync($"/patients/{id}");

            response.EnsureSuccessStatusCode();

            var entity = await response.Content.ReadFromJsonAsync<PatientEntity>();

            return entity;
        }

        public async Task Update(PatientEntity patient)
        {
            var response = await _http.PutAsJsonAsync($"/patients/{patient.Id}", (PatientData)patient);

            response.EnsureSuccessStatusCode();
        }
    }
}