namespace Mediscreen
{
    public interface IPatientService
    {
        public Task<PatientEntity> Create(PatientData patient);
        public Task<IEnumerable<PatientEntity>> Read();
        public Task<PatientEntity?> Read(Guid id);
        Task Update(PatientEntity entity);
        public Task Delete(Guid id);
    }
}