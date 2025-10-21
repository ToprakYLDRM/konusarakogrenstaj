from fastapi import FastAPI
from pydantic import BaseModel
from transformers import pipeline
import torch

# --- 1. Model Yükleme ---
# CPU kullanılacak şekilde ayarlandı
sentiment_pipeline = pipeline(
    "sentiment-analysis",
    model="savasy/bert-base-turkish-sentiment-cased",
    device=-1 # CPU için -1, GPU için 0
)

# --- 2. FastAPI Uygulaması ---
app = FastAPI()

# Gelen isteğin formatını belirleyen model
class PredictionRequest(BaseModel):
    data: list[str]

# --- 3. API Endpoint ---
# Bu, backend'imizin konuşacağı tek ve güvenilir adres
@app.post("/predict")
def predict(request: PredictionRequest):
    try:
        text_to_analyze = request.data[0]
        
        # Analiz işlemini yap
        result = sentiment_pipeline(text_to_analyze)[0]
        
        # Sonucu Gradio ile aynı formatta döndür
        return {"data": [{"label": result['label'], "score": result['score']}]}
    except Exception as e:
        return {"error": str(e)}

# Ana endpoint (sadece test için)
@app.get("/")
def read_root():
    return {"message": "AI Sentiment Analysis API is running correctly"}

