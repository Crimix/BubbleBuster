using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public static class FileHelper
    {

        public static void WriteStringToFile(string folderName, string fileName, string data)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, data);
        }

        public static string ReadStringFromFile(string folderName, string fileName)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            return File.ReadAllText(filePath);
        }

        public static void WriteObjectToFile(string folderName, string fileName, Object data)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        }

        public static T ReadObjectFromFile<T>(string folderName, string fileName)
        {
            fileName = CheckFileName(fileName);
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }


        private static string CheckFileName(string input)
        {
            if (input.EndsWith(".txt"))
            {
                return input;
            }
            else
            {
                return input + ".txt";
            }
        }
    }
}
