using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Article : BaseEntity
{
    public int? CategoryId { get; set; }

    public string? Title { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedById { get; set; }

    public string? Content { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? CreatedBy { get; set; }
    public string? ImgUrl { get; set; }
    public virtual ICollection<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

}
