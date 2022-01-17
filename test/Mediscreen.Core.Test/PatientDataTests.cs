using Mediscreen.Validation;
using System;
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

        [Theory]
        [InlineData(1990, 6, 1, 10)]
        [InlineData(1998, 5, 31, 2)]
        [InlineData(1998, 6, 1, 2)]
        [InlineData(1998, 6, 2, 1)]
        [InlineData(1999, 6, 1, 1)]
        [InlineData(1999, 6, 2, 0)]
        [InlineData(2000, 6, 1, 0)]
        [InlineData(2000, 5, 31, 0)]
        [InlineData(2001, 6, 1, 0)]
        public void GetAge_WhenCalled_ReturnsAge(int year, int month, int day, int expectedAge)
        {
            var date = new Date(year, month, day);
            var patient = new PatientData() { DateOfBirth = date };
            var today = new DateTimeOffset(year: 2000, month: 6, day: 1, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero);

            var age = patient.GetAge(today);

            Assert.Equal(expectedAge, age);
        }
    }
}