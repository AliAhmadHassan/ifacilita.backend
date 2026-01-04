using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Common.Impl
{
    public class HttpClientFW : IHttpClientFW
    {
        private string urlBase;

        public HttpClientFW(string urlBase)
        {
            this.urlBase = urlBase;
        }

        public IActionResult Delete(string[] urlComplements)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    StringBuilder url = new StringBuilder();
                    url.Append(this.urlBase);
                    foreach (var complement in urlComplements)
                    {
                        url.Append($"/{complement}");
                    }

                    HttpResponseMessage respToken = client.DeleteAsync($"{url}").Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        return new OkResult();
                    }
                    else
                    {
                        return new StatusCodeResult((int)respToken.StatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }

        public HttpResult<Ret> Get<Ret>()
        {
            return Get<Ret>(new string[] { });
        }

        public HttpResult<Ret> Get<Ret>(string[] urlComplements)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    StringBuilder url = new StringBuilder();
                    url.Append(this.urlBase);

                    if (urlComplements != null)
                        foreach (var complement in urlComplements)
                        {
                            if (!string.IsNullOrEmpty(complement))
                                url.Append($"/{complement}");
                        }

                    HttpResponseMessage respToken = client.GetAsync($"{url}").Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        return new HttpResult<Ret>()
                        {
                            Value = JsonConvert.DeserializeObject<Ret>(conteudo2),
                            ActionResult = new OkResult(),
                            IsSuccessfully = true
                        };
                    }
                    else
                    {
                        return new HttpResult<Ret>()
                        {
                            ActionResult = new StatusCodeResult((int)respToken.StatusCode)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpResult<Ret>()
                {
                    ActionResult = new BadRequestObjectResult(e)
                };
            }
        }

        public string GetUrlBase()
        {
            return this.urlBase;
        }

        public HttpResult<Ret> Post<Ret, Send>(string[] urlComplements, Send entity)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    StringBuilder url = new StringBuilder();
                    url.Append(this.urlBase);
                    foreach (var complement in urlComplements)
                    {
                        url.Append($"/{complement}");
                    }

                    HttpResponseMessage respToken = client.PostAsync($"{url}", new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")).Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        return new HttpResult<Ret>()
                        {
                            Value = Newtonsoft.Json.JsonConvert.DeserializeObject<Ret>(conteudo2),
                            ActionResult = new OkResult(),
                            IsSuccessfully = true
                        };
                    }
                    else
                    {
                        return new HttpResult<Ret>()
                        {
                            ActionResult = new StatusCodeResult((int)respToken.StatusCode)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpResult<Ret>()
                {
                    ActionResult = new BadRequestObjectResult(e)
                };
            }
        }

        public HttpResult<Ret> Put<Ret, Send>(string[] urlComplements, Send entity)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    StringBuilder url = new StringBuilder();
                    url.Append(this.urlBase);
                    foreach (var complement in urlComplements)
                    {
                        url.Append($"/{complement}");
                    }

                    HttpResponseMessage respToken = client.PutAsync($"{url}", new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")).Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        return new HttpResult<Ret>()
                        {
                            Value = Newtonsoft.Json.JsonConvert.DeserializeObject<Ret>(conteudo2),
                            ActionResult = new OkResult(),
                            IsSuccessfully = true
                        };
                    }
                    else
                    {
                        return new HttpResult<Ret>()
                        {
                            ActionResult = new StatusCodeResult((int)respToken.StatusCode)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new HttpResult<Ret>()
                {
                    ActionResult = new BadRequestObjectResult(e)
                };
            }
        }

        public static string WaitingDownloaded(string fileName)
        {
            //string fileName = "Relatorio_Certidao_Imob_";

            var downloadsPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\" + fileName;
            for (var i = 0; i < 30; i++)
            {
                if (File.Exists(downloadsPath)) { break; }
                string temp = Directory.GetFiles(Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\").Where(c => c.Contains(fileName)).FirstOrDefault();
                if (temp != null)
                {
                    fileName = new FileInfo(temp).Name;
                    downloadsPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\" + fileName;
                    break;
                }

                System.Threading.Thread.Sleep(1000);
            }
            var length = new FileInfo(downloadsPath).Length;
            for (var i = 0; i < 30; i++)
            {
                System.Threading.Thread.Sleep(1000);
                var newLength = new FileInfo(downloadsPath).Length;
                if (newLength == length && length != 0) { break; }
                length = newLength;
            }

            return downloadsPath;
        }
    }
}
