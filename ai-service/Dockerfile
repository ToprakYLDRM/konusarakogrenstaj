# Python 3.10 imajını kullan
FROM python:3.10

# Çalışma dizinini ayarla
WORKDIR /code

# requirements.txt dosyasını kopyala ve kütüphaneleri yükle
COPY ./requirements.txt /code/requirements.txt
RUN pip install --no-cache-dir --upgrade -r /code/requirements.txt

# Uygulama kodunu kopyala
COPY ./app.py /code/app.py

# Uygulamayı çalıştıracak komut
# Hugging Face tarafından sağlanan 7860 portunu kullan
CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "7860"]