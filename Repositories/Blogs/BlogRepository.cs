using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vegetarian.Commons.Schemas;
using vegetarian.Database;
using vegetarian.Databases.Entities;
using vegetarian.Extensions;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Repositories.Blogs
{
    public interface IBlogRepository
    {
        Task<Blog?> GetBlogByIdAsync(int id);
        Task<(int, IEnumerable<Blog>)> GetBlogsAsync(string? keyword, PaginationRequest paginationRequest);
        Task<(int, IEnumerable<Blog>)> GetPublicBlogsAsync(PaginationRequest paginationRequest, BlogType blogType);
        Task<int> AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
        Task<IEnumerable<BlogsUser>> GetBlogsUsersAsync(int blogId);
        Task<bool> RegisterEventAsync(int blogId, string userFullName, string userEmail, string userPhone);
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

        public async Task<(int, IEnumerable<Blog>)> GetPublicBlogsAsync(PaginationRequest paginationRequest, BlogType blogType)
        {
            var query = _context.Blogs.AsQueryable();

            query = query.Where(c => c.Type == blogType && c.Status == BlogStatus.Published && c.PublishedAt <= DateTime.UtcNow);

            var totalCount = await query.CountAsync();

            query = query
                .Include(x => x.BlogsUsers)
                .OrderByDynamic(paginationRequest.OrderBy, paginationRequest.OrderDirection);

            return (totalCount, await query
                .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync());
        }

        public async Task<bool> RegisterEventAsync(int blogId, string userFullName, string userEmail, string userPhone)
        {
            var blog = await GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return false;
            }

            if (blog.BlogsUsers.Any(x => x.UserEmail == userEmail))
            {
                return false;
            }

            var currentUserCounts = blog.BlogsUsers.Count;
            if (currentUserCounts >= blog.MaxUsers)
            {
                return false;
            }

            var blogsUser = new BlogsUser
            {
                BlogId = blogId,
                UserFullName = userFullName,
                UserEmail = userEmail,
                UserPhone = userPhone
            };

            await _context.BlogsUsers.AddAsync(blogsUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}