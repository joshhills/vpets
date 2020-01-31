FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY *.sln .
COPY VPets/VPets.csproj VPets/
COPY VPetsUnitTests/VPetsUnitTests.csproj VPetsUnitTests/
COPY VPetsIntegrationTests/VPetsIntegrationTests.csproj VPetsIntegrationTests/
RUN dotnet restore
COPY . .

FROM build as testing
WORKDIR /src/VPets
run dotnet build
WORKDIR /src/VPetsUnitTests
run dotnet test
WORKDIR /src/VPetsIntegrationTests
run dotnet test

FROM build AS publish
WORKDIR /src/VPets
RUN dotnet publish -c Release -o /src/publish

FROM base AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet VPets.dll