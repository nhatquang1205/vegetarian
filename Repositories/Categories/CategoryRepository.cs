using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vegetarian.Commons.Schemas;
using vegetarian.Database;
using vegetarian.Databases.Entities;
using vegetarian.Extensions;

namespace vegetarian.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<(int, IEnumerable<Category>)> GetCategoriesAsync(string? keyword, PaginationRequest paginationRequest);
        Task<int> AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
    public class CategoryRepository(DataContext context) : ICategoryRepository
    {
        // Assuming you have a DbContext or similar to interact with the database
        private readonly DataContext _context = context;

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<(int, IEnumerable<Category>)> GetCategoriesAsync(string? keyword, PaginationRequest paginationRequest)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Name.Contains(keyword) || c.Description.Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDynamic(paginationRequest.OrderBy, paginationRequest.OrderDirection);

            return (totalCount, await query
                .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync());
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}