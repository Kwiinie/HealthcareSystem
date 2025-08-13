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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(u => u.Patient)
                   .WithOne(p => p.User)
                   .HasForeignKey<Patient>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Professional)
                 .WithOne(p => p.User)
                 .HasForeignKey<Professional>(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Articles)
                .WithOne(a => a.CreatedBy)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                 new User
                 {
                     Id = 1,
                     Role = Role.Admin,
                     Fullname = "Nguyễn Văn Admin",
                     Email = "admin@example.com",
                     Password = "ad123456",
                     PhoneNumber = "0901234567",
                     Gender = "Nam",
                     Birthday = new DateOnly(1990, 1, 1),
                     Status = UserStatus.Active,
                     ImgUrl = "/images/users/admin_avatar.jpg"
                 },
    new User
    {
        Id = 2,
        Role = Role.Patient,
        Fullname = "Trần Thị Bích",
        Email = "patient1@example.com",
        Password = "pa123456",
        PhoneNumber = "0902345678",
        Gender = "Nữ",
        Birthday = new DateOnly(1995, 5, 20),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_female_1.jpg"
    },
    new User
    {
        Id = 3,
        Role = Role.Patient,
        Fullname = "Lê Văn Cường",
        Email = "patient2@example.com",
        Password = "pa123456",
        PhoneNumber = "0903456789",
        Gender = "Nam",
        Birthday = new DateOnly(1988, 10, 12),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_male_1.jpg"
    },
    new User
    {
        Id = 4,
        Role = Role.Professional,
        Fullname = "Phạm Minh Đức",
        Email = "professional1@example.com",
        Password = "pro123456",
        PhoneNumber = "0904567890",
        Gender = "Nam",
        Birthday = new DateOnly(1985, 3, 15),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_1.jpg"
    },
    new User
    {
        Id = 5,
        Role = Role.Professional,
        Fullname = "Vũ Thị Hương",
        Email = "professional2@example.com",
        Password = "pro123456",
        PhoneNumber = "0905678901",
        Gender = "Nữ",
        Birthday = new DateOnly(1987, 7, 30),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_1.jpg"
    },
    new User
    {
        Id = 6,
        Role = Role.Patient,
        Fullname = "Hoàng Thị Mai",
        Email = "patient3@example.com",
        Password = "pa123456",
        PhoneNumber = "0906789012",
        Gender = "Nữ",
        Birthday = new DateOnly(1992, 8, 25),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_female_2.jpg"
    },
    new User
    {
        Id = 7,
        Role = Role.Patient,
        Fullname = "Đỗ Quang Nam",
        Email = "patient4@example.com",
        Password = "pa123456",
        PhoneNumber = "0907890123",
        Gender = "Nam",
        Birthday = new DateOnly(1998, 4, 17),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_male_2.jpg"
    },
    new User
    {
        Id = 8,
        Role = Role.Professional,
        Fullname = "Ngô Thanh Tùng",
        Email = "professional3@example.com",
        Password = "pro123456",
        PhoneNumber = "0908901234",
        Gender = "Nam",
        Birthday = new DateOnly(1980, 11, 5),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_2.jpg"
    },
    new User
    {
        Id = 9,
        Role = Role.Professional,
        Fullname = "Lý Thị Hoa",
        Email = "professional4@example.com",
        Password = "pro123456",
        PhoneNumber = "0909012345",
        Gender = "Nữ",
        Birthday = new DateOnly(1983, 6, 10),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_2.jpg"
    },
    new User
    {
        Id = 10,
        Role = Role.Patient,
        Fullname = "Dương Văn Khải",
        Email = "patient5@example.com",
        Password = "pa123456",
        PhoneNumber = "0910123456",
        Gender = "Nam",
        Birthday = new DateOnly(1990, 9, 15),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_male_3.jpg"
    },
    new User
    {
        Id = 11,
        Role = Role.Patient,
        Fullname = "Trịnh Thu Phương",
        Email = "patient6@example.com",
        Password = "pa123456",
        PhoneNumber = "0911234567",
        Gender = "Nữ",
        Birthday = new DateOnly(1994, 2, 28),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/patient_female_3.jpg"
    },
    new User
    {
        Id = 12,
        Role = Role.Professional,
        Fullname = "Bùi Quốc Anh",
        Email = "professional5@example.com",
        Password = "pro123456",
        PhoneNumber = "0912345678",
        Gender = "Nam",
        Birthday = new DateOnly(1978, 4, 20),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_3.jpg"
    },
    new User
    {
        Id = 13,
        Role = Role.Professional,
        Fullname = "Nguyễn Thị Lan Anh",
        Email = "lananh@example.com",
        Password = "pro123456",
        PhoneNumber = "0913456789",
        Gender = "Nữ",
        Birthday = new DateOnly(1982, 5, 15),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_3.jpg"
    },
    new User
    {
        Id = 14,
        Role = Role.Professional,
        Fullname = "Trần Văn Minh",
        Email = "tranminh@example.com",
        Password = "pro123456",
        PhoneNumber = "0912345678",
        Gender = "Nam",
        Birthday = new DateOnly(1970, 8, 22),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_4.jpg"
    },
    new User
    {
        Id = 15,
        Role = Role.Professional,
        Fullname = "Phan Thị Thanh Hương",
        Email = "thanhhuong@example.com",
        Password = "pro123456",
        PhoneNumber = "0923456789",
        Gender = "Nữ",
        Birthday = new DateOnly(1990, 3, 10),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_4.jpg"
    },
    new User
    {
        Id = 16,
        Role = Role.Professional,
        Fullname = "Ngô Thị Mỹ Linh",
        Email = "mylinh@example.com",
        Password = "pro123456",
        PhoneNumber = "0934567890",
        Gender = "Nữ",
        Birthday = new DateOnly(1988, 11, 5),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_5.jpg"
    },
    new User
    {
        Id = 17,
        Role = Role.Professional,
        Fullname = "Đặng Quốc Tuấn",
        Email = "quoctuan@example.com",
        Password = "pro123456",
        PhoneNumber = "0945678901",
        Gender = "Nam",
        Birthday = new DateOnly(1985, 4, 18),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_5.jpg"
    },
    new User
    {
        Id = 18,
        Role = Role.Professional,
        Fullname = "Lê Thị Kim Ngân",
        Email = "kimngan@example.com",
        Password = "pro123456",
        PhoneNumber = "0956789012",
        Gender = "Nữ",
        Birthday = new DateOnly(1981, 6, 25),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_6.jpg"
    },
    new User
    {
        Id = 19,
        Role = Role.Professional,
        Fullname = "Hoàng Đức Thịnh",
        Email = "ducthinh@example.com",
        Password = "pro123456",
        PhoneNumber = "0967890123",
        Gender = "Nam",
        Birthday = new DateOnly(1965, 10, 15),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_6.jpg"
    },
    new User
    {
        Id = 20,
        Role = Role.Professional,
        Fullname = "Vũ Minh Quân",
        Email = "minhquan@example.com",
        Password = "pro123456",
        PhoneNumber = "0978901234",
        Gender = "Nam",
        Birthday = new DateOnly(1989, 2, 8),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_7.jpg"
    },
    new User
    {
        Id = 21,
        Role = Role.Professional,
        Fullname = "Trịnh Minh Hải",
        Email = "minhhai@example.com",
        Password = "pro123456",
        PhoneNumber = "0989012345",
        Gender = "Nam",
        Birthday = new DateOnly(1986, 7, 12),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_8.jpg"
    },
    new User
    {
        Id = 22,
        Role = Role.Professional,
        Fullname = "Phạm Thị Thùy Trang",
        Email = "thuytrang@example.com",
        Password = "pro123456",
        PhoneNumber = "0990123456",
        Gender = "Nữ",
        Birthday = new DateOnly(1982, 9, 28),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_7.jpg"
    },
    new User
    {
        Id = 23,
        Role = Role.Professional,
        Fullname = "Bùi Văn Hưng",
        Email = "vanhung@example.com",
        Password = "pro123456",
        PhoneNumber = "0901234567",
        Gender = "Nam",
        Birthday = new DateOnly(1979, 12, 5),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_9.jpg"
    },
    new User
    {
        Id = 24,
        Role = Role.Professional,
        Fullname = "Nguyễn Thị Bích Ngọc",
        Email = "bichngoc@example.com",
        Password = "pro123456",
        PhoneNumber = "0912345678",
        Gender = "Nữ",
        Birthday = new DateOnly(1984, 5, 15),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_8.jpg"
    },
    new User
    {
        Id = 25,
        Role = Role.Professional,
        Fullname = "Trương Quang Vinh",
        Email = "quangvinh@example.com",
        Password = "pro123456",
        PhoneNumber = "0923456789",
        Gender = "Nam",
        Birthday = new DateOnly(1978, 8, 20),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_male_10.jpg"
    },
    new User
    {
        Id = 26,
        Role = Role.Professional,
        Fullname = "Dương Thị Hà",
        Email = "duongha@example.com",
        Password = "pro123456",
        PhoneNumber = "0934567890",
        Gender = "Nữ",
        Birthday = new DateOnly(1987, 3, 12),
        Status = UserStatus.Active,
        ImgUrl = "/images/users/doctor_female_9.jpg"
    },
    new User
    {
        Id = 27,
        Role = Role.Professional,
        Fullname = "Mai Văn Thắng",
        Email = "vanthang@example.com",
        Password = "pro123456",
        PhoneNumber = "0945678901",
        Gender = "Nam",
        Birthday = new DateOnly(1991, 10, 8),
        Status = UserStatus.Inactive,
        ImgUrl = "/images/users/doctor_male_11.jpg"
    }


            ) ;
        }
    }
}
