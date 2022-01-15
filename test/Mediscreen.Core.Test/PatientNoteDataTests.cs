using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static ThoughtHaven.TestHelpers;

namespace Mediscreen
{
    public class PatientNoteDataTests
    {
        [Fact]
        public void PatientId_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientNoteData, RequiredAttribute>("PatientId");

            Assert.NotNull(attribute);
        }

        [Fact]
        public void PatientId_HasDisplayAttribute()
        {
            var attribute = GetPropertyAttribute<PatientNoteData, DisplayAttribute>("PatientId");

            Assert.Equal("Patient Id", attribute.Name);
        }

        [Fact]
        public void Text_HasRequiredAttribute()
        {
            var attribute = GetPropertyAttribute<PatientNoteData, RequiredAttribute>("Text");

            Assert.NotNull(attribute);
        }

        [Theory]
        [InlineData("text1")]
        [InlineData("text2")]
        public void Constructor_NoteOverload_SetsProperties(string text)
        {
            var note = new PatientNoteData()
            {
                PatientId = Guid.NewGuid(),
                Text = text,
            };

            var data = new PatientNoteData(note);

            Assert.Equal(note.PatientId, data.PatientId);
            Assert.Equal(text, data.Text);
        }
    }
}