FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY [".editorconfig", "Directory.Build.*", "nuget.config", "."]
COPY ["build/msbuild/", "build/msbuild/"]
COPY ["source/", "source/"]
WORKDIR /src/source/Api
RUN dotnet publish Company.Product.WebApi.Api.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Product.WebApi.Api.dll"]
