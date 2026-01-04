using System;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Service
{
    public class Class1
    {
        public Class1()
        {
            Common.IS3 s3 = new Common.Impl.S3();

            string URLRetorno = s3.SaveFile("Base64", ".pdf"); // < == asdasd.asdasd.asdasd.pdf
            string URLRetorno2 = s3.SaveFile("Base64", "Marcelo", ".pdf"); // < == asdasd.asdasd.asdasd.pdf
        }
    }
}
