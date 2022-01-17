using Mediscreen.Mocks;
using System;
using Xunit;

namespace Mediscreen.Data
{
    public class DiabetesRiskAnalyzerTests
    {
        [Theory]
        [InlineData(29, BiologicalSex.Female, 0, DiabetesRiskLevel.None)]
        [InlineData(29, BiologicalSex.Female, 1, DiabetesRiskLevel.Borderline)]
        [InlineData(29, BiologicalSex.Female, 2, DiabetesRiskLevel.Borderline)]
        [InlineData(29, BiologicalSex.Female, 3, DiabetesRiskLevel.Borderline)]
        [InlineData(29, BiologicalSex.Female, 4, DiabetesRiskLevel.InDanger)]
        [InlineData(29, BiologicalSex.Female, 5, DiabetesRiskLevel.InDanger)]
        [InlineData(29, BiologicalSex.Female, 6, DiabetesRiskLevel.InDanger)]
        [InlineData(29, BiologicalSex.Female, 7, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(29, BiologicalSex.Female, 8, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(30, BiologicalSex.Female, 0, DiabetesRiskLevel.None)]
        [InlineData(30, BiologicalSex.Female, 1, DiabetesRiskLevel.None)]
        [InlineData(30, BiologicalSex.Female, 2, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Female, 3, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Female, 4, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Female, 5, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Female, 6, DiabetesRiskLevel.InDanger)]
        [InlineData(30, BiologicalSex.Female, 7, DiabetesRiskLevel.InDanger)]
        [InlineData(30, BiologicalSex.Female, 8, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(30, BiologicalSex.Female, 9, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(29, BiologicalSex.Male, 0, DiabetesRiskLevel.None)]
        [InlineData(29, BiologicalSex.Male, 1, DiabetesRiskLevel.Borderline)]
        [InlineData(29, BiologicalSex.Male, 2, DiabetesRiskLevel.Borderline)]
        [InlineData(29, BiologicalSex.Male, 3, DiabetesRiskLevel.InDanger)]
        [InlineData(29, BiologicalSex.Male, 4, DiabetesRiskLevel.InDanger)]
        [InlineData(29, BiologicalSex.Male, 5, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(29, BiologicalSex.Male, 6, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(30, BiologicalSex.Male, 0, DiabetesRiskLevel.None)]
        [InlineData(30, BiologicalSex.Male, 1, DiabetesRiskLevel.None)]
        [InlineData(30, BiologicalSex.Male, 2, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Male, 3, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Male, 4, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Male, 5, DiabetesRiskLevel.Borderline)]
        [InlineData(30, BiologicalSex.Male, 6, DiabetesRiskLevel.InDanger)]
        [InlineData(30, BiologicalSex.Male, 7, DiabetesRiskLevel.InDanger)]
        [InlineData(30, BiologicalSex.Male, 8, DiabetesRiskLevel.EarlyOnset)]
        [InlineData(30, BiologicalSex.Male, 9, DiabetesRiskLevel.EarlyOnset)]
        public void AnalyzeRisk_WhenCalled_ReturnsRiskLevel(int patientAge,
            BiologicalSex biologicalSex, int triggerTermsCount, DiabetesRiskLevel expectedRisk)
        {
            var clock = Clock();
            var analyzer = Analyzer(clock);
            var patient = Patient(patientAge, biologicalSex, clock);

            var risk = analyzer.AnalyzeRisk(patient, triggerTermsCount);

            Assert.Equal(expectedRisk, risk);
        }

        static MockSystemClock Clock() => new(new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
        static DiabetesRiskAnalyzer Analyzer(MockSystemClock? clock = null) => new(clock ?? Clock());
        static PatientData Patient(int age, BiologicalSex biologicalSex, MockSystemClock clock)
        {
            var dob = clock.UtcNow.AddYears(-age);

            return new PatientData()
            {
                DateOfBirth = new Date(dob.Year, dob.Month, dob.Day),
                BiologicalSex = biologicalSex,
            };
        }
    }
}