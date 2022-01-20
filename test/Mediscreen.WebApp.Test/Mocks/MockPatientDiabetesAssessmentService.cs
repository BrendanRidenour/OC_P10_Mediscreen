using Mediscreen.Data;
using System;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockPatientDiabetesAssessmentService : IPatientDiabetesAssessmentService
    {
        public Guid? GenerateDiabetesReport_ParamPatientId;
        public string? GenerateDiabetesReport_Return = "report";
        public Task<string?> GenerateDiabetesReport(Guid patientId)
        {
            GenerateDiabetesReport_ParamPatientId = patientId;

            return Task.FromResult(GenerateDiabetesReport_Return);
        }

        public string? GenerateDiabetesReport_ParamPatientFamilyName;
        public Task<string?> GenerateDiabetesReport(string patientFamilyName)
        {
            GenerateDiabetesReport_ParamPatientFamilyName = patientFamilyName;

            return Task.FromResult(GenerateDiabetesReport_Return);
        }
    }
}
