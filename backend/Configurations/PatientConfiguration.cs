
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.DateOfBirth).IsRequired();
            builder.Property(p => p.Gender).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(100);
            builder.Property(p => p.Occupation).HasMaxLength(50);
            builder.Property(p => p.EmergencyContactName).HasMaxLength(50);
            builder.Property(p => p.EmergencyPhoneNumber).HasMaxLength(20);
            builder.Property(p => p.InsuranceProvider).HasMaxLength(150);
            builder.Property(p => p.InsurancePolicyNumber).HasMaxLength(50);
            builder.Property(p => p.Allergies).HasColumnType("TEXT");
            builder.Property(p => p.CurrentMedications).HasColumnType("TEXT");
            builder.Property(p => p.FamilyMedicalHistory).HasColumnType("TEXT");
            builder.Property(p => p.PastMedicalHistory).HasColumnType("TEXT");
            builder.Property(p => p.IdentificationType).HasMaxLength(50);
            builder.Property(p => p.IdentificationNumber).HasMaxLength(50);
            builder.Property(p => p.IdentificationDocumentURL).HasMaxLength(150);
            builder.Property(p => p.DisclosureOfHealthInfo).IsRequired();

            builder.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.PrimaryCarePhysician)
                .WithOne()
                .HasForeignKey<Patient>(p => p.PrimaryCarePhysicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(p => p.HasCheckConstraint("CK_Emergency_Phone",
             "EmergencyPhoneNumber LIKE '+%'"));
        }
    }
}
