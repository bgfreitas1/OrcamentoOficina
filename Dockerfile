FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["OrcamentoOficina.API/OrcamentoOficina.API.csproj", "OrcamentoOficina.API/"]
COPY ["OrcamentoOficina.Application/OrcamentoOficina.Application.csproj", "OrcamentoOficina.Application/"]
COPY ["OrcamentoOficina.Domain/OrcamentoOficina.Domain.csproj", "OrcamentoOficina.Domain/"]
COPY ["OrcamentoOficina.Infrastructure/OrcamentoOficina.Infrastructure.csproj", "OrcamentoOficina.Infrastructure/"]

RUN dotnet restore "OrcamentoOficina.API/OrcamentoOficina.API.csproj"

COPY . .

WORKDIR "/src/OrcamentoOficina.API"
RUN dotnet publish "OrcamentoOficina.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrcamentoOficina.API.dll"]