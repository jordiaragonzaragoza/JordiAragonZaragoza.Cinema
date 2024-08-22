#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Directory.Packages.props", "src/"]
COPY ["../JordiAragon.SharedKernel/src/Directory.Packages.props", "../JordiAragon.SharedKernel/src/"]
COPY ["nuget.config", "."]
COPY ["../JordiAragon.SharedKernel/nuget.config", "../JordiAragon.SharedKernel/"]
COPY ["src/JordiAragon.Cinema.Reservation/JordiAragon.Cinema.Reservation.csproj", "src/JordiAragon.Cinema.Reservation/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Domain/JordiAragon.SharedKernel.Domain.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Domain/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel/JordiAragon.SharedKernel.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Contracts/JordiAragon.SharedKernel.Contracts.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Contracts/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Domain.Contracts/JordiAragon.SharedKernel.Domain.Contracts.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Domain.Contracts/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application/JordiAragon.SharedKernel.Application.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application.Contracts/JordiAragon.SharedKernel.Application.Contracts.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application.Contracts/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages/JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure/JordiAragon.SharedKernel.Infrastructure.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure.EntityFramework/JordiAragon.SharedKernel.Infrastructure.EntityFramework.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure.EntityFramework/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure.EventStore/JordiAragon.SharedKernel.Infrastructure.EventStore.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Infrastructure.EventStore/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Presentation.HttpRestfulApi/JordiAragon.SharedKernel.Presentation.HttpRestfulApi.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Presentation.HttpRestfulApi/"]
COPY ["../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts/JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts.csproj", "../JordiAragon.SharedKernel/src/JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts/"]
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
