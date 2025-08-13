using BusinessObjects.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class ArticleImage : BaseEntity
    {
        public string ImgUrl { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

    }
}
