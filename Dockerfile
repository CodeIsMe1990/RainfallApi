FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/RainfallApi.Api/RainfallApi.Api.csproj", "RainfallApi.Api/"]
COPY ["src/RainfallApi.Application/RainfallApi.Application.csproj", "RainfallApi.Application/"]
COPY ["src/RainfallApi.Domain/RainfallApi.Domain.csproj", "RainfallApi.Domain/"]
COPY ["src/RainfallApi.Contracts/RainfallApi.Contracts.csproj", "RainfallApi.Contracts/"]
COPY ["src/RainfallApi.Infrastructure/RainfallApi.Infrastructure.csproj", "RainfallApi.Infrastructure/"]
COPY ["Directory.Packages.props", "./"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore "RainfallApi.Api/RainfallApi.Api.csproj"
COPY . ../
WORKDIR /src/RainfallApi.Api
RUN dotnet build "RainfallApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RainfallApi.Api.dll"]