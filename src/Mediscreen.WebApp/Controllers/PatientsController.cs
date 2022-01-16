using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [Route("[controller]")]
    public class PatientsController : Controller
    {
        readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        [HttpGet("create")]
        public IActionResult CreatePatient() => View();

        [HttpPost("create")]
        public async Task<IActionResult> CreatePatient([FromForm] PatientData patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            var entity = await _patientService.Create(patient);

            return RedirectToAction(actionName: nameof(ReadPatient), routeValues: new { id = entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ReadPatients()
        {
            var patients = await _patientService.Read();

            return View(patients ?? Array.Empty<PatientEntity>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadPatient([FromRoute] Guid id)
        {
            var patient = await _patientService.Read(id);

            if (patient is null)
                return NotFound();

            return View(patient);
        }

        [HttpGet("{id}/update")]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id)
        {
            var patient = await _patientService.Read(id);

            if (patient is null)
                return NotFound();

            return View(patient);
        }

        [HttpPost("{id}/update")]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, [FromForm] PatientData patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            await _patientService.Update(new PatientEntity(id, patient));

            return RedirectToAction(actionName: nameof(ReadPatient), routeValues: new { id });
        }
    }
}