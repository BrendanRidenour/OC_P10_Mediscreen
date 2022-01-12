using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mediscreen.Data
{
    public class EntityFrameworkPatientServiceTests
    {
        [Fact]
        public void ImplementsIPatientService()
        {
            Assert.True(typeof(IPatientService)
                .IsAssignableFrom(typeof(EntityFrameworkPatientService)));
        }

        [Theory]
        [InlineData("G1", "F1", 1990, 1, 2, 0, "123 Fake Street", "(555) 555-5555")]
        [InlineData("G2", "F2", 1991, 2, 1, 1, "321 Fake Street", "(111) 111-1111")]
        public async Task Create_WhenCalled_AddsEntityToDb(string givenName, string familyName,
            int dobYear, int dobMonth, int dobDay, int biologicalSex, string homeAddress,
            string phoneNumber)
        {
            using var db = Db();
            var service = new EntityFrameworkPatientService(db);
            var data = new PatientData()
            {
                GivenName = givenName,
                FamilyName = familyName,
                DateOfBirth = new Date(dobYear, dobMonth, dobDay),
                BiologicalSex = (BiologicalSex)biologicalSex,
                HomeAddress = homeAddress,
                PhoneNumber = phoneNumber,
            };

            var createEntity = await service.Create(data);

            Assert.NotNull(createEntity);
            Assert.False(createEntity.Id == System.Guid.Empty);
            Assert.Equal(data.GivenName, createEntity.GivenName);
            Assert.Equal(data.FamilyName, createEntity.FamilyName);
            Assert.Equal(data.DateOfBirth, createEntity.DateOfBirth);
            Assert.Equal(data.BiologicalSex, createEntity.BiologicalSex);
            Assert.Equal(data.HomeAddress, createEntity.HomeAddress);
            Assert.Equal(data.PhoneNumber, createEntity.PhoneNumber);

            var dbEntity = await db.Patients.SingleAsync(e => e.Id == createEntity.Id);

            Assert.Equal(dbEntity, createEntity);
        }

        [Fact]
        public async Task Read_WhenCalled_ReturnsEntities()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            using var seed = Db();
            seed.Patients.Add(Entity(id1));
            seed.Patients.Add(Entity(id2));

            await seed.SaveChangesAsync();

            var service = new EntityFrameworkPatientService(Db());

            // Act
            var entities = await service.Read();

            // Assert
            Assert.Equal(id1, entities.Single(e => e.Id == id1).Id);
            Assert.Equal(id2, entities.Single(e => e.Id == id2).Id);
        }

        [Fact]
        public async Task ReadById_IdDoesNotExist_ReturnsNull()
        {
            var db = Db();
            var service = new EntityFrameworkPatientService(db);

            var result = await service.Read(id: Guid.Empty);

            Assert.Null(result);
        }

        [Fact]
        public async Task ReadById_IdDoesExist_ReturnsEntity()
        {
            // Arrange
            var entity = Entity(Guid.NewGuid());

            var db = Db();
            db.Patients.Add(entity);
            await db.SaveChangesAsync();

            var service = new EntityFrameworkPatientService(db);

            // Act
            var result = await service.Read(id: entity.Id);

            // Assert
            Assert.Equal(entity, result);
        }

        [Theory]
        [InlineData("G1")]
        [InlineData("G2")]
        public async Task Update_WhenCalled_UpdatesEntity(string givenName)
        {
            // Arrange
            var entity = Entity(Guid.NewGuid());

            var db = Db();
            db.Patients.Add(entity);
            await db.SaveChangesAsync();

            var service = new EntityFrameworkPatientService(db);

            entity.GivenName = givenName;

            // Act
            await service.Update(entity);

            // Assert
            entity = await db.Patients.SingleAsync(e => e.Id == entity.Id);

            Assert.Equal(givenName, entity.GivenName);
        }

        static PatientDbContext Db()
        {
            var builder = new DbContextOptionsBuilder<PatientDbContext>()
                .UseInMemoryDatabase("PatientDemographics");

            return new PatientDbContext(builder.Options);
        }
        static PatientEntity Entity(Guid id) => new()
        {
            Id = id,
            GivenName = "GN",
            FamilyName = "FN",
            DateOfBirth = new Date(1990, 1, 1),
        };
    }
}