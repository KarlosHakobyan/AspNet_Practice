using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    internal class CodeLauncher
    {
        public static void Launch()
        {
            string path = "text.txt";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Hello my C# friends!");
                writer.WriteLine("Learning Streams");
            }

            using FileStream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read
            );

            Console.WriteLine($"CanWrite: {stream.CanWrite}");
            Console.WriteLine($"CanRead: {stream.CanRead}");
            Console.WriteLine($"CanSeek: {stream.CanSeek}");
            Console.WriteLine($"Length: {stream.Length}");
            Console.WriteLine($"Position: {stream.Position}");

            byte[] buffer = new byte[10];
            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(
                buffer, 0, buffer.Length)) > 0)
            {
                totalBytesRead += bytesRead;

                Console.WriteLine(
                    $"Read: {bytesRead} bytes"
                );
            }

            Console.WriteLine(
                $"Total bytes read: {totalBytesRead}"
            );

            stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new StreamReader(stream);

            string content = reader.ReadToEnd();

            Console.WriteLine("--- File Body ---");
            Console.WriteLine(content);
        }
    }
}
