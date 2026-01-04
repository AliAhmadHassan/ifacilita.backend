using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Attributes;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa.RpaProcess
{
    public class ProcessServiceBase<TConfig, TEntity> where TConfig : BaseMapperConfig
    {
        private TConfig _mapperConfig;
        private TEntity _entity;
        private IWebDriver _webDriver;

        public ProcessServiceBase(TConfig mapperConfig, TEntity entity, IWebDriver webDriver)
        {
            _mapperConfig = mapperConfig;
            _entity = entity;
            _webDriver = webDriver;
        }

        public async Task<bool> Clear()
        {
            return await ClickButton("Clear");
        }

        public async Task<bool> ExecuteRead(bool confirmSubmit = true)
        {
            foreach (var p in _mapperConfig.GetType().GetProperties())
            {
                //Ignora os botões
                if (await IgnoreAttribute(p) || p.Name.Equals("Captcha") || p.Name.Equals("Refazer"))
                    continue;

                var pValue = p.GetValue(_mapperConfig, null);

                IWebElement element = null;
                try { element = _webDriver.FindElement(By.Name(pValue.ToString())); }
                catch { element = null; }

                if (element == null)
                {
                    try { element = _webDriver.FindElement(By.XPath(pValue.ToString())); }
                    catch { element = null; }
                }

                if (element == null)
                    return await Task.FromResult(false);

                switch (await GetHtmlElementAsync(p))
                {
                    case "td":
                    case "text":
                        _entity.GetType().GetProperty(p.Name).SetValue(_entity, element.Text);
                        break;

                    case "list":
                        List<String> itens = new List<String>();
                        var ils = element.FindElements(By.TagName("li"));

                        foreach (var il in ils)
                        {
                            var _p = il.FindElement(By.TagName("p"));
                            itens.Add(_p.Text);
                        }

                        _entity.GetType().GetProperty($"{p.Name}").SetValue(_entity, itens);
                        break;
                    default:
                        break;
                }
            }

            if (confirmSubmit) await Submit();

            return await Task.FromResult(true);
        }

        public async Task<Dto.ValidationField> ExecuteWrite()
        {
            var countIntent = 1;
            var fillForm = false;
            do
            {
                fillForm = await FillForm();

                if (fillForm)
                    break;

                countIntent++;
                System.Threading.Thread.Sleep(1000);

            } while (countIntent <= 5);

            if (!fillForm) return await Task.FromResult(new ValidationField() { Message = "Erro não identificado. Revisar tela do RPA", Field = "General" });

            countIntent = 1;
            do
            {
                var msgSubmit = await Submit();
                if (msgSubmit.Message == "")
                    return await Task.FromResult(msgSubmit);

                if (msgSubmit.Message != "wait")
                    return await Task.FromResult(msgSubmit);

                await FillForm();

                countIntent++;
                System.Threading.Thread.Sleep(1000);

            } while (countIntent <= 5);

            return await Task.FromResult(new ValidationField() { Message = "", Field = "General" });
        }

        private async Task<string> GetHtmlElementAsync(PropertyInfo property)
        {
            var htmlElement = "";
            foreach (MapperConfigAttributes attr in property.GetCustomAttributes(true))
                htmlElement = attr.HtmlElementType;

            return await Task.FromResult(htmlElement);
        }

        private async Task<bool> IgnoreAttribute(PropertyInfo property)
        {
            var ignore = property.Name.Equals("Submeter") || property.Name.Equals("Clear") || property.Name.Equals("PageName") || property.Name.Equals("TextId") || property.Name.Equals("Desfazer");
            return await Task.FromResult(ignore);
        }

        public async Task<bool> FillForm()
        {
            foreach (var p in _mapperConfig.GetType().GetProperties())
            {
                try
                {
                    //Ignora os botões
                    if (await IgnoreAttribute(p))
                        continue;

                    var pValue = p.GetValue(_mapperConfig, null);

                    IWebElement element = null;

                    try { element = _webDriver.FindElement(By.Name(pValue.ToString())); } catch { element = null; }

                    if (element == null)
                        try { element = _webDriver.FindElement(By.XPath(pValue.ToString())); } catch { element = null; }

                    if (element == null)
                        return await Task.FromResult(false);

                    switch (await GetHtmlElementAsync(p))
                    {
                        case "captcha":
                            var elementImageCaptcha = _webDriver.FindElements(By.TagName("img"));
                            foreach (var img in elementImageCaptcha)
                            {
                                if (img.GetAttribute("alt").ToLowerInvariant().Equals("captcha"))
                                {
                                    var fileName = Path.Combine(Environment.CurrentDirectory, "download-captcha", Guid.NewGuid().ToString("N") + ".jpg");
                                    WebClient webClient = new WebClient();
                                    webClient.DownloadFile(img.GetAttribute("src"), fileName);

                                    var captchaSolve = await CaptchaSolve.AntiCaptchaClient.CaptchaSolve(fileName);

                                    element.Clear();
                                    element.SendKeys(captchaSolve);

                                    break;
                                }
                            }

                            break;
                        case "captcha2":
                            var elementImageCaptcha2 = _webDriver.FindElements(By.TagName("img"));
                            string captchaFull = string.Empty;
                            foreach (var img in elementImageCaptcha2)
                            {
                                var src = img.GetAttribute("src");
                                var srcSplit = img.GetAttribute("src").Split('/');
                                var nameFileSplit = srcSplit[srcSplit.Length - 1].Split('.');
                                var charName = nameFileSplit[0];
                                captchaFull += charName;
                            }

                            element.Clear();
                            element.SendKeys(captchaFull);

                            break;
                        case "text":
                            var valueText = _entity.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(p.Name))?.GetValue(_entity);
                            if (valueText != null)
                            {
                                element.Clear();
                                element.SendKeys(valueText.ToString());
                            }

                            break;
                        case "select":

                            var selectHtml = new SelectElement(element);
                            var valueSelect = _entity.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(p.Name))?.GetValue(_entity);

                            var valueSet = false;
                            try { selectHtml.SelectByValue(valueSelect.ToString()); valueSet = true; } catch { valueSet = false; }

                            if (!valueSet)
                                try { selectHtml.SelectByText(valueSelect.ToString()); valueSet = true; } catch { valueSet = false; }

                            if (!valueSet && valueSelect.Equals("0"))
                                try { selectHtml.SelectByValue(" "); valueSet = true; } catch { valueSet = false; }

                            if (!valueSet)
                                return await Task.FromResult(true);

                            break;
                        case "radio":
                            ReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(By.Name(pValue.ToString()));

                            if (element == null)
                                return await Task.FromResult(true);

                            var valueRadio = _entity.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(p.Name))?.GetValue(_entity);
                            foreach (var el in elements)
                            {
                                var elVal = el.GetAttribute("value");
                                if (valueRadio.Equals(elVal))
                                {
                                    el.Click();
                                    break;
                                }
                            }

                            break;
                        case "submit":
                        case "buttom":
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message);
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        private async Task<string> AlertIsPresent(int timeout = 3)
        {
            try
            {
                var alert = _webDriver.SwitchTo().Alert();
                string message = null;
                if (alert != null)
                {
                    message = alert.Text;
                    alert.Accept();
                }

                return await Task.FromResult(message);
            }
            catch (NoAlertPresentException aEx)
            {
                return await Task.FromResult("");
            }
        }

        private async Task<string> LoadCompleted(int timeout = 3)
        {
            var msg = await AlertIsPresent(timeout);
            if (!string.IsNullOrEmpty(msg))
                return await Task.FromResult(msg);

            IJavaScriptExecutor js = (IJavaScriptExecutor)_webDriver;
            WebDriverWait wait = new WebDriverWait(_webDriver, new TimeSpan(0, 0, timeout));
            var loaded = await Task.FromResult(wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete"));

            if (!loaded)
            {
                return await Task.FromResult("wait");
            }
            else
            {
                return await Task.FromResult("");
            }
        }

        public async Task<Dto.ValidationField> Submit()
        {
            if (await ClickButton("Submeter"))
            {
                System.Threading.Thread.Sleep(500);

                while (true)
                {
                    var msg = await LoadCompleted();
                    msg = msg.Replace("\n", "").Replace("\r","");

                    if (msg == "")
                        break;

                    if (msg.ToLowerInvariant() == "seqüência de caracteres não confere, tente novamente!")
                    {
                        return await Task.FromResult(new ValidationField() { Message = "wait", Field = "General" });
                    }
                    else
                    {
                        if (msg != "wait") // msg of error
                        {
                            switch (msg.ToLowerInvariant())
                            {
                                case "inscrição imobiliária inválida. entre em contato com um dos postos de atendimento do iptu.": return await Task.FromResult(new ValidationField() { Message = msg, Field = "Iptu" });
                                default: return await Task.FromResult(new ValidationField() { Message = msg, Field = "General" });
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(250);
                }


                var pageName = _mapperConfig.PageName;
                var pageId = _mapperConfig.TextId;
                var pageSource = _webDriver.PageSource;

                if (pageSource.Contains(pageName.ToString()) || pageSource.Contains(pageId))
                    return await Task.FromResult(new ValidationField() { Message = "Error not defined", Field = "General" });
            }

            return await Task.FromResult(new ValidationField() { Field = "General", Message = "" });
        }

        private async Task<bool> ClickButton(string name)
        {
            var buttomName = _mapperConfig.GetType().GetProperty(name).GetValue(_mapperConfig, null).ToString();

            IWebElement buttom = null;
            try { buttom = _webDriver.FindElement(By.Name(buttomName)); } catch { buttom = null; }
            if (buttom == null)
                try { buttom = _webDriver.FindElement(By.XPath(buttomName)); } catch { buttom = null; }

            if (buttom == null)
                return await Task.FromResult(false);

            buttom.Click();

            return await Task.FromResult(true);
        }
    }
}
