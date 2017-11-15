using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.WordUpdater
{
    public class UncommonWordObj
    {
        public UncommonWordObj(string wordString)
        {
            word = wordString;
            LeftCount = 0;
            RightCount = 0;
            CenterCount = 0;
        }

        public string word = "";
        public int LeftCount { get; set; }
        public int RightCount { get; set; }
        public int CenterCount { get; set; }

    }
}
