namespace Mediscreen.Data
{
    public interface IPatientService
    {
        public Task<PatientEntity> Create(PatientData patient);
        public Task<IEnumerable<PatientEntity>> Read();
        public Task<PatientEntity?> Read(Guid id);
        public Task<PatientEntity?> Read(string familyName);
        Task Update(PatientEntity patient);
    }
}