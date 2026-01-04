namespace Com.ByteAnalysis.IFacilita.Core.Service.Exceptions
{
    public class GuideRequestInvalidException : BaseException
    {
        public GuideRequestInvalidException(string message) : base(message, 400)
        {

        }
    }
}
