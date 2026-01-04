namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {

        }
    }
}
