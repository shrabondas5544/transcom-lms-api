# Transcom LMS API

[![Development Status](https://img.shields.io/badge/status-active-success.svg)](#)

ASP.NET Core Web API backend for the Transcom Learning Management System (LMS).

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Local Setup

1. Copy the example configuration file to create your local environment configuration:
   ```bash
   cp appsettings.Example.json appsettings.Development.json
   ```
2. Open `appsettings.Development.json` and customize the connection strings, JWT keys, or other configurations.
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Build the application:
   ```bash
   dotnet build
   ```
5. Run the application:
   ```bash
   dotnet run
   ```