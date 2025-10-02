# Train Monitor

## CI result: [![.NET CI](https://github.com/andrei-hutsan/TrainMonitor/actions/workflows/dotnet_ci.yml/badge.svg)](https://github.com/andrei-hutsan/TrainMonitor/actions/workflows/dotnet_ci.yml)

A simulated **real-time train monitoring system** built with **ASP.NET Core MVC** and **MariaDB**.  
It displays currently running trains, highlights delays, and allows users to log incidents (e.g. reasons for delays).  
The project simulates real-time updates by periodically parsing a provided JSON file (mimicking a WebSocket stream).  

---

## Features

- **ASP.NET Core MVC frontend + backend**
- **MariaDB** database integration
- **Simulated real-time updates** (JSON refreshed every 5 seconds by a background service)
- **Train Dashboard**
  - Train name
  - Train number
  - Delay time (highlighted if > 10 minutes)
  - Next station name
  - Last update timestamp
- **Incident Logging**
  - Users can add delay reports with username, reason, and comments
  - Incidents saved to the database
  - Trains with incidents show a warning indicator
  - View incident history per train
- **Unit Tests**
  - Repository tests
  - Controller tests
- **GitHub Actions CI**
  - Build, restore, and run unit tests automatically
- **Docker**
  - Implement Docker Containerization, to run from different machines
  - [Docker image in DockerHub](https://hub.docker.com/r/andreihutsan/trainmonitor)

---

## Tech Stack

- **Backend & Web Framework**: [ASP.NET Core 8.0 MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc)
- **Database**: [MariaDB](https://mariadb.org/)
- **ORM**: [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- **Background Jobs**: `BackgroundService` (for real-time train updates)
- **Serialization**: [Newtonsoft.Json](https://www.newtonsoft.com/json)
- **Unit Testing**: 
  - [xUnit](https://xunit.net/)  
  - [EF Core InMemory provider](https://learn.microsoft.com/en-us/ef/core/testing/)  
  - [Moq](https://github.com/moq/moq4)  
- **CI/CD**: [GitHub Actions](https://docs.github.com/en/actions)
- **Docker**: [Docker](https://www.docker.com/)

---

## Project Structure
TrainMonitor/

├── Controllers/ # MVC controllers (TrainController, IncidentController)

├── Data/ # AppDbContext

├── Interfaces/ # Repository interfaces

├── Models/ # Train, Incident, JsonRoot DTOs

├── Migrations/ # Migrations (InitialCreate)

├── Repositories/ # Repositories (TrainRepository, IncidentRepository)

├── Services/ # Background updater

├── Views/ # Razor views for trains and incidents

└── wwwroot/data/ # Sample JSON (simulated WebSocket feed)

TrainMonitor.Tests/

├── ControllersTests/ # xUnit tests for controllers

└── ServicesTests/ # xUnit tests for repositories

## UI

**Trains Table with info**

<img width="1823" height="918" alt="image" src="https://github.com/user-attachments/assets/4caf44e7-b54a-4cf9-828f-dbc49236607b" />

**Report Page with field validation**

<img width="1728" height="933" alt="image" src="https://github.com/user-attachments/assets/498d8379-6d42-4a7d-bf9b-b241b8c0db3d" />

**Train details**

<img width="939" height="839" alt="image" src="https://github.com/user-attachments/assets/ab520520-1320-4585-a5ff-ea3d8218bb2f" />




