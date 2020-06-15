using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using GroupProject.Classes;
using GroupProject.Models;

namespace GroupProject.Extensions
{
    public static class ActionResultExtensions
    {
        public static ImageResult Image(this Controller controller, Stream imageStream, string contentType)
        {
            return new ImageResult(imageStream, contentType);
        }


        public static ImageResult Image(this Controller controller, byte[] imageBytes, string contentType)
        {
            return new ImageResult(new MemoryStream(imageBytes), contentType);
        }


        public static ImageResult Image(this Controller controller, string filePath, string contentType)
        {
            return new ImageResult(File.OpenRead(filePath), contentType);
        }
    }
}