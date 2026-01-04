#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Com.ByteAnalysis.IFacilita.PushNotification.API/Com.ByteAnalysis.IFacilita.PushNotification.API.csproj", "Com.ByteAnalysis.IFacilita.PushNotification.API/"]
RUN dotnet restore "Com.ByteAnalysis.IFacilita.PushNotification.API/Com.ByteAnalysis.IFacilita.PushNotification.API.csproj"
COPY . .
WORKDIR "/src/Com.ByteAnalysis.IFacilita.PushNotification.API"
RUN dotnet build "Com.ByteAnalysis.IFacilita.PushNotification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Com.ByteAnalysis.IFacilita.PushNotification.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.PushNotification.API.dll"]