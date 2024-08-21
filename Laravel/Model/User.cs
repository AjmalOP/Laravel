using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Laravel.Model
{
    public class User : IdentityUser
    {
        public ICollection<Blog> Blogs { get; set; }
    }
}
