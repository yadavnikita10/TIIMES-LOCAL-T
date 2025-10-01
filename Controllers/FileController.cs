using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TuvVision.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
        [Route("api/file/download")]
        public HttpResponseMessage DownloadFile()
        {
            string filePath = @"C:\SharedFolder\sample.txt"; // File path on server

            if (!File.Exists(filePath))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "sample.txt"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }
    }
}
