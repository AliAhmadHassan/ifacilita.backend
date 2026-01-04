namespace Com.ByteAnalysis.IFacilita.Core.Service.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 404)
        {

        }
    }
}
