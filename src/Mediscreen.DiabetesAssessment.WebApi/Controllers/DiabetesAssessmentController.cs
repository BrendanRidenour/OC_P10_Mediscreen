using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiabetesAssessmentController : ControllerBase
    {
        readonly IPatientDiabetesAssessmentService _assessmentService;

        public DiabetesAssessmentController(IPatientDiabetesAssessmentService assessmentService)
        {
            _assessmentService = assessmentService ?? throw new ArgumentNullException(nameof(assessmentService));
        }

        [HttpGet("{patientId:guid}")]
        public async Task<ActionResult<string>> GenerateDiabetesReport([FromRoute] Guid patientId)
        {
            var result = await _assessmentService.GenerateDiabetesReport(patientId);

            if (result is null)
                return NotFound();

            return result;
        }

        [HttpGet("{patientFamilyName}")]
        public async Task<ActionResult<string>> GenerateDiabetesReport([FromRoute] string patientFamilyName)
        {
            var result = await _assessmentService.GenerateDiabetesReport(patientFamilyName);

            if (result is null)
                return NotFound();

            return result;
        }
    }
}