using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Common
{
    public class HttpResult<Ret>
    {
        public Ret Value { get; set; }
        public IActionResult ActionResult { get; set; }
        public bool IsSuccessfully { get; set; } = false;
    }
}
