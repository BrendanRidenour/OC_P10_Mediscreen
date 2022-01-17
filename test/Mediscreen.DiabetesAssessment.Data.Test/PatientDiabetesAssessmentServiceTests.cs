using Xunit;
using Mediscreen.Mocks;
using System;
using System.Threading.Tasks;

namespace Mediscreen.Data
{
    public class PatientDiabetesAssessmentServiceTests
    {
        [Fact]
        public async Task GenerateDiabetesReport_PatientServiceReturnsNull_ReturnsNull()
        {
            var patientService = PatientService();
            patientService.ReadById_Return = null;
            var service = Service(patientService);
            var patientId = Guid.NewGuid();

            var result = await service.GenerateDiabetesReport(patientId);

            Assert.Equal(patientId, patientService.ReadById_ParamId);
            Assert.Null(result);
        }

        [Fact]
        public async Task GenerateDiabetesReport_NoteServiceReturnsNull_ReturnsReport()
        {
            var patientId = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = new()
            {
                Id = patientId,
                GivenName = "GN",
                FamilyName = "FN",
                DateOfBirth = new Date(2000, 1, 1),
            };
            var noteService = NoteService();
            noteService.ReadByPatientId_Return = null!;
            var clock = Clock(new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
            var service = Service(patientService, noteService, clock: clock);

            var result = await service.GenerateDiabetesReport(patientId);

            Assert.Equal(patientId, noteService.ReadByPatientId_ParamPatientId);
            Assert.Equal("Patient: GN FN (age 20) diabetes assessment is: None", result);
        }

        [Fact]
        public async Task GenerateDiabetesReport_WhenCalled_CallsTriggerTermsCounter()
        {
            var patientId = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = new()
            {
                Id = patientId,
                GivenName = "GN",
                FamilyName = "FN",
                DateOfBirth = new Date(2000, 1, 1),
            };
            var noteService = NoteService();
            var triggerTermCounter = TriggerTermCounter();
            var service = Service(patientService, noteService, triggerTermCounter: triggerTermCounter);

            await service.GenerateDiabetesReport(patientId);

            Assert.Equal(noteService.ReadByPatientId_Return,
                triggerTermCounter.CountTriggerTerms_ParamNotes);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GenerateDiabetesReport_WhenCalled_CallsDiabetesRiskAnalyzer(int triggerTerms)
        {
            var patientId = Guid.NewGuid();
            var patientService = PatientService();
            patientService.ReadById_Return = new()
            {
                Id = patientId,
                GivenName = "GN",
                FamilyName = "FN",
                DateOfBirth = new Date(2000, 1, 1),
            };
            var noteService = NoteService();
            var triggerTermCounter = TriggerTermCounter();
            triggerTermCounter.CountTriggerTerms_Return = triggerTerms;
            var diabetesRiskAnalyzer = DiabetesRiskAnalyzer();
            var service = Service(patientService, noteService, triggerTermCounter: triggerTermCounter,
                diabetesRiskAnalyzer: diabetesRiskAnalyzer);

            await service.GenerateDiabetesReport(patientId);

            Assert.Equal(patientService.ReadById_Return,
                diabetesRiskAnalyzer.AnalyzeRisk_ParamPatient);
            Assert.Equal(triggerTerms,
                diabetesRiskAnalyzer.AnalyzeRisk_ParamTriggerTermsCount);
        }

        [Theory]
        [InlineData("GN1", "FN1", 2000, 20, 0)]
        [InlineData("GN2", "FN2", 1999, 21, 1)]
        [InlineData("GN3", "FN3", 1998, 22, 2)]
        [InlineData("GN4", "FN4", 1997, 23, 3)]
        public async Task GenerateDiabetesReport_WhenCalled_ReturnsReport(string givenName,
            string familyName, int birthYear, int expectedAge, int expectedDiabetesRiskLevel)
        {
            var patientId = Guid.NewGuid();
            var patient = new PatientEntity()
            {
                Id = patientId,
                GivenName = givenName,
                FamilyName = familyName,
                DateOfBirth = new Date(birthYear, 1, 1),
            };
            var patientService = PatientService();
            patientService.ReadById_Return = patient;
            var noteService = NoteService();
            var triggerTermCounter = TriggerTermCounter();
            var diabetesRiskAnalyzer = DiabetesRiskAnalyzer();
            diabetesRiskAnalyzer.AnalyzeRisk_Return = (DiabetesRiskLevel)expectedDiabetesRiskLevel;
            var clock = Clock(new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
            var service = Service(patientService, noteService, triggerTermCounter,
                diabetesRiskAnalyzer, clock);

            var result = await service.GenerateDiabetesReport(patientId);

            Assert.Equal($"Patient: {patient.GetFullName()} (age {expectedAge}) diabetes assessment is: {ToString((DiabetesRiskLevel)expectedDiabetesRiskLevel)}", result);

            string ToString(DiabetesRiskLevel diabetesRiskLevel)
            {
                if (diabetesRiskLevel == DiabetesRiskLevel.None)
                    return "None";

                if (diabetesRiskLevel == DiabetesRiskLevel.Borderline)
                    return "Borderline";

                if (diabetesRiskLevel == DiabetesRiskLevel.InDanger)
                    return "In danger";

                return "Early onset";
            }
        }

        static MockPatientService PatientService() => new();
        static MockPatientNoteService NoteService() => new();
        static MockTriggerTermCounter TriggerTermCounter() => new();
        static MockDiabetesRiskAnalyzer DiabetesRiskAnalyzer() => new();
        static MockSystemClock Clock(DateTimeOffset? utcNow = null) =>
            new(utcNow ?? DateTimeOffset.UtcNow);
        static PatientDiabetesAssessmentService Service(MockPatientService? patientService = null,
            MockPatientNoteService? noteService = null, MockTriggerTermCounter? triggerTermCounter = null,
            MockDiabetesRiskAnalyzer? diabetesRiskAnalyzer = null, MockSystemClock? clock = null) =>
            new(patientService ?? PatientService(), noteService ?? NoteService(),
                triggerTermCounter ?? TriggerTermCounter(),
                diabetesRiskAnalyzer ?? DiabetesRiskAnalyzer(),
                clock ?? Clock());
    }
}