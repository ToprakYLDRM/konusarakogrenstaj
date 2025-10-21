# 1. Adım: Build Ortamı - SDK imajını kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj dosyasını kopyala ve paketleri geri yükle (daha hızlı build için)
COPY ["backend/ProjectIntern/ProjectIntern.csproj", "backend/ProjectIntern/"]
RUN dotnet restore "backend/ProjectIntern/ProjectIntern.csproj"

# Tüm proje dosyalarını kopyala
COPY . .
WORKDIR "/src/backend/ProjectIntern"
RUN dotnet build "ProjectIntern.csproj" -c Release -o /app/build

# 2. Adım: Yayınlama
FROM build AS publish
RUN dotnet publish "ProjectIntern.csproj" -c Release -o /app/publish

# 3. Adım: Çalıştırma Ortamı - Sadece runtime imajını kullan (daha küçük)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectIntern.dll"]
