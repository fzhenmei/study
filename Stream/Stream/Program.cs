using System;
using System.Net;
using System.Text;
using System.IO;

namespace Stream
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Please enter URL");
                var url = Console.ReadLine();
                if(string.IsNullOrEmpty(url))
                    return;
                WebResposneTest(url);    
            } while (true);
            
        }

        private static void WebResposneTest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            
            var response = request.GetResponse();
            var buffer = GetBytes(response);
            var html = GetString(buffer, ((HttpWebResponse)response).CharacterSet);

            Console.WriteLine(html);
        }

        private static string GetString(byte[] buffer, string charSet)
        {
            if (charSet == "ISO-8859-1")
            {
                charSet = GetEncodingFromBody(buffer);
            }
            var encoding = Encoding.GetEncoding(charSet);

            return encoding.GetString(buffer);
        }

        private static string GetEncodingFromBody(byte[] buffer)
        {
            string encodingName = null;
            string dataAsAscii = Encoding.ASCII.GetString(buffer);
            
            int i = dataAsAscii.IndexOf("charset=");
            if (i != -1)
            {
                int j = dataAsAscii.IndexOf("\"", i);
                if (j != -1)
                {
                    int k = i + 8;
                    encodingName = dataAsAscii.Substring(k, (j - k) + 1);
                    var chArray = new char[2] { '>', '"' };
                    encodingName = encodingName.TrimEnd(chArray);
                }
            }

            return encodingName;
        }

        public static byte[] GetBytes(WebResponse response)
        {
            var length = (int)response.ContentLength;

            var memoryStream = new MemoryStream();
            var buffer = new byte[0x100];

            var rs = response.GetResponseStream();
            for (var i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
            {
                memoryStream.Write(buffer, 0, i);
            }
            rs.Close();

            return memoryStream.ToArray();
        }

        private static System.IO.Stream CopyStream(System.IO.Stream input)
        {
            const int bufferLength = 255;
            var buffer = new byte[bufferLength];
            var output = new MemoryStream();
            while (true)
            {
                var count = input.Read(buffer, 0, bufferLength);
                if (count <= 0)
                {
                    break;
                }
                output.Write(buffer, 0, count);
            }
            return output;
        }

        private static void MemoryStreamTest()
        {
            var text = "Hello world! 你好，世界！";
            var buffer = Encoding.UTF8.GetBytes(text);
            var streamData1 = new MemoryStream(buffer);
            var streamData2 = new MemoryStream(buffer);

            using (var reader = new StreamReader(streamData1, Encoding.UTF8))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            using (var reader = new StreamReader(streamData2, Encoding.UTF8))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
