using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster;

namespace BubbleBuster
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestBuilder rb = new RequestBuilder();

            string returnedString = Web.WebHandler.MakeRequest(rb.buildRequest(DataType.friendsId, "pewdiepie"));
            Console.WriteLine(returnedString);

        }
    

        private static void SaveDataToFile(string folderName, string fileName, string data)
        {
            string folderPath = Path.GetTempPath() + folderName;
            string filePath = Path.GetTempPath() + folderName + @"\" + fileName;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(filePath, data);
        }
    }
}
