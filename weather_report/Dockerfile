##################
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["weather_report/weather_report.csproj", "weather_report/"]
RUN dotnet restore "weather_report/weather_report.csproj"
COPY . .
WORKDIR "/src/weather_report"
RUN dotnet build "weather_report.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "weather_report.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "weather_report.dll"]