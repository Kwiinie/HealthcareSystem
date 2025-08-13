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
    public class PublicServiceConfiguration : IEntityTypeConfiguration<PublicService>
    {
        public void Configure(EntityTypeBuilder<PublicService> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.Facility)
                .WithMany(f => f.PublicServices) 
                .HasForeignKey(ps => ps.FacilityId) 
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
               new PublicService
               {
                   Id = 1,
                   Name = "Khám nội khoa tổng quát",
                   Price = 350000,
                   Description = "Khám và tư vấn các bệnh lý nội khoa tổng quát, bao gồm đánh giá sức khỏe toàn diện.",
                   FacilityId = 1 // Bệnh viện Bạch Mai
               },
    new PublicService
    {
        Id = 2,
        Name = "Khám và điều trị bệnh tiêu hóa",
        Price = 450000,
        Description = "Khám, chẩn đoán và điều trị các bệnh lý về đường tiêu hóa như viêm dạ dày, trào ngược dạ dày.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 3,
        Name = "Khám nội khoa tổng quát",
        Price = 400000,
        Description = "Khám và tư vấn các bệnh lý nội khoa, bao gồm đánh giá chức năng các cơ quan nội tạng.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },
    new PublicService
    {
        Id = 4,
        Name = "Khám bệnh tiểu đường",
        Price = 500000,
        Description = "Khám, chẩn đoán và tư vấn điều trị cho bệnh nhân tiểu đường, bao gồm kiểm tra đường huyết.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },

    // Surgery (Khoa Ngoại) Services
    new PublicService
    {
        Id = 5,
        Name = "Khám ngoại khoa tổng quát",
        Price = 400000,
        Description = "Khám và tư vấn các vấn đề ngoại khoa, bao gồm các bệnh lý cần can thiệp phẫu thuật.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 6,
        Name = "Phẫu thuật cắt ruột thừa",
        Price = 5000000,
        Description = "Phẫu thuật cắt ruột thừa viêm bằng phương pháp nội soi hoặc mổ mở.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },

    // Obstetrics (Khoa Sản) Services
    new PublicService
    {
        Id = 7,
        Name = "Khám thai định kỳ",
        Price = 400000,
        Description = "Khám thai định kỳ, theo dõi sự phát triển của thai nhi và sức khỏe của mẹ.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 8,
        Name = "Sinh thường",
        Price = 6000000,
        Description = "Dịch vụ sinh thường trọn gói, bao gồm theo dõi chuyển dạ và chăm sóc sau sinh.",
        FacilityId = 3 // Bệnh viện Đa khoa Quốc tế Vinmec Times City
    },
    new PublicService
    {
        Id = 9,
        Name = "Sinh mổ theo yêu cầu",
        Price = 25000000,
        Description = "Dịch vụ sinh mổ trọn gói theo yêu cầu, bao gồm phẫu thuật, chăm sóc sau sinh và hỗ trợ nuôi con bằng sữa mẹ.",
        FacilityId = 17 // Bệnh viện Đa khoa Phương Châu
    },

    // Pediatrics (Khoa Nhi) Services
    new PublicService
    {
        Id = 10,
        Name = "Khám nhi tổng quát",
        Price = 350000,
        Description = "Khám sức khỏe tổng quát cho trẻ em, đánh giá sự phát triển và tầm soát bệnh lý.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 11,
        Name = "Tiêm chủng cho trẻ",
        Price = 1500000,
        Description = "Gói tiêm chủng cơ bản cho trẻ theo lịch của Bộ Y tế, bao gồm vắc xin và theo dõi sau tiêm.",
        FacilityId = 4 // Bệnh viện Trung ương Huế
    },

    // Laboratory (Khoa Xét nghiệm) Services
    new PublicService
    {
        Id = 12,
        Name = "Xét nghiệm máu cơ bản",
        Price = 200000,
        Description = "Xét nghiệm máu cơ bản bao gồm công thức máu, đường huyết và chức năng gan thận.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 13,
        Name = "Xét nghiệm vi sinh",
        Price = 350000,
        Description = "Xét nghiệm vi sinh để phát hiện các tác nhân gây bệnh như vi khuẩn, virus.",
        FacilityId = 5 // Bệnh viện Đa khoa Trung ương Cần Thơ
    },

    // Diagnostic Imaging (Khoa Chẩn đoán hình ảnh) Services
    new PublicService
    {
        Id = 14,
        Name = "Chụp X-quang",
        Price = 180000,
        Description = "Chụp X-quang các bộ phận cơ thể để chẩn đoán bệnh lý xương khớp và phổi.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 15,
        Name = "Chụp cộng hưởng từ (MRI)",
        Price = 2500000,
        Description = "Chụp cộng hưởng từ để chẩn đoán chi tiết các bệnh lý về não, cột sống và các cơ quan nội tạng.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },
    new PublicService
    {
        Id = 16,
        Name = "Chụp cắt lớp vi tính (CT)",
        Price = 1500000,
        Description = "Chụp cắt lớp vi tính để chẩn đoán chi tiết các bệnh lý khác nhau trong cơ thể.",
        FacilityId = 6 // Bệnh viện K Tân Triều
    },

    // Dental (Khoa Răng Hàm Mặt) Services
    new PublicService
    {
        Id = 17,
        Name = "Khám răng tổng quát",
        Price = 150000,
        Description = "Khám tổng quát răng miệng, tư vấn chăm sóc và phòng ngừa bệnh lý răng miệng.",
        FacilityId = 3 // Bệnh viện Đa khoa Quốc tế Vinmec Times City
    },
    new PublicService
    {
        Id = 18,
        Name = "Cạo vôi răng",
        Price = 350000,
        Description = "Cạo vôi răng để loại bỏ cao răng và mảng bám, giúp răng chắc khỏe và ngăn ngừa bệnh nướu.",
        FacilityId = 20 // Bệnh viện Quốc tế City
    },

    // Ophthalmology (Khoa Mắt) Services
    new PublicService
    {
        Id = 19,
        Name = "Khám mắt tổng quát",
        Price = 200000,
        Description = "Khám tổng quát mắt, kiểm tra thị lực và chẩn đoán các bệnh lý về mắt.",
        FacilityId = 7 // Bệnh viện Mắt Trung ương
    },
    new PublicService
    {
        Id = 20,
        Name = "Phẫu thuật Lasik",
        Price = 25000000,
        Description = "Phẫu thuật Lasik điều trị cận thị, viễn thị và loạn thị bằng công nghệ laser hiện đại.",
        FacilityId = 7 // Bệnh viện Mắt Trung ương
    },

    // ENT (Khoa Tai Mũi Họng) Services
    new PublicService
    {
        Id = 21,
        Name = "Khám tai mũi họng",
        Price = 250000,
        Description = "Khám tổng quát tai, mũi, họng và chẩn đoán các bệnh lý liên quan.",
        FacilityId = 13 // Bệnh viện Đa khoa tỉnh Hải Dương
    },
    new PublicService
    {
        Id = 22,
        Name = "Nội soi tai mũi họng",
        Price = 450000,
        Description = "Nội soi tai mũi họng để chẩn đoán chính xác các bệnh lý tai, mũi, họng.",
        FacilityId = 13 // Bệnh viện Đa khoa tỉnh Hải Dương
    },

    // Dermatology (Khoa Da Liễu) Services
    new PublicService
    {
        Id = 23,
        Name = "Khám da liễu",
        Price = 300000,
        Description = "Khám và điều trị các bệnh lý về da như mụn trứng cá, viêm da, chàm.",
        FacilityId = 3 // Bệnh viện Đa khoa Quốc tế Vinmec Times City
    },
    new PublicService
    {
        Id = 24,
        Name = "Điều trị mụn chuyên sâu",
        Price = 800000,
        Description = "Điều trị chuyên sâu mụn trứng cá và cải thiện tình trạng sẹo sau mụn.",
        FacilityId = 15 // Bệnh viện Đa khoa Quốc tế Nha Trang
    },

    // Emergency (Khoa Cấp cứu) Services
    new PublicService
    {
        Id = 25,
        Name = "Cấp cứu chấn thương",
        Price = 500000,
        Description = "Dịch vụ cấp cứu cho các trường hợp chấn thương, tai nạn.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 26,
        Name = "Cấp cứu nội khoa",
        Price = 450000,
        Description = "Dịch vụ cấp cứu cho các trường hợp đột quỵ, nhồi máu cơ tim và các cấp cứu nội khoa khác.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },

    // Intensive Care (Khoa Hồi sức tích cực) Services
    new PublicService
    {
        Id = 27,
        Name = "Hồi sức tích cực",
        Price = 3500000,
        Description = "Chăm sóc đặc biệt cho bệnh nhân trong tình trạng nguy kịch (tính theo ngày).",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },

    // Psychology (Khoa Tâm lý) Services
    new PublicService
    {
        Id = 28,
        Name = "Tư vấn tâm lý",
        Price = 400000,
        Description = "Tư vấn và hỗ trợ tâm lý cho người gặp vấn đề về stress, lo âu, trầm cảm.",
        FacilityId = 14 // Bệnh viện Đa khoa tỉnh Quảng Nam
    },

    // Rehabilitation (Khoa Phục hồi chức năng) Services
    new PublicService
    {
        Id = 29,
        Name = "Vật lý trị liệu",
        Price = 250000,
        Description = "Vật lý trị liệu cho các bệnh nhân đau cột sống, chấn thương và sau phẫu thuật.",
        FacilityId = 5 // Bệnh viện Đa khoa Trung ương Cần Thơ
    },

    // Urology (Khoa Tiết niệu) Services
    new PublicService
    {
        Id = 30,
        Name = "Khám tiết niệu",
        Price = 350000,
        Description = "Khám và điều trị các bệnh lý về hệ tiết niệu như sỏi thận, viêm đường tiết niệu.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },

    // Cardiology (Khoa Tim mạch) Services
    new PublicService
    {
        Id = 31,
        Name = "Khám tim mạch",
        Price = 400000,
        Description = "Khám và tư vấn các bệnh lý tim mạch như tăng huyết áp, rối loạn nhịp tim.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 32,
        Name = "Siêu âm tim",
        Price = 500000,
        Description = "Siêu âm tim để đánh giá cấu trúc và chức năng của tim.",
        FacilityId = 20 // Bệnh viện Quốc tế City
    },

    // Respiratory (Khoa Hô hấp) Services
    new PublicService
    {
        Id = 33,
        Name = "Khám hô hấp",
        Price = 350000,
        Description = "Khám và điều trị các bệnh lý hô hấp như viêm phổi, hen suyễn, COPD.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },
    new PublicService
    {
        Id = 34,
        Name = "Đo chức năng hô hấp",
        Price = 300000,
        Description = "Đo và đánh giá chức năng hô hấp để chẩn đoán các bệnh lý phổi.",
        FacilityId = 18 // Bệnh viện Đa khoa tỉnh Lâm Đồng
    },

    // Endocrinology (Khoa Nội tiết) Services
    new PublicService
    {
        Id = 35,
        Name = "Khám nội tiết",
        Price = 400000,
        Description = "Khám và điều trị các bệnh lý nội tiết như tiểu đường, rối loạn tuyến giáp.",
        FacilityId = 2 // Bệnh viện Chợ Rẫy
    },

    // Oncology (Khoa Ung bướu) Services
    new PublicService
    {
        Id = 36,
        Name = "Khám ung bướu",
        Price = 500000,
        Description = "Khám, chẩn đoán và tư vấn điều trị các bệnh lý ung thư.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },
    new PublicService
    {
        Id = 37,
        Name = "Xạ trị ung thư",
        Price = 12000000,
        Description = "Điều trị ung thư bằng phương pháp xạ trị với máy móc hiện đại (tính cho một liệu trình).",
        FacilityId = 6 // Bệnh viện K Tân Triều
    },

    // Nutrition (Khoa Dinh dưỡng) Services
    new PublicService
    {
        Id = 38,
        Name = "Tư vấn dinh dưỡng",
        Price = 300000,
        Description = "Tư vấn chế độ dinh dưỡng cá nhân hóa cho các bệnh lý khác nhau.",
        FacilityId = 1 // Bệnh viện Bạch Mai
    },

    // Health Checkup Packages
    new PublicService
    {
        Id = 39,
        Name = "Gói khám sức khỏe tổng quát cơ bản",
        Price = 1500000,
        Description = "Gói khám sức khỏe tổng quát cơ bản bao gồm khám nội khoa, xét nghiệm máu, X-quang ngực.",
        FacilityId = 3 // Bệnh viện Đa khoa Quốc tế Vinmec Times City
    },
    new PublicService
    {
        Id = 40,
        Name = "Gói khám sức khỏe toàn diện",
        Price = 5000000,
        Description = "Gói khám sức khỏe toàn diện bao gồm khám chuyên khoa, xét nghiệm, siêu âm và chẩn đoán hình ảnh.",
        FacilityId = 8 // Bệnh viện FV
    },

    // Specialized services for provincial hospitals
    new PublicService
    {
        Id = 41,
        Name = "Khám và điều trị y học cổ truyền",
        Price = 200000,
        Description = "Khám và điều trị bằng các phương pháp y học cổ truyền như châm cứu, bấm huyệt.",
        FacilityId = 12 // Bệnh viện Đa khoa tỉnh Lào Cai
    },
    new PublicService
    {
        Id = 42,
        Name = "Khám sức khỏe đi xuất khẩu lao động",
        Price = 1800000,
        Description = "Khám sức khỏe toàn diện theo yêu cầu cho người đi xuất khẩu lao động.",
        FacilityId = 16 // Bệnh viện Đa khoa tỉnh Đồng Tháp
    },

    // Basic services for medical centers
    new PublicService
    {
        Id = 43,
        Name = "Khám và điều trị bệnh thông thường",
        Price = 120000,
        Description = "Khám và điều trị các bệnh thông thường như cảm cúm, viêm họng, tiêu chảy.",
        FacilityId = 10 // Trung tâm Y tế huyện Bình Chánh
    },
    new PublicService
    {
        Id = 44,
        Name = "Tiêm vắc xin theo yêu cầu",
        Price = 500000,
        Description = "Tiêm các loại vắc xin theo yêu cầu (giá chưa bao gồm vắc xin).",
        FacilityId = 11 // Trung tâm Y tế quận Nam Từ Liêm
    },

    // Rural health center services
    new PublicService
    {
        Id = 45,
        Name = "Khám sức khỏe cơ bản",
        Price = 80000,
        Description = "Khám sức khỏe cơ bản và tư vấn phòng bệnh cho người dân địa phương.",
        FacilityId = 19 // Trung tâm Y tế huyện Mèo Vạc
    }
           );
        }
    }
}
