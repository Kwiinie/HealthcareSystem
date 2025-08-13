using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
    {
        public void Configure(EntityTypeBuilder<Professional> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.User)
                .WithOne(u => u.Professional)
                .HasForeignKey<Professional>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Expertise)
                .WithMany()
                .HasForeignKey(p => p.ExpertiseId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.PrivateServices)
                .WithOne(ps => ps.Professional)
                .HasForeignKey(ps => ps.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProfessionalSpecialties)
           .WithOne(ps => ps.Professional) 
           .HasForeignKey(ps => ps.ProfessionalId) 
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                                 new Professional
                                 {
                                     Id = 1,
                                     UserId = 4,
                                     ExpertiseId = 8, // Bác sĩ chuyên khoa I
                                     Province = "Thành phố Hà Nội",
                                     District = "Quận Hoàn Kiếm",
                                     Ward = "Phường Hàng Bạc",
                                     Address = "Số 15, Phố Hàng Bông",
                                     Degree = "Chuyên khoa I Nội khoa",
                                     Experience = "Có 12 năm kinh nghiệm trong lĩnh vực khám chữa bệnh nội khoa",
                                     WorkingHours = "8:00 - 17:00",
                                     RequestStatus = ProfessionalRequestStatus.Approved
                                 },
    new Professional
    {
        Id = 2,
        UserId = 5,
        ExpertiseId = 2, // Bác sĩ y học cổ truyền
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận 1",
        Ward = "Phường Bến Nghé",
        Address = "Số 25, Đường Nguyễn Huệ",
        Degree = "Đại học Y học cổ truyền",
        Experience = "Có 8 năm kinh nghiệm trong điều trị các bệnh lý bằng y học cổ truyền",
        WorkingHours = "9:00 - 18:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 3,
        UserId = 8,
        ExpertiseId = 3, // Bác sĩ Răng - Hàm - Mặt
        Province = "Thành phố Đà Nẵng",
        District = "Quận Hải Châu",
        Ward = "Phường Thạch Thang",
        Address = "Số 42, Đường Trần Phú",
        Degree = "Đại học Răng Hàm Mặt",
        Experience = "Có 15 năm kinh nghiệm trong lĩnh vực nha khoa và phẫu thuật hàm mặt",
        WorkingHours = "8:30 - 17:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 4,
        UserId = 9,
        ExpertiseId = 9, // Bác sĩ chuyên khoa II
        Province = "Thành phố Cần Thơ",
        District = "Quận Ninh Kiều",
        Ward = "Phường Tân An",
        Address = "Số 28, Đường Hòa Bình",
        Degree = "Chuyên khoa II Tim mạch",
        Experience = "Có 9 năm kinh nghiệm trong lĩnh vực tim mạch và nội khoa",
        WorkingHours = "7:00 - 16:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 5,
        UserId = 12,
        ExpertiseId = 11, // Tiến sĩ Y khoa
        Province = "Tỉnh Bắc Ninh",
        District = "Thành phố Bắc Ninh",
        Ward = "Phường Đại Phúc",
        Address = "Số 55, Đường Ngô Gia Tự",
        Degree = "Tiến sĩ Y khoa chuyên ngành Ngoại khoa",
        Experience = "Có 18 năm kinh nghiệm trong lĩnh vực ngoại khoa và phẫu thuật tổng quát",
        WorkingHours = "7:30 - 16:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },

    // New professionals (6-20)
    new Professional
    {
        Id = 6,
        UserId = 13, // You'll need to add this user
        ExpertiseId = 10, // Thạc sĩ Y khoa
        Province = "Thành phố Hà Nội",
        District = "Quận Cầu Giấy",
        Ward = "Phường Dịch Vọng",
        Address = "Số 105, Đường Xuân Thủy",
        Degree = "Thạc sĩ Y khoa chuyên ngành Sản phụ khoa",
        Experience = "Có 11 năm kinh nghiệm trong lĩnh vực sản khoa và phụ khoa",
        WorkingHours = "8:00 - 17:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 7,
        UserId = 14, // You'll need to add this user
        ExpertiseId = 12, // Phó Giáo sư - Tiến sĩ
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận 5",
        Ward = "Phường 5",
        Address = "Số 215, Đường Hồng Bàng",
        Degree = "Phó Giáo sư - Tiến sĩ Y khoa",
        Experience = "Có 25 năm kinh nghiệm trong lĩnh vực thần kinh và nghiên cứu khoa học",
        WorkingHours = "9:00 - 16:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 8,
        UserId = 15, // You'll need to add this user
        ExpertiseId = 1, // Bác sĩ đa khoa
        Province = "Tỉnh Khánh Hòa",
        District = "Thành phố Nha Trang",
        Ward = "Phường Lộc Thọ",
        Address = "Số 38, Đường Trần Phú",
        Degree = "Đại học Y khoa",
        Experience = "Có 5 năm kinh nghiệm trong lĩnh vực khám chữa bệnh tổng quát",
        WorkingHours = "7:30 - 17:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 9,
        UserId = 16, // You'll need to add this user
        ExpertiseId = 6, // Cử nhân Điều dưỡng
        Province = "Tỉnh Bình Dương",
        District = "Thành phố Thủ Dầu Một",
        Ward = "Phường Phú Cường",
        Address = "Số 77, Đường Lê Hồng Phong",
        Degree = "Cử nhân Điều dưỡng",
        Experience = "Có 7 năm kinh nghiệm trong chăm sóc và điều dưỡng bệnh nhân",
        WorkingHours = "7:00 - 19:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 10,
        UserId = 17, // You'll need to add this user
        ExpertiseId = 5, // Dược sĩ đại học
        Province = "Thành phố Hải Phòng",
        District = "Quận Hồng Bàng",
        Ward = "Phường Hoàng Văn Thụ",
        Address = "Số 12, Đường Lạch Tray",
        Degree = "Dược sĩ đại học",
        Experience = "Có 10 năm kinh nghiệm trong lĩnh vực dược phẩm và tư vấn thuốc",
        WorkingHours = "8:00 - 18:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 11,
        UserId = 18, // You'll need to add this user
        ExpertiseId = 4, // Bác sĩ Y học dự phòng
        Province = "Tỉnh Thừa Thiên Huế",
        District = "Thành phố Huế",
        Ward = "Phường Phú Nhuận",
        Address = "Số 65, Đường Nguyễn Huệ",
        Degree = "Đại học Y học dự phòng",
        Experience = "Có 14 năm kinh nghiệm trong lĩnh vực y học dự phòng và kiểm soát dịch bệnh",
        WorkingHours = "7:30 - 16:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 12,
        UserId = 19, // You'll need to add this user
        ExpertiseId = 13, // Giáo sư - Tiến sĩ
        Province = "Thành phố Hà Nội",
        District = "Quận Ba Đình",
        Ward = "Phường Kim Mã",
        Address = "Số 43, Đường Liễu Giai",
        Degree = "Giáo sư - Tiến sĩ Y khoa",
        Experience = "Có 32 năm kinh nghiệm trong lĩnh vực nghiên cứu và điều trị ung thư",
        WorkingHours = "9:00 - 15:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 13,
        UserId = 20, // You'll need to add this user
        ExpertiseId = 7, // Bác sĩ nội trú
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận 7",
        Ward = "Phường Tân Phú",
        Address = "Số 153, Đường Nguyễn Thị Thập",
        Degree = "Bác sĩ nội trú chuyên ngành Nhi",
        Experience = "Có 6 năm kinh nghiệm trong lĩnh vực nhi khoa và hồi sức cấp cứu nhi",
        WorkingHours = "8:00 - 20:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 14,
        UserId = 21, // You'll need to add this user
        ExpertiseId = 3, // Bác sĩ Răng - Hàm - Mặt
        Province = "Tỉnh Nghệ An",
        District = "Thành phố Vinh",
        Ward = "Phường Hà Huy Tập",
        Address = "Số 88, Đường Lê Lợi",
        Degree = "Đại học Răng Hàm Mặt",
        Experience = "Có 9 năm kinh nghiệm trong lĩnh vực chỉnh nha và thẩm mỹ răng",
        WorkingHours = "8:30 - 17:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 15,
        UserId = 22, // You'll need to add this user
        ExpertiseId = 8, // Bác sĩ chuyên khoa I
        Province = "Tỉnh Bà Rịa - Vũng Tàu",
        District = "Thành phố Vũng Tàu",
        Ward = "Phường Thắng Tam",
        Address = "Số 27, Đường Lê Hồng Phong",
        Degree = "Chuyên khoa I Da liễu",
        Experience = "Có 13 năm kinh nghiệm trong lĩnh vực da liễu và thẩm mỹ da",
        WorkingHours = "8:00 - 17:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 16,
        UserId = 23, // You'll need to add this user
        ExpertiseId = 9, // Bác sĩ chuyên khoa II
        Province = "Tỉnh Lâm Đồng",
        District = "Thành phố Đà Lạt",
        Ward = "Phường 1",
        Address = "Số 56, Đường Phan Đình Phùng",
        Degree = "Chuyên khoa II Ngoại Tiêu hóa",
        Experience = "Có 16 năm kinh nghiệm trong lĩnh vực ngoại tiêu hóa và phẫu thuật nội soi",
        WorkingHours = "8:00 - 16:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 17,
        UserId = 24, // You'll need to add this user
        ExpertiseId = 2, // Bác sĩ y học cổ truyền
        Province = "Tỉnh Quảng Ninh",
        District = "Thành phố Hạ Long",
        Ward = "Phường Bãi Cháy",
        Address = "Số 19, Đường Hạ Long",
        Degree = "Đại học Y học cổ truyền",
        Experience = "Có 11 năm kinh nghiệm trong lĩnh vực y học cổ truyền và châm cứu",
        WorkingHours = "8:00 - 17:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 18,
        UserId = 25, // You'll need to add this user
        ExpertiseId = 11, // Tiến sĩ Y khoa
        Province = "Tỉnh Phú Thọ",
        District = "Thành phố Việt Trì",
        Ward = "Phường Nông Trang",
        Address = "Số 33, Đường Hùng Vương",
        Degree = "Tiến sĩ Y khoa chuyên ngành Tai Mũi Họng",
        Experience = "Có 17 năm kinh nghiệm trong lĩnh vực tai mũi họng và phẫu thuật đầu cổ",
        WorkingHours = "7:30 - 17:00",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 19,
        UserId = 26, // You'll need to add this user
        ExpertiseId = 10, // Thạc sĩ Y khoa
        Province = "Tỉnh Cà Mau",
        District = "Thành phố Cà Mau",
        Ward = "Phường 5",
        Address = "Số 48, Đường Nguyễn Tất Thành",
        Degree = "Thạc sĩ Y khoa chuyên ngành Mắt",
        Experience = "Có 8 năm kinh nghiệm trong lĩnh vực nhãn khoa và phẫu thuật mắt",
        WorkingHours = "7:30 - 16:30",
        RequestStatus = ProfessionalRequestStatus.Approved
    },
    new Professional
    {
        Id = 20,
        UserId = 27, // You'll need to add this user
        ExpertiseId = 1, // Bác sĩ đa khoa
        Province = "Tỉnh Sơn La",
        District = "Thành phố Sơn La",
        Ward = "Phường Quyết Thắng",
        Address = "Số 10, Đường Tô Hiệu",
        Degree = "Đại học Y khoa",
        Experience = "Có 4 năm kinh nghiệm trong lĩnh vực y học gia đình và chăm sóc sức khỏe cộng đồng",
        WorkingHours = "7:00 - 17:00",
        RequestStatus = ProfessionalRequestStatus.Pending
    }
                );

        }
    }
}
