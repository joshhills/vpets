FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY VPets/VPets.csproj VPets/
RUN dotnet restore "VPets/VPets.csproj"
COPY . .
WORKDIR "/src/VPets"
RUN dotnet build "VPets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VPets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VPets.dll"]
