using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Web.ReturnedObjects
{
    public class FollowersID
    {
        private List<int> followersIDs = new List<int>();

        public FollowersID(List<int> id)
        {
            followersIDs = id;
        }
    }
}
