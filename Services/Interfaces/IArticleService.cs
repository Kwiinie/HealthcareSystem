using BusinessObjects.DTOs.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDTO>> GetAllArticlesAsync();
        Task<ArticleDTO> GetArticleByIdAsync(int id);
        Task<bool> UpdateArticleAsync(ArticleDTO articleDto);
        Task AddArticleAsync(ArticleDTO articleDTO);
        Task DeleteArticleAsync(int id);
    }
}
