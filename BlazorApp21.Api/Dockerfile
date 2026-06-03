FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore BlazorApp21.Api.csproj --no-cache
RUN dotnet publish BlazorApp21.Api.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BlazorApp21.Api.dll"]
