using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Classes
{
    public class ImageResult : ActionResult
    {
        private Stream imageStream;
        private string contentType = "";

        public Stream ImageStream
        {
            get
            {
                return imageStream;
            }
            set
            {
                imageStream = value;
            }
        }

        public string ContentType
        {
            get
            {
                return contentType;
            }
            set
            {
                contentType = value;
            }
        }

        public ImageResult(Stream _imageStream, string _contentType)
        {
            if (_imageStream == null)
            {
                throw new ArgumentNullException("_imageStream is null");
            }
            if (string.IsNullOrEmpty(_contentType))
            {
                throw new ArgumentNullException("_contentType is null");
            }
            ImageStream = _imageStream;
            ContentType = _contentType;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context is null");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = ContentType;
            byte[] buffer = new byte[1024];
            do
            {
                int read = ImageStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    break;
                }
                response.OutputStream.Write(buffer, 0, read);
            } while (true);
            response.End();
        }
    }
}