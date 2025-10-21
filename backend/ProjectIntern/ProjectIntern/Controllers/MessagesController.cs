using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIntern.Data;
using ProjectIntern.Models;
using ProjectIntern.DTOs;
using System.Text;
using System.Text.Json;

namespace ProjectIntern.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public MessagesController(ChatDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] Message newMessage)
        {
            if (newMessage == null || string.IsNullOrWhiteSpace(newMessage.Username) || string.IsNullOrWhiteSpace(newMessage.Text))
            {
                return BadRequest("Kullanıcı adı ve mesaj metni boş olamaz.");
            }

            try
            {
                // YENİLENDİ: Doğru API adresini tekrar kontrol edelim.
                var aiApiUrl = "https://xas-ty-ko.hf.space/predict";

                // YENİLENDİ: Gönderilecek verinin formatını Hugging Face'in beklediği gibi hazırlayalım.
                var requestPayload = new AiRequest();
                requestPayload.data.Add(newMessage.Text);

                var jsonPayload = JsonSerializer.Serialize(requestPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // --- HATA AYIKLAMA İÇİN EKLENDİ ---
                Console.WriteLine("--- Yeni İstek Başladı ---");
                Console.WriteLine($"AI Servisine Gönderilen JSON: {jsonPayload}");
                // ------------------------------------

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync(aiApiUrl, content);

                // --- HATA AYIKLAMA İÇİN EKLENDİ ---
                Console.WriteLine($"AI Servisinden Gelen Yanıt Durumu: {response.StatusCode}");
                // ------------------------------------

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    // --- HATA AYIKLAMA İÇİN EKLENDİ ---
                    Console.WriteLine($"AI Servisinden Gelen Yanıt Body: {responseBody}");
                    // ------------------------------------

                    var aiResponse = JsonSerializer.Deserialize<AiResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (aiResponse != null && aiResponse.data.Any())
                    {
                        var sentiment = aiResponse.data.First();
                        newMessage.SentimentLabel = sentiment.label;
                        newMessage.SentimentScore = sentiment.score;
                        Console.WriteLine($"Duygu analizi BAŞARILI: {sentiment.label}");
                    }
                }
                else
                {
                    // --- HATA AYIKLAMA İÇİN EKLENDİ ---
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"AI Servisinden Gelen HATA: {errorBody}");
                    // ------------------------------------
                }
            }
            catch (Exception ex)
            {
                // --- HATA AYIKLAMA İÇİN EKLENDİ ---
                Console.WriteLine($"!!! DUYGU ANALİZİ SIRASINDA KRİTİK HATA: {ex.Message}");
                Console.WriteLine($"!!! HATA DETAYI (STACK TRACE): {ex.StackTrace}");
                // ------------------------------------
            }

            newMessage.Timestamp = DateTime.UtcNow;
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            Console.WriteLine("--- İstek Tamamlandı ---");
            return CreatedAtAction(nameof(GetMessages), new { id = newMessage.Id }, newMessage);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllMessages()
        {
            try
            {
                var allMessages = await _context.Messages.ToListAsync();
                _context.Messages.RemoveRange(allMessages);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Tüm mesajlar silindi. Toplam silinen mesaj sayısı: {allMessages.Count}");
                return Ok(new { message = "Tüm mesajlar başarıyla silindi.", deletedCount = allMessages.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mesajlar silinirken hata: {ex.Message}");
                return StatusCode(500, new { error = "Mesajlar silinirken bir hata oluştu." });
            }
        }
    }
}

