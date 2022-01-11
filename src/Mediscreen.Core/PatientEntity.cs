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

            this.Id = id;
            this.GivenName = data.GivenName;
            this.FamilyName = data.FamilyName;
            this.DateOfBirth = data.DateOfBirth;
            this.BiologicalSex = data.BiologicalSex;
            this.HomeAddress = data.HomeAddress;
            this.PhoneNumber = data.PhoneNumber;
        }

        public PatientEntity() { }
    }
}