using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebServer.Web;

namespace WebServer.Http
{
    public class Response
    {
        private readonly Dictionary<int, string> responseCodes = new Dictionary<int, string>()
        {
            { 200, "OK" }, { 301, "Permanently Moved" }, { 302, "Temporarily Moved" }, { 403, "Forbidden" }, { 404, "Not Found" }, { 418, "I'm a Teapot" },
            { 500, "Internal Server Error" }, { 502, "Bad Gateway" }, { 503, "Service Temporarily Unavailable" }
        };

        public int Status { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public static Response FromFilePath(string filePath)
        {
            int status = 200;

            if (!File.Exists(filePath))
                status = 404;

            string filePathExtension = Path.GetExtension(filePath);
            return new Response(status, Mime.GetFromExtension(filePathExtension));
        }

        private Response()
        {
            Headers = new Dictionary<string, string>();
        }

        public Response(int status, string contentType = null)
            : this()
        {
            Status = status;

            if (contentType != null)
                Headers.Add("Content-Type", contentType);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("HTTP/1.1 {0} {1}\r\n", Status, responseCodes[Status] ?? string.Empty));

            foreach (KeyValuePair<string, string> pair in Headers)
                builder.Append(string.Format("{0}: {1}\r\n", pair.Key, pair.Value));

            builder.Append("\r\n");
            builder.Append(Body);

            return builder.ToString();
        }
    }
}
