FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EnergyLegacyApp.Business/EnergyLegacyApp.Business.csproj", "EnergyLegacyApp.Business/"]
COPY ["EnergyLegacyApp.Data/EnergyLegacyApp.Data.csproj", "EnergyLegacyApp.Data/"]
RUN dotnet restore "EnergyLegacyApp.Business/EnergyLegacyApp.Business.csproj"
COPY . .
WORKDIR "/src/EnergyLegacyApp.Business"
RUN dotnet build "EnergyLegacyApp.Business.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnergyLegacyApp.Business.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnergyLegacyApp.Business.dll"]