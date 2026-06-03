# Esta fase se usa cuando se ejecuta desde VS en modo rápido
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# ✅ NUEVO: argumentos para versión y release
ARG APP_VERSION=1.0.0
ARG APP_RELEASE=stable

WORKDIR /src
COPY ["UDEM.DEVOPS.DogSitter.Api/nuget.config", "UDEM.DEVOPS.DogSitter.Api/"]
COPY ["UDEM.DEVOPS.DogSitter.Application/nuget.config", "UDEM.DEVOPS.DogSitter.Application/"]
COPY ["UDEM.DEVOPS.DogSitter.Domain/nuget.config", "UDEM.DEVOPS.DogSitter.Domain/"]
COPY ["UDEM.DEVOPS.DogSitter.Infrastructure/nuget.config", "UDEM.DEVOPS.DogSitter.Infrastructure/"]
COPY ["UDEM.DEVOPS.DogSitter.Api/UDEM.DEVOPS.DogSitter.Api.csproj", "UDEM.DEVOPS.DogSitter.Api/"]
COPY ["UDEM.DEVOPS.DogSitter.Application/UDEM.DEVOPS.DogSitter.Application.csproj", "UDEM.DEVOPS.DogSitter.Application/"]
COPY ["UDEM.DEVOPS.DogSitter.Domain/UDEM.DEVOPS.DogSitter.Domain.csproj", "UDEM.DEVOPS.DogSitter.Domain/"]
COPY ["UDEM.DEVOPS.DogSitter.Infrastructure/UDEM.DEVOPS.DogSitter.Infrastructure.csproj", "UDEM.DEVOPS.DogSitter.Infrastructure/"]
RUN dotnet restore "./UDEM.DEVOPS.DogSitter.Api/UDEM.DEVOPS.DogSitter.Api.csproj"
COPY . .
WORKDIR "/src/UDEM.DEVOPS.DogSitter.Api"

# ✅ MODIFICADO: inyecta la versión en el ensamblado al compilar
RUN dotnet build "./UDEM.DEVOPS.DogSitter.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    /p:Version=$APP_VERSION \
    /p:InformationalVersion=$APP_VERSION-$APP_RELEASE \
    -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG APP_VERSION=1.0.0
ARG APP_RELEASE=stable

# ✅ MODIFICADO: misma inyección en el publish
RUN dotnet publish "./UDEM.DEVOPS.DogSitter.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    /p:Version=$APP_VERSION \
    /p:InformationalVersion=$APP_VERSION-$APP_RELEASE \
    /p:UseAppHost=false \
    -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ✅ NUEVO: variables de entorno disponibles en runtime para appsettings
ENV AppVersion__Version=$APP_VERSION
ENV AppVersion__Release=$APP_RELEASE

ENTRYPOINT ["dotnet", "UDEM.DEVOPS.DogSitter.Api.dll"]