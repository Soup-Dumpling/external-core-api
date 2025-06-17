# External API App Practice

[![Project Status: WIP – Initial development is in progress, but there has not yet been a stable, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)
![dotnet version](https://img.shields.io/badge/dotnet-8.0-green)

## TODO - in no particular order

- [x] A README (how to build, version info)
- [x] A Dockerfile
- [x] A unit test project
- [x] An integration test project
- [x] AppSettings based CORS
- [x] Hide server name
- [x] Version endpoint
- [x] Add mediator and clean architecture
- [x] Add initial db layer
- [x] Add a helm chart
- [x] Add health check
- [x] Lowercase routes

## Configuration

### Docker Compose

In order to run the project using docker compose on visual studio you must set the docker-compose project as a start up project and add a local file on root directory of the project **docker-compose.override.yml** as following:\
**Note**: Replace values within () and make sure to remove the brackets

| Variable                    | Description                                                                |
| --------------------------- | -------------------------------------------------------------------------- |
| SA Password                 | Password for SQL Server Image                                              |
| Volume Bind Source          | Location on your computer to bind sql databases to e.g ~/Documents/SqlData |
| Volume Bind Source RabbitMQ | Location on your computer to bind rabbit data to e.g ~/Documents/RabbitMQ  |
| UAM_SECRET                  | Ask a member of the team for this token                                    |

#### Docker Compose Override (Windows)

```
version: '3.4'

services:
  sqldata:
    environment:
      - SA_PASSWORD=(SA Password)
      - MSSQL_PID=Developer
      - ACCEPT_EULA=Y
    volumes:
      - type: bind
        source: (Volume Bind Source)
        target: /var/opt/mssql/data
  rabbitmq:
    volumes:
        - (Volume Bind Source RabbitMQ)\data:/var/lib/rabbitmq/
        - (Volume Bind Source RabbitMQ)\log:/var/log/rabbitmq
  employee.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Employees;User Id=sa;Password=(SA Password)
      - External__Uam__ClientSecret=(UAM_SECRET)
  project.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Projects;User Id=sa;Password=(SA Password)
  notification.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Notifications;User Id=sa;Password=(SA Password)
  celebratingSuccess.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_CelebratingSuccess;User Id=sa;Password=(SA Password)
  assetManagement.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_AssetManagement;User Id=sa;Password=(SA Password)
  careersPortal.api:
    volumes:
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_CareersPortal;User Id=sa;Password=(SA Password)
      - External__Uam__ClientSecret=(UAM_SECRET)
```

#### Docker Compose Override (Mac)

```
version: '3.4'

services:
  sqldata:
    environment:
      - SA_PASSWORD=(SA Password)
      - MSSQL_PID=Developer
      - ACCEPT_EULA=Y
    volumes:
      - type: bind
        source: (Volume Bind Source)
        target: /var/opt/mssql/data
  rabbitmq:
    volumes:
        - (Volume Bind Source RabbitMQ)/data:/var/lib/rabbitmq/
        - (Volume Bind Source RabbitMQ)/log:/var/log/rabbitmq
  employee.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Employees;User Id=sa;Password=(SA Password)
      - External__Uam__ClientSecret=(UAM_SECRET)
  project.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Projects;User Id=sa;Password=(SA Password)
  notification.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_Notifications;User Id=sa;Password=(SA Password)
  celebratingSuccess.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_CelebratingSuccess;User Id=sa;Password=(SA Password)
  assetManagement.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_AssetManagement;User Id=sa;Password=(SA Password)
  careersPortal.api:
    volumes:
      - ${HOME}/.aspnet/https:/root/.aspnet/https:ro
      - ${HOME}/.microsoft/usersecrets:/root/.microsoft/usersecrets:ro
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqldata;Database=Beacon_CareersPortal;User Id=sa;Password=(SA Password)
      - External__Uam__ClientSecret=(UAM_SECRET)
```

### Visual Studio for Mac (Without Docker Compose)

#### Docker Commands (Old Mac => Intel)

In a terminal, run the following docker commands to initialise the containers:

#### SQL Server

```
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=(SA Password)' -e 'MSSQL_PID=Developer' -p 1433:1433 --name Beacon_App_SQLServer -v (Volume Bind Source SQLData):/var/opt/mssql/data -d mcr.microsoft.com/mssql/server:2019-latest
```

#### RabbitMQ

```
docker run -p 5672:5672 -p 15672:15672 --name rabbitmq -v (Volume Bind Source RabbitMQ)/data:/var/lib/rabbitmq/ -v (Volume Bind Source RabbitMQ)/log:/var/log/rabbitmq -d rabbitmq:3-management-alpine
```

#### Docker Commands (New Mac => M1 Chip)

In a terminal, run the following docker commands to initialise the containers:

#### SQL Server

```
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=(SA Password)' -e 'MSSQL_PID=Developer' -p 1433:1433 --name Beacon_App_SQLServer -v (Volume Bind Source SQLData):/var/opt/mssql/data -d mcr.microsoft.com/azure-sql-edge
```

#### RabbitMQ

```
docker run -p 5672:5672 -p 15672:15672 --name rabbitmq -v (Volume Bind Source RabbitMQ)/data:/var/lib/rabbitmq/ -v (Volume Bind Source RabbitMQ)/log:/var/log/rabbitmq -d rabbitmq
```

#### Multiple Projects

In order to run the project without using docker compose on Visual Studio you must set the multiple projects as a startup project. Click the Project tab and click Solution Options. This will open up a window, click on Configurations -> New -> call the configuration 'Multiple Projects'. Click on this newly created configuration and tick the following and press OK:

- [x] Employee.Api
- [x] Project.Api
- [x] Notification.Api
- [x] CelebratingSuccess.Api
- [x] AssetManagement.Api
- [x] CareersPortal.Api

#### Environment Variables

On Visual Studio right click on Employee.Api / Project.Api / Notification.Api / CelebratingSuccess.Api / AssetManagement.Api / CareersPortal.Api and click Options. This will open up a window, click on Default under Configurations. Set the following environment variables inside the Employee.Api / Project.Api / Notification.Api / CelebratingSuccess.Api / AssetManagement.Api / CareersPortal.Api Options settings:\
**Note**: Replace values within () and make sure to remove the brackets

#### Employee.Api Environment Variables

| Variable                               | Value                                                                        |
| -------------------------------------- | ---------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_Employees;User Id=sa;Password=(SA Password) |
| External\_\_Uam\_\_ClientSecret        | (UAM_SECRET)                                                                 |

#### Project.Api Environment Variables

| Variable                               | Value                                                                       |
| -------------------------------------- | --------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_Projects;User Id=sa;Password=(SA Password) |

#### Notification.Api Environment Variables

| Variable                               | Value                                                                            |
| -------------------------------------- | -------------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_Notifications;User Id=sa;Password=(SA Password) |

#### CelebratingSuccess.Api Environment Variables

| Variable                               | Value                                                                                 |
| -------------------------------------- | ------------------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_CelebratingSuccess;User Id=sa;Password=(SA Password) |

#### AssetManagement.Api Environment Variables

| Variable                               | Value                                                                              |
| -------------------------------------- | ---------------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_AssetManagement;User Id=sa;Password=(SA Password) |

#### CareersPortal.Api Environment Variables

| Variable                               | Value                                                                            |
| -------------------------------------- | -------------------------------------------------------------------------------- |
| ConnectionStrings\_\_DefaultConnection | Server=localhost;Database=Beacon_CareersPortal;User Id=sa;Password=(SA Password) |
| External\_\_Uam\_\_ClientSecret        | (UAM_SECRET)                                                                     |

#### Application URL

Click on ASP.NET Core and enter in the following to the App URL field:\
For the Employee.Api Configurations - https://localhost:5001
For the Project.Api Configurations - https://localhost:5002
For the Notification.Api Configurations - https://localhost:5003
For the CelebratingSuccess.Api Configurations - https://localhost:5004
For the AssetManagement.Api Configurations - https://localhost:5005
For the CareersPortal.Api Configurations - https://localhost:5006

### Cors

Set allowed origins as per below as a comma separated list

```
"Cors": {
    "AllowedOrigins": "http://allowedorigin1,http://allowedorigin2"
}
```

### Frameworks / Packages Used

| Framework / Package | Purpose                              |
| ------------------- | ------------------------------------ |
| XUnit               | Unit Testing                         |
| NSubstitute         | Fakes/Mocks/Substitutes              |
| Alba                | Integration Testing                  |
| Swashbuckle         | OpenAPI doc                          |
| Mediatr             | Mediator + Request/Response Handling |
| FluentValidation    | Validation                           |

### Core Endpoints

| Endpoint     | Purpose             |
| ------------ | ------------------- |
| /api/health  | Healthy / Unhealthy |
| /api/version | Version             |

### Environments - Employee

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5001 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### Environments - Project

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5002 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### Environments - Notification

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5003 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### Environments - CelebratingSuccess

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5004 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### Environments - AssetManagement

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5005 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### Environments - CareersPortal

| Environment | Url                    |
| ----------- | ---------------------- |
| Local       | https://localhost:5006 |
| Dev         | https://some-url-dev/  |
| Uat         | https://some-url-uat/  |
| Prod        | https://some-url-prod/ |

### About each project in solution

| Project                                   | Description                                                                  |
| ----------------------------------------- | ---------------------------------------------------------------------------- |
| Employee.Api                              | Create your endpoints related to employee service here                       |
| Employee.Core                             | Core of the employee service - including use cases, domain and context       |
| Employee.Infrastructure                   | Where all the implementation of employee service, repos, providers go        |
| Project.Api                               | Create your endpoints related to project service here                        |
| Project.Core                              | Core of the project service - including use cases, domain and context        |
| Project.Infrastructure                    | Where all the implementation of project service, repos, providers go         |
| Notification.Api                          | Create your endpoints related to notification service here                   |
| Notification.Core                         | Core of the notification service - including use cases, domain and context   |
| Notification.Infrastructure               | Where all the implementation of notification service, repos, providers go    |
| CelebratingSuccess.Api                    | Create your endpoints related to celebrating success service here            |
| CelebratingSuccess.Core                   | Core of celebrating success service - use cases, domain and context          |
| CelebratingSuccess.Infrastructure         | Where implementation of celebrating success service, repos,providers go      |
| AssetManagement.Api                       | Create your endpoints related to asset management service here               |
| AssetManagement.Core                      | Core of asset management service - use cases, domain and context             |
| AssetManagement.Infrastructure            | Where implementation of asset management service, repos,providers go         |
| CareersPortal.Api                         | Create your endpoints related to careers portal service here                 |
| CareersPortal.Core                        | Core of the careers portal service - including use cases, domain and context |
| CareersPortal.Infrastructure              | Where all the implementation of careers portal service, repos, providers go  |
| Beacon.Integration.MessageBroker.Core     | Abstraction layer for message broker                                         |
| Beacon.Integration.MessageBroker.RabbitMq | Implementation of Rabbit MQ message broker                                   |
| Beacon.Integration.Uam                    | Implementation of UAM identity provider                                      |

### Code Coverage

Collect test coverage metrics

```
dotnet test --collect:"XPlat Code Coverage"
```

```
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Generate report

```
reportgenerator "-reports:tests/**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```
