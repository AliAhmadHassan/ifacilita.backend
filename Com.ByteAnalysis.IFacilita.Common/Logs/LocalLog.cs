using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.Common.Logs
{
    public static class LocalLog
    {
        public static void WriteLogStart(IConfiguration configuration, string serviceName, string serviceDescription)
        {
            try
            {
                var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

                if (!Directory.Exists(configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(configuration["OutputLogs:Path"]);

                var pathLog = $"{configuration["OutputLogs:Path"]}/ifacilitaStart-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{envStart}] - {serviceName} - {serviceDescription}";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tenta gravar log de inicialização. " + ex.Message);
            }
        }
    }
}
