
REM -> Set Environment
$keyStart = [guid]::NewGuid()
echo $keyStart

setx IFACILITASTART $keyStart
setx DOTNETCORE_ENVIRONMENT "Production"
setx ASPNETCORE_ENVIRONMENT "Production"

REM -> Iniciar APIs
set /A waitStart = 1
cd C:\iFacilita\api\CertidaoDebitoCreditoSP
start C:\iFacilita\api\CertidaoDebitoCreditoSP\Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\eCartorioSP
start C:\iFacilita\api\eCartorioSP\Com.ByteAnalysis.IFacilita.eCartorioSp.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\eSajSP
start C:\iFacilita\api\eSajSP\Com.ByteAnalysis.IFacilita.CertificateESajSp.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\IptuDebit
start C:\iFacilita\api\IptuDebit\Com.ByteAnalysis.IFacilita.IptuDebit.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\OnusReal
start C:\iFacilita\api\OnusReal\Com.ByteAnalysis.IFacilita.OnusReal.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\CertiCadSp
start C:\iFacilita\api\CertiCadSp\Com.ByteAnalysis.IFacilita.CertiCadSp.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\SearchProtest
start C:\iFacilita\api\SearchProtest\Com.ByteAnalysis.IFacilita.SearchProtest.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\ItbiSp
start C:\iFacilita\api\ItbiSp\Com.ByteAnalysis.IFacilita.ITBISP.Api.exe
timeout /t %waitStart%

cd C:\iFacilita\api\ServiceMail
start C:\iFacilita\api\ServiceMail\Com.ByteAnalysis.IFacilita.ServiceMail.Api
timeout /t %waitStart%

REM ->Iniciar os Rpas
cd C:\iFacilita\rpa\CertidaoDebitoCreditoSP
start C:\iFacilita\rpa\CertidaoDebitoCreditoSP\Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.RPA.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\eSajSp
start C:\iFacilita\rpa\eSajSp\Com.ByteAnalysis.IFacilita.CertificateESajSp.Rpa.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\IptuDebit
start C:\iFacilita\rpa\IptuDebit\Com.ByteAnalysis.IFacilita.IptuDebit.RPA.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\OnusReal
start C:\iFacilita\rpa\OnusReal\Com.ByteAnalysis.IFacilita.OnusReal.RPA.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\CertiCadSp
start C:\iFacilita\rpa\CertiCadSp\Com.ByteAnalysis.IFacilita.CertiCadSp.RPA.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\SearchProtest
start C:\iFacilita\rpa\SearchProtest\Com.ByteAnalysis.IFacilita.SearchProtest.Rpa.exe
timeout /t %waitStart%

cd C:\iFacilita\rpa\ItbiSp
start C:\iFacilita\rpa\ItbiSp\Com.ByteAnalysis.IFacilita.ITBISP.RPA.exe

@echo on 
exit 0