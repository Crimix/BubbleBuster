using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This object is used to represent a twitter user, combined with a value for their political affiliation.
/// If the affiliation is = '-1', the user is regarded as left-wing, and '1' is right-wing rightWing
/// </summary>
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
