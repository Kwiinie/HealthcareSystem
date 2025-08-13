using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(d => d.FacilityDepartments)
               .WithOne(fd => fd.Department)
               .HasForeignKey(fd => fd.DepartmentId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
            new Department
           {
               Id = 1,
               Name = "Khoa Nội",
               Description = "Chuyên điều trị các bệnh lý nội khoa như tim mạch, tiêu hóa, thần kinh."
           },
            new Department
            {
                Id = 2,
                Name = "Khoa Ngoại",
                Description = "Chuyên phẫu thuật và điều trị các bệnh lý ngoại khoa."
            },
            new Department
            {
                Id = 3,
                Name = "Khoa Sản",
                Description = "Chuyên chăm sóc sức khỏe phụ nữ, mang thai, sinh nở và các vấn đề liên quan."
            },
            new Department
            {
                Id = 4,
                Name = "Khoa Nhi",
                Description = "Chuyên điều trị các bệnh lý liên quan đến trẻ em và trẻ sơ sinh."
            },
            new Department
            {
                Id = 5,
                Name = "Khoa Xét nghiệm",
                Description = "Chuyên thực hiện các xét nghiệm chẩn đoán bệnh lý."
            },
            new Department
            {
                Id = 6,
                Name = "Khoa Chẩn đoán hình ảnh",
                Description = "Chuyên thực hiện các kỹ thuật hình ảnh như X-quang, MRI, CT scan."
            },
            new Department
            {
                Id = 7,
                Name = "Khoa Răng Hàm Mặt",
                Description = "Chuyên điều trị các vấn đề về răng miệng và các bệnh lý liên quan."
            },
            new Department
            {
                Id = 8,
                Name = "Khoa Mắt",
                Description = "Chuyên khám và điều trị các bệnh lý về mắt."
            },
            new Department
            {
                Id = 9,
                Name = "Khoa Tai Mũi Họng",
                Description = "Chuyên khám và điều trị các bệnh lý về tai, mũi, họng."
            },
            new Department
            {
                Id = 10,
                Name = "Khoa Da Liễu",
                Description = "Chuyên điều trị các bệnh lý về da và thẩm mỹ."
            },
            new Department
            {
                Id = 11,
                Name = "Khoa Cấp cứu",
                Description = "Chuyên cấp cứu và điều trị các bệnh nhân trong tình trạng khẩn cấp."
            },
            new Department
            {
                Id = 12,
                Name = "Khoa Hồi sức tích cực",
                Description = "Chuyên theo dõi và điều trị bệnh nhân trong tình trạng nguy kịch."
            },
            new Department
            {
                Id = 13,
                Name = "Khoa Tâm lý",
                Description = "Chuyên điều trị các vấn đề liên quan đến tâm lý, stress và trầm cảm."
            },
            new Department
            {
                Id = 14,
                Name = "Khoa Phục hồi chức năng",
                Description = "Chuyên phục hồi chức năng cho bệnh nhân sau tai nạn hoặc phẫu thuật."
            },
            new Department
            {
                Id = 15,
                Name = "Khoa Tiết niệu",
                Description = "Chuyên điều trị các bệnh lý liên quan đến hệ tiết niệu và thận."
            },
            new Department
            {
                Id = 16,
                Name = "Khoa Tim mạch",
                Description = "Chuyên điều trị các bệnh lý về tim và mạch máu."
            },
            new Department
            {
                Id = 17,
                Name = "Khoa Hô hấp",
                Description = "Chuyên điều trị các bệnh lý liên quan đến hệ hô hấp như phổi và khí quản."
            },
            new Department
            {
                Id = 18,
                Name = "Khoa Nội tiết",
                Description = "Chuyên điều trị các bệnh lý về nội tiết như tiểu đường, tuyến giáp."
            },
            new Department
            {
                Id = 19,
                Name = "Khoa Ung bướu",
                Description = "Chuyên điều trị các bệnh lý ung thư và các bệnh lý ác tính."
            },
            new Department
            {
                Id = 20,
                Name = "Khoa Dinh dưỡng",
                Description = "Chuyên tư vấn và điều trị các vấn đề liên quan đến dinh dưỡng."
            }
        );
        }
    }
}
