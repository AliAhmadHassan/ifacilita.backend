namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message) : base(message, 500)
        {

        }
    }
}
