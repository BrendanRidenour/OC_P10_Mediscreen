using Mediscreen.Validation;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen
{
    public class PatientDataTests
    {
        [Fact]
        public void GivenName_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, RequiredAttribute>("GivenName");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void GivenName_HasMaxLengthAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, MaxLengthAttribute>("GivenName");

            Assert.NotNull(attribute);
            Assert.Equal(100, attribute.Length);
        }

        [Fact]
        public void GivenName_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("GivenName");

            Assert.NotNull(attribute);
            Assert.Equal("Given Name", attribute.Name);
        }

        [Fact]
        public void FamilyName_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, RequiredAttribute>("FamilyName");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void FamilyName_HasMaxLengthAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, MaxLengthAttribute>("FamilyName");

            Assert.NotNull(attribute);
            Assert.Equal(100, attribute.Length);
        }

        [Fact]
        public void FamilyName_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("FamilyName");

            Assert.NotNull(attribute);
            Assert.Equal("Family Name", attribute.Name);
        }

        [Fact]
        public void DateOfBirth_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, RequiredAttribute>("DateOfBirth");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void DateOfBirth_HasDateAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DateAttribute>("DateOfBirth");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void DateOfBirth_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("DateOfBirth");

            Assert.NotNull(attribute);
            Assert.Equal("Date of Birth", attribute.Name);
        }

        [Fact]
        public void BiologicalSex_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, RequiredAttribute>("BiologicalSex");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void BiologicalSex_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("BiologicalSex");

            Assert.NotNull(attribute);
            Assert.Equal("Sex Assigned at Birth", attribute.Name);
        }

        [Fact]
        public void HomeAddress_HasMaxLengthAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, MaxLengthAttribute>("HomeAddress");

            Assert.NotNull(attribute);
            Assert.Equal(255, attribute.Length);
        }

        [Fact]
        public void HomeAddress_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("HomeAddress");

            Assert.NotNull(attribute);
            Assert.Equal("Home Address", attribute.Name);
        }

        [Fact]
        public void PhoneNumber_HasPhoneAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, PhoneAttribute>("PhoneNumber");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void PhoneNumber_HasMaxLengthAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, MaxLengthAttribute>("PhoneNumber");

            Assert.Equal(100, attribute.Length);
        }

        [Fact]
        public void PhoneNumber_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientData, DisplayAttribute>("PhoneNumber");

            Assert.NotNull(attribute);
            Assert.Equal("Phone Number", attribute.Name);
        }

        [Theory]
        [InlineData("G1", "F1")]
        [InlineData("G2", "F2")]
        public void GetFullName_ReturnsName(string givenName, string familyName)
        {
            var patient = new PatientData()
            {
                GivenName = givenName,
                FamilyName = familyName,
            };

            Assert.Equal($"{givenName} {familyName}", patient.GetFullName());
        }
    }
}