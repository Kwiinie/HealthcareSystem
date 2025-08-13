using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Article
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public string? ImgUrl { get; set; }
        public List<string> ImgUrls { get; set; }

        public bool IsDeleted { get; set; }

    }
}
