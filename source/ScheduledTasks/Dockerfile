FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY [".editorconfig", "Directory.Build.*", "nuget.config", "."]
COPY ["build/msbuild/", "build/msbuild/"]
COPY ["source/", "source/"]
WORKDIR /src/source/ScheduledTasks
RUN dotnet publish "Company.Product.WebApi.ScheduledTasks.csproj" --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Product.WebApi.ScheduledTasks.dll"]
