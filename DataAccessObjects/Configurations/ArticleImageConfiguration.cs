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
    public class ArticleImageConfiguration : IEntityTypeConfiguration<ArticleImage>
    {
        public void Configure(EntityTypeBuilder<ArticleImage> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(ai => ai.Article)
           .WithMany(a => a.ArticleImages)
           .HasForeignKey(ai => ai.ArticleId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new ArticleImage
                {
                    Id = 1,
                    ArticleId = 1,
                    ImgUrl = "/images/articles/health-checkup-1.jpg"
                },
                new ArticleImage
                {
                    Id = 2,
                    ArticleId = 1,
                    ImgUrl = "/images/articles/health-checkup-2.jpg"
                },

                // Images for Article 2 - Vaccination
                new ArticleImage
                {
                    Id = 3,
                    ArticleId = 2,
                    ImgUrl = "/images/articles/vaccination-1.jpg"
                },
                new ArticleImage
                {
                    Id = 4,
                    ArticleId = 2,
                    ImgUrl = "/images/articles/vaccination-2.jpg"
                },

                // Images for Article 3 - Balanced Diet
                new ArticleImage
                {
                    Id = 5,
                    ArticleId = 3,
                    ImgUrl = "/images/articles/balanced-diet-1.jpg"
                },
                new ArticleImage
                {
                    Id = 6,
                    ArticleId = 3,
                    ImgUrl = "/images/articles/balanced-diet-2.jpg"
                },

                // Images for Article 4 - Digestive Health
                new ArticleImage
                {
                    Id = 7,
                    ArticleId = 4,
                    ImgUrl = "/images/articles/digestive-health-1.jpg"
                },
                new ArticleImage
                {
                    Id = 8,
                    ArticleId = 4,
                    ImgUrl = "/images/articles/digestive-health-2.jpg"
                },

                // Images for Article 5 - Heart Health
                new ArticleImage
                {
                    Id = 9,
                    ArticleId = 5,
                    ImgUrl = "/images/articles/heart-health-1.jpg"
                },
                new ArticleImage
                {
                    Id = 10,
                    ArticleId = 5,
                    ImgUrl = "/images/articles/heart-health-2.jpg"
                },

                // Images for Article 6 - Lung Cancer
                new ArticleImage
                {
                    Id = 11,
                    ArticleId = 6,
                    ImgUrl = "/images/articles/lung-cancer-1.jpg"
                },
                new ArticleImage
                {
                    Id = 12,
                    ArticleId = 6,
                    ImgUrl = "/images/articles/lung-cancer-2.jpg"
                }
                );
        }
    }
}
