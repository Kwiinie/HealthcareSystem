using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessObjects.Configurations
{
    public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
        {
            builder.ToTable("EmailVerificationTokens");
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
                
            builder.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(e => e.ExpiryDate)
                .IsRequired();
                
            builder.Property(e => e.IsUsed)
                .IsRequired()
                .HasDefaultValue(false);
                
            // Relationships
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexes
            builder.HasIndex(e => e.Token)
                .IsUnique();
                
            builder.HasIndex(e => e.Email);
            
            builder.HasIndex(e => e.UserId);
        }
    }
}
