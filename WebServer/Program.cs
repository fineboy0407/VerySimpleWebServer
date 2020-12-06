using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Http;

namespace WebServer
{
    public class Program
    {
        public static Logger Logger { get; set; }
        public static Config Config { get; set; }

        private static void Main()
        {
            Logger = new Logger();
            
            string configFilePath = Path.Combine(Environment.CurrentDirectory, "data", "config.json");
            Logger.Write(string.Format("Reading config from {0}...", configFilePath));
            
            if (File.Exists(configFilePath))
            {
                string text = File.ReadAllText(configFilePath);
                Config = JsonConvert.DeserializeObject<Config>(text);
            }
            else
            {
                Config = Config.Default;
                string text = JsonConvert.SerializeObject(Config);

                Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));

                using (StreamWriter writer = File.CreateText(configFilePath))
                    writer.Write(text);

                Logger.Write(string.Format("Created new config file at {0}", configFilePath), ConsoleColor.Yellow);
            }

            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Config.Port);
            listener.Start();

            Logger.Write(string.Format("Server is now listening on port {0}!", Config.Port));

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                Logger.Write(string.Format("Incoming connection from {0}", client.Client.RemoteEndPoint));

                if (!stream.CanRead || !stream.CanWrite)
                    return;

                Request request = Request.FromStream(stream);

                Logger.Write(string.Format("{0} {1}", request.Method, request.Uri));

                string requestFileName = request.Uri == "/" ? Config.Index : request.Uri.Substring(1).Replace('/', Path.DirectorySeparatorChar);
                string requestFilePath = Path.Combine(Config.DocumentRoot, requestFileName);

                Response response = Response.FromFilePath(requestFilePath);

                response.Headers.Add("Server", "DelightedCat/1.0.0");
                response.Headers.Add("Connection", "keep-alive");

                if (File.Exists(requestFilePath))
                    response.Body = File.ReadAllText(requestFilePath);

                response.Send(stream);
                client.Close();
            }
        }
    }
}
