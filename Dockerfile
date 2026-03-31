# ── Stage 1: Restore ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first — cached layer, only invalidated when .csproj changes
COPY Learnmate.sln .
COPY BusinessObject/BusinessObject.csproj           BusinessObject/
COPY DataAccessLayer/DataAccessLayer.csproj         DataAccessLayer/
COPY BusinessLogicLayer/BusinessLogicLayer.csproj   BusinessLogicLayer/
COPY LearnmateSolution/LearnmateSolution.csproj     LearnmateSolution/

RUN dotnet restore Learnmate.sln

# ── Stage 2: Publish ───────────────────────────────────────────────────────────
FROM build AS publish
COPY . .
RUN dotnet publish LearnmateSolution/LearnmateSolution.csproj \
    -c Release -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ── Stage 3: Runtime ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Run as non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

# ASP.NET Core listens on 8080 by default in .NET 8 Docker images
EXPOSE 8080

ENTRYPOINT ["dotnet", "LearnmateSolution.dll"]
