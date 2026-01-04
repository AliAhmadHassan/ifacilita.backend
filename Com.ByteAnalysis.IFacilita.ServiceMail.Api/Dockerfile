#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY *.sln .

COPY Com.ByteAnalysis.IFacilita.ServiceMail.Api/*.csproj Com.ByteAnalysis.IFacilita.ServiceMail.Api/
COPY Com.ByteAnalysis.IFacilita.ServiceMail.Service/*.csproj Com.ByteAnalysis.IFacilita.ServiceMail.Service/
COPY Com.ByteAnalysis.IFacilita.Common/*.csproj Com.ByteAnalysis.IFacilita.Common/
RUN dotnet restore "Com.ByteAnalysis.IFacilita.ServiceMail.Api/Com.ByteAnalysis.IFacilita.ServiceMail.Api.csproj"

COPY . .

WORKDIR "/src/Com.ByteAnalysis.IFacilita.ServiceMail.Api"
RUN dotnet build "Com.ByteAnalysis.IFacilita.ServiceMail.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Com.ByteAnalysis.IFacilita.ServiceMail.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.ServiceMail.Api.dll"]