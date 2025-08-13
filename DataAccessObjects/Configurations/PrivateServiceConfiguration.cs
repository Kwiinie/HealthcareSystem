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
    public class PrivateServiceConfiguration : IEntityTypeConfiguration<PrivateService>
    {
        public void Configure(EntityTypeBuilder<PrivateService> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.Professional)
                .WithMany(p => p.PrivateServices) 
                .HasForeignKey(ps => ps.ProfessionalId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                    new PrivateService
                    {
                        Id = 1,
                        Name = "Khám và tư vấn bệnh lý nội khoa",
                        Price = 500000,
                        Description = "Khám tổng quát và tư vấn các bệnh lý nội khoa như tim mạch, tiêu hóa, hô hấp.",
                        ProfessionalId = 1
                    },
    new PrivateService
    {
        Id = 2,
        Name = "Điều trị bệnh lý tiêu hóa",
        Price = 600000,
        Description = "Chẩn đoán và điều trị các bệnh lý về đường tiêu hóa như viêm dạ dày, trào ngược dạ dày.",
        ProfessionalId = 1
    },
    new PrivateService
    {
        Id = 3,
        Name = "Khám và quản lý bệnh mạn tính",
        Price = 450000,
        Description = "Khám, tư vấn và quản lý các bệnh mạn tính như tăng huyết áp, tiểu đường.",
        ProfessionalId = 8
    },
    new PrivateService
    {
        Id = 4,
        Name = "Tư vấn dinh dưỡng cho bệnh nhân nội khoa",
        Price = 350000,
        Description = "Tư vấn chế độ dinh dưỡng phù hợp cho người mắc các bệnh lý nội khoa.",
        ProfessionalId = 8
    },
    new PrivateService
    {
        Id = 5,
        Name = "Khám bệnh tiêu hóa chuyên sâu",
        Price = 700000,
        Description = "Khám và chẩn đoán chuyên sâu các bệnh lý tiêu hóa phức tạp.",
        ProfessionalId = 16
    },

    // Chuyên khoa Y học cổ truyền (Traditional Medicine) - Professional 2, 17
    new PrivateService
    {
        Id = 6,
        Name = "Khám và điều trị bằng y học cổ truyền",
        Price = 450000,
        Description = "Khám và điều trị các bệnh lý bằng phương pháp y học cổ truyền.",
        ProfessionalId = 2
    },
    new PrivateService
    {
        Id = 7,
        Name = "Châm cứu điều trị đau nhức",
        Price = 400000,
        Description = "Sử dụng phương pháp châm cứu để điều trị các chứng đau nhức cơ xương khớp.",
        ProfessionalId = 2
    },
    new PrivateService
    {
        Id = 8,
        Name = "Xoa bóp bấm huyệt",
        Price = 350000,
        Description = "Xoa bóp, bấm huyệt kết hợp với các bài thuốc cổ truyền để điều trị đau mỏi.",
        ProfessionalId = 17
    },
    new PrivateService
    {
        Id = 9,
        Name = "Cấy chỉ điều trị đau thần kinh tọa",
        Price = 600000,
        Description = "Sử dụng phương pháp cấy chỉ kết hợp với châm cứu để điều trị đau thần kinh tọa.",
        ProfessionalId = 17
    },

    // Chuyên khoa Răng - Hàm - Mặt (Dentistry) - Professional 3, 14
    new PrivateService
    {
        Id = 10,
        Name = "Khám và tư vấn răng miệng",
        Price = 300000,
        Description = "Khám tổng quát răng miệng, tư vấn chăm sóc và phòng ngừa bệnh lý răng miệng.",
        ProfessionalId = 3
    },
    new PrivateService
    {
        Id = 11,
        Name = "Điều trị tủy răng",
        Price = 1200000,
        Description = "Điều trị tủy răng cho các trường hợp viêm tủy, hoại tử tủy.",
        ProfessionalId = 3
    },
    new PrivateService
    {
        Id = 12,
        Name = "Trám răng thẩm mỹ",
        Price = 500000,
        Description = "Trám răng sâu bằng các vật liệu thẩm mỹ, màu sắc tự nhiên.",
        ProfessionalId = 14
    },
    new PrivateService
    {
        Id = 13,
        Name = "Nhổ răng khôn",
        Price = 1500000,
        Description = "Nhổ răng khôn với phương pháp ít đau, an toàn, phục hồi nhanh.",
        ProfessionalId = 14
    },

    // Chuyên khoa Tim mạch (Cardiology) - Professional 4
    new PrivateService
    {
        Id = 14,
        Name = "Khám và tư vấn bệnh lý tim mạch",
        Price = 650000,
        Description = "Khám, chẩn đoán và tư vấn các bệnh lý tim mạch như tăng huyết áp, rối loạn nhịp tim.",
        ProfessionalId = 4
    },
    new PrivateService
    {
        Id = 15,
        Name = "Siêu âm tim",
        Price = 600000,
        Description = "Siêu âm tim để đánh giá cấu trúc và chức năng của tim.",
        ProfessionalId = 4
    },
    new PrivateService
    {
        Id = 16,
        Name = "Điện tâm đồ",
        Price = 250000,
        Description = "Đo điện tâm đồ để đánh giá nhịp tim và phát hiện các bất thường về điện tim.",
        ProfessionalId = 4
    },

    // Chuyên khoa Ngoại (Surgery) - Professional 5, 16
    new PrivateService
    {
        Id = 17,
        Name = "Khám và tư vấn phẫu thuật",
        Price = 600000,
        Description = "Khám, chẩn đoán và tư vấn các trường hợp cần can thiệp phẫu thuật.",
        ProfessionalId = 5
    },
    new PrivateService
    {
        Id = 18,
        Name = "Phẫu thuật cắt u lành tính",
        Price = 3500000,
        Description = "Phẫu thuật cắt bỏ các u lành tính trên da và dưới da.",
        ProfessionalId = 5
    },
    new PrivateService
    {
        Id = 19,
        Name = "Phẫu thuật nội soi tiêu hóa",
        Price = 5000000,
        Description = "Phẫu thuật nội soi điều trị các bệnh lý tiêu hóa như viêm ruột thừa, sỏi mật.",
        ProfessionalId = 16
    },

    // Chuyên khoa Sản phụ khoa (Obstetrics & Gynecology) - Professional 6
    new PrivateService
    {
        Id = 20,
        Name = "Khám phụ khoa tổng quát",
        Price = 500000,
        Description = "Khám phụ khoa tổng quát định kỳ, phát hiện sớm các bệnh lý phụ khoa.",
        ProfessionalId = 6
    },
    new PrivateService
    {
        Id = 21,
        Name = "Siêu âm phụ khoa",
        Price = 400000,
        Description = "Siêu âm đánh giá tình trạng tử cung, buồng trứng và các cơ quan sinh sản nữ.",
        ProfessionalId = 6
    },
    new PrivateService
    {
        Id = 22,
        Name = "Khám thai định kỳ",
        Price = 550000,
        Description = "Khám thai định kỳ, theo dõi sự phát triển của thai nhi và sức khỏe của mẹ.",
        ProfessionalId = 6
    },

    // Chuyên khoa Thần kinh (Neurology) - Professional 7
    new PrivateService
    {
        Id = 23,
        Name = "Khám và tư vấn bệnh lý thần kinh",
        Price = 700000,
        Description = "Khám, chẩn đoán và tư vấn các bệnh lý thần kinh như đau đầu, động kinh, đột quỵ.",
        ProfessionalId = 7
    },
    new PrivateService
    {
        Id = 24,
        Name = "Điện não đồ",
        Price = 650000,
        Description = "Đo điện não đồ để đánh giá hoạt động điện của não và phát hiện bất thường.",
        ProfessionalId = 7
    },

    // Chuyên khoa Phục hồi chức năng (Rehabilitation) - Professional 9
    new PrivateService
    {
        Id = 25,
        Name = "Đánh giá và lập kế hoạch phục hồi chức năng",
        Price = 500000,
        Description = "Đánh giá tình trạng bệnh nhân và lập kế hoạch phục hồi chức năng cá nhân hóa.",
        ProfessionalId = 9
    },
    new PrivateService
    {
        Id = 26,
        Name = "Vật lý trị liệu cột sống",
        Price = 400000,
        Description = "Vật lý trị liệu chuyên sâu cho các bệnh lý về cột sống như thoát vị đĩa đệm, đau thắt lưng.",
        ProfessionalId = 9
    },

    // Chuyên khoa Nội tiết (Endocrinology) - Professional 10
    new PrivateService
    {
        Id = 27,
        Name = "Khám và điều trị bệnh tiểu đường",
        Price = 600000,
        Description = "Khám, tư vấn và quản lý bệnh tiểu đường, bao gồm kế hoạch điều trị và chế độ dinh dưỡng.",
        ProfessionalId = 10
    },
    new PrivateService
    {
        Id = 28,
        Name = "Khám và điều trị rối loạn tuyến giáp",
        Price = 650000,
        Description = "Khám, chẩn đoán và điều trị các bệnh lý của tuyến giáp như cường giáp, suy giáp.",
        ProfessionalId = 10
    },

    // Chuyên khoa Hô hấp (Respiratory) - Professional 11
    new PrivateService
    {
        Id = 29,
        Name = "Khám và điều trị bệnh hô hấp",
        Price = 550000,
        Description = "Khám, chẩn đoán và điều trị các bệnh lý hô hấp như viêm phổi, hen suyễn, COPD.",
        ProfessionalId = 11
    },
    new PrivateService
    {
        Id = 30,
        Name = "Đo chức năng hô hấp",
        Price = 450000,
        Description = "Đo và đánh giá chức năng hô hấp để chẩn đoán các bệnh lý phổi.",
        ProfessionalId = 11
    },

    // Chuyên khoa Ung bướu (Oncology) - Professional 12
    new PrivateService
    {
        Id = 31,
        Name = "Tư vấn và đánh giá nguy cơ ung thư",
        Price = 1000000,
        Description = "Đánh giá các yếu tố nguy cơ và tư vấn phòng ngừa ung thư cá nhân hóa.",
        ProfessionalId = 12
    },
    new PrivateService
    {
        Id = 32,
        Name = "Khám và theo dõi sau điều trị ung thư",
        Price = 1200000,
        Description = "Khám định kỳ và theo dõi cho bệnh nhân sau điều trị ung thư.",
        ProfessionalId = 12
    },

    // Chuyên khoa Nhi (Pediatrics) - Professional 13
    new PrivateService
    {
        Id = 33,
        Name = "Khám sức khỏe tổng quát cho trẻ",
        Price = 500000,
        Description = "Khám sức khỏe tổng quát định kỳ cho trẻ em, theo dõi sự phát triển và tầm soát bệnh lý.",
        ProfessionalId = 13
    },
    new PrivateService
    {
        Id = 34,
        Name = "Tư vấn dinh dưỡng và tiêm chủng cho trẻ",
        Price = 400000,
        Description = "Tư vấn chế độ dinh dưỡng và lịch tiêm chủng phù hợp theo từng độ tuổi của trẻ.",
        ProfessionalId = 13
    },

    // Chuyên khoa Da liễu (Dermatology) - Professional 15
    new PrivateService
    {
        Id = 35,
        Name = "Khám và điều trị bệnh da liễu",
        Price = 550000,
        Description = "Khám, chẩn đoán và điều trị các bệnh lý về da như mụn trứng cá, viêm da, chàm.",
        ProfessionalId = 15
    },
    new PrivateService
    {
        Id = 36,
        Name = "Điều trị mụn và sẹo",
        Price = 700000,
        Description = "Điều trị chuyên sâu mụn trứng cá và cải thiện tình trạng sẹo sau mụn.",
        ProfessionalId = 15
    },

    // Chuyên khoa Tai Mũi Họng (ENT) - Professional 18
    new PrivateService
    {
        Id = 37,
        Name = "Khám tai mũi họng tổng quát",
        Price = 500000,
        Description = "Khám tổng quát tai, mũi, họng và chẩn đoán các bệnh lý liên quan.",
        ProfessionalId = 18
    },
    new PrivateService
    {
        Id = 38,
        Name = "Nội soi tai mũi họng",
        Price = 600000,
        Description = "Nội soi để chẩn đoán chính xác các bệnh lý tai, mũi, họng khó phát hiện bằng khám thường.",
        ProfessionalId = 18
    },

    // Chuyên khoa Mắt (Ophthalmology) - Professional 19
    new PrivateService
    {
        Id = 39,
        Name = "Khám mắt tổng quát",
        Price = 450000,
        Description = "Khám tổng quát mắt, kiểm tra thị lực và chẩn đoán các bệnh lý về mắt.",
        ProfessionalId = 19
    },
    new PrivateService
    {
        Id = 40,
        Name = "Đo khúc xạ và kê đơn kính",
        Price = 350000,
        Description = "Đo khúc xạ chính xác và kê đơn kính phù hợp cho các trường hợp cận, viễn, loạn thị.",
        ProfessionalId = 19
    },

    // Chuyên khoa Nội (General Practice) - Professional 20
    new PrivateService
    {
        Id = 41,
        Name = "Khám sức khỏe tổng quát",
        Price = 400000,
        Description = "Khám sức khỏe tổng quát và tầm soát các bệnh lý thường gặp.",
        ProfessionalId = 20
    },
    new PrivateService
    {
        Id = 42,
        Name = "Tư vấn phòng ngừa bệnh tật",
        Price = 300000,
        Description = "Tư vấn các biện pháp phòng ngừa bệnh tật và duy trì lối sống lành mạnh.",
        ProfessionalId = 20
    }
        
             );
        }
    }
}
