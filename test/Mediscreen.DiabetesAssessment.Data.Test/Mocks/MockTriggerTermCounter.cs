using Mediscreen.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockTriggerTermCounter : ITriggerTermCounter
    {
        public IEnumerable<PatientNoteData>? CountTriggerTerms_ParamNotes;
        public int CountTriggerTerms_Return = 0;
        public Task<int> CountTriggerTerms(IEnumerable<PatientNoteData> notes)
        {
            CountTriggerTerms_ParamNotes = notes;

            return Task.FromResult(CountTriggerTerms_Return);
        }
    }
}