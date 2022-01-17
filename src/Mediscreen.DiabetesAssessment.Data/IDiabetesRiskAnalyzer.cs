namespace Mediscreen.Data
{
    public interface IDiabetesRiskAnalyzer
    {
        Task<DiabetesRiskLevel> AnalyzeRisk(PatientData patient, int triggerTermsCount);
    }
}