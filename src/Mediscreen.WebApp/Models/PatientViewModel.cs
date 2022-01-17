namespace Mediscreen.Models
{
    public class PatientViewModel<T>
    {
        public PatientEntity Patient { get; }
        public T Value { get; }

        public PatientViewModel(PatientEntity patient, T value)
        {
            Patient = patient;
            Value = value;
        }
    }
}