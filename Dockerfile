
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Test

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY Com.ByteAnalysis.IFacilita.Core.sln .
COPY ["Com.ByteAnalysis.IFacilita.Chat.API/*.csproj", "./Com.ByteAnalysis.IFacilita.Chat.API/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Service/*.csproj", "./Com.ByteAnalysis.IFacilita.Chat.Service/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Repository/*.csproj", "./Com.ByteAnalysis.IFacilita.Chat.Repository/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Model/*.csproj", "./Com.ByteAnalysis.IFacilita.Chat.Model/"]
COPY ["Com.ByteAnalysis.IFacilita.Common/*.csproj", "./Com.ByteAnalysis.IFacilita.Common/"]

RUN dotnet restore "Com.ByteAnalysis.IFacilita.Chat.API/Com.ByteAnalysis.IFacilita.Chat.API.csproj"

COPY Com.ByteAnalysis.IFacilita.Core.sln .
COPY ["Com.ByteAnalysis.IFacilita.Chat.API/*", "./Com.ByteAnalysis.IFacilita.Chat.API/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Service/*", "./Com.ByteAnalysis.IFacilita.Chat.Service/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Repository/*", "./Com.ByteAnalysis.IFacilita.Chat.Repository/"]
COPY ["Com.ByteAnalysis.IFacilita.Chat.Model/*", "./Com.ByteAnalysis.IFacilita.Chat.Model/"]
COPY ["Com.ByteAnalysis.IFacilita.Common/*", "./Com.ByteAnalysis.IFacilita.Common/"]

WORKDIR "/src/Com.ByteAnalysis.IFacilita.Chat.API"
RUN dotnet build "Com.ByteAnalysis.IFacilita.Chat.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Com.ByteAnalysis.IFacilita.Chat.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.Chat.API.dll"]

#iFacilita-CORE
#FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
#WORKDIR /app
#EXPOSE 80
#ENV ASPNETCORE_ENVIRONMENT=Test
#
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY Com.ByteAnalysis.IFacilita.Core.sln .
#COPY Com.ByteAnalysis.IFacilita.Core.API/*.csproj ./Com.ByteAnalysis.IFacilita.Core.API/
#COPY Com.ByteAnalysis.IFacilita.Core.Entity/*.csproj Com.ByteAnalysis.IFacilita.Core.Entity/
#COPY Com.ByteAnalysis.IFacilita.Core.Service/*.csproj Com.ByteAnalysis.IFacilita.Core.Service/
#COPY Com.ByteAnalysis.IFacilita.Core.Repository/*.csproj Com.ByteAnalysis.IFacilita.Core.Repository/
#COPY Com.ByteAnalysis.IFacilita.Common/*.csproj Com.ByteAnalysis.IFacilita.Common/
#
#RUN dotnet restore "Com.ByteAnalysis.IFacilita.Core.API/Com.ByteAnalysis.IFacilita.Core.Api.csproj"
#
#COPY Com.ByteAnalysis.IFacilita.Core.sln .
#COPY Com.ByteAnalysis.IFacilita.Core.API/* ./Com.ByteAnalysis.IFacilita.Core.API/
#COPY Com.ByteAnalysis.IFacilita.Core.Entity/* Com.ByteAnalysis.IFacilita.Core.Entity/
#COPY Com.ByteAnalysis.IFacilita.Core.Service/* Com.ByteAnalysis.IFacilita.Core.Service/
#COPY Com.ByteAnalysis.IFacilita.Core.Repository/* Com.ByteAnalysis.IFacilita.Core.Repository/
#COPY Com.ByteAnalysis.IFacilita.Common/* Com.ByteAnalysis.IFacilita.Common/
#
#WORKDIR "/src/Com.ByteAnalysis.IFacilita.Core.API"
#RUN dotnet build "Com.ByteAnalysis.IFacilita.Core.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Com.ByteAnalysis.IFacilita.Core.Api.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.Core.Api.dll"]