using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Web
{
    public static class Mime
    {
        private static Dictionary<string, string> types = new Dictionary<string, string>()
        {
            { ".html", "text/html" }, { ".htm", "text/html" }, { ".css", "text/css" }
        };

        public static string GetFromExtension(string extension)
        {
            if (!types.ContainsKey(extension))
                return null;

            return types[extension];
        }

        public static void RegisterType(string extension, string type)
        {
            types[extension] = type;
        }
    }
}
