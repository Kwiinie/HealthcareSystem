using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasOne(a => a.CreatedBy)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Category)
                .WithMany(c => c.Articles) 
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.ArticleImages)
                .WithOne() 
                .HasForeignKey(ai => ai.ArticleId)  
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasData(
                 new Article
                 {
                     Id = 1,
                     CategoryId = 1,
                     Title = "5 Lợi Ích Của Việc Kiểm Tra Sức Khỏe Định Kỳ",
                     CreatedAt = new DateTime(2025, 1, 1),
                     CreatedById = 1,
                     Content = "<p>Kiểm tra sức khỏe định kỳ là một trong những phương pháp quan trọng giúp phát hiện sớm các vấn đề sức khỏe tiềm ẩn. Việc kiểm tra thường xuyên không chỉ giúp bạn hiểu rõ hơn về tình trạng sức khỏe của mình mà còn giúp bác sĩ đưa ra các biện pháp điều trị kịp thời nếu phát hiện ra vấn đề.</p>" +
                      "<p><strong>Các lợi ích chính của việc kiểm tra sức khỏe định kỳ bao gồm:</strong></p>" +
                      "<ol>" +
                      "<li><strong>Phát hiện sớm các bệnh lý tiềm ẩn</strong>: Việc kiểm tra sức khỏe giúp phát hiện sớm các bệnh lý như tiểu đường, huyết áp cao, hay các vấn đề tim mạch mà bạn có thể không nhận ra. Việc phát hiện sớm giúp điều trị hiệu quả hơn, giảm thiểu các biến chứng nghiêm trọng về sau.</li>" +
                      "<li><strong>Tiết kiệm chi phí điều trị</strong>: Việc phát hiện bệnh sớm sẽ giúp bạn tiết kiệm chi phí điều trị, bởi vì bệnh sẽ dễ dàng được điều trị hơn khi phát hiện ở giai đoạn đầu. Điều này không chỉ giúp giảm chi phí cá nhân mà còn giúp hệ thống y tế giảm gánh nặng.</li>" +
                      "<li><strong>Tăng tuổi thọ</strong>: Các kiểm tra định kỳ giúp phát hiện sớm các yếu tố nguy cơ sức khỏe và điều chỉnh kịp thời, từ đó tăng khả năng sống lâu. Ví dụ, việc kiểm soát mức huyết áp hoặc cholesterol có thể giúp giảm nguy cơ đột quỵ và các bệnh tim mạch.</li>" +
                      "<li><strong>Cải thiện chất lượng cuộc sống</strong>: Việc kiểm tra sức khỏe sẽ giúp bạn có lối sống lành mạnh hơn, với chế độ ăn uống và tập thể dục phù hợp. Điều này sẽ giúp bạn có nhiều năng lượng hơn và cảm thấy tự tin vào sức khỏe của mình.</li>" +
                      "<li><strong>Giảm căng thẳng và lo âu</strong>: Khi bạn biết rằng sức khỏe của mình ổn định, bạn sẽ cảm thấy an tâm và ít lo lắng hơn. Việc biết rằng bạn không mắc bệnh gì nghiêm trọng giúp bạn giảm bớt lo âu, từ đó cải thiện tâm trạng và chất lượng cuộc sống.</li>" +
                      "</ol>",
                     ImgUrl = "/images/articles/health-checkup.jpg"
                 },
                 new Article
                 {
                     Id = 2,
                     CategoryId = 1,
                     Title = "Tầm Quan Trọng Của Việc Tiêm Chủng Định Kỳ",
                     CreatedAt = new DateTime(2025, 2, 1),
                     CreatedById = 1,
                     Content = "<p>Tiêm chủng là một trong những phương pháp phòng ngừa bệnh hiệu quả nhất mà chúng ta có. Việc tiêm chủng định kỳ giúp cơ thể tạo ra miễn dịch đối với các bệnh truyền nhiễm, bảo vệ không chỉ cho bản thân mà còn cho cộng đồng. Dưới đây là những lý do tại sao tiêm chủng là quan trọng và cần thiết:</p>" +
                      "<p><strong>1. Bảo vệ khỏi bệnh truyền nhiễm</strong>: Tiêm chủng giúp cơ thể chống lại các bệnh như sởi, thủy đậu, viêm gan B, bạch hầu và nhiều bệnh khác. Bằng cách tiêm phòng, bạn giảm nguy cơ mắc phải các bệnh này, giúp bạn bảo vệ sức khỏe của mình và những người xung quanh. Các bệnh truyền nhiễm có thể gây hậu quả nghiêm trọng, thậm chí là tử vong, nhưng có thể phòng ngừa dễ dàng nhờ tiêm chủng.</p>" +
                      "<p><strong>2. Bảo vệ cộng đồng</strong>: Tiêm chủng không chỉ giúp bảo vệ bản thân mà còn giúp bảo vệ những người xung quanh, đặc biệt là những người không thể tiêm chủng như trẻ em, phụ nữ mang thai hoặc người có hệ miễn dịch yếu. Khi càng nhiều người trong cộng đồng tiêm phòng, khả năng lây lan của bệnh sẽ giảm thiểu, từ đó bảo vệ cả cộng đồng khỏi sự bùng phát của các dịch bệnh.</p>" +
                      "<p><strong>3. Ngăn ngừa dịch bệnh</strong>: Khi đủ nhiều người trong cộng đồng được tiêm phòng, các dịch bệnh sẽ không có cơ hội bùng phát, giúp bảo vệ cả cộng đồng khỏi những đợt dịch nguy hiểm. Điều này đã được chứng minh qua nhiều quốc gia trên thế giới khi tiêm chủng giúp ngăn ngừa sự bùng phát của các bệnh như sởi, bại liệt, và cúm.</p>" +
                      "<p><strong>4. Giảm chi phí chăm sóc sức khỏe</strong>: Khi tiêm chủng, bạn giảm nguy cơ mắc bệnh, từ đó giảm chi phí điều trị và chăm sóc y tế lâu dài. Các bệnh do không tiêm phòng có thể tốn kém hơn rất nhiều trong việc điều trị và chăm sóc sau này.</p>" +
                      "<p><strong>5. Đảm bảo an toàn cho trẻ em</strong>: Việc tiêm chủng cho trẻ em giúp bảo vệ các em khỏi những bệnh tật nguy hiểm và giảm tỷ lệ tử vong do bệnh truyền nhiễm. Trẻ em có hệ miễn dịch yếu, nên việc tiêm chủng là biện pháp cần thiết để bảo vệ các em khỏi các mối đe dọa bệnh tật.</p>",
                     ImgUrl = "/images/articles/vaccination.jpg"
                 },
                 new Article
                 {
                     Id = 3,
                     CategoryId = 2,
                     Title = "Chế Độ Ăn Uống Cân Bằng Cho Sức Khỏe",
                     CreatedAt = new DateTime(2025, 3, 1),
                     CreatedById = 1,
                     Content = "<p>Một chế độ ăn uống cân bằng là nền tảng quan trọng để duy trì sức khỏe. Để có một chế độ ăn uống lành mạnh, bạn cần đảm bảo rằng cơ thể nhận được đủ các nhóm chất dinh dưỡng cần thiết. Sau đây là một số lời khuyên để duy trì chế độ ăn uống cân bằng và hợp lý:</p>" +
                      "<p><strong>1. Ăn đủ 5 nhóm thực phẩm</strong>: Đảm bảo rằng mỗi bữa ăn của bạn bao gồm đủ các nhóm thực phẩm như tinh bột (gạo, khoai), protein (thịt, cá, đậu), chất béo lành mạnh (dầu olive, bơ), vitamin và khoáng chất (rau xanh, trái cây), và chất xơ. Việc kết hợp đa dạng các thực phẩm sẽ cung cấp đầy đủ dinh dưỡng cho cơ thể.</p>" +
                      "<p><strong>2. Hạn chế thức ăn chế biến sẵn</strong>: Thực phẩm chế biến sẵn chứa nhiều chất bảo quản, đường và muối, có thể gây hại cho sức khỏe nếu tiêu thụ quá nhiều. Hãy tránh thức ăn nhanh, thực phẩm chiên rán, và thay vào đó là các món ăn tươi sống, chế biến tại nhà.</p>" +
                      "<p><strong>3. Uống đủ nước</strong>: Cung cấp đủ nước cho cơ thể là một yếu tố quan trọng trong chế độ ăn uống. Nước giúp cơ thể hấp thu chất dinh dưỡng, giải độc và duy trì nhiệt độ cơ thể ổn định. Bạn nên uống ít nhất 8 cốc nước mỗi ngày và uống thêm nếu bạn tham gia các hoạt động thể thao.</p>" +
                      "<p><strong>4. Ăn nhiều rau củ quả</strong>: Rau củ quả chứa nhiều vitamin, khoáng chất và chất xơ, giúp hỗ trợ hệ tiêu hóa, tăng cường hệ miễn dịch và giúp da khỏe mạnh. Hãy cố gắng ăn ít nhất 5 khẩu phần rau quả mỗi ngày để cung cấp các dưỡng chất thiết yếu cho cơ thể.</p>" +
                      "<p><strong>5. Kiểm soát lượng đường và muối</strong>: Việc giảm lượng đường và muối trong chế độ ăn uống có thể giúp ngăn ngừa các bệnh lý như tiểu đường, huyết áp cao và bệnh tim. Bạn nên hạn chế các thực phẩm ngọt và thức uống có gas, thay vào đó là ăn trái cây tươi và sử dụng gia vị tự nhiên.</p>",
                     ImgUrl = "/images/articles/balanced-diet.jpg"
                 },
                new Article
                {
                    Id = 4,
                    CategoryId = 2,
                    Title = "Thực Phẩm Giúp Cải Thiện Hệ Tiêu Hóa",
                    CreatedAt = new DateTime(2025, 3, 10),
                    CreatedById = 1,
                    Content = "<p>Hệ tiêu hóa đóng một vai trò quan trọng trong việc duy trì sức khỏe. Khi hệ tiêu hóa hoạt động tốt, cơ thể sẽ hấp thụ dinh dưỡng hiệu quả, giảm nguy cơ mắc các bệnh lý và cải thiện chất lượng cuộc sống. Dưới đây là một số thực phẩm giúp cải thiện hệ tiêu hóa:</p>" +
                      "<p><strong>1. Sữa chua</strong>: Sữa chua chứa các vi khuẩn có lợi giúp duy trì sự cân bằng của hệ vi sinh đường ruột, từ đó giúp hệ tiêu hóa hoạt động hiệu quả hơn. Các lợi khuẩn này giúp cải thiện sự hấp thu chất dinh dưỡng và tăng cường hệ miễn dịch.</p>" +
                      "<p><strong>2. Chuối</strong>: Chuối là một nguồn cung cấp chất xơ tuyệt vời, giúp cải thiện nhu động ruột và ngăn ngừa táo bón. Chuối cũng có thể làm dịu dạ dày và giúp giảm cảm giác đầy bụng.</p>" +
                      "<p><strong>3. Rau xanh</strong>: Các loại rau như rau cải, rau bina và bông cải xanh chứa nhiều chất xơ và vitamin, giúp tăng cường chức năng tiêu hóa và làm sạch đường ruột. Rau xanh giúp cải thiện nhu động ruột và giảm nguy cơ mắc bệnh về đường tiêu hóa.</p>" +
                      "<p><strong>4. Hạt chia</strong>: Hạt chia giàu chất xơ, giúp cải thiện nhu động ruột và giảm táo bón. Ngoài ra, hạt chia còn cung cấp các axit béo omega-3 có lợi cho sức khỏe.</p>" +
                      "<p><strong>5. Gừng</strong>: Gừng có tính kháng viêm và có thể giúp làm dịu dạ dày, hỗ trợ tiêu hóa và giảm đầy bụng. Uống trà gừng hoặc thêm gừng tươi vào các món ăn có thể giúp cải thiện hệ tiêu hóa.</p>",
                    ImgUrl = "/images/articles/digestive-health.jpg"
                },
                new Article
                {
                    Id = 5,
                    CategoryId = 1,
                    Title = "Cách Phòng Ngừa Bệnh Tim Mạch",
                    CreatedAt = new DateTime(2025, 3, 15),
                    CreatedById = 1,
                    Content = "<p>Bệnh tim mạch là một trong những nguyên nhân hàng đầu gây tử vong trên toàn cầu. Tuy nhiên, bệnh tim mạch có thể được phòng ngừa thông qua các biện pháp thay đổi lối sống lành mạnh. Dưới đây là những phương pháp phòng ngừa hiệu quả bệnh tim mạch:</p>" +
                  "<p><strong>1. Duy trì một chế độ ăn uống lành mạnh</strong>: Chế độ ăn uống giàu trái cây, rau củ, ngũ cốc nguyên hạt, và giảm thiểu các thực phẩm giàu chất béo bão hòa và cholesterol sẽ giúp bảo vệ tim mạch. Hãy bổ sung các thực phẩm giàu omega-3 như cá hồi và các loại hạt giúp làm giảm nguy cơ bệnh tim.</p>" +
                  "<p><strong>2. Tập thể dục thường xuyên</strong>: Các nghiên cứu đã chứng minh rằng việc tập thể dục thường xuyên, ít nhất 30 phút mỗi ngày, giúp cải thiện sức khỏe tim mạch. Việc này giúp tăng cường lưu thông máu, kiểm soát huyết áp và cholesterol.</p>" +
                  "<p><strong>3. Kiểm soát cân nặng</strong>: Thừa cân làm tăng nguy cơ mắc bệnh tim mạch. Việc duy trì cân nặng hợp lý thông qua chế độ ăn uống và luyện tập sẽ giảm thiểu gánh nặng cho tim, giúp tim hoạt động hiệu quả hơn.</p>" +
                  "<p><strong>4. Hạn chế căng thẳng</strong>: Căng thẳng kéo dài có thể làm tăng huyết áp và làm tổn thương mạch máu. Hãy áp dụng các phương pháp giảm stress như thiền, yoga, hoặc đi bộ để giảm mức độ căng thẳng trong cuộc sống hàng ngày.</p>" +
                  "<p><strong>5. Kiểm tra sức khỏe định kỳ</strong>: Việc kiểm tra sức khỏe định kỳ, bao gồm kiểm tra huyết áp và mức cholesterol, sẽ giúp bạn phát hiện sớm các yếu tố nguy cơ và có biện pháp can thiệp kịp thời để bảo vệ sức khỏe tim mạch.</p>",
                    ImgUrl = "/images/articles/heart-health.jpg"
                },
                new Article
                {
                    Id = 6,
                    CategoryId = 3,
                    Title = "Những Dấu Hiệu Cảnh Báo Ung Thư Phổi",
                    CreatedAt = new DateTime(2025, 2, 15),
                    CreatedById = 1,
                    Content = "<p>Ung thư phổi là một trong những loại ung thư nguy hiểm và có tỷ lệ tử vong cao. Việc phát hiện bệnh sớm sẽ giúp điều trị hiệu quả và cải thiện cơ hội sống sót. Dưới đây là một số dấu hiệu cảnh báo ung thư phổi mà bạn không nên bỏ qua:</p>" +
                  "<p><strong>1. Ho kéo dài</strong>: Ho kéo dài, đặc biệt là ho có đờm hoặc ho ra máu, có thể là dấu hiệu của ung thư phổi. Nếu bạn có ho liên tục trong nhiều tuần, hãy đi kiểm tra để xác định nguyên nhân.</p>" +
                  "<p><strong>2. Khó thở</strong>: Khó thở hoặc cảm giác hụt hơi khi làm những việc bình thường có thể là triệu chứng của bệnh ung thư phổi. Sự tắc nghẽn trong phổi do khối u có thể làm giảm khả năng hô hấp của bạn.</p>" +
                  "<p><strong>3. Đau ngực</strong>: Đau hoặc cảm giác tức ngực, đặc biệt là khi ho hoặc thở sâu, có thể là dấu hiệu của ung thư phổi. Cơn đau có thể lan ra vai hoặc lưng, đặc biệt khi khối u chèn ép lên các cơ quan lân cận.</p>" +
                  "<p><strong>4. Giảm cân không rõ lý do</strong>: Giảm cân đột ngột mà không thay đổi chế độ ăn uống hoặc lối sống có thể là một dấu hiệu của ung thư phổi. Đây là triệu chứng chung của nhiều loại ung thư, trong đó có ung thư phổi.</p>" +
                  "<p><strong>5. Mệt mỏi kéo dài</strong>: Cảm giác mệt mỏi và yếu ớt kéo dài có thể là dấu hiệu của ung thư phổi. Khi các tế bào ung thư phát triển, cơ thể sẽ trở nên mệt mỏi hơn, và năng lượng của bạn sẽ giảm sút.</p>" +
                  "<p>Việc kiểm tra y tế kịp thời sẽ giúp phát hiện ung thư phổi ở giai đoạn sớm, từ đó tăng cơ hội điều trị và cải thiện khả năng sống sót của bệnh nhân.</p>",
                    ImgUrl = "/images/articles/lung-cancer.jpg"
                }
            ); ;
        }
    }
}
