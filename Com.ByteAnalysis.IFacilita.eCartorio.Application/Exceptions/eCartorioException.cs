using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions
{
    public class eCartorioException : BaseException
    {
        public eCartorioException(string message) : base(message, 500)
        {

        }

        public eCartorioException()
        {
        }

        public eCartorioException(string message, int statusCodeException) : base(message, statusCodeException)
        {
        }

        public eCartorioException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
