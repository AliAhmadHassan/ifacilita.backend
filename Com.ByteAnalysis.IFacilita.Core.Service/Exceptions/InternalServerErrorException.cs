namespace Com.ByteAnalysis.IFacilita.Core.Service.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message) : base(message, 500)
        {

        }
    }
}
