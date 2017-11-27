using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public static class Log
    {
        public static void Debug(object obj)
        {
            Append("Debug~ " + obj.ToString());
        }

        public static void Error(object obj)
        {
            Append("Error~ " + obj.ToString());
        }

        public static void Warn(object obj)
        {
            Append("Warn~ " + obj.ToString());
        }

        public static void Info(object obj)
        {
            Append("Info~ " + obj.ToString());
        }

        public static void ClearLog()
        {
            File.Delete(Path.GetTempPath() + @"BubbleBuster\log.txt");
        }

        async private static Task  Append(string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(Constants.PROGRAM_DATA_FILEPATH + @"\log.txt",
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
