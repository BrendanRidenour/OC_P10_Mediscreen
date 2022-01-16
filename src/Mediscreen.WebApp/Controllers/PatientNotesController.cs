using Mediscreen.Data;
using Mediscreen.Models.PatientNotes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mediscreen.Controllers
{
    [Route("[controller]/{patientId}")]
    public class PatientNotesController : Controller
    {
        readonly IPatientService _patientService;
        readonly IPatientNoteService _noteService;

        public PatientNotesController(IPatientService patientService, IPatientNoteService noteService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateNote([FromRoute] Guid patientId)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            var viewModel = new PatientViewModel<string>(patient, value: string.Empty);

            return View(viewModel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote([FromRoute] Guid patientId, [FromForm, Required] string text)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                var viewModel = new PatientViewModel<string>(patient, text);

                return View(viewModel);
            }

            var note = new PatientNoteData()
            {
                PatientId = patientId,
                Text = text,
            };

            var entity = await _noteService.Create(note);

            return RedirectToAction(actionName: nameof(ReadNotes), new { patientId = entity.PatientId });
        }

        [HttpGet]
        public async Task<IActionResult> ReadNotes([FromRoute] Guid patientId)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            var notes = await _noteService.Read(patientId);

            var viewModel = new PatientViewModel<IEnumerable<PatientNoteEntity>>(patient, notes);

            return View(viewModel);
        }

        [HttpGet("{noteId}/update")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid patientId, [FromRoute] Guid noteId)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            var note = await _noteService.Read(patientId, noteId);

            if (note is null)
                return NotFound();

            var viewModel = new PatientViewModel<string>(patient, note.Text);

            return View(viewModel);
        }

        [HttpPost("{noteId}/update")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid patientId, [FromRoute] Guid noteId,
            [FromForm, Required] string text)
        {
            var patient = await _patientService.Read(patientId);

            if (patient is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                var viewModel = new PatientViewModel<string>(patient, text);

                return View(viewModel);
            }

            var note = new PatientNoteEntity()
            {
                PatientId = patientId,
                Id = noteId,
                Text = text,
            };

            await _noteService.Update(note);

            return RedirectToAction(actionName: nameof(ReadNotes), new { patientId });
        }
    }
}