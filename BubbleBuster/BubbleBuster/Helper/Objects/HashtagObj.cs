﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper.Objects
{
    public class HashtagObj
    {
        string name = "";
        public int neg = 0;
        public int pos = 0;
        public int bas = 0;

        public HashtagObj(string nameVal, int negVal, int baseVal, int posVal)
        {
            name = nameVal;
            neg = negVal;
            bas = baseVal;
            pos = posVal;
        }
    }
}
