using Microsoft.Extensions.Internal;

namespace Mediscreen.Data
{
    public class DiabetesRiskAnalyzer : IDiabetesRiskAnalyzer
    {
        readonly ISystemClock _clock;

        public DiabetesRiskAnalyzer(ISystemClock clock)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public DiabetesRiskLevel AnalyzeRisk(PatientData patient, int triggerTermsCount)
        {
            if (triggerTermsCount == 0)
                return DiabetesRiskLevel.None;

            var patientAge = patient.GetAge(_clock.UtcNow);

            if (patientAge < 30)
            {
                if (patient.BiologicalSex == BiologicalSex.Male)
                {
                    if (triggerTermsCount < 3)
                        return DiabetesRiskLevel.Borderline;

                    if (triggerTermsCount < 5)
                        return DiabetesRiskLevel.InDanger;

                    return DiabetesRiskLevel.EarlyOnset;
                }
                else if (patient.BiologicalSex == BiologicalSex.Female)
                {
                    if (triggerTermsCount < 4)
                        return DiabetesRiskLevel.Borderline;

                    if (triggerTermsCount < 7)
                        return DiabetesRiskLevel.InDanger;

                    return DiabetesRiskLevel.EarlyOnset;
                }
            }

            if (triggerTermsCount < 2)
                return DiabetesRiskLevel.None;

            if (triggerTermsCount < 6)
                return DiabetesRiskLevel.Borderline;

            if (triggerTermsCount < 8)
                return DiabetesRiskLevel.InDanger;

            return DiabetesRiskLevel.EarlyOnset;
        }
    }
}