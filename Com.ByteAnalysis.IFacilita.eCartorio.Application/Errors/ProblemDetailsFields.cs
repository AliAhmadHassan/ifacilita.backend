using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors
{
    public class ProblemDetailsFields : ProblemDetails
    {
        public List<FieldError> FieldErrors { get; set; }
    }
}
