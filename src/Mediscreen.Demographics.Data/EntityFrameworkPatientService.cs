using Microsoft.EntityFrameworkCore;

namespace Mediscreen.Data
{
    public class EntityFrameworkPatientService : IPatientService
    {
        readonly PatientDbContext _db;

        public EntityFrameworkPatientService(PatientDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<PatientEntity> Create(PatientData patient)
        {
            var entity = new PatientEntity(id: Guid.NewGuid(), patient);

            _db.Patients.Add(entity);

            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<PatientEntity>> Read() =>
            await _db.Patients.ToListAsync();

        public Task<PatientEntity?> Read(Guid id) =>
            _db.Patients.SingleOrDefaultAsync(e => e.Id == id);

        public Task<PatientEntity?> Read(string familyName) =>
            _db.Patients.SingleOrDefaultAsync(e => e.FamilyName == familyName);

        public async Task Update(PatientEntity entity)
        {
            _db.Patients.Update(entity);

            await _db.SaveChangesAsync();
        }
    }
}