using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoboAntiCaptchaService.CaptchaSolve
{
    public static class AntiCaptchaClient
    {
        public static async Task<string> CaptchaSolve(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(await ImageToBase64(pathImage));

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
