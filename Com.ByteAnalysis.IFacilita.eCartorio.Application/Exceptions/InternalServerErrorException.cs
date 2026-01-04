using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message) : base(message, 500)
        {

        }

        public InternalServerErrorException(string message, int statusCodeException) : base(message, statusCodeException)
        {
        }

        public InternalServerErrorException() : base()
        {
        }

        public InternalServerErrorException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
