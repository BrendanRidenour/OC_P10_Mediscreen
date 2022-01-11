using System;
using Xunit;

namespace Mediscreen
{
    public class BirthdateTests
    {
        [Fact]
        public void Constructor_MalformedValue_Throws()
        {
            var exception = Assert.Throws<ArgumentException>("value", () =>
            {
                new Birthdate(value: "bad_value");
            });

            Assert.Equal("Value could not be parsed. (Parameter 'value')", exception.Message);
        }

        [Theory]
        [InlineData(2020, 1, 2)]
        [InlineData(2021, 3, 4)]
        public void Constructor_WhenCalled_SetsProperties(int year, int month, int day)
        {
            var value = $"{year}-{month:00}-{day:00}";

            var birthdate = new Birthdate(value);

            Assert.Equal(year, birthdate.Year);
            Assert.Equal(month, birthdate.Month);
            Assert.Equal(day, birthdate.Day);
        }

        [Theory]
        [InlineData(2020, 1, 2)]
        [InlineData(2021, 3, 4)]
        public void ToString_WhenCalled_SetsProperties(int year, int month, int day)
        {
            var birthdate = new Birthdate(year, month, day);

            var result = birthdate.ToString();

            Assert.Equal($"{year}-{month:00}-{day:00}", result);
        }
    }
}