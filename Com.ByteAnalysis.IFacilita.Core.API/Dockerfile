FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
MAINTAINER francisco@aumsistemas.com.br
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Test
COPY dist .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.Core.API.dll"]