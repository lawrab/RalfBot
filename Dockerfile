# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
RUN dotnet nuget add source https://nuget.emzi0767.com/api/v3/index.json -n Slimget

# copy and publish app and libraries
COPY SnailRacing.Ralf/ SnailRacing.Ralf/
COPY SnailRacing.Store/ SnailRacing.Store/

RUN dotnet restore SnailRacing.Ralf/
RUN dotnet restore SnailRacing.Store/
RUN dotnet publish SnailRacing.Ralf/ -c release -o /app --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app .

RUN apt-get update -qq && apt-get -y install wkhtmltopdf
ENTRYPOINT ["dotnet", "SnailRacing.Ralf.dll"]
