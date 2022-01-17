namespace Mediscreen.Data
{
    public class TriggerTermsCounter : ITriggerTermCounter
    {
        public virtual IEnumerable<string> TriggerTerms { get; } = new string[]
        {
            "Hemoglobin A1C",
            "Microalbumin",
            "Body Height",
            "Body Weight",
            "Smoker",
            "Abnormal",
            "Cholesterol",
            "Dizziness",
            "Relapse",
            "Reaction",
            "Antibodies",
        };

        public Task<int> CountTriggerTerms(IEnumerable<PatientNoteData> notes)
        {
            var count = 0;

            foreach (var term in TriggerTerms)
                foreach (var note in notes)
                {
                    if (note.Text.Contains(term))
                    {
                        count++;

                        continue;
                    }
                }

            return Task.FromResult(count);
        }
    }
}