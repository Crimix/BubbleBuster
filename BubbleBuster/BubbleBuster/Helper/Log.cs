using System;
using System.IO;

namespace BubbleBuster.Helper
{
    public static class Log
    {
        /// <summary>
        /// Lock object, such that the log can be locked in threads.
        /// </summary>
        public static object LOCK = new object();

        /// <summary>
        /// Writes the object to the log
        /// </summary>
        /// <param name="obj">The object</param>
        public static void Debug(object obj)
        {
            if (Constants.DEBUG)
            {
                Append(DateTime.Now + " | Debug~ " + obj.ToString());
            }
        }

        /// <summary>
        /// Writes the object to the log
        /// </summary>
        /// <param name="obj">The object</param>
        public static void Error(object obj)
        {
            Append(DateTime.Now + " | Error~ " + obj.ToString());
        }

        /// <summary>
        /// Writes the object to the log
        /// </summary>
        /// <param name="obj">The object</param>
        public static void Warn(object obj)
        {
            Append(DateTime.Now + " | Warn~ " + obj.ToString());
        }

        /// <summary>
        /// Writes the object to the log
        /// </summary>
        /// <param name="obj">The object</param>
        public static void Info(object obj)
        {
            Append(DateTime.Now + " | Info~ " + obj.ToString());
        }

        /// <summary>
        /// Clears the log, by deleting the file
        /// </summary>
        public static void ClearLog()
        {
            File.Delete(Path.GetTempPath() + @"BubbleBuster\log.txt");
        }

        //Appends the file with the supplied text and a new line.
        private static void Append(string text)
        {
            lock (LOCK)
            {
                File.AppendAllText(Constants.PROGRAM_DATA_FILEPATH + @"\log.txt", text + Environment.NewLine);
            }
        }
    }
}
