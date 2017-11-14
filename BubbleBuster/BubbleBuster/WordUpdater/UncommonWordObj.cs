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
        }

        public string word = "";
        public int leftCount = 0;
        public int rightCount = 0;
        public int centerCount = 0;
    }
}
