using Mediscreen.Data;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockDiabetesRiskAnalyzer : IDiabetesRiskAnalyzer
    {
        public PatientData? AnalyzeRisk_ParamPatient;
        public int? AnalyzeRisk_ParamTriggerTermsCount;
        public DiabetesRiskLevel AnalyzeRisk_Return = DiabetesRiskLevel.None;
        public Task<DiabetesRiskLevel> AnalyzeRisk(PatientData patient, int triggerTermsCount)
        {
            AnalyzeRisk_ParamPatient = patient;
            AnalyzeRisk_ParamTriggerTermsCount = triggerTermsCount;

            return Task.FromResult(AnalyzeRisk_Return);
        }
    }
}