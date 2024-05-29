FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Task2/Task2.csproj", "Task2/"]
RUN dotnet restore "Task2/Task2.csproj"
COPY . .
WORKDIR "/src/Task2"
RUN dotnet build "Task2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Task2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task2.dll"]
