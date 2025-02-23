#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Directory.Packages.props", "src/"]
COPY ["nuget.config", "."]
COPY ["src/JordiAragonZaragoza.Cinema/JordiAragonZaragoza.Cinema.csproj", "src/JordiAragonZaragoza.Cinema/"]
COPY ["src/JordiAragonZaragoza.Cinema.ServiceDefaults/JordiAragonZaragoza.Cinema.ServiceDefaults.csproj", "src/JordiAragonZaragoza.Cinema.ServiceDefaults/"]
COPY ["src/JordiAragonZaragoza.Cinema.SharedKernel/JordiAragonZaragoza.Cinema.SharedKernel.csproj", "src/JordiAragonZaragoza.Cinema.SharedKernel/"]
COPY ["src/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.csproj", "src/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2/"]
COPY ["src/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.csproj", "src/JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1/"]
RUN dotnet restore "src/JordiAragonZaragoza.Cinema.Reservation/JordiAragonZaragoza.Cinema.Reservation.csproj"
COPY . .
WORKDIR "/src/src/JordiAragonZaragoza.Cinema.Reservation"
RUN dotnet build "JordiAragonZaragoza.Cinema.Reservation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JordiAragonZaragoza.Cinema.Reservation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JordiAragonZaragoza.Cinema.Reservation.dll"]
