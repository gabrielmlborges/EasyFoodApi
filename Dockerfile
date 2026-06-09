FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /repo
COPY src/EasyFood.csproj src/
RUN dotnet restore src/EasyFood.csproj
COPY . .
RUN dotnet publish src/EasyFood.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /publish .
EXPOSE 10000
ENTRYPOINT ["/bin/sh", "-c", "dotnet EasyFood.dll --urls http://+:${PORT:-10000}"]
