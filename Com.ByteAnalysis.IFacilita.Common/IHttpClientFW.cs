using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Common
{
    public interface IHttpClientFW
    {
        string GetUrlBase();

        HttpResult<Ret> Get<Ret>();
        HttpResult<Ret> Get<Ret>(string[] urlComplements);

        HttpResult<Ret> Post<Ret, Send>(string[] urlComplements, Send entity);

        HttpResult<Ret> Put<Ret, Send>(string[] urlComplements, Send entity);

        IActionResult Delete(string[] urlComplements);

    }
}
