FROM mcr.microsoft.com/dotnet/aspnet:8.0

USER app

WORKDIR /app

COPY Api/ /app/

ENTRYPOINT ["dotnet", "DGW.DogSitter.DogWalker.Api.dll"]
