using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        [HttpPost]
        public async Task<ActionResult<PatientEntity>> Create([FromBody] PatientData patient)
        {
            var entity = await _patientService.Create(patient);

            return CreatedAtAction(nameof(Read), new { id = entity.Id }, entity);
        }

        [HttpGet]
        public Task<IEnumerable<PatientEntity>> Read() => _patientService.Read();

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PatientEntity>> Read([FromRoute] Guid id)
        {
            var entity = await _patientService.Read(id);

            if (entity == null)
                return NotFound();

            return entity;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PatientData patient)
        {
            var entity = await _patientService.Read(id);

            if (entity is null)
                return NotFound();

            entity.Copy(patient);

            await _patientService.Update(entity);

            return NoContent();
        }
    }
}