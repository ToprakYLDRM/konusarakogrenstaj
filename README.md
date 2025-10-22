[README.md](https://github.com/user-attachments/files/23064210/README.md)
Konuşarak Öğren Staj Projesi: AI Destekli Duygu Analizli Chat Uygulaması

Bu proje, kullanıcıların metin tabanlı sohbet edebildiği ve mesajların anlık olarak yapay zeka ile duygu analizine (pozitif, negatif, nötr) tabi tutulduğu bir web uygulamasıdır.  
Frontend, backend ve AI servislerinden oluşmaktadır. Tüm servisler ücretsiz platformlarda barındırılmıştır.

---

Proje Özeti

Frontend:  
React (Vite) ile geliştirilmiş basit bir chat arayüzü.  
Kullanıcılar burada mesaj gönderip geçmiş mesajları ve duygu analiz sonuçlarını görebiliyor.

Backend: 
.NET 8 Core Web API ile yazıldı.  
Kullanıcı adı ve mesajı alır, AI servisine gönderir, sonucu veritabanına kaydeder ve mesaj listesini döner.

AI Servisi:
Python + FastAPI ile geliştirildi.  
Hugging Face üzerindeki `savasy/bert-base-turkish-sentiment-cased` modelini kullanarak Türkçe metinlerin duygu analizini yapar.  
Bu servis Docker ile Hugging Face Spaces üzerinde çalışıyor.

Veritabanı
PostgreSQL (Render üzerinde barındırıldı).

---

Kullanılan Teknolojiler

Frontend
- React (Vite)
- JavaScript
- CSS  
- Hosting: Vercel

Backend
- .NET 8 Core Web API
- C#
- Entity Framework Core (Npgsql)
- Hosting: Render (Web Service)

AI Servisi
- Python 3.10
- FastAPI
- Transformers (Hugging Face)
- Docker
- Hosting: Hugging Face Spaces

Veritabanı
- PostgreSQL
- Hosting: Render

---

Klasör Yapısı

```
konusarakogrenstaj/
├── backend/
│   └── ProjectIntern/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Migrations/
│       ├── Models/
│       ├── Program.cs
│       ├── Dockerfile
│       └── ProjectIntern.csproj
├── frontend/
│   └── chatwweb/
│       ├── public/
│       ├── src/
│       │   ├── App.jsx
│       │   └── App.css
│       ├── index.html
│       ├── package.json
│       └── vite.config.js
└── README.md
```

---

Kurulum (Yerel Geliştirme Ortamı)

Gereksinimler
- .NET 8 SDK  
- Node.js + npm  
- Python 3.10  
- Git  
- PostgreSQL  
- VS Code veya Visual Studio

---

Backend Kurulumu
```bash
cd backend/ProjectIntern/ProjectIntern
dotnet restore
dotnet ef database update
dotnet run
```
> API genellikle `https://localhost:PORT` adresinde çalışır.

---

Frontend Kurulumu
```bash
cd frontend/chatwweb
npm install
npm run dev
```
> Frontend genellikle `http://localhost:5173` adresinde açılır.

---

AI Servisi (Yerel veya Hugging Face)
AI servisi Hugging Face üzerinde çalıştığı için yerelde çalıştırmak zorunlu değil ama istersen:
```bash
cd ai-service
pip install -r requirements.txt
uvicorn app:app --reload
```

---

Canlıya Alma (Deployment)

| Katman | Platform | Not |
|--------|-----------|-----|
| AI Servisi| Hugging Face Spaces (Docker) | `/predict` endpoint |
| Backend API | Render (Web Service) | Dockerfile: `backend/ProjectIntern/ProjectIntern/Dockerfile` |
| Veritabanı | Render (PostgreSQL) | Internal URL kullanıldı |
| Frontend | Vercel (Vite) | Root: `frontend/chatwweb` |

---

Veri Akışı (Nasıl Çalışıyor?)

1. Kullanıcı frontend’den mesaj gönderir.  
2. React, mesajı backend API’ye (`/api/messages`) gönderir.  
3. .NET API mesajı AI servisine yollar.  
4. Python servisi duygu analizini yapar ve sonucu döner.  
5. Backend sonucu veritabanına kaydeder.  
6. Frontend mesaj listesini yeniler ve sonucu ekranda gösterir.

---

Kullanılan AI Araçları

Proje boyunca bazı kısımlarda AI yardımı aldım:

- Backend: Küçük bir kısımda Gemini AI’dan destek alındı (özellikle AI servisi isteği atma kısmında).  
- AI Servisi (Python): Tamamen Gemini AI tarafından oluşturuldu.  
- Frontend: Cursor editörünü kullanarak React tarafını yarı yarıya kendi yazdım, yarı yarıya Cursor yardımıyla oluşturdum.  
- Kod düzeni, CSS ve bazı hata ayıklama işlemlerini kendim yaptım.  

Genel olarak proje yapısını, bağlantı ayarlarını, platform kurulumlarını ve servislerin birbirine bağlanmasını kendim yaptım.

---

Kişisel Katkım

- Backend tarafında temel API yapısını kurdum, veritabanı bağlantılarını ayarladım.  
- Gemini AI’ı sadece belirli küçük bölümlerde kullandım.  
- Python AI servisini tamamen Gemini AI’a yaptırdım ama entegrasyon kısmını ben yaptım.  
- Frontend’de Cursor ile birlikte çalışarak bileşenleri oluşturdum.  
- Tüm servisleri Render, Vercel ve Hugging Face üzerinde birbirine bağlayarak çalışan bir sistem haline getirdim.

---

Sonuç

Bu proje, farklı teknolojileri bir araya getirerek uçtan uca çalışan bir yapay zekâ destekli chat uygulaması ortaya koymayı amaçlamaktadır.  
AI servisinden frontend’e kadar tüm bileşenlerin birlikte çalışmasıyla sistem, mesajların anlık duygu analizini başarıyla gerçekleştirmektedir.

---

Geliştirici: Toprak Yıldırım  
Proje: Konuşarak Öğren Staj Projesi  
Yıl: 2025
