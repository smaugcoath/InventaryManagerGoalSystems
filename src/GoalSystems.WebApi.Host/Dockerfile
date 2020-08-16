#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GoalSystems.WebApi.Host/GoalSystems.WebApi.Host.csproj", "GoalSystems.WebApi.Host/"]
RUN dotnet restore "GoalSystems.WebApi.Host/GoalSystems.WebApi.Host.csproj"
COPY . .
WORKDIR "/src/GoalSystems.WebApi.Host"
RUN dotnet build "GoalSystems.WebApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoalSystems.WebApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoalSystems.WebApi.Host.dll"]