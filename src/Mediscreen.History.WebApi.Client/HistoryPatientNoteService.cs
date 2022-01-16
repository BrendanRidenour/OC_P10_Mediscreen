using System.Net.Http.Json;

namespace Mediscreen.Data
{
    public class HistoryPatientNoteService : IPatientNoteService
    {
        readonly HttpClient _http;

        public HistoryPatientNoteService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<PatientNoteEntity> Create(PatientNoteData note)
        {
            var response = await _http.PostAsJsonAsync("/patientnotes", note);

            response.EnsureSuccessStatusCode();

            var entity = await response.Content.ReadFromJsonAsync<PatientNoteEntity>();

            return entity!;
        }

        public async Task<IEnumerable<PatientNoteEntity>> Read(Guid patientId)
        {
            var response = await _http.GetAsync($"/patientnotes/{patientId}");

            response.EnsureSuccessStatusCode();

            var entities = await response.Content.ReadFromJsonAsync<IEnumerable<PatientNoteEntity>>();

            return entities ?? Array.Empty<PatientNoteEntity>();
        }

        public async Task<PatientNoteEntity?> Read(Guid patientId, Guid noteId)
        {
            var response = await _http.GetAsync($"/patientnotes/{patientId}/{noteId}");

            response.EnsureSuccessStatusCode();

            var entity = await response.Content.ReadFromJsonAsync<PatientNoteEntity>();

            return entity;
        }

        public async Task Update(PatientNoteEntity note)
        {
            var response = await _http.PutAsJsonAsync($"/patientnotes/{note.PatientId}/{note.Id}",
                note.Text);

            response.EnsureSuccessStatusCode();
        }
    }
}