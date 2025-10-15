# Multi-stage Dockerfile for building and running the MyFinanceMCP ASP.NET Core app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as a separate step to leverage Docker layer caching
COPY ["MyFinanceMCP.App/MyFinanceMCP.App.csproj", "MyFinanceMCP.App/"]
RUN dotnet restore "MyFinanceMCP.App/MyFinanceMCP.App.csproj"

# Copy everything else and publish
COPY . .
WORKDIR /src/MyFinanceMCP.App
RUN dotnet publish "MyFinanceMCP.App.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Use the framework-dependent deployment
ENTRYPOINT ["dotnet", "MyFinanceMCP.App.dll"]
