# CinemaBookingApi için Dockerfile
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
# USER app
# WORKDIR /app
# EXPOSE 5000
# EXPOSE 5001

# # .NET Build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["CinemaBookingApi.csproj", "CinemaBookingApi/"]
# RUN dotnet restore "CinemaBookingApi/CinemaBookingApi.csproj"
# COPY . .
# WORKDIR "/src/CinemaBookingApi"
# RUN dotnet build -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "./CinemaBookingApi.csproj" -c Release -o /app/publish

# # .NET Run
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/publish .
# ENTRYPOINT ["dotnet", "CinemaBookingApi.dll"]

# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
WORKDIR /src
COPY ["CinemaBookingApi.csproj", "CinemaBookingApi/"]
RUN dotnet restore "CinemaBookingApi/CinemaBookingApi.csproj"
WORKDIR "/src/CinemaBookingApi"
# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port your application will run on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "CinemaBookingApi.dll"]