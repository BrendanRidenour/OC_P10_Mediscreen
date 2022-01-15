using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mediscreen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientNotesController : ControllerBase
    {
        readonly IPatientNotesService _notesService;

        public PatientNotesController(IPatientNotesService notesService)
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
        public Task<IEnumerable<PatientNoteEntity>> Read([FromRoute] Guid patientId) =>
            _notesService.Read(patientId);

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
        [Route("{patientId}/{noteId}")]
        public async Task<IActionResult> Update([FromRoute] Guid patientId, [FromRoute] Guid noteId,
            [FromBody, Required] string text)
        {
            var entity = await _notesService.Read(patientId, noteId);

            if (entity is null)
                return NotFound();

            entity = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            await _notesService.Update(entity);

            return NoContent();
        }
    }
}