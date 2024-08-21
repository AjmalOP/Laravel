using Laravel.Data;
using Laravel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Laravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _context.Blogs.ToListAsync();
            return Ok(blogs);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blog)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            blog.Author = userId;

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return Ok(blog);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog updatedBlog)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.Name);
            if (blog.Author != userId)
            {
                return Forbid();
            }

            blog.Title = updatedBlog.Title;
            blog.Content = updatedBlog.Content;

            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();

            return Ok(blog);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.Name);
            if (blog.Author != userId)
            {
                return Forbid();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Blog deleted successfully." });
        }
    }
}
