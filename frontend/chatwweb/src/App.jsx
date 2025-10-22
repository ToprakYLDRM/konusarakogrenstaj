import React, { useState, useEffect, useRef } from 'react';
import './App.css';

export default function App() {
  const [messages, setMessages] = useState([]);
  const [username, setUsername] = useState('');
  const [text, setText] = useState('');
  const [currentUser, setCurrentUser] = useState(''); // Mevcut kullanıcı adı
  const messagesEndRef = useRef(null);

  // DİKKAT: Backend'ini çalıştırdığında adres çubuğunda yazan port
  // numarasını kontrol et ve gerekirse burayı güncelle
  const apiBaseUrl = 'https://konusarakogrenstaj-1.onrender.com';

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  // Bileşen ilk yüklendiğinde mesajları çek
  useEffect(() => {
    fetchMessages();
  }, []);

  // Her yeni mesaj geldiğinde en alta kaydır
  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const fetchMessages = async () => {
    try {
      const response = await fetch(`${apiBaseUrl}/api/messages`);
      const data = await response.json();
      console.log("Gelen mesajlar:", data);

      // Her mesajın sentiment bilgilerini kontrol et
      data.forEach((msg, index) => {
        console.log(`Mesaj ${index}:`, {
          text: msg.text,
          sentimentLabel: msg.sentimentLabel,
          sentimentScore: msg.sentimentScore
        });
      });

      setMessages(data);
    } catch (error) {
      console.error("Mesajlar alınırken hata:", error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!username.trim() || !text.trim()) {
      alert("Kullanıcı adı ve mesaj boş olamaz!");
      return;
    }

    // İlk mesaj gönderildiğinde currentUser'ı ayarla
    if (!currentUser) {
      setCurrentUser(username);
    }

    const newMessage = { username, text };
    console.log("Gönderilen mesaj:", newMessage);

    try {
      const response = await fetch(`${apiBaseUrl}/api/messages`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newMessage),
      });

      console.log("Response status:", response.status);
      const responseData = await response.json();
      console.log("Response data:", responseData);

      if (response.ok) {
        setText('');
        fetchMessages();
      } else {
        console.error("Mesaj gönderilemedi:", responseData);
      }
    } catch (error) {
      console.error("Mesaj gönderilirken hata:", error);
    }
  };

  const clearChat = async () => {
    if (window.confirm("Tüm mesajları silmek istediğinizden emin misiniz?")) {
      try {
        const response = await fetch(`${apiBaseUrl}/api/messages`, {
          method: 'DELETE',
        });

        if (response.ok) {
          console.log("Chat temizlendi");
          fetchMessages(); // Mesajları yenile
        } else {
          console.error("Chat temizlenemedi");
        }
      } catch (error) {
        console.error("Chat temizlenirken hata:", error);
      }
    }
  };

  // Duygu analiz sonucuna göre emoji döndüren fonksiyon
  const getSentimentEmoji = (label) => {
    if (!label) return '🤔'; // Eğer etiket yoksa
    if (label.toUpperCase() === 'POSITIVE') return '😊';
    if (label.toUpperCase() === 'NEGATIVE') return '😠';
    if (label.toUpperCase() === 'NEUTRAL') return '😐';
    return '🤔';
  };

  return (
    <div className="app-container">
      <div className="chat-window">
        <div className="chat-header">
          <h2>AI Duygu Analizli Chat</h2>
          <button className="clear-button" onClick={clearChat} title="Tüm mesajları sil">
            🗑️
          </button>
        </div>
        <div className="messages-list">
          {messages.map((msg) => {
            const isCurrentUser = msg.username === currentUser;
            const hasSentiment = msg.sentimentLabel && msg.sentimentScore;

            return (
              <div key={msg.id}>
                {/* Ana mesaj */}
                <div className={`message-item ${isCurrentUser ? 'current-user' : 'other-user'}`}>
                  <div className="message-header">
                    <span className="username">{msg.username}</span>
                    <span className="timestamp">{new Date(msg.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
                  </div>
                  <p className="message-text">{msg.text}</p>
                </div>

                {/* Sentiment analizi ayrı mesaj olarak */}
                {hasSentiment && (
                  <div className="sentiment-message">
                    <div className="sentiment-content">
                      <span className="sentiment-label">AI Analiz:</span>
                      <span className="sentiment-result">
                        {getSentimentEmoji(msg.sentimentLabel)} {msg.sentimentLabel} ({((msg.sentimentScore || 0) * 100).toFixed(1)}%)
                      </span>
                    </div>
                  </div>
                )}
              </div>
            );
          })}
          <div ref={messagesEndRef} />
        </div>
        <form className="message-form" onSubmit={handleSubmit}>
          <input
            type="text"
            className="username-input"
            placeholder="Kullanıcı Adı"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <input
            type="text"
            className="message-input"
            placeholder="Mesajınızı yazın..."
            value={text}
            onChange={(e) => setText(e.target.value)}
          />
          <button type="submit" className="send-button">Gönder</button>
        </form>
      </div>
    </div>
  );
}

