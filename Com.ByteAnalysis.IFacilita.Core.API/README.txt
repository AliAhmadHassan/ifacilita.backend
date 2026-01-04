#DOTNET PUBLISH for Linux x64
dotnet build Com.ByteAnalysis.IFacilita.Core.API.csproj --runtime linux-x64 -o dist

#DOCKER BUILD
 docker build -t aumsistemas/ifacilita_core:<CURRENT-TAG> .

#DOCKER PUSH
docker push  aumsistemas/ifacilita_core:<CURRENT-TAG>