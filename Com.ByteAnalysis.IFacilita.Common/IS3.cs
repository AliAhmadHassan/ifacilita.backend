using System;

namespace Com.ByteAnalysis.IFacilita.Common
{
    public interface IS3
    {
        string SaveFile(string base64encodedstring, string fileNameWithoutExtension, string fileExtensionWithDot);
        string SaveFile(string base64encodedstring, string fileExtensionWithDot);
        string GetFile(string key);
    }
}
