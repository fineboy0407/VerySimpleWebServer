using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebServer
{
    public class Config
    {
        public int Port { get; set; }
        public string DocumentRoot { get; set; }
        public string Index { get; set; }

        public static Config Default
        { 
            get
            {
                return new Config
                {
                    Port = 80,
                    DocumentRoot = Path.Combine(Environment.CurrentDirectory, "www"),
                    Index = "index.html",
                };
            }
        }
    }
}
