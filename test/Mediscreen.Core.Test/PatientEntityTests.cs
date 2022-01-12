using System;
using Xunit;

namespace Mediscreen
{
    public class PatientEntityTests
    {
        [Fact]
        public void InheritsPatientData()
        {
            Assert.True(typeof(PatientData).IsAssignableFrom(typeof(PatientEntity)));
        }

        [Theory]
        [InlineData("8d29e605-697c-4459-9cb9-738bac29fff5", "G1", "F1", 1990, 1, 2, 0, "123 Fake Street", "(555) 555-5555")]
        [InlineData("65c096ac-5a6d-4e6f-b480-6335afd970d6", "G2", "F2", 1991, 2, 1, 1, "321 Fake Street", "(111) 111-1111")]
        public void IdAndDataCtor_WhenCalled_SetsProperties(string id, string givenName,
            string familyName, int dobYear, int dobMonth, int dobDay, int biologicalSex,
            string homeAddress, string phoneNumber)
        {
            var guidId = Guid.Parse(id);
            var data = new PatientData()
            {
                GivenName = givenName,
                FamilyName = familyName,
                DateOfBirth = new Date(dobYear, dobMonth, dobDay),
                BiologicalSex = (BiologicalSex)biologicalSex,
                HomeAddress = homeAddress,
                PhoneNumber = phoneNumber,
            };

            var entity = new PatientEntity(guidId, data);

            Assert.Equal(guidId, entity.Id);
            Assert.Equal(data.GivenName, entity.GivenName);
            Assert.Equal(data.FamilyName, entity.FamilyName);
            Assert.Equal(data.DateOfBirth, entity.DateOfBirth);
            Assert.Equal(data.BiologicalSex, entity.BiologicalSex);
            Assert.Equal(data.HomeAddress, entity.HomeAddress);
            Assert.Equal(data.PhoneNumber, entity.PhoneNumber);
        }

        [Theory]
        [InlineData("G1", "F1", 1990, 1, 2, 0, "123 Fake Street", "(555) 555-5555")]
        [InlineData("G2", "F2", 1991, 2, 1, 1, "321 Fake Street", "(111) 111-1111")]
        public void Copy_WhenCalled_SetsProperties(string givenName, string familyName, int dobYear,
            int dobMonth, int dobDay, int biologicalSex, string homeAddress, string phoneNumber)
        {
            var data = new PatientData()
            {
                GivenName = givenName,
                FamilyName = familyName,
                DateOfBirth = new Date(dobYear, dobMonth, dobDay),
                BiologicalSex = (BiologicalSex)biologicalSex,
                HomeAddress = homeAddress,
                PhoneNumber = phoneNumber,
            };
            var entity = new PatientEntity();

            entity.Copy(data);

            Assert.Equal(data.GivenName, entity.GivenName);
            Assert.Equal(data.FamilyName, entity.FamilyName);
            Assert.Equal(data.DateOfBirth, entity.DateOfBirth);
            Assert.Equal(data.BiologicalSex, entity.BiologicalSex);
            Assert.Equal(data.HomeAddress, entity.HomeAddress);
            Assert.Equal(data.PhoneNumber, entity.PhoneNumber);
        }
    }
}