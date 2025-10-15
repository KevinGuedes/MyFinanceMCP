# MyFinanceMCP

This repository contains the MyFinanceMCP ASP.NET Core application. The project can be built and published with the .NET SDK, or run inside Docker for easier distribution.

## Docker

A multi-stage `Dockerfile` has been added at `MyFinanceMCP.App/Dockerfile`. The image uses the .NET 9 SDK to build and the .NET 9 ASP.NET runtime to run the app.

Build and run using Docker (PowerShell):

```powershell
# Build the image
docker build -f MyFinanceMCP.App/Dockerfile -t myfinancemcp:latest .

# Run the container (maps container port 80 to host port 5000)
docker run --rm -p 5000:80 --name myfinancemcp myfinancemcp:latest
```

Or use Docker Compose (file: `docker-compose.yml`) to build and run:

```powershell
# Build and start services
docker-compose up --build

# Stop services
docker-compose down
```

Notes:
- The Docker image is framework-dependent and runs `dotnet MyFinanceMCP.App.dll`.
- The container exposes port 80; `docker-compose.yml` maps host port 5000 -> container port 80.
