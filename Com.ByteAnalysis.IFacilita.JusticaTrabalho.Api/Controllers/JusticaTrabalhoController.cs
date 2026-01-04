using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace Com.ByteAnalysis.IFacilita.JusticaTrabalho.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JusticaTrabalhoController : ControllerBase
    {
        [HttpGet("{socialSecurityNumber}")]
        public IActionResult Get(Int64 socialSecurityNumber)
        {

            //Loader Driver
            
            ChromeOptions options = new ChromeOptions();
            //options.AddExtensions(new string[] { "C:\\temp\\anticaptcha-plugin_v0.50.crx" });
            options.AddArguments("load-extension=C:\\temp\\anticaptcha-plugin_v0.50");
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(ChromeOptions.Capability, options);

            using (ChromeDriver chrome = new ChromeDriver(options))
            {
                chrome.Navigate().GoToUrl("https://cndt-certidao.tst.jus.br/inicio.faces");

                //Melhor visualizado em 800 x 600 ou superior e Internet Explorer 6.0 ou superior. 

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        chrome.FindElementByName("j_id_jsp_992698495_2:j_id_jsp_992698495_3").Click();
                        break;
                    }
                    catch (NoSuchElementException error)
                    {
                        if (error.Message.Contains("no such element: Unable to locate element"))
                        {
                            System.Threading.Thread.Sleep(1000);
                            continue;
                        }
                    }
                }

                //http://ifacilita.com:5000/JusticaTrabalho/33486173839

                chrome.FindElementByName("gerarCertidaoForm:cpfCnpj").SendKeys(socialSecurityNumber.ToString());


                //var api = new RecaptchaV2Proxyless
                //{
                //    ClientKey = "08fa42956bda7a3e3896ccba6b454c92",
                //    WebsiteUrl = new Uri("https://cndt-certidao.tst.jus.br/inicio.faces"),
                //    WebsiteKey = "6LeKKAoUAAAAAJwv60Xf2N9-8Ri2mVJVp6dQaw6H"
                //};


                //if (!api.CreateTask())
                //    DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                //else if (!api.WaitForResult())
                //    DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                //else
                //    DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);



                //        for (int i = 0; i < 100; i++)
                //        {
                //            var imageBase64 = chrome.ExecuteScript(@"
                //var c = document.createElement('canvas');
                //var ctx = c.getContext('2d');
                //var img = document.getElementById('img');
                //c.height=img.naturalHeight;
                //c.width=img.naturalWidth;
                //ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
                //var base64String = c.toDataURL();
                //return base64String;
                //") as string;

                //            string solved = CaptchaSolveWithBase64(imageBase64.Remove(0, imageBase64.IndexOf(",") + 1)).Result;
                //            chrome.FindElementById("texto_imagem").SendKeys(solved);
                //            chrome.FindElementByName("formulario").Submit();

                //            string text = "";
                //            bool reinicia = false;
                //            try
                //            {
                //                text = chrome.FindElementByTagName("body").Text;
                //            }
                //            catch (UnhandledAlertException exception)
                //            {
                //                if (exception.AlertText != "CÓDIGO digitado Inválido!")
                //                    return BadRequest(exception.AlertText);

                //                reinicia = true;
                //            }

                //            if (!reinicia)
                //                break;
                //        }

                System.Threading.Thread.Sleep(3000);
                for (int i = 0; i < 1000; i++)
                {
                    string text = chrome.FindElementByTagName("body").FindElement(By.ClassName("antigate_solver")).Text;
                    if (text == "Solved")
                    {
                        bool encontrou = true;
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                }

                //        var screenshot = chrome.GetScreenshot();
                //        if (encontrou)
                //        {
                //            return Ok(screenshot);
                //        }
                //        else
                //        {
                //            return BadRequest(screenshot);
                //        }

                //        //chrome.Quit();
                //        //chrome.Dispose();

                //chrome.ExecuteScript("javascript:document.getElementById(\"recaptcha-anchor\").click()");


                chrome.FindElementById("gerarCertidaoForm:btnEmitirCertidao").Click();


                System.Threading.Thread.Sleep(10000);

                string base64 = ImageToBase64($@"C:\Users\manager\Downloads\certidao_{socialSecurityNumber}.pdf");
                System.IO.File.Delete($@"C:\Users\manager\Downloads\certidao_{socialSecurityNumber}.pdf");

                return Ok(base64);

            }


            return BadRequest();
        }


        private static string ImageToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }

    }
}
