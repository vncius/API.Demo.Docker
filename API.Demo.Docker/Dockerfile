#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API.Demo.Docker/API.Demo.Docker.csproj", "API.Demo.Docker/"]
RUN dotnet restore "API.Demo.Docker/API.Demo.Docker.csproj"
COPY . .
WORKDIR "/src/API.Demo.Docker"
RUN dotnet build "API.Demo.Docker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Demo.Docker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Demo.Docker.dll"]