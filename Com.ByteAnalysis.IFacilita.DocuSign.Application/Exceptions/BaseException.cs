using Com.ByteAnalysis.IFacilita.DocuSign.Application.Errors;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Exceptions
{
    public class BaseException : Exception
    {
        public int StatusCodeException { get; }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, int statusCodeException) : base(message)
        {
            StatusCodeException = statusCodeException;
        }

        public Exception AddFieldErrorsData(List<FieldError> fieldErrors)
        {
            this.Data.Add("FieldErrors", fieldErrors);
            return this;
        }
    }
}
