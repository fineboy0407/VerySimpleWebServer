using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WebServer.Http
{
    public class Request
    {
        public bool IsValid { get; set; }

        public string Method { get; set; }
        public string Uri { get; set; }

        public static Request FromStream(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int length;

            StringBuilder builder = new StringBuilder();

            do
            {
                length = stream.Read(buffer, 0, buffer.Length);
                builder.Append(Encoding.ASCII.GetString(buffer, 0, length));
            }
            while (stream.DataAvailable);

            return Parse(builder.ToString());
        }

        public static Request Parse(string input)
        {
            string[] lines = input.Split("\r\n");
            string[] status = lines[0].Split(' ');

            if (status.Length < 2)
                return null;
            
            return new Request(status[0], status[1]);
        }

        public Request(string method, string uri)
        {
            Method = method;
            Uri = uri;
        }
    }
}
