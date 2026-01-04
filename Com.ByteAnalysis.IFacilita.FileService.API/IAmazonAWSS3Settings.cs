using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.FileService.API
{
    public interface IAmazonAWSS3Settings
    {
        string AmazonWS { get; set; }
        string AWSAccessKeyId { get; set; }
        string AWSSecretKey { get; set; }
        string Bucket { get; set; }
    }

    public class AmazonAWSS3Settings : IAmazonAWSS3Settings
    {
        public string AmazonWS { get; set; }
        public string AWSAccessKeyId { get; set; }
        public string AWSSecretKey { get; set; }
        public string Bucket { get; set; }
    }
}
