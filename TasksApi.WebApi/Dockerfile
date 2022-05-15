#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TasksApi.WebApi/TasksApi.WebApi.csproj", "TasksApi.WebApi/"]
COPY ["TasksApi.Application/TasksApi.Application.csproj", "TasksApi.Application/"]
COPY ["TasksApi.Domain/TasksApi.Domain.csproj", "TasksApi.Domain/"]
COPY ["TasksApi.Data/TasksApi.Data.csproj", "TasksApi.Data/"]
RUN dotnet restore "TasksApi.WebApi/TasksApi.WebApi.csproj"
COPY . .
WORKDIR "/src/TasksApi.WebApi"
RUN dotnet build "TasksApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TasksApi.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TasksApi.WebApi.dll"]