using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Efiteutica.Model;
using Com.ByteAnalysis.IFacilita.Efiteutica.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EfiteuticaController : ControllerBase
    {
        private readonly IEfiteuticaService _service;

        public EfiteuticaController(IEfiteuticaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAsync();
                if (result == null || result.Count() == 0)
                    return NotFound(new { message = "Efiteutica not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            try
            {
                var result = await _service.GetAsync(id);
                if (result == null)
                    return NotFound(new { message = "Efiteutica not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendings([FromQuery] int page, [FromQuery] int count)
        {
            try
            {
                _ = page > 0 ? page-- : page;
                var result = await _service.GetPendingsAsync();

                if (result == null || result.ToList().Count() == 0)
                    return NotFound(new { message = "Efiteutica not found" });

                return Ok(result.ToList().Skip(page * 10).Take(count));

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequisitionModel req)
        {
            try
            {
                var result = await _service.CreateAsync(req);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RequisitionModel req)
        {
            var erros = new List<GlobalError>();
           
            try
            {
                var current = await _service.GetAsync(req.Id);
                if (current.Errors != null)
                    erros = current.Errors.ToList();

                await _service.UpdateAsync(req.Id, req);

                if (!string.IsNullOrEmpty(req.UrlCallback))
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var response = await client.GetAsync(string.Format(req.UrlCallback, req.Id));

                    if (!response.IsSuccessStatusCode)
                    {
                        erros.Add(new GlobalError()
                        {
                            Code = 0,
                            Field = "UrlCallback",
                            Message = await response.Content.ReadAsStringAsync()
                        });
                    }

                    req.Errors = erros;

                    await _service.UpdateAsync(req.Id, req);
                }

                return Ok(req);
            }
            catch (System.Exception ex)
            {
                erros.Add(new GlobalError()
                {
                    Code = 0,
                    Field = "UrlCallback",
                    Message = ex.Message
                });
                req.Errors = erros;

                await _service.UpdateAsync(req.Id, req);

                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await _service.RemoveAsync(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        #region MyRegion
        //[HttpGet("{iptuNumber}")]
        //public IActionResult Get(Int64 iptuNumber)
        //{
        //    //Loader Driver
        //    bool encontrou = false;
        //    using (ChromeDriver chrome = new ChromeDriver())
        //    {
        //        chrome.Navigate().GoToUrl("http://www2.rio.rj.gov.br/smf/siam2/situacaofiscal.asp");

        //        //Melhor visualizado em 800 x 600 ou superior e Internet Explorer 6.0 ou superior. 



        //        for (int i = 0; i < 100; i++)
        //        {
        //            chrome.FindElementByName("insEfiteuticacao").Clear();
        //            chrome.FindElementByName("insEfiteuticacao").SendKeys(iptuNumber.ToString());
        //            var imageBase64 = chrome.ExecuteSEfiteuticapt(@"
        //                var c = document.createElement('canvas');
        //                var ctx = c.getContext('2d');
        //                var img = document.getElementById('img');
        //                c.height=img.naturalHeight;
        //                c.width=img.naturalWidth;
        //                ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
        //                var base64String = c.toDataURL();
        //                return base64String;
        //                ") as string;

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
        //                if(exception.AlertText != "CÓDIGO digitado Inválido!")
        //                    return BadRequest(exception.AlertText);

        //                reinicia = true;
        //            }

        //            if (!reinicia)
        //                break;
        //        }


        //        for (int i = 0; i < 100; i++)
        //        {
        //            string text = chrome.FindElementByTagName("body").Text;
        //            if (text.Contains("OBSERVANDO O QUE DISPÕE A LEGISLAÇÃO EM VIGOR."))
        //            {
        //                encontrou = true;
        //                break;
        //            }

        //            System.Threading.Thread.Sleep(100);
        //        }

        //        var screenshot = chrome.GetScreenshot();
        //        if (encontrou)
        //        {
        //            return Ok(screenshot.AsBase64EncodedString);
        //        }
        //        else
        //        {
        //            return BadRequest(screenshot);
        //        }

        //        //chrome.Quit();
        //        //chrome.Dispose();
        //    }


        //    return BadRequest();
        //}


        //public static async Task<string> CaptchaSolve(string pathImage)
        //{
        //    var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
        //    var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

        //    var balance = await captcha.GetBalance();

        //    var image = await captcha.SolveImage(await ImageToBase64(pathImage));

        //    return image.Response;
        //}

        //public static async Task<string> CaptchaSolveWithBase64(string pathImage)
        //{
        //    var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
        //    var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

        //    var balance = await captcha.GetBalance();

        //    var image = await captcha.SolveImage(pathImage);

        //    return image.Response;
        //}

        //private static async Task<string> ImageToBase64(string path)
        //{
        //    byte[] imageArray = System.IO.File.ReadAllBytes(path);
        //    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

        //    return await Task.FromResult(base64ImageRepresentation);
        //} 
        #endregion

    }
}
