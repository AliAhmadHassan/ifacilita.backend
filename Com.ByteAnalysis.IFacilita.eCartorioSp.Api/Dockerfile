FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Test
COPY dist .
ENTRYPOINT ["dotnet", "Com.ByteAnalysis.IFacilita.eCartorioSp.Api.dll"]
