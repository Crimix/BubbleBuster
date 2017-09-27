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
                var friends = Web.WebHandler.MakeRequest<Friends>(RequestBuilder.BuildRequest(DataType.friendsObj, "screen_name=" + screenName , "cursor=" + cursor));
                tempList.AddRange(friends.Users);
                cursor = friends.NextCursor;
            }
            Friends result = new Friends();
            result.Users = tempList;
            return result;
        }

    }
}
