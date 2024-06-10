# UserAuthApi için Dockerfile

# .NET Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserAuthApi/UserAuthApi.csproj", "UserAuthApi/"]
RUN dotnet restore "UserAuthApi/UserAuthApi.csproj"
COPY . .
WORKDIR "/src/UserAuthApi"
RUN dotnet build "UserAuthApi.csproj" -c Release -o /app/build
RUN dotnet publish "UserAuthApi.csproj" -c Release -o /app/publish

# .NET Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserAuthApi.dll"]