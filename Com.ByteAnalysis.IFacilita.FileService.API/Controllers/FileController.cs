using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Net.Http;
using System.IO;
using Amazon.S3.Model;

namespace Com.ByteAnalysis.IFacilita.FileService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IAmazonAWSS3Settings amazonS3Settings;
        private IAmazonS3 s3Client;
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        public FileController(IAmazonAWSS3Settings amazonS3Settings)
        {
            this.amazonS3Settings = amazonS3Settings;
            s3Client = new AmazonS3Client(this.amazonS3Settings.AWSAccessKeyId, this.amazonS3Settings.AWSSecretKey, bucketRegion);
        }

        [HttpGet("{key}")]
        public async Task<string> Get(string key)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest { BucketName = this.amazonS3Settings.Bucket, Key = key };
                using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                {
                    using (MemoryStream imageStream = new MemoryStream())
                    {
                        response.ResponseStream.CopyTo(imageStream);
                        //string base64 = response.Metadata["x-amz-meta-data-type"] + "," + Convert.ToBase64String(imageStream.ToArray());
                        string base64 = Convert.ToBase64String(imageStream.ToArray());
                        return base64;
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.StackTrace);
            }



            return "";
        }

        [HttpPost]
        public async Task<string> Post(FileInfo fileInfo)
        {
            string key;
            if(fileInfo.FileNameWithoutExtension != null && fileInfo.FileNameWithoutExtension != "")
                key = $"{fileInfo.FileNameWithoutExtension}{fileInfo.FileExtensionWithDot}";
            else
                key = $"{Guid.NewGuid().ToString()}{fileInfo.FileExtensionWithDot}";

            try
            {

                string datatype;
                string converted;

                if (fileInfo.Base64encodedstring.Contains(","))
                {
                    datatype = fileInfo.Base64encodedstring.Substring(0, fileInfo.Base64encodedstring.IndexOf(","));
                    converted = fileInfo.Base64encodedstring.Replace(datatype + ",", string.Empty);
                }
                else
                {
                    converted = fileInfo.Base64encodedstring;
                }

                var bytes = Convert.FromBase64String(converted);

                TransferUtilityUploadRequest uR = new TransferUtilityUploadRequest
                {
                    BucketName = amazonS3Settings.Bucket,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = key,
                    InputStream = new MemoryStream(bytes)
                };

                //uR.Metadata.Add("data-type", datatype);

                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(uR);

                return key;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.StackTrace);
                return string.Format("Error encountered on server. Message:'{0}' when writing an object", e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.StackTrace);
                return string.Format("Unknown encountered on server. Message:'{0}' when writing an object", e.StackTrace);
            }
        }
    }
}
