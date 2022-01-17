namespace Mediscreen.Data
{
    public interface ITriggerTermCounter
    {
        Task<int> CountTriggerTerms(IEnumerable<PatientNoteData> notes);
    }
}