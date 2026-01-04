namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Exceptions
{
    public class GuideRequestInvalidException : BaseException
    {
        public GuideRequestInvalidException(string message) : base(message, 400)
        {

        }
    }
}
