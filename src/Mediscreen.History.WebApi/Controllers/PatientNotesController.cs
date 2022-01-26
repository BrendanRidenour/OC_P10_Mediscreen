using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mediscreen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientNotesController : ControllerBase
    {
        readonly IPatientNoteService _notesService;

        public PatientNotesController(IPatientNoteService notesService)
        {
            _notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }

        [HttpPost]
        public async Task<ActionResult<PatientNoteEntity>> Create([FromBody] PatientNoteData note)
        {
            var entity = await _notesService.Create(note);

            return CreatedAtAction(nameof(Read), new { patientId = entity.PatientId, noteId = entity.Id }, entity);
        }

        [HttpGet]
        [Route("{patientId}")]
        public async Task<ActionResult<IEnumerable<PatientNoteEntity>>> Read([FromRoute] Guid patientId)
        {
            var entities = await _notesService.Read(patientId);

            if (entities is null)
                return NotFound();

            return entities.ToArray();
        }

        [HttpGet]
        [Route("{patientId}/{noteId}")]
        public async Task<ActionResult<PatientNoteEntity>> Read([FromRoute] Guid patientId, [FromRoute] Guid noteId)
        {
            var entity = await _notesService.Read(patientId, noteId);

            if (entity == null)
                return NotFound();

            return entity;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PatientNoteEntity note)
        {
            var entity = await _notesService.Read(note.PatientId, note.Id);

            if (entity is null)
                return NotFound();

            await _notesService.Update(note);

            return NoContent();
        }
    }
}