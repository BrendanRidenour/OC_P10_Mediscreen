namespace Mediscreen.Data
{
    public interface IDiabetesRiskAnalyzer
    {
        DiabetesRiskLevel AnalyzeRisk(PatientData patient, int triggerTermsCount);
    }
}