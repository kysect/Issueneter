FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
<<<<<<< HEAD:Issueneter.Host/Dockerfile
COPY ["Issueneter.Host/Issueneter.Host.csproj", "Issueneter.Host/"]
RUN dotnet restore "Issueneter.Host/Issueneter.Host.csproj"
=======

COPY ["./Issueneter.WebAPI/", "./Issueneter.WebAPI/src/"]
RUN dotnet restore "./Issueneter.WebAPI/src/Issueneter.WebAPI.fsproj"

>>>>>>> master:Issueneter.WebAPI/Dockerfile
COPY . .
WORKDIR "/src/Issueneter.Host"
RUN dotnet build "Issueneter.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Issueneter.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Issueneter.Host.dll"]
