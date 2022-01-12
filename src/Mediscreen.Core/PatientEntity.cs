namespace Mediscreen
{
    public class PatientEntity : PatientData
    {
        public virtual Guid Id { get; set; }

        public PatientEntity(Guid id, PatientData data)
            : this()
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            Id = id;
            Copy(data);
        }

        public PatientEntity() { }

        public void Copy(PatientData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            GivenName = data.GivenName;
            FamilyName = data.FamilyName;
            DateOfBirth = data.DateOfBirth;
            BiologicalSex = data.BiologicalSex;
            HomeAddress = data.HomeAddress;
            PhoneNumber = data.PhoneNumber;
        }
    }
}