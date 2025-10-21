using System.ComponentModel.DataAnnotations;

namespace ProjectIntern.Models; // Namespace'i proje adına göre güncelledim

public class Message
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Text { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? SentimentLabel { get; set; }

    public double? SentimentScore { get; set; }
}
