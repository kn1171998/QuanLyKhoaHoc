using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DoAnTotNghiep.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private string name;
        public VideosController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            filename = _hostingEnvironment.WebRootPath + filename;
            //if(System.IO.File.Exists(filename))
            //{
            //string file = Path.GetFileNameWithoutExtension(filename);
            //filename = _hostingEnvironment.WebRootPath+@"\" + file + ".webm";
            //}
            name = filename;
            var video = new VideoStream(filename);
            var response = new HttpResponseMessage
            {
                Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>) video.WriteToStream)
            };
            var objectResult = new ObjectResult(response);
            objectResult.ContentTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("video/mp4"));
            return objectResult;
        }
    }
}