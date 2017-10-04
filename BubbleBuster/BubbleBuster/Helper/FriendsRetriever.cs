using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    class FriendsRetriever
    {
        private static FriendsRetriever _instance;

        private FriendsRetriever()
        {

        }

        public static FriendsRetriever Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FriendsRetriever();
                }
                return _instance;
            }
        }

        public Friends getFriends(string screenName)
        {
            List<User> tempList = new List<User>();
            long cursor = -1;
            
            while (cursor != 0)
            {
                var friends = new WebHandler().MakeRequest<Friends>(RequestBuilder.BuildRequest(DataType.friendsObj, "screen_name=" + screenName , "count=200",  "cursor=" + cursor));
                tempList.AddRange(friends.Users);
                cursor = friends.NextCursor;
                friends = null;
            }
            Friends result = new Friends();
            result.Users = tempList;
            tempList = null;
            return result;
        }

    }
}
