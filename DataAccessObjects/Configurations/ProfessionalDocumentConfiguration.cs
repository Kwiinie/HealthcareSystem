using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessObjects.Configurations
{
    public class ProfessionalDocumentConfiguration : IEntityTypeConfiguration<ProfessionalDocument>
    {
        public void Configure(EntityTypeBuilder<ProfessionalDocument> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DocumentName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.DocumentUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.DocumentNumber)
                .HasMaxLength(100);

            builder.Property(e => e.IssuingAuthority)
                .HasMaxLength(200);

            builder.Property(e => e.AdminNotes)
                .HasMaxLength(1000);

            builder.Property(e => e.RejectionReason)
                .HasMaxLength(500);

            builder.Property(e => e.FileExtension)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.OriginalFileName)
                .HasMaxLength(255);

            // Foreign Key relationships
            builder.HasOne(d => d.Professional)
                .WithMany()
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.ReviewedByUser)
                .WithMany()
                .HasForeignKey(d => d.ReviewedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(e => e.ProfessionalId);
            builder.HasIndex(e => e.VerificationStatus);
            builder.HasIndex(e => e.DocumentType);
            builder.HasIndex(e => e.ExpiryDate);
        }
    }
}
