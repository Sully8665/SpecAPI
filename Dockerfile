# Multi-stage Dockerfile for SpecAPI
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /src

# Copy project files
COPY ["src/SpecAPI/SpecAPI.csproj", "src/SpecAPI/"]
COPY ["src/SpecAPI/", "src/SpecAPI/"]

# Restore dependencies
RUN dotnet restore "src/SpecAPI/SpecAPI.csproj"

# Build the application
RUN dotnet build "src/SpecAPI/SpecAPI.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "src/SpecAPI/SpecAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Create non-root user for security
RUN groupadd -r specapi && useradd -r -g specapi specapi
RUN chown -R specapi:specapi /app
USER specapi

# Expose port (if needed for web interface in future)
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Entry point
ENTRYPOINT ["dotnet", "SpecAPI.dll"]

# Default command (can be overridden)
CMD ["--help"] 