# ── Stage 1: Build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for layer caching (restore only re-runs when .csproj changes)
COPY Learnmate.sln .
COPY BusinessObject/BusinessObject.csproj           BusinessObject/
COPY DataAccessLayer/DataAccessLayer.csproj         DataAccessLayer/
COPY BusinessLogicLayer/BusinessLogicLayer.csproj   BusinessLogicLayer/
COPY LearnmateSolution/LearnmateSolution.csproj     LearnmateSolution/

RUN dotnet restore Learnmate.sln

# Copy all source and publish
COPY . .
RUN dotnet publish LearnmateSolution/LearnmateSolution.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# ASP.NET Core listens on 8080 by default in .NET 8 Docker images
EXPOSE 8080

ENTRYPOINT ["dotnet", "LearnmateSolution.dll"]
