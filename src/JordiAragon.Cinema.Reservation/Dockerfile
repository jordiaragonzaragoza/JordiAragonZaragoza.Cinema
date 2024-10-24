#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Directory.Packages.props", "src/"]
COPY ["nuget.config", "."]
COPY ["src/JordiAragon.Cinema/JordiAragon.Cinema.csproj", "src/JordiAragon.Cinema/"]
COPY ["src/JordiAragon.Cinema.ServiceDefaults/JordiAragon.Cinema.ServiceDefaults.csproj", "src/JordiAragon.Cinema.ServiceDefaults/"]
COPY ["src/JordiAragon.Cinema.SharedKernel/JordiAragon.Cinema.SharedKernel.csproj", "src/JordiAragon.Cinema.SharedKernel/"]
COPY ["src/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.csproj", "src/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2/"]
COPY ["src/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.csproj", "src/JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1/"]
RUN dotnet restore "src/JordiAragon.Cinema.Reservation/JordiAragon.Cinema.Reservation.csproj"
COPY . .
WORKDIR "/src/src/JordiAragon.Cinema.Reservation"
RUN dotnet build "JordiAragon.Cinema.Reservation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JordiAragon.Cinema.Reservation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JordiAragon.Cinema.Reservation.dll"]
