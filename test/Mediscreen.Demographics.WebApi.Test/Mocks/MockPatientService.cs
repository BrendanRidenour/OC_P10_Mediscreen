using Mediscreen.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediscreen.Mocks
{
    public class MockPatientService : IPatientService
    {
        public PatientData? Create_ParamPatient;
        public PatientEntity? Create_Return;
        public Task<PatientEntity> Create(PatientData patient)
        {
            this.Create_ParamPatient = patient;

            this.Create_Return = new PatientEntity(Guid.NewGuid(), patient);

            return Task.FromResult(Create_Return);
        }

        public IEnumerable<PatientEntity> Read_Return = new List<PatientEntity>();
        public Task<IEnumerable<PatientEntity>> Read() =>
            Task.FromResult(Read_Return);

        public Guid? ReadById_ParamId;
        public PatientEntity? ReadById_Return;
        public Task<PatientEntity?> Read(Guid id)
        {
            ReadById_ParamId = id;

            return Task.FromResult(ReadById_Return);
        }

        public PatientEntity? Update_ParamEntity;
        public Task Update(PatientEntity entity)
        {
            Update_ParamEntity = entity;

            return Task.CompletedTask;
        }

        public Guid? Delete_ParamId;
        public Task Delete(Guid id)
        {
            Delete_ParamId = id;

            return Task.CompletedTask;
        }
    }
}