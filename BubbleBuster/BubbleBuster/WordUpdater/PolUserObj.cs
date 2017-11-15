using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.WordUpdater
{
    public class PolUserObj
    {
        public PolUserObj(long id, int aff)
        {
            twitterId = id;
            affiliation = aff;
        }

        public long twitterId = 0;
        public int affiliation = 0; //-1 = leftWing, 1 = rightWing
    }
}
