using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vegetarian.Commons.Schemas;
using vegetarian.Database;
using vegetarian.Databases.Entities;
using vegetarian.Extensions;

namespace vegetarian.Repositories.Blogs
{
    public interface IBlogRepository
    {
        Task<Blog?> GetBlogByIdAsync(int id);
        Task<(int, IEnumerable<Blog>)> GetBlogsAsync(string? keyword, PaginationRequest paginationRequest);
        Task<int> AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
        Task<IEnumerable<BlogsUser>> GetBlogsUsersAsync(int blogId);
    }
    public class BlogRepository(DataContext context) : IBlogRepository
    {
        // Assuming you have a DbContext or similar to interact with the database
        private readonly DataContext _context = context;

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(x => x.BlogsUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(int, IEnumerable<Blog>)> GetBlogsAsync(string? keyword, PaginationRequest paginationRequest)
        {
            var query = _context.Blogs.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Title.Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            query = query
                .Include(x => x.BlogsUsers)
                .OrderByDynamic(paginationRequest.OrderBy, paginationRequest.OrderDirection);

            return (totalCount, await query
                .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync());
        }

        public async Task<int> AddBlogAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return blog.Id;
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await GetBlogByIdAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BlogsUser>> GetBlogsUsersAsync(int blogId)
        {
            var blog = await GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return [];
            }

            return await _context.BlogsUsers
                .Where(bu => bu.BlogId == blogId)
                .ToListAsync();
        }
    }
}