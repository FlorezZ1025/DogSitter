# =========================
# Etapa 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UDEM.DEVOPS.DogSitter.sln", "./"]

COPY ["UDEM.DEVOPS.DogSitter.Api/UDEM.DEVOPS.DogSitter.Api.csproj", "UDEM.DEVOPS.DogSitter.Api/"]
COPY ["UDEM.DEVOPS.DogSitter.Api.Tests/UDEM.DEVOPS.DogSitter.Api.Tests.csproj", "UDEM.DEVOPS.DogSitter.Api.Tests/"]
COPY ["UDEM.DEVOPS.DogSitter.Infrastructure/UDEM.DEVOPS.DogSitter.Infrastructure.csproj", "UDEM.DEVOPS.DogSitter.Infrastructure/"]
COPY ["UDEM.DEVOPS.DogSitter.Application/UDEM.DEVOPS.DogSitter.Application.csproj", "UDEM.DEVOPS.DogSitter.Application/"]
COPY ["UDEM.DEVOPS.DogSitter.Domain/UDEM.DEVOPS.DogSitter.Domain.csproj", "UDEM.DEVOPS.DogSitter.Domain/"]
COPY ["UDEM.DEVOPS.DogSitter.Domain.Tests/UDEM.DEVOPS.DogSitter.Domain.Tests.csproj", "UDEM.DEVOPS.DogSitter.Domain.Tests/"]

RUN dotnet restore

COPY . .
WORKDIR "/src/UDEM.DEVOPS.DogSitter.Api"

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# =========================
# Etapa 2: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "UDEM.DEVOPS.DogSitter.Api.dll"]