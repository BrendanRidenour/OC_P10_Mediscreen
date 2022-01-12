using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Mediscreen.Validation
{
    public class DateAttributeTests
    {
        [Fact]
        public void InheritsValidationAttribute()
        {
            Assert.True(typeof(ValidationAttribute).IsAssignableFrom(typeof(DateAttribute)));
        }

        [Fact]
        public void DefaultErrorMessage()
        {
            var attribute = new DateAttribute();

            Assert.Equal("Please enter a valid date.", attribute.ErrorMessage);
        }

        [Fact]
        public void IsValid_ValueIsNotDate_ReturnsTrue()
        {
            var attribute = new DateAttribute();

            var result = attribute.IsValid(value: "notdate");

            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(2000, 1, 32)]
        [InlineData(2000, 13, 1)]
        public void IsValid_InvalidDate_ReturnsFalse(int year, int month, int day)
        {
            var attribute = new DateAttribute();
            var date = new Date()
            {
                Year = year,
                Month = month,
                Day = day,
            };

            var result = attribute.IsValid(value: date);

            Assert.False(result);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2000, 1, 1)]
        public void IsValid_ValidDate_ReturnsTrue(int year, int month, int day)
        {
            var attribute = new DateAttribute();
            var date = new Date()
            {
                Year = year,
                Month = month,
                Day = day,
            };

            var result = attribute.IsValid(value: date);

            Assert.True(result);
        }
    }
}