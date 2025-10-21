using Microsoft.EntityFrameworkCore;
using ProjectIntern.Models; // using ProjectIntern.Models; olarak güncelledim

namespace ProjectIntern.Data; // Namespace'i proje adına göre güncelledim

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
}
