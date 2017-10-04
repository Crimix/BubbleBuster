using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class UrlHeper
    {

        public void CutUrl()
        {
            FileStream file = File.OpenRead(@"C:\Users\Bruger\AppData\Local\Temp\BubbleBuster\url.txt");
            FileStream newfile = File.Create(@"C:\Users\Bruger\AppData\Local\Temp\BubbleBuster\urlCleanen.txt");
            StreamWriter writer = new StreamWriter(newfile);
            StreamReader reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                string temp = reader.ReadLine();
                string[] words = { "www.", "http://", "https://", "www1." };
                string[] endWorkds = { ".com/", ".org/", ".co.uk/", ".net/", ".mit.edu/", ".ca/", ".org.il/", ".edu/", ".us/" };
                try
                {
                    foreach (var item in words)
                    {
                        if (temp.Contains(item))
                            temp = temp.Substring(temp.IndexOf(item) + item.Length);
                    }

                    foreach (var item in endWorkds)
                    {
                        if (temp.Contains(item))
                            temp = temp.Remove(temp.IndexOf(item) + item.Length);
                    }


                }
                catch (Exception)
                {

                }

                writer.WriteLine(temp);
            }

            reader.Dispose();
            writer.Dispose();
            reader.Close();
            writer.Close();
            file.Close();
            newfile.Close();
        }
    }
}
