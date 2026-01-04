using System;
using System.Drawing;
using System.IO;

namespace Anticaptcha_example.Helper
{
    public class StringHelper
    {
        public static string ImageFileToBase64String(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }
    }
}