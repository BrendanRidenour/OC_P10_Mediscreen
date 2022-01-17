using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mediscreen.Data
{
    public class TriggerTermsCounterTests
    {
        [Fact]
        public void TriggerTerms_ContainsExpectedValues()
        {
            var counter = new TriggerTermsCounter();

            Assert.Equal(11, counter.TriggerTerms.Count());
            Assert.Equal("Hemoglobin A1C", counter.TriggerTerms.ElementAt(0));
            Assert.Equal("Microalbumin", counter.TriggerTerms.ElementAt(1));
            Assert.Equal("Body Height", counter.TriggerTerms.ElementAt(2));
            Assert.Equal("Body Weight", counter.TriggerTerms.ElementAt(3));
            Assert.Equal("Smoker", counter.TriggerTerms.ElementAt(4));
            Assert.Equal("Abnormal", counter.TriggerTerms.ElementAt(5));
            Assert.Equal("Cholesterol", counter.TriggerTerms.ElementAt(6));
            Assert.Equal("Dizziness", counter.TriggerTerms.ElementAt(7));
            Assert.Equal("Relapse", counter.TriggerTerms.ElementAt(8));
            Assert.Equal("Reaction", counter.TriggerTerms.ElementAt(9));
            Assert.Equal("Antibodies", counter.TriggerTerms.ElementAt(10));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        public async Task CountTriggerTerms_WhenCalled_ReturnsCount(int expectedCount)
        {
            // Arrange
            var counter = Counter();
            var notes = new List<PatientNoteData>();

            for (var i = 0; i < expectedCount; i++)
                notes.Add(new PatientNoteData()
                {
                    Text = counter.TriggerTerms.ElementAt(i),
                });

            // Act
            var count = await counter.CountTriggerTerms(notes);

            // Assert
            Assert.Equal(expectedCount, count);
        }

        static TriggerTermsCounter Counter() => new();
    }
}