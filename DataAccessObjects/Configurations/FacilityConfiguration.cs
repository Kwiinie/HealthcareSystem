using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccessObjects.Configurations
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasMany(f => f.FacilityDepartments)
                   .WithOne(fd => fd.Facility)
                   .HasForeignKey(fd => fd.FacilityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(f => f.PublicServices)
                   .WithOne(ps => ps.Facility)
                   .HasForeignKey(ps => ps.FacilityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Type)
                   .WithMany(ft => ft.Facilities)
                   .HasForeignKey(f => f.TypeId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
                        new Facility
                        {
                            Id = 1,
                            TypeId = 1, // Bệnh viện công
                            Name = "Bệnh viện Bạch Mai",
                            OperationDay = new DateOnly(2020, 5, 15),
                            Province = "Thành phố Hà Nội",
                            District = "Quận Đống Đa",
                            Ward = "Phường Phương Mai",
                            Address = "78 Đường Giải Phóng",
                            Description = "Một trong những bệnh viện đa khoa lớn nhất Việt Nam, cung cấp dịch vụ y tế chất lượng cao với đội ngũ y bác sĩ hàng đầu.",
                            Status = FacilityStatus.Active,
                            ImgUrl = "/images/facilities/bach-mai.jpg"
                        },
    new Facility
    {
        Id = 2,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Chợ Rẫy",
        OperationDay = new DateOnly(2019, 7, 20),
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận 5",
        Ward = "Phường 12",
        Address = "201 Nguyễn Chí Thanh",
        Description = "Bệnh viện hạng đặc biệt tại miền Nam, với trang thiết bị hiện đại và đội ngũ y bác sĩ giỏi chuyên môn.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/cho-ray.jpg"
    },
    new Facility
    {
        Id = 3,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện Đa khoa Quốc tế Vinmec Times City",
        OperationDay = new DateOnly(2022, 3, 10),
        Province = "Thành phố Hà Nội",
        District = "Quận Hai Bà Trưng",
        Ward = "Phường Vĩnh Tuy",
        Address = "458 Minh Khai",
        Description = "Bệnh viện tư nhân đẳng cấp quốc tế với cơ sở vật chất và trang thiết bị hiện đại hàng đầu Việt Nam.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/vinmec.jpg"
    },

    // Regional Hospitals
    new Facility
    {
        Id = 4,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Trung ương Huế",
        OperationDay = new DateOnly(2021, 8, 5),
        Province = "Thừa Thiên Huế",
        District = "Thành phố Huế",
        Ward = "Phường Phước Vĩnh",
        Address = "16 Lê Lợi",
        Description = "Bệnh viện đa khoa hạng đặc biệt tại miền Trung, cung cấp dịch vụ y tế chất lượng cao cho khu vực.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/hue-central.jpg"
    },
    new Facility
    {
        Id = 5,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa Trung ương Cần Thơ",
        OperationDay = new DateOnly(2020, 10, 17),
        Province = "Thành phố Cần Thơ",
        District = "Quận Ninh Kiều",
        Ward = "Phường Tân An",
        Address = "315 Nguyễn Văn Cừ",
        Description = "Bệnh viện đa khoa hạng đặc biệt tại miền Tây Nam Bộ, cung cấp dịch vụ y tế cho khu vực Đồng bằng sông Cửu Long.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/cantho-central.jpg"
    },

    // Specialized Hospitals
    new Facility
    {
        Id = 6,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện K Tân Triều",
        OperationDay = new DateOnly(2022, 1, 20),
        Province = "Thành phố Hà Nội",
        District = "Huyện Thanh Trì",
        Ward = "Xã Tân Triều",
        Address = "30 Cầu Bươu",
        Description = "Bệnh viện chuyên khoa ung bướu hàng đầu, cung cấp dịch vụ chẩn đoán và điều trị ung thư.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/k-hospital.jpg"
    },
    new Facility
    {
        Id = 7,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Mắt Trung ương",
        OperationDay = new DateOnly(2021, 5, 5),
        Province = "Thành phố Hà Nội",
        District = "Quận Đống Đa",
        Ward = "Phường Trung Liệt",
        Address = "85 Bà Triệu",
        Description = "Bệnh viện chuyên khoa mắt hàng đầu, cung cấp dịch vụ chẩn đoán và điều trị các bệnh lý về mắt.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/eye-hospital.jpg"
    },

    // Private Hospitals
    new Facility
    {
        Id = 8,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện FV",
        OperationDay = new DateOnly(2023, 2, 15),
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận 7",
        Ward = "Phường Tân Phú",
        Address = "6 Nguyễn Lương Bằng",
        Description = "Bệnh viện quốc tế với đội ngũ bác sĩ trong nước và quốc tế, cung cấp dịch vụ y tế chất lượng cao.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/fv-hospital.jpg"
    },
    new Facility
    {
        Id = 9,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện Đa khoa Hoàn Mỹ Đà Nẵng",
        OperationDay = new DateOnly(2022, 6, 10),
        Province = "Thành phố Đà Nẵng",
        District = "Quận Hải Châu",
        Ward = "Phường Thạch Thang",
        Address = "291 Nguyễn Văn Linh",
        Description = "Bệnh viện tư nhân hiện đại tại miền Trung, cung cấp dịch vụ y tế chất lượng cao.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/hoan-my.jpg"
    },

    // Medical Centers
    new Facility
    {
        Id = 10,
        TypeId = 3, // Trung tâm y tế
        Name = "Trung tâm Y tế huyện Bình Chánh",
        OperationDay = new DateOnly(2021, 9, 8),
        Province = "Thành phố Hồ Chí Minh",
        District = "Huyện Bình Chánh",
        Ward = "Thị trấn Tân Túc",
        Address = "84 Đường Vành Đai 4",
        Description = "Trung tâm y tế cung cấp dịch vụ khám chữa bệnh cho người dân địa phương với chi phí hợp lý.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/binh-chanh.jpg"
    },
    new Facility
    {
        Id = 11,
        TypeId = 3, // Trung tâm y tế
        Name = "Trung tâm Y tế quận Nam Từ Liêm",
        OperationDay = new DateOnly(2022, 4, 12),
        Province = "Thành phố Hà Nội",
        District = "Quận Nam Từ Liêm",
        Ward = "Phường Mỹ Đình",
        Address = "30 Đường Đỗ Đức Dục",
        Description = "Trung tâm y tế cung cấp dịch vụ chăm sóc sức khỏe ban đầu và phòng ngừa dịch bệnh cho cộng đồng.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/nam-tu-liem.jpg"
    },

    // Facilities in Northern Provinces
    new Facility
    {
        Id = 12,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa tỉnh Lào Cai",
        OperationDay = new DateOnly(2021, 11, 20),
        Province = "Tỉnh Lào Cai",
        District = "Thành phố Lào Cai",
        Ward = "Phường Nam Cường",
        Address = "638 Đường Trần Hưng Đạo",
        Description = "Bệnh viện đa khoa hạng I cung cấp dịch vụ y tế cho người dân tỉnh Lào Cai và các tỉnh lân cận.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/laocai.jpg"
    },
    new Facility
    {
        Id = 13,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa tỉnh Hải Dương",
        OperationDay = new DateOnly(2020, 12, 15),
        Province = "Tỉnh Hải Dương",
        District = "Thành phố Hải Dương",
        Ward = "Phường Trần Phú",
        Address = "225 Đường Nguyễn Lương Bằng",
        Description = "Bệnh viện đa khoa hạng I cung cấp dịch vụ y tế chất lượng cho người dân trong tỉnh.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/hai-duong.jpg"
    },

    // Facilities in Central Provinces
    new Facility
    {
        Id = 14,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa tỉnh Quảng Nam",
        OperationDay = new DateOnly(2022, 2, 8),
        Province = "Tỉnh Quảng Nam",
        District = "Thành phố Tam Kỳ",
        Ward = "Phường An Xuân",
        Address = "Đường Phan Châu Trinh",
        Description = "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ khám chữa bệnh cho người dân trong tỉnh Quảng Nam.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/quang-nam.jpg"
    },
    new Facility
    {
        Id = 15,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện Đa khoa Quốc tế Nha Trang",
        OperationDay = new DateOnly(2023, 1, 10),
        Province = "Tỉnh Khánh Hòa",
        District = "Thành phố Nha Trang",
        Ward = "Phường Vĩnh Hải",
        Address = "52 Trần Phú",
        Description = "Bệnh viện tư nhân cung cấp dịch vụ y tế chất lượng cao cho người dân và du khách tại Nha Trang.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/nhatrang.jpg"
    },

    // Facilities in Southern Provinces
    new Facility
    {
        Id = 16,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa tỉnh Đồng Tháp",
        OperationDay = new DateOnly(2021, 7, 8),
        Province = "Tỉnh Đồng Tháp",
        District = "Thành phố Cao Lãnh",
        Ward = "Phường 1",
        Address = "144 Đường Nguyễn Thị Minh Khai",
        Description = "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ y tế cho người dân tỉnh Đồng Tháp.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/dong-thap.jpg"
    },
    new Facility
    {
        Id = 17,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện Đa khoa Phương Châu",
        OperationDay = new DateOnly(2022, 9, 15),
        Province = "Thành phố Cần Thơ",
        District = "Quận Ninh Kiều",
        Ward = "Phường An Bình",
        Address = "70 Đường 30/4",
        Description = "Bệnh viện tư nhân chất lượng cao chuyên về sản phụ khoa và nhi khoa tại khu vực Đồng bằng sông Cửu Long.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/phuong-chau.jpg"
    },

    // Highland Facilities
    new Facility
    {
        Id = 18,
        TypeId = 1, // Bệnh viện công
        Name = "Bệnh viện Đa khoa tỉnh Lâm Đồng",
        OperationDay = new DateOnly(2021, 3, 25),
        Province = "Tỉnh Lâm Đồng",
        District = "Thành phố Đà Lạt",
        Ward = "Phường 8",
        Address = "17B Phù Đổng Thiên Vương",
        Description = "Bệnh viện đa khoa tuyến tỉnh, cung cấp dịch vụ y tế cho người dân Lâm Đồng và các tỉnh lân cận.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/lam-dong.jpg"
    },

    // Testing a facility with Inactive status
    new Facility
    {
        Id = 19,
        TypeId = 3, // Trung tâm y tế
        Name = "Trung tâm Y tế huyện Mèo Vạc",
        OperationDay = new DateOnly(2022, 8, 5),
        Province = "Tỉnh Hà Giang",
        District = "Huyện Mèo Vạc",
        Ward = "Thị trấn Mèo Vạc",
        Address = "Tổ 3",
        Description = "Trung tâm y tế cung cấp dịch vụ chăm sóc sức khỏe cơ bản cho đồng bào dân tộc vùng cao.",
        Status = FacilityStatus.Inactive,
        ImgUrl = "/images/facilities/meovac.jpg"
    },

    // A newly registered facility
    new Facility
    {
        Id = 20,
        TypeId = 2, // Bệnh viện tư
        Name = "Bệnh viện Quốc tế City",
        OperationDay = new DateOnly(2023, 3, 1),
        Province = "Thành phố Hồ Chí Minh",
        District = "Quận Bình Tân",
        Ward = "Phường Bình Trị Đông",
        Address = "24 Đường Số 4",
        Description = "Bệnh viện tư nhân mới thành lập, trang bị hiện đại và cung cấp dịch vụ y tế chất lượng cao.",
        Status = FacilityStatus.Active,
        ImgUrl = "/images/facilities/city-hospital.jpg"
    }
            );
        }
    }
}
