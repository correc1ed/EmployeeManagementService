# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY *.sln .
COPY EmployeeManagementService.Application/*.csproj ./EmployeeManagementService.Application/
COPY EmployeeManagementService.Domain/*.csproj ./EmployeeManagementService.Domain/
COPY EmployeeManagementService.Infrastructure/*.csproj ./EmployeeManagementService.Infrastructure/
COPY EmployeeManagementService.API/*.csproj ./EmployeeManagementService.API/

# Restore packages
RUN dotnet restore

# Copy everything
COPY . .

# Build and publish API project
WORKDIR /src/EmployeeManagementService.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagementService.API.dll"]