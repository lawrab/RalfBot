# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
RUN dotnet nuget add source https://nuget.emzi0767.com/api/v3/index.json -n Slimget

# copy and publish app and libraries
COPY SnailRacing.Ralf/ SnailRacing.Ralf/
COPY SnailRacing.Store/ SnailRacing.Store/

RUN dotnet restore SnailRacing.Ralf/ -r linux-musl-x64
RUN dotnet restore SnailRacing.Store/ -r linux-musl-x64
RUN dotnet publish SnailRacing.Ralf/ -c release -o /app -r linux-musl-x64 --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .
RUN apk add --no-cache bash
RUN apk --no-cache update && apk add --no-cache wkhtmltopdf
ENV PATH="/usr/bin:${PATH}"
ENTRYPOINT ["dotnet", "SnailRacing.Ralf.dll"]
