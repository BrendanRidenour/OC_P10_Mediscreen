using Mediscreen.Data;
using Mediscreen.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [Route("[controller]/{patientId}")]
    public class PatientDiabetesAssessmentController : Controller
    {
        readonly IPatientService _patientService;
        readonly IPatientDiabetesAssessmentService _assessmentService;

        public PatientDiabetesAssessmentController(IPatientService patientService,
            IPatientDiabetesAssessmentService assessmentService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _assessmentService = assessmentService ?? throw new ArgumentNullException(nameof(assessmentService));
        }

        [HttpGet]
        public async Task<IActionResult> ReadReport([FromRoute] Guid patientId)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            var report = await _assessmentService.GenerateDiabetesReport(patientId);

            if (report is null)
                return NotFound();

            var viewModel = new PatientViewModel<string>(patient, report);

            return View(viewModel);
        }
    }
}
