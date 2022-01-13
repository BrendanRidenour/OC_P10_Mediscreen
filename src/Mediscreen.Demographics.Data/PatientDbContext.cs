using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Mediscreen.Data
{
    public class PatientDbContext : DbContext
    {
        public DbSet<PatientEntity> Patients { get; set; } = null!;

        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientEntity>()
                .Property(e => e.DateOfBirth)
                .HasConversion(
                    convertToProviderExpression: date => JsonSerialize(date),
                    convertFromProviderExpression: json => JsonDeserialize(json));

            modelBuilder.Entity<PatientEntity>()
                .Property(e => e.BiologicalSex)
                .HasConversion(
                    convertToProviderExpression: value => (int)value,
                    convertFromProviderExpression: value => (BiologicalSex)value);
        }

        static string JsonSerialize(Date date) => JsonSerializer.Serialize(date);
        static Date JsonDeserialize(string json) => JsonSerializer.Deserialize<Date>(json)!;
    }
}