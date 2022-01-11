namespace Mediscreen.Mocks
{
    public class InMemoryPatientService : IPatientService
    {
        private static readonly List<PatientEntity> store = new()
        {
            new PatientEntity()
            {
                Id = Guid.Parse("30ef51ca-a698-41bf-9abb-14c0b1bf0950"),
                GivenName = "John",
                FamilyName = "Smith",
                BiologicalSex = BiologicalSex.Male,
                DateOfBirth = new Birthdate(1960, 1, 1),
                PhoneNumber = "(123) 456-7890",
            },
            new PatientEntity()
            {
                Id = Guid.Parse("ca6af5e9-bbad-455f-8b61-b2e05b5b16b0"),
                GivenName = "Jane",
                FamilyName = "Doe",
                BiologicalSex = BiologicalSex.Female,
                DateOfBirth = new Birthdate(1970, 12, 12),
                HomeAddress = "123 Fake Street, Citi, ST 54321",
            },
        };

        public Task<PatientEntity> Create(PatientData patient)
        {
            var entity = new PatientEntity(id: Guid.NewGuid(), patient);

            store.Add(entity);

            return Task.FromResult(entity);
        }

        public Task<IEnumerable<PatientEntity>> Read() =>
            Task.FromResult<IEnumerable<PatientEntity>>(store);

        public Task<PatientEntity?> Read(Guid id) =>
            Task.FromResult(store.SingleOrDefault(p => p.Id == id));

        public Task Update(PatientEntity entity)
        {
            var index = store.FindIndex(e => e.Id == entity.Id);

            if (index == -1)
                throw new InvalidOperationException();

            store[index] = entity;

            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            store.RemoveAll(e => e.Id == id);

            return Task.CompletedTask;
        }
    }
}