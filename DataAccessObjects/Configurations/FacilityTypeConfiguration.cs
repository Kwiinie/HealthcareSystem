using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace DataAccessObjects.Configurations
{
    public class FacilityTypeConfiguration : IEntityTypeConfiguration<FacilityType>
    {
        public void Configure(EntityTypeBuilder<FacilityType> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(ft => ft.Facilities)
            .WithOne(f => f.Type)
            .HasForeignKey(f => f.TypeId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
                new FacilityType
                {
                    Id = 1,
                    Name = "Bệnh viện công",
                    Description = "Bệnh viện do nhà nước sở hữu và quản lý, cung cấp dịch vụ y tế cho người dân với chi phí thấp hơn."
                },
                new FacilityType
                {
                    Id = 2,
                    Name = "Bệnh viện tư",
                    Description = "Bệnh viện thuộc sở hữu của các cá nhân hoặc tổ chức tư nhân, cung cấp dịch vụ y tế với chất lượng cao và chi phí có thể cao hơn."
                },
                new FacilityType
                {
                    Id = 3,
                    Name = "Trung tâm y tế",
                    Description = "Cơ sở cung cấp dịch vụ y tế cơ bản và phòng ngừa cho cộng đồng."
                }
            );
        }
    }
}
