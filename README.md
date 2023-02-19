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

Live demo : https://mydevblogapi.herokuapp.com/swagger/

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
 - Solution contains also End-to-End Unit Testing (ensure that Blog Core API's endpoints work)
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


## Getting Started

Tutorial available in [Getting Started](https://github.com/VianneyDoleans/BlogCore/wiki/GettingStarted) wiki section.
