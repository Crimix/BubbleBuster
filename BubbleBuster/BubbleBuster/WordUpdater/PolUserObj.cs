using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BubbleBuster.Web.ReturnedObjects;

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
            TwitterId = id;
            Affiliation = aff;
        }

        public long TwitterId { get; set; } = 0;
        public int Affiliation { get; set; } = 0; //-1 = leftWing, 1 = rightWing

        public User ToUser()
        {
            return new User() { Id = TwitterId };
        }
    }
}
