using Mediscreen.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mediscreen.Controllers
{
    [Route("[controller]/{patientId}")]
    public class PatientNotesController : Controller
    {
        readonly IPatientNoteService _noteService;

        public PatientNotesController(IPatientNoteService noteService)
        {
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }

        [HttpGet("create")]
        public IActionResult CreateNote() => View();

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote([FromRoute] Guid patientId, [FromForm, Required] string text)
        {
            if (!ModelState.IsValid)
                return View(model: text);

            var note = new PatientNoteData()
            {
                PatientId = patientId,
                Text = text,
            };

            var entity = await _noteService.Create(note);

            return RedirectToAction(actionName: nameof(ReadNote),
                new { patientId = entity.PatientId, noteId = entity.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ReadNotes([FromRoute] Guid patientId)
        {
            var notes = await _noteService.Read(patientId);

            return View(notes ?? Array.Empty<PatientNoteEntity>());
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> ReadNote([FromRoute] Guid patientId, [FromRoute] Guid noteId)
        {
            var note = await _noteService.Read(patientId, noteId);

            if (note is null)
                return NotFound();

            return View(note);
        }

        [HttpGet("{noteId}/update")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid patientId, [FromRoute] Guid noteId)
        {
            var note = await _noteService.Read(patientId, noteId);

            if (note is null)
                return NotFound();

            return View(note);
        }

        [HttpPost("{noteId}/update")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid patientId, [FromRoute] Guid noteId,
            [FromForm, Required] string text)
        {
            if (!ModelState.IsValid)
                return View(model: text);

            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            await _noteService.Update(note);

            return RedirectToAction(actionName: nameof(ReadNote), new { patientId, noteId });
        }
    }
}