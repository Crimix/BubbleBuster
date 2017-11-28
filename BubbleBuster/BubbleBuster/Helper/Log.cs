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

        public static string LOCK = "_LogLock";

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

        private static void Append(string text)
        {
            File.AppendAllText(Constants.PROGRAM_DATA_FILEPATH+@"\log.txt", text + Environment.NewLine);
        }
    }
}
