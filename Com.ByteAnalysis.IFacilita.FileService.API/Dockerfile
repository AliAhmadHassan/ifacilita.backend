#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Com.ByteAnalysis.IFacilita.FileService.API/Com.ByteAnalysis.IFacilita.FileService.API.csproj", "Com.ByteAnalysis.IFacilita.FileService.API/"]
RUN dotnet restore "Com.ByteAnalysis.IFacilita.FileService.API/Com.ByteAnalysis.IFacilita.FileService.API.csproj"
COPY . .
WORKDIR "/src/Com.ByteAnalysis.IFacilita.FileService.API"
RUN dotnet build "Com.ByteAnalysis.IFacilita.FileService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Com.ByteAnalysis.IFacilita.FileService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.FileService.API.dll"]