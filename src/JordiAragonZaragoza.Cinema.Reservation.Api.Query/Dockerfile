# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/Directory.Build.props", "src/"]
COPY ["nuget.config", "."]
COPY ["src/JordiAragonZaragoza.Cinema.Reservation.Api.Query/JordiAragonZaragoza.Cinema.Reservation.Api.Query.csproj", "src/JordiAragonZaragoza.Cinema.Reservation.Api.Query/"]
COPY ["src/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.csproj", "src/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1/"]
COPY ["src/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.csproj", "src/JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2/"]
COPY ["src/JordiAragonZaragoza.Cinema.ServiceDefaults/JordiAragonZaragoza.Cinema.ServiceDefaults.csproj", "src/JordiAragonZaragoza.Cinema.ServiceDefaults/"]
COPY ["src/JordiAragonZaragoza.Cinema.SharedKernel/JordiAragonZaragoza.Cinema.SharedKernel.csproj", "src/JordiAragonZaragoza.Cinema.SharedKernel/"]
COPY ["src/JordiAragonZaragoza.Cinema/JordiAragonZaragoza.Cinema.csproj", "src/JordiAragonZaragoza.Cinema/"]
RUN dotnet restore "./src/JordiAragonZaragoza.Cinema.Reservation.Api.Query/JordiAragonZaragoza.Cinema.Reservation.Api.Query.csproj"
COPY . .
WORKDIR "/src/src/JordiAragonZaragoza.Cinema.Reservation.Api.Query"
RUN dotnet build "./JordiAragonZaragoza.Cinema.Reservation.Api.Query.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JordiAragonZaragoza.Cinema.Reservation.Api.Query.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/publish \
    /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JordiAragonZaragoza.Cinema.Reservation.Api.Query.dll"]