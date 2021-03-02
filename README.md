<h1 align="center">MyBlog</h4>

<p align="center">
</p>

<p align="center">
  <a href="#presentation">Presentation</a> •
  <a href="#technologies">Technologies</a> •
  <a href="#architecture">Architecture</a> •
    <a href="#api">API</a>
</p>


## Presentation

MyBlog project is a blog that I develop with the aim of sharing my knowledge, experiences, make readers discover things,  
but most importantly, make readers think on some subjects...  

Although my personal domain is IT, this blog is not only intended for IT readers.  
 I plan to make some posts about positive thinking, productivity, methodology, etc.

## Technologies

#### API
 - [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
 - [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
 - [Automapper](https://automapper.org/)
 - [Entity Framework Core](https://docs.microsoft.com/fr-fr/ef/core/)
 - [XUnit](https://xunit.net/)
 - [Swagger](https://swagger.io/)

## Architecture

- The project is composed of :
  - **A main API** (In progress)
  - **An authentication API** (Todo)
  - **A Progressive Web App** (PWA) (Todo)
  - **2 SQL databases** (One for the **main API** and one for the **Auth API**)

## API

- The main API have the purpose to encapsulate the business logic of the blog and restrict the access on the database.
- MyBlog project have an API because it is plan that the platform will have a mobile App for android and IOS
- Authentication part of the website is realized on another API/Database for maintainability, scalability and security purpose
  -   Security
      - If a hacker gains access to main API, he still doesn't have access to email/password of the users (Auth API)
      - In this scenario, even if the hacker corrupt the main database (so the one without credentials), I will just have to restore it with Back-Up database
  - Scalability
      - Because the credential part is separate form the rest of the API, I can decide to take a bigger machine for Auth API or Main API, depending of the necessity
  - Maintainability
    - One of the two API can be reworked  without modifying the other one
