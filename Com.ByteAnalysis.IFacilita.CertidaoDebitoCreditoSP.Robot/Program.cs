using Com.ByteAnalysis.IFacilita.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            Common.IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW("http://localhost:5135/api/CertidaoDebitoCreditoSP");
            IS3 s3 = new Common.Impl.S3();

            while (true)
            {
                Common.HttpResult<Model.CertidaoDebitoCreditoSP> get = httpClientFW.Get<Model.CertidaoDebitoCreditoSP>(new[] { "" });
                //Model.Requisition requisition = get.
                if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                {
                    Model.CertidaoDebitoCreditoSP certidao = get.Value;


                    var proc = System.Diagnostics.Process.GetProcessesByName("chromedriver.exe (64 bit)");
                    if (proc != null && proc.Length > 0)
                        proc.ToList().ForEach(x => x.Kill());

                    proc = System.Diagnostics.Process.GetProcessesByName("chromedriver.exe");
                    if (proc != null && proc.Length > 0)
                        proc.ToList().ForEach(x => x.Kill());


                    ChromeOptions options = new ChromeOptions();
                    //options.AddExtensions(new string[] { "C:\\temp\\anticaptcha-plugin_v0.50.crx" });
                    options.AddArguments("load-extension=C:\\temp\\anticaptcha-plugin_v0.50");
                    options.AddArguments("disable-infobars");



                    using (ChromeDriver chrome = new ChromeDriver(options))
                    {

                        if (certidao.PessoaFisica)
                            chrome.Navigate().GoToUrl($"http://servicos.receita.fazenda.gov.br/Servicos/certidao/CndConjuntaInter/InformaNICertidao.asp?Tipo=2&ERR=parmacessoexpirado&NI={certidao.CpfCnpj}");
                        else
                            chrome.Navigate().GoToUrl($"http://servicos.receita.fazenda.gov.br/Servicos/certidao/CndConjuntaInter/InformaNICertidao.asp?Tipo=1&ERR=parmacessoexpirado&NI={certidao.CpfCnpj}");


                        chrome.Manage().Window.Size = new System.Drawing.Size(1280, 800);

                        #region Quebrar o Captcha

                        string imagem = chrome.FindElement(By.Id("imgCaptchaSerpro")).GetAttribute("src");

                        using (WebClient client = new WebClient())
                        {
                            Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("imgCaptchaSerpro"))).GetScreenshot();
                            image.SaveAsFile("catptcha.jpg");
                        }

                        string imgValue = CaptchaSolveWithBase64("catptcha.jpg").Result;

                        chrome.FindElement(By.Id("txtTexto_captcha_serpro_gov_br")).SendKeys(imgValue);

                        #endregion

                        chrome.FindElement(By.Id("submit1")).Click();

                        System.Threading.Thread.Sleep(3000);

                        bool loading = false;
                        bool exists = chrome.FindElements(By.XPath("//a [text()='Emissão de nova certidão']")).Count > 0;

                        if (exists)
                            chrome.FindElement(By.XPath("//a [text()='Emissão de nova certidão']")).Click();

                        while (loading == false)
                        {
                            loading = chrome.FindElements(By.XPath("//a [text()='Nova Consulta']")).Count > 0;

                            if (loading)
                                break;
                        }

                        #region salvar na S3

                        var doc = chrome.FindElement(By.Id("PRINCIPAL")).GetAttribute("innerHTML");

                        using (WebClient client = new WebClient())
                        {
                            Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("PRINCIPAL"))).GetScreenshot();
                            image.SaveAsFile($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg");

                            string baseContract64 = FileToBase64($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg").Result;
                            string keyS3 = s3.SaveFile(baseContract64, ".jpg");

                            certidao.IdDocS3 = $"https://ifacilita.s3.us-east-2.amazonaws.com/CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg";
                        }

                        #endregion

                        File.Delete($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg");

                    }
                       

                        Common.HttpResult<Model.CertidaoDebitoCreditoSP> postResult = httpClientFW.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);

                    if (postResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                    {
                        var callbackResult = new Common.Impl.HttpClientFW(certidao.UrlCallback).Get<Object>(new string[] { "" });

                        if (callbackResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                        {
                            certidao.Status = Model.Status.Finished;
                            certidao.StatusModified = DateTime.Now;
                        }
                        else
                        {
                            certidao.Status = Model.Status.ErrorOnCallback;
                            certidao.StatusModified = DateTime.Now;

                        }
                    }

                    httpClientFW.Post<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                }

                Thread.Sleep(60 * 1000);
            }

           
        }

        public static async Task<string> CaptchaSolveWithBase64(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(pathImage);

            return image.Response;
        }

        private static async Task<string> FileToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }

    }
    
}
    


