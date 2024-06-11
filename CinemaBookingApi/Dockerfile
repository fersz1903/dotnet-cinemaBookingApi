# # Uygulama için kullanılan temel imaj
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# # Build için kullanılan imaj
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["CinemaBookingApi.csproj", "./"]
# RUN dotnet restore "./CinemaBookingApi.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "CinemaBookingApi.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "CinemaBookingApi.csproj" -c Release -o /app/publish

# # Publish edilen imajın kopyalanması
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "CinemaBookingApi.dll"]


# # Use the official .NET Core SDK as a parent image
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# # Copy the project file and restore any dependencies (use .csproj for the project name)
# COPY *.csproj ./
# RUN dotnet restore

# # Copy the rest of the application code
# COPY . .

# # Publish the application
# RUN dotnet publish -c Release -o out

# # Build the runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/out ./

# # Expose the port your application will run on
# EXPOSE 80

# # Start the application
# ENTRYPOINT ["dotnet", "CinemaBookingApi.dll"]



# Build aşaması için kullanılan temel imaj
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyalayın ve restore yapın
COPY ["CinemaBookingApi/CinemaBookingApi.csproj", "CinemaBookingApi/"]
RUN dotnet restore "CinemaBookingApi/CinemaBookingApi.csproj"

# Geri kalan tüm dosyaları kopyalayın ve build yapın
COPY . .
WORKDIR "/src/CinemaBookingApi"
RUN dotnet build "CinemaBookingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CinemaBookingApi.csproj" -c Release -o /app/publish

# Runtime aşaması için kullanılan temel imaj
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaBookingApi.dll"]
