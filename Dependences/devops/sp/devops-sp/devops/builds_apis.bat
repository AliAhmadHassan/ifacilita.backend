REM baixar os fontes da master
start /WAIT stop_ifacilita.bat

rmdir C:\iFacilita\devops\src
rmdir /S /Q C:\iFacilita\api
rmdir /S /Q C:\iFacilita\rpa
rmdir /S /Q C:\iFacilita\Dependences

md C:\iFacilita\devops\src
cd C:\iFacilita\devops\src
git clone https://AumSistemas@bitbucket.org/byteanalysis/ifacilita-backend.git

REM -> eCartorioSP
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.eCartorioSp.Api
dotnet publish -c Release -o C:\iFacilita\api\eCartorioSP

REM -> SearchProtest: Pesquisa de Protestos
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.SearchProtest.Api
dotnet publish -c Release -o C:\iFacilita\api\SearchProtest

REM -> DefectsDefined:  Defeitos Ajuizados
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertificateESajSp.Api
dotnet publish -c Release -o C:\iFacilita\api\eSajSP

REM -> TaxDebts: Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.API
dotnet publish -c Release -o C:\iFacilita\api\CertidaoDebitoCreditoSP

REM -> IptuDebts: Débitos do IPTU
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.IptuDebit.Api
dotnet publish -c Release -o C:\iFacilita\api\IptuDebit

REM -> RealOnus: Ônus Reais
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.OnusReal.Api
dotnet publish -c Release -o C:\iFacilita\api\OnusReal


REM -> PropertyRegistrationData: Dados Cadastrais do Imóvel
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertiCadSp.Api
dotnet publish -c Release -o C:\iFacilita\api\CertiCadSp

REM -> ITBI-SP: Impostos de Transmissão de Bens Imóveis
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.ITBISP.API
dotnet publish -c Release -o C:\iFacilita\api\ItbiSp

REM -> Email Service
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.ServiceMail.Api
dotnet publish -c Release -o C:\iFacilita\api\ServiceMail

REM -> RPA'S

rmdir C:\iFacilita\rpa\SearchProtest /s /q
rmdir C:\iFacilita\rpa\eSajSP /s /q
rmdir C:\iFacilita\rpa\CertidaoDebitoCreditoSP /s /q
rmdir C:\iFacilita\rpa\IptuDebit /s /q
rmdir C:\iFacilita\rpa\OnusReal /s /q
rmdir C:\iFacilita\rpa\CertiCadSp /s /q
rmdir C:\iFacilita\rpa\ItbiSp /s /q

REM -> SearchProtest: Pesquisa de Protestos
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.SearchProtest.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\SearchProtest

REM -> DefectsDefined:  Defeitos Ajuizados
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertificateESajSp.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\eSajSP

REM -> TaxDebts: Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\CertidaoDebitoCreditoSP

REM -> IptuDebts: Débitos do IPTU
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.IptuDebit.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\IptuDebit

REM -> RealOnus: Ônus Reais
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.OnusReal.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\OnusReal
xcopy  C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.OnusReal.RPA\Documents\* C:\iFacilita\rpa\OnusReal\Documents\ /E /Y

REM -> PropertyRegistrationData: Dados Cadastrais do Imóvel
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.CertiCadSp.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\CertiCadSp

REM -> ITBI-SP: Impostos de Transmissão de Bens Imóveis
cd C:\iFacilita\devops\src\ifacilita-backend\Com.ByteAnalysis.IFacilita.ITBISP.RPA
dotnet publish -c Release -o C:\iFacilita\rpa\ItbiSp

REM -> Dependences
xcopy C:\iFacilita\devops\src\ifacilita-backend\Dependences\* C:\iFacilita\Dependences\ /E /Y

cd C:\iFacilita\devops\
rmdir C:\iFacilita\devops\src /s /q

start start_ifacilita.bat

echo Processo de publicacao dos RPA's e API finalizado com sucesso
