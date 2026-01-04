namespace Com.ByteAnalysis.IFacilita.Core.Service.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {

        }
    }
}
