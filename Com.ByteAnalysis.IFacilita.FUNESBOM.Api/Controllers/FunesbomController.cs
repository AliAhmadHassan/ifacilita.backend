using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.FUNESBOM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FunesbomController : ControllerBase
    {

        [HttpGet("{iptu}")]
        public IActionResult Get(Int64 iptu)
        {
            //Loader Driver

            bool encontrou = false;
            using (ChromeDriver chrome = new ChromeDriver())
            {
                chrome.Navigate().GoToUrl("http://www.funesbom.rj.gov.br/modules.php?name=Servicos&op=consultar_numcbmerj");

                //Melhor visualizado em 800 x 600 ou superior e Internet Explorer 6.0 ou superior. 
                chrome.FindElementByName("inscricao_e").SendKeys(iptu.ToString());
                var municipio = chrome.FindElement(By.Name("cod_mun_e"));

                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(municipio);
                selectElement.SelectByValue("064");

                chrome.FindElementByClassName("botao").Click();


                for (int i = 0; i < 100; i++)
                {

                    var imageBase64 = chrome.ExecuteScript(@"
        var c = document.createElement('canvas');
        var ctx = c.getContext('2d');
        var img = document.getElementById('cod_seguranca');
        c.height=img.naturalHeight;
        c.width=img.naturalWidth;
        ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
        var base64String = c.toDataURL();
        return base64String;
        ") as string;

                    string solved = CaptchaSolveWithBase64(imageBase64.Remove(0, imageBase64.IndexOf(",") + 1)).Result;
                    chrome.FindElementById("seguranca").Clear();
                    chrome.FindElementById("seguranca").SendKeys(solved);
                    System.Threading.Thread.Sleep(1500);
                    chrome.FindElementByClassName("botao").Click();

                    string text = "";
                    try
                    {
                        text = chrome.FindElementByTagName("body").Text;
                    }
                    catch (UnhandledAlertException exception)
                    {
                        return BadRequest(exception.AlertText);
                    }

                    if (!text.Contains("Código de Segurança inválido. Favor retornar."))
                        break;
                    chrome.ExecuteJavaScript("javascript: history.back()");
                }
                System.Threading.Thread.Sleep(1500);
                chrome.FindElementByClassName("botao").Click();

                for (int i = 0; i < 100; i++)
                {
                    string text = chrome.FindElementByTagName("body").Text;
                    if (text.Contains("visitas ao Portal desde"))
                    {
                        encontrou = true;
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                }

                var screenshot = chrome.GetScreenshot();
                if (encontrou)
                {
                    return Ok(screenshot.AsBase64EncodedString);
                }
                else
                {
                    return BadRequest(screenshot);
                }

                //chrome.Quit();
                //chrome.Dispose();
            }


            return BadRequest();
        }

        public static async Task<string> CaptchaSolve(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(await ImageToBase64(pathImage));

            return image.Response;
        }

        public static async Task<string> CaptchaSolveWithBase64(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(pathImage);

            return image.Response;
        }

        private static async Task<string> ImageToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }


    }
}
