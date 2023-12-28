FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build
# ENV ASPNETCORE_URLS http://*:5000

WORKDIR /app

# copy sln and csproj files into the image
COPY *.sln ./
COPY BlogCoreAPI/*.csproj ./BlogCoreAPI/
COPY BlogCoreAPI.FunctionalTests/*.csproj ./BlogCoreAPI.FunctionalTests/
COPY BlogCoreAPI.Tests/*.csproj ./BlogCoreAPI.Tests/
COPY DBAccess/*csproj ./DBAccess/
COPY DBAccess.Tests/*csproj ./DBAccess.Tests/
RUN dotnet restore

COPY . ./
RUN dotnet build

FROM build AS publish
WORKDIR /app/BlogCoreAPI
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.18 AS runtime
WORKDIR /app

# Requred to make code works on 7.0-alpine3.16. See https://docs.microsoft.com/en-us/answers/questions/728280/running-net-6-project-in-docker-throws-globalizati.html
RUN apk add --no-cache icu-libs

COPY --from=publish /app/BlogCoreAPI/out ./
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "BlogCoreAPI.dll"]
