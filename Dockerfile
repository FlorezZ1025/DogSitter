# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
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
RUN dotnet build "./UDEM.DEVOPS.DogSitter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UDEM.DEVOPS.DogSitter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UDEM.DEVOPS.DogSitter.Api.dll"]