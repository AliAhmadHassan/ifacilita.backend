using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 404)
        {

        }
    }
}
