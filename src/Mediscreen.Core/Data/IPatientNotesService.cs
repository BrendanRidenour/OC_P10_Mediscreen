namespace Mediscreen.Data
{
    public interface IPatientNotesService
    {
        public Task<PatientNoteEntity> Create(PatientNoteData note);
        public Task<IEnumerable<PatientNoteEntity>> Read(Guid patientId);
        public Task<PatientNoteEntity?> Read(Guid patientId, Guid noteId);
        Task Update(PatientNoteEntity note);
    }
}