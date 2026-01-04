using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Common.Impl
{
    public class S3 : IS3
    {
        public string GetFile(string key)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.GetAsync($"http://ifacilita.com:5600/api/file/{key}").Result;

                string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo2);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    return conteudo2.Replace("\"", "");
                }
            }
            return null;
        }

        public string SaveFile(string base64encodedstring, string fileNameWithoutExtension, string fileExtensionWithDot)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync($"http://40.124.89.158:5600/api/file", new StringContent(JsonConvert.SerializeObject(new
                {
                    base64encodedstring,
                    fileExtensionWithDot,
                    fileNameWithoutExtension
                }), Encoding.UTF8, "application/json")).Result;

                string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo2);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    return conteudo2.Replace("\"", "");
                }
            }
            return null;
        }

        public string SaveFile(string base64encodedstring, string fileExtensionWithDot)
        {
            return this.SaveFile(base64encodedstring, Guid.NewGuid().ToString(), fileExtensionWithDot);
        }
    }
}
