using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper.Objects
{
    public class HashtagObj
    {
        public string Name { get; set; } = "";
        public int Neg { get; set; } = 0;
        public int Pos { get; set; } = 0;
        public int Bas { get; set; } = 0;

        public HashtagObj(string nameVal, int negVal, int baseVal, int posVal)
        {
            Name = nameVal;
            Neg = negVal;
            Bas = baseVal;
            Pos = posVal;
        }
    }
}
