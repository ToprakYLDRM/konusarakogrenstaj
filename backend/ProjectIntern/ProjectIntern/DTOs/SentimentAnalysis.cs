
namespace ProjectIntern.DTOs
{
    public class AiRequest
    {
        public List<string> data { get; set; } = new List<string>();
    }

    public class AiResponse
    {
        public List<SentimentResult> data { get; set; } = new List<SentimentResult>();
    }

    public class SentimentResult
    {
        public string label { get; set; } = string.Empty;
        public double score { get; set; }
    }
}
