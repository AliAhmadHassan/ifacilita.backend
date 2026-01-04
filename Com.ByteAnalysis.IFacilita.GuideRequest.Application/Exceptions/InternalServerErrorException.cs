namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message) : base(message, 500)
        {

        }
    }
}
