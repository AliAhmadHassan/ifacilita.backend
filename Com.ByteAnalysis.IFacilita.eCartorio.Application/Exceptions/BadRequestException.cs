namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {

        }

        public BadRequestException(string message, int statusCodeException) : base(message, statusCodeException)
        {
        }

        public BadRequestException() : base()
        {
        }

        public BadRequestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
