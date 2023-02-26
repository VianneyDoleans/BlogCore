<p align="center">
<h1 align="center">BlogCore.</h1>
<p align="center">Powerful .NET 7 Blog API</p>

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

Live demo : https://blogcoredemo.onrender.com/swagger

## Powerful features

 - **Advanced search** by combining filters on all resources (user, post, comment, etc.)
 - **Advanced user permissions management** by creating roles and permissions for users (CanEdit own Posts, CanDelete All Comments, etc.)
 - **Community features** (create comments on a post, like a comment or a post, add a comment on a comment, consult profiles, etc.)
 - **Configurable log system** (implementation of [Serilog library](https://serilog.net/))

## Compatibilities

Compatible with Linux / Windows / MacOS and can be deployed with [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [PostgreSQL](https://www.postgresql.org/) database. Can also be [deployed on Heroku](https://github.com/VianneyDoleans/BlogCore/wiki/HostingOnHeroku).

## Clean code

 - Follow guidelines from DDD (Domain Driven Design).
 - Good code coverage
 - [SonarCloud](https://sonarcloud.io/summary/overall?id=VianneyDoleans_BlogCore) used to ensure code quality
 - Solution also contains End-to-End Testing (ensure that Blog Core API's endpoints work)
 - [Resharper](https://www.jetbrains.com/fr-fr/resharper/) and [SonarLint](https://www.sonarsource.com/products/sonarlint/) are used during the development of this project

## Technologies

 - [.NET 7](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-7)
 - [EntityFramework](https://learn.microsoft.com/en-us/ef/)
 - [Microsoft Dependency Injection (DI)](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
 - [FluentValidation](https://github.com/FluentValidation/FluentValidation)
 - [Serilog](https://serilog.net/)
 - [Swashbuckle (Swagger)](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio)
 - [xUnit](https://xunit.net/)
 - [Moq](https://github.com/moq/moq4)
 - [AutoMapper](https://automapper.org/)
 - [Docker](https://www.docker.com/)


## Getting Started

- Install [Microsoft SQL Server](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads) or [PostgreSQL](https://www.postgresql.org/download/)
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

### Users
Default users are :

| User        | Password         | Role(s)        |
| ----------- |:----------------:| :--------------|
| Sam         | 0a1234A@         | User           |
| Frodon      | 0a0000A@         | User           |
| Jamy        | 0JamyRedactA@    | User, Redactor |
| Fred        | 0FredRedactA@    | User, Redactor |
| AdminUser   | 0adminPasswordA@ | User, Admin    |

### Roles & Permissions
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

## wiki

A wiki for this project is available on github : [link](https://github.com/VianneyDoleans/BlogCore/wiki)  
The wiki give more explanations about configuration, online deployment ([render](https://render.com/), [heroku](https://www.heroku.com/)) and architecture of the project.
