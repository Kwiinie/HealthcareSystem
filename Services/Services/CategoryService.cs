using AutoMapper;
using BusinessObjects.DTOs.Article;
using BusinessObjects.DTOs.Category;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddCategoryAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null) throw new ArgumentNullException(nameof(categoryDTO));
            User user = null;


            var category = _mapper.Map<Category>(categoryDTO);
            category.CreatedAt = DateTime.UtcNow.AddHours(7);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var result = await _unitOfWork.CategoryRepository.GetAllAsync();
            if(result != null)
            {
                return _mapper.Map<IEnumerable<CategoryDTO>>(result);
            }
            else
            {
                return new List<CategoryDTO>();
            }
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var result = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(result == null)
            {
                return null;
            }
            return _mapper.Map<CategoryDTO>(result);
        }

        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null) throw new ArgumentNullException(nameof(categoryDTO));
            User user = null;


            var category = _mapper.Map<Category>(categoryDTO);

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
