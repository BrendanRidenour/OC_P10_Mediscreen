namespace Mediscreen.Data
{
    public interface IPatientDiabetesAssessmentService
    {
        Task<string?> GenerateDiabetesReport(Guid patientId);
        Task<string?> GenerateDiabetesReport(string patientFamilyName);
    }
}