<p align="center">
<h1 align="center">BlogCore.</h1>
<h3 align="center"><strong>Powerful .NET 8 Blog API</strong></h3>
<p align="center">Create your own custom front-end or explore a complete blog API.</p>

<p align="center">
  <a href="https://github.com/VianneyDoleans/BlogCore/actions/workflows/dotnet.yml">
   <img src="https://github.com/VianneyDoleans/BlogCore/actions/workflows/dotnet.yml/badge.svg?branch=master" alt="build">
  </a>
    <a href="https://codecov.io/gh/VianneyDoleans/BlogCore">
   <img src="https://codecov.io/gh/VianneyDoleans/BlogCore/branch/master/graph/badge.svg" alt="codecov">
  </a>
    <a href="https://github.com/VianneyDoleans/BlogCore/blob/master/LICENSE">
   <img src="https://img.shields.io/badge/License-GPLv3-blue.svg" alt="License: GPL v3">
  </a>
    <a href="https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore">
   <img src="https://sonarcloud.io/api/project_badges/measure?project=VianneyDoleans_BlogCore&metric=security_rating" alt="Security Rating">
  </a>
    <a href="https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore">
   <img src="https://sonarcloud.io/api/project_badges/measure?project=VianneyDoleans_BlogCore&metric=sqale_rating" alt="Maintainability Rating">
  </a>
    <a href="https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore">
   <img src="https://sonarcloud.io/api/project_badges/measure?project=VianneyDoleans_BlogCore&metric=reliability_rating" alt="Reliability Rating">
  </a>  
    <a href="https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore">
   <img src="https://sonarcloud.io/api/project_badges/measure?project=VianneyDoleans_BlogCore&metric=vulnerabilities" alt="Vulnerabilities">
  </a>
</p>

Live demo : https://blogcore.fly.dev/swagger

## Powerful features

 - **Advanced search** by combining criteria (filters) on all resources
 - **Advanced user permissions management** by creating roles and permissions for users via the API at runtime (combination of **Permission-Based Authorization** & **Resource-based Authorization**)
   - ex: create a role "mini-moderator" with the permissions CanEdit own Posts, CanDelete All Comments, etc.
   - Default role(s) given to new users can also be defined via the API.
 - **Complete community features** (create comments on a post, like a comment or a post, add a comment on a comment, consult profiles, etc.)
 - **Configurable log system** that logs all user actions / events (implementation of [Serilog library](https://serilog.net/), can be connected to Kibana, Seq and other solutions, for monitoring)
 - **CRUD** is provided on all resources. Everything can be manipulated via endpoints, giving the possibility out-of-the-box to develop an admin interface or a mobile app. (current Swagger interface can act as an administrator interface)
 - **Pagination** strategy has been implemented.
 - **OAuth2.0 standard** (access / refresh token) has been implemented for authorization.
 - **Email SMTP configuration** : Email confirmation & password reset are provided by sending emails (can use Gmail SMTP server for example)

## Clean code

- Good code coverage
- The Project was developed by relying on guidelines from **DDD** (Domain Driven Design) and **Clean Code Book** by Robert C. Martin.
- [SonarCloud](https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore) used to ensure code quality
- Implementation of **End-to-End Testing** to ensure the functioning of Blog Core API endpoints.
- Utilization of [Resharper](https://www.jetbrains.com/fr-fr/resharper/) and [SonarLint](https://www.sonarsource.com/products/sonarlint/) during the development process to enhance code quality and consistency.
- The project follows [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) and [Git Feature Workflow with Develop Branch](https://rovitpm.com/5-git-workflows-to-improve-development/), ensuring a clean and organized **git history** with meaningful and clear **commit messages**.
- **Code Quality** is one of the main focuses on this project

## Compatibilities

Compatible with Linux / Windows / MacOS and can be deployed with [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [PostgreSQL](https://www.postgresql.org/) database.  
[Docker](https://www.docker.com/) is also available (Dockerfile at the root of the project).


## Technologies

 - [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
 - [EntityFramework](https://learn.microsoft.com/en-us/ef/)
 - [Microsoft Dependency Injection (DI)](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
 - [FluentValidation](https://github.com/FluentValidation/FluentValidation)
 - [Serilog](https://serilog.net/)
 - [Swashbuckle (Swagger)](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio)
 - [xUnit](https://xunit.net/)
 - [Moq](https://github.com/moq/moq4)
 - [AutoMapper](https://automapper.org/)

## Getting Started

### Prerequisites

- Install [Microsoft SQL Server](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads) or [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio](https://visualstudio.microsoft.com/fr/) (or [Rider](https://www.jetbrains.com/rider/) with [Entity Framework Core UI Plugin](https://plugins.jetbrains.com/plugin/18147-entity-framework-core-ui))

### Installation

- Open ``appsettings.json`` file inside **BlogCoreAPI** project and edit the database settings :

For **Microsoft SQL Server** :

```json
"DatabaseProvider": "MsSQL",
"ConnectionStrings": {
 "Default": "Server=.;Database=BlogCore;Integrated Security=True;TrustServerCertificate=True;"
}
```

For **PostgreSQL** :

```json
"DatabaseProvider": "PostgreSQL",
"ConnectionStrings": {
 "Default": "Host=localhost;Port=5432;Database=BlogCore;Username=postgres;Password=[YourPassword];"
}
```

Then in visual studio :

1. Set **BlogCoreApi** as project to run
1. Open the Package Manager Console (Tools -> Nuget Package Manager -> Package Manager Console).
2. In the package Manager Console, select **DBAccess** as Default project
3. Run the following commands:

- **Microsoft SQL Server**
```
Add-Migration CreateInitialDatabase -Context MsSqlDbContext
Update-Database -Context MsSqlDbContext
```
- **PostgreSQL**
```
Add-Migration CreateInitialDatabase -Context PostgreSqlDbContext
Update-Database -Context PostgreSqlDbContext
```
5. Now press F5 and run the application.
6. Refresh the page. The API was busy filling the default data in the database. Now it can respond.

## Default Users, Roles and Permissions

### Users
Default users are :

| User        | Password         | Role(s)        |
| ----------- |:----------------:| :--------------|
| Sam         | 0a1234A@         | User           |
| Frodon      | 0a0000A@         | User           |
| Jamy        | 0JamyRedactA@    | User, Redactor |
| Fred        | 0FredRedactA@    | User, Redactor |
| AdminUser   | 0adminPasswordA@ | User, Admin    |

### Defalt Roles and Permissions

Roles and permissions can be created / configured by API endpoints.  
The default configuration is :

**User** :

|            | CanRead | CanCreate | CanUpdate | CanDelete  |
| -----------|:-------:| :--------:|:---------:|:----------:|
| Category   | All     |           |           |            |
| Comment    | All     | Own       | Own       |   Own      |
| Like       | All     | Own       | Own       |   Own      |
| Post       | All     |           |           |            |
| Tag        | All     |           |           |            |
| User       | All     |    X      |   X       |     X      |
| Role       | All     |           |           |            |
| Permission | All     |           |   X       |            |
| Account    | Own     |           | Own       | Own        |

**Redactor** :

|            | CanRead | CanCreate | CanUpdate | CanDelete  |
| -----------|:-------:| :--------:|:---------:|:----------:|
| Category   |         |    All    |           |            |
| Comment    |         |           |           |            |
| Like       |         |           |           |            |
| Post       |         |    Own    |  Own      |   Own      |
| Tag        |         |    All    |           |            |
| User       |         |      X    |    X      |   X        |
| Role       |         |           |           |            |
| Permission |         |           |    X      |            |
| Account    |         |           |           |            |

**Admin** :

|            | CanRead | CanCreate | CanUpdate | CanDelete  |
| -----------|:-------:| :--------:|:---------:|:----------:|
| Category   |   All   |    All    |  All      | All        |
| Comment    |   All   |    All    |  All      | All        |
| Like       |   All   |    All    |  All      | All        |
| Post       |   All   |    All    |  All      | All        |
| Tag        |   All   |    All    |  All      | All        |
| User       |   All   |     X     |   X       | X          |
| Role       |   All   |    All    |  All      | All        |
| Permission |   All   |    All    |  X        | All        |
| Account    |   All   |    All    |  All      | All        |

## Wiki

A wiki for this project is available on github : [link](https://github.com/VianneyDoleans/BlogCore/wiki)  
The wiki give more explanations about configuration, online deployment ([render](https://github.com/VianneyDoleans/BlogCore/wiki/HostingOnRender), [heroku](https://github.com/VianneyDoleans/BlogCore/wiki/HostingOnHeroku)) and architecture of the project.
