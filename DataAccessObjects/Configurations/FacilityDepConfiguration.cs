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
    public class FacilityDepConfiguration : IEntityTypeConfiguration<FacilityDepartment>
    {
        public void Configure(EntityTypeBuilder<FacilityDepartment> builder)
        {
            builder.HasKey(fd => fd.Id);

            builder.HasOne(fd => fd.Facility)
                .WithMany(f => f.FacilityDepartments) 
                .HasForeignKey(fd => fd.FacilityId)  
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(fd => fd.Department)
                .WithMany(d => d.FacilityDepartments) 
                .HasForeignKey(fd => fd.DepartmentId) 
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasData(
    // Bệnh viện Bạch Mai (Large public hospital - has many departments)
    new FacilityDepartment { Id = 1, FacilityId = 1, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 2, FacilityId = 1, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 3, FacilityId = 1, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 4, FacilityId = 1, DepartmentId = 4 },  // Khoa Nhi
    new FacilityDepartment { Id = 5, FacilityId = 1, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 6, FacilityId = 1, DepartmentId = 6 },  // Khoa Chẩn đoán hình ảnh
    new FacilityDepartment { Id = 7, FacilityId = 1, DepartmentId = 11 }, // Khoa Cấp cứu
    new FacilityDepartment { Id = 8, FacilityId = 1, DepartmentId = 12 }, // Khoa Hồi sức tích cực
    new FacilityDepartment { Id = 9, FacilityId = 1, DepartmentId = 16 }, // Khoa Tim mạch
    new FacilityDepartment { Id = 10, FacilityId = 1, DepartmentId = 19 }, // Khoa Ung bướu

    // Bệnh viện Chợ Rẫy (Another large public hospital)
    new FacilityDepartment { Id = 11, FacilityId = 2, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 12, FacilityId = 2, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 13, FacilityId = 2, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 14, FacilityId = 2, DepartmentId = 6 },  // Khoa Chẩn đoán hình ảnh
    new FacilityDepartment { Id = 15, FacilityId = 2, DepartmentId = 11 }, // Khoa Cấp cứu
    new FacilityDepartment { Id = 16, FacilityId = 2, DepartmentId = 15 }, // Khoa Tiết niệu
    new FacilityDepartment { Id = 17, FacilityId = 2, DepartmentId = 17 }, // Khoa Hô hấp
    new FacilityDepartment { Id = 18, FacilityId = 2, DepartmentId = 18 }, // Khoa Nội tiết

    // Bệnh viện Đa khoa Quốc tế Vinmec Times City (Private hospital)
    new FacilityDepartment { Id = 19, FacilityId = 3, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 20, FacilityId = 3, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 21, FacilityId = 3, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 22, FacilityId = 3, DepartmentId = 4 },  // Khoa Nhi
    new FacilityDepartment { Id = 23, FacilityId = 3, DepartmentId = 7 },  // Khoa Răng Hàm Mặt
    new FacilityDepartment { Id = 24, FacilityId = 3, DepartmentId = 10 }, // Khoa Da Liễu

    // Bệnh viện Trung ương Huế (Regional hospital)
    new FacilityDepartment { Id = 25, FacilityId = 4, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 26, FacilityId = 4, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 27, FacilityId = 4, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 28, FacilityId = 4, DepartmentId = 4 },  // Khoa Nhi
    new FacilityDepartment { Id = 29, FacilityId = 4, DepartmentId = 11 }, // Khoa Cấp cứu

    // Bệnh viện Đa khoa Trung ương Cần Thơ (Regional hospital)
    new FacilityDepartment { Id = 30, FacilityId = 5, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 31, FacilityId = 5, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 32, FacilityId = 5, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 33, FacilityId = 5, DepartmentId = 14 }, // Khoa Phục hồi chức năng

    // Bệnh viện K Tân Triều (Specialized oncology hospital)
    new FacilityDepartment { Id = 34, FacilityId = 6, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 35, FacilityId = 6, DepartmentId = 6 },  // Khoa Chẩn đoán hình ảnh
    new FacilityDepartment { Id = 36, FacilityId = 6, DepartmentId = 19 }, // Khoa Ung bướu

    // Bệnh viện Mắt Trung ương (Specialized eye hospital)
    new FacilityDepartment { Id = 37, FacilityId = 7, DepartmentId = 8 },  // Khoa Mắt

    // Bệnh viện FV (Private hospital)
    new FacilityDepartment { Id = 38, FacilityId = 8, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 39, FacilityId = 8, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 40, FacilityId = 8, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 41, FacilityId = 8, DepartmentId = 10 }, // Khoa Da Liễu

    // Bệnh viện Đa khoa Hoàn Mỹ Đà Nẵng (Private hospital)
    new FacilityDepartment { Id = 42, FacilityId = 9, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 43, FacilityId = 9, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 44, FacilityId = 9, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 45, FacilityId = 9, DepartmentId = 8 },  // Khoa Mắt

    // Trung tâm Y tế huyện Bình Chánh (Medical center)
    new FacilityDepartment { Id = 46, FacilityId = 10, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 47, FacilityId = 10, DepartmentId = 5 },  // Khoa Xét nghiệm

    // Trung tâm Y tế quận Nam Từ Liêm (Medical center)
    new FacilityDepartment { Id = 48, FacilityId = 11, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 49, FacilityId = 11, DepartmentId = 4 },  // Khoa Nhi

    // Bệnh viện Đa khoa tỉnh Lào Cai (Provincial hospital)
    new FacilityDepartment { Id = 50, FacilityId = 12, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 51, FacilityId = 12, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 52, FacilityId = 12, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 53, FacilityId = 12, DepartmentId = 4 },  // Khoa Nhi

    // Bệnh viện Đa khoa tỉnh Hải Dương (Provincial hospital)
    new FacilityDepartment { Id = 54, FacilityId = 13, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 55, FacilityId = 13, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 56, FacilityId = 13, DepartmentId = 9 },  // Khoa Tai Mũi Họng

    // Bệnh viện Đa khoa tỉnh Quảng Nam (Provincial hospital)
    new FacilityDepartment { Id = 57, FacilityId = 14, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 58, FacilityId = 14, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 59, FacilityId = 14, DepartmentId = 13 }, // Khoa Tâm lý

    // Bệnh viện Đa khoa Quốc tế Nha Trang (Private hospital)
    new FacilityDepartment { Id = 60, FacilityId = 15, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 61, FacilityId = 15, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 62, FacilityId = 15, DepartmentId = 10 }, // Khoa Da Liễu

    // Bệnh viện Đa khoa tỉnh Đồng Tháp (Provincial hospital)
    new FacilityDepartment { Id = 63, FacilityId = 16, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 64, FacilityId = 16, DepartmentId = 2 },  // Khoa Ngoại

    // Bệnh viện Đa khoa Phương Châu (Private hospital specialized in OB/GYN)
    new FacilityDepartment { Id = 65, FacilityId = 17, DepartmentId = 3 },  // Khoa Sản
    new FacilityDepartment { Id = 66, FacilityId = 17, DepartmentId = 4 },  // Khoa Nhi
    new FacilityDepartment { Id = 67, FacilityId = 17, DepartmentId = 5 },  // Khoa Xét nghiệm

    // Bệnh viện Đa khoa tỉnh Lâm Đồng (Provincial hospital)
    new FacilityDepartment { Id = 68, FacilityId = 18, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 69, FacilityId = 18, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 70, FacilityId = 18, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 71, FacilityId = 18, DepartmentId = 17 }, // Khoa Hô hấp

    // Trung tâm Y tế huyện Mèo Vạc (Rural medical center - inactive)
    new FacilityDepartment { Id = 72, FacilityId = 19, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 73, FacilityId = 19, DepartmentId = 4 },  // Khoa Nhi

    // Bệnh viện Quốc tế City (New private hospital)
    new FacilityDepartment { Id = 74, FacilityId = 20, DepartmentId = 1 },  // Khoa Nội
    new FacilityDepartment { Id = 75, FacilityId = 20, DepartmentId = 2 },  // Khoa Ngoại
    new FacilityDepartment { Id = 76, FacilityId = 20, DepartmentId = 5 },  // Khoa Xét nghiệm
    new FacilityDepartment { Id = 77, FacilityId = 20, DepartmentId = 6 },  // Khoa Chẩn đoán hình ảnh
    new FacilityDepartment { Id = 78, FacilityId = 20, DepartmentId = 7 },  // Khoa Răng Hàm Mặt
    new FacilityDepartment { Id = 79, FacilityId = 20, DepartmentId = 10 }, // Khoa Da Liễu
    new FacilityDepartment { Id = 80, FacilityId = 20, DepartmentId = 16 }  // Khoa Tim mạch
            );
        }
    }
}
