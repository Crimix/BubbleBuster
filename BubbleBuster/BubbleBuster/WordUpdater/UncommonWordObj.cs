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
            LeftPosCount = 0;
            LeftNegCount = 0;
            LeftNeuCount = 0;
            RightPosCount = 0;
            RightNegCount = 0;
            RightNeuCount = 0;
            CenterPosCount = 0;
            CenterNegCount = 0;
            CenterNeuCount = 0;
        }

        public string word = "";
        public int LeftPosCount { get; set; }
        public int RightPosCount { get; set; }
        public int CenterPosCount { get; set; }
        public int LeftNegCount { get; set; }
        public int RightNegCount { get; set; }
        public int CenterNegCount { get; set; }
        public int LeftNeuCount { get; set; }
        public int RightNeuCount { get; set; }
        public int CenterNeuCount { get; set; }

    }
}
