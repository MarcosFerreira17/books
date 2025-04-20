
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Host/TJRJ.WebApi/TJRJ.WebApi.csproj", "src/Host/TJRJ.WebApi/"]
RUN dotnet restore "./src/Host/TJRJ.WebApi/TJRJ.WebApi.csproj"
COPY . .
WORKDIR "/src/Host/TJRJ.WebApi"
RUN dotnet build "./TJRJ.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TJRJ.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TJRJ.WebApi.dll"]