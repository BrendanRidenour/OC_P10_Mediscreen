using Mediscreen.Data;
using System;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockPatientDiabetesAssessmentService : IPatientDiabetesAssessmentService
    {
        public Guid? GenerateDiabetesReport_ParamPatientId;
        public string? GenerateDiabetesReport_Return;
        public Task<string?> GenerateDiabetesReport(Guid patientId)
        {
            GenerateDiabetesReport_ParamPatientId = patientId;

            return Task.FromResult(GenerateDiabetesReport_Return);
        }
    }
}