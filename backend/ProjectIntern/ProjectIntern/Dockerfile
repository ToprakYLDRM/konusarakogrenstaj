# 1. Adım: Build Ortamı - SDK imajını kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Bütün proje dosyalarını (tüm evi) kopyala
COPY . .

# Projeyi yayınla.
# DİKKAT: Bu yol, senin GitHub yapınla BİREBİR EŞLEŞEN DOĞRU YOLDUR.
RUN dotnet publish "backend/ProjectIntern/ProjectIntern/ProjectIntern.csproj" -c Release -o /app/publish

# 2. Adım: Çalıştırma Ortamı - Sadece runtime imajını kullan (daha küçük)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjectIntern.dll"]
