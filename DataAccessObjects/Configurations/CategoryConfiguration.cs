using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Articles)
                   .WithOne(a => a.Category)
                   .HasForeignKey(a => a.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
                            new Category
                            {
                                Id = 1,
                                Name = "Y tế",
                                Description = "Các bài viết về các chủ đề y tế, chăm sóc sức khỏe, và bệnh lý."
                            },
                            new Category
                            {
                                Id = 2,
                                Name = "Dinh dưỡng",
                                Description = "Các bài viết về dinh dưỡng, chế độ ăn uống và cách duy trì sức khỏe qua thực phẩm."
                            },
                            new Category
                            {
                                Id = 3,
                                Name = "Bệnh lý",
                                Description = "Các bài viết về các bệnh lý phổ biến, nguyên nhân và cách phòng ngừa."
                            },
                            new Category
                            {
                                Id = 4,
                                Name = "Chăm sóc sức khỏe",
                                Description = "Các bài viết về cách chăm sóc sức khỏe bản thân và gia đình."
                            },
                            new Category
                            {
                                Id = 5,
                                Name = "Phẫu thuật",
                                Description = "Các bài viết về các loại phẫu thuật, quy trình và phục hồi sau phẫu thuật."
                            },
                            new Category
                            {
                                Id = 6,
                                Name = "Sức khỏe tâm lý",
                                Description = "Các bài viết về sức khỏe tinh thần, tâm lý học và cách duy trì tinh thần khỏe mạnh."
                            }
            );
        }
    }
}
