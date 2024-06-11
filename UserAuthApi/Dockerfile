# Build aşaması için kullanılan temel imaj
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyalayın ve restore yapın
COPY ["UserAuthApi/UserAuthApi.csproj", "UserAuthApi/"]
RUN dotnet restore "UserAuthApi/UserAuthApi.csproj"

# Geri kalan tüm dosyaları kopyalayın ve build yapın
COPY . .
WORKDIR "/src/UserAuthApi"
RUN dotnet build "UserAuthApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserAuthApi.csproj" -c Release -o /app/publish

# Runtime aşaması için kullanılan temel imaj
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAuthApi.dll"]