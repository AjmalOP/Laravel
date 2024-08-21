using System.ComponentModel.DataAnnotations;

namespace Laravel.Model
{
    public class Blog
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public string Author { get; set; } // The author's username
}
}
