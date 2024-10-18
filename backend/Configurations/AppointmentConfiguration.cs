using backend.Enums;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Status)
                .IsRequired();
            builder.Property(a => a.Status)
                .HasConversion(
                    s => s.ToString(),
                    s => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), s)
                );

            builder.HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Patient)  
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(b => b.HasCheckConstraint("CK_Status_Reason",
                "(Status = 'Cancelled' AND ReasonForCancellation IS NOT NULL) OR (Status != 'Cancelled' AND ReasonForCancellation IS NULL)"));
            builder.ToTable(b => b.HasCheckConstraint("CK_Expected_Appointment_Date",
                "ExpectedAppointmentDate > GETDATE()")); 
        }
    }
}