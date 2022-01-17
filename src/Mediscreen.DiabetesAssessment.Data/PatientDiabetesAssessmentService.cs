//using Microsoft.Extensions.Internal;

//namespace Mediscreen.Data
//{
//    public class PatientDiabetesAssessmentService : IPatientDiabetesAssessmentService
//    {
//        readonly IPatientService _patientService;
//        readonly IPatientNoteService _noteService;
//        readonly ITriggerTermCounter _triggerTermsCounter;
//        readonly IDiabetesRiskAnalyzer _diabetesRiskAnalyzer;
//        readonly ISystemClock _clock;

//        public PatientDiabetesAssessmentService(IPatientService patientService,
//            IPatientNoteService noteService, ITriggerTermCounter triggerTermCounter,
//            IDiabetesRiskAnalyzer diabetesRiskAnalyzer, ISystemClock clock)
//        {
//            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
//            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
//            _triggerTermsCounter = triggerTermCounter ?? throw new ArgumentNullException(nameof(triggerTermCounter));
//            _diabetesRiskAnalyzer = diabetesRiskAnalyzer ?? throw new ArgumentNullException(nameof(diabetesRiskAnalyzer));
//            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
//        }

//        public async Task<string?> GenerateDiabetesReport(Guid patientId)
//        {
//            var patient = await _patientService.Read(patientId);

//            if (patient is null)
//                return null;

//            var notes = await _noteService.Read(patient.Id);

//            if (notes is null || !notes.Any())
//                return null;

//            var triggerTermsCount = await _triggerTermsCounter.CountTriggerTerms(notes);

//            var diabetesRiskLevel = await _diabetesRiskAnalyzer.AnalyzeRisk(patient, triggerTermsCount);

//            return $"Patient: {patient.GetFullName()} (age {patient.GetAge(_clock.UtcNow)}) diabetes assessment is: {ToString(diabetesRiskLevel)}";
//        }

//        static string ToString(DiabetesRiskLevel riskLevel)
//        {
//            if (riskLevel == DiabetesRiskLevel.None)
//                return "None";

//            if (riskLevel == DiabetesRiskLevel.Borderline)
//                return "Borderline";

//            if (riskLevel == DiabetesRiskLevel.InDanger)
//                return "In danger";

//            if (riskLevel == DiabetesRiskLevel.EarlyOnset)
//                return "Early onset";

//            throw new ArgumentException(paramName: nameof(riskLevel), message: $"Unrecognized {nameof(DiabetesRiskLevel)} value: '{riskLevel}'");
//        }
//    }
//}