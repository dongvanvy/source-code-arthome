using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("api/Upload/{filename}")]
        public Task<HttpResponseMessage> Upload(string filename)
        {
            filename = filename + ".jpg";
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {
                    FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);
                    if (File.Exists(Path.Combine(root, filename)))
                    {
                        File.Delete(Path.Combine(root, filename));
                        File.Move(finfo.FullName, Path.Combine(root, filename));
                    }
                    else
                    {
                        File.Move(finfo.FullName, Path.Combine(root, filename));
                    }
                    
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("File uploaded.")
                    };
                }
            );
            return task;
        }
    }
}
