using BubbleBuster.Helper.Objects;
using BubbleBuster.Web;
using BubbleBuster.Web.ReturnedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class FriendsRetriever
    {
        private static FriendsRetriever _instance; //Variable for the singleton instance

        //To make it a singleton 
        private FriendsRetriever()
        {

        }

        /// <summary>
        /// Method to get access to the FriendsRetriever.
        /// Is the only static method, because it is not possible to create an instance outside of this class
        /// </summary>
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

        /// <summary>
        /// Returns the user's friends by delegating the work to the private method with either the user's id or screen name
        /// depending upon which one is present in the user object.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="auth">The auth object</param>
        /// <returns>A Friends object</returns>
        public Friends GetFriends(User user, AuthObj auth)
        {
            if (user.Id != 0)
            {
                return GetFriends("user_id=" + user.Id, auth);
            }
            else if (string.IsNullOrWhiteSpace(user.Name))
            {
                return GetFriends("screen_name=" + user.Name, auth);
            }
            else
            {
                return new Friends();
            }
        }

        /// <summary>
        /// Retrieves the user's friends using the parameter
        /// </summary>
        /// <param name="parameter">The parameter used to select the user</param>
        /// <param name="auth">The auth object</param>
        /// <returns></returns>
        private Friends GetFriends(string parameter, AuthObj auth)
        {
            List<User> tempList = new List<User>();
            long cursor = -1;
            
            while (cursor != 0)
            {
                var friends = new WebHandler(auth).TwitterGetRequest<Friends>(TwitterRequestBuilder.BuildRequest(RequestType.friendsObj, auth, parameter, "count=200",  "cursor=" + cursor));
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
