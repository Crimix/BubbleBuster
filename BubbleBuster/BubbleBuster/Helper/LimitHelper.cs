using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBuster.Helper
{
    public class LimitHelper
    {

        private static LimitHelper _instance;

        private LimitHelper()
        {

        }

        public static LimitHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LimitHelper();
                }
                return _instance;
            }
        }



        public int FriendsIdsCallsRemaining { get; private set; }
        public int FriendsObjectCallsRemaining { get; private set; }
        public int TweetCallsRemaining { get; private set; }
        public int LimitCallsRemaining { get; private set; }
        public long FriendsIdsCallsReset { get; private set; }
        public long FriendsObjectCallsReset { get; private set; }
        public long TweetCallsReset { get; private set; }
        public long LimitCallsReset { get; private set; }

        private Limit Limit { get; set; }

        public void SetLimit(Limit limit)
        {
            Limit = limit;
            InitPropertises();
        }


        private void InitPropertises()
        {
            TweetCallsRemaining = Limit.Resources.Statuses.UserTimeLine.Remaining;
            FriendsIdsCallsRemaining = Limit.Resources.Friends.Ids.Remaining;
            FriendsObjectCallsRemaining = Limit.Resources.Friends.List.Remaining;
            LimitCallsRemaining = Limit.Resources.Application.Status.Remaining;

            TweetCallsReset = Limit.Resources.Statuses.UserTimeLine.Reset;
            FriendsIdsCallsReset = Limit.Resources.Friends.Ids.Reset;
            FriendsObjectCallsReset = Limit.Resources.Friends.List.Reset;
            LimitCallsReset = Limit.Resources.Application.Status.Reset;
        }

        public bool AllowedToMakeRequest(DataType type)
        {
            switch (type)
            {
                case DataType.friendsId:
                    return (0 + Constants.REMAINING_OFFSET) < FriendsIdsCallsRemaining;
                case DataType.friendsObj:
                    return (0 + Constants.REMAINING_OFFSET) < FriendsObjectCallsRemaining;
                case DataType.tweets:
                    return (0 + Constants.REMAINING_OFFSET) < TweetCallsRemaining;
                case DataType.limit:
                    return (0 + Constants.REMAINING_OFFSET) < LimitCallsRemaining;
                default:
                    return false;
            }
        }

        public TimeSpan GetResetTime(DataType type)
        {
            long resetTime = 0;
            switch (type)
            {
                case DataType.friendsId:
                    resetTime = FriendsIdsCallsReset;
                    break;
                case DataType.friendsObj:
                    resetTime = FriendsObjectCallsReset;
                    break;
                case DataType.tweets:
                    resetTime = TweetCallsReset;
                    break;
                case DataType.limit:
                    resetTime = LimitCallsReset;
                    break;
            }

            DateTime now = DateTime.Now;
            DateTime to = DateTimeOffset.FromUnixTimeSeconds(resetTime).DateTime.ToLocalTime();
            return to-now;
        }

        public void SubtractFrom(DataType type)
        {
            switch (type)
            {
                case DataType.friendsId:
                    FriendsIdsCallsRemaining--;
                    break;
                case DataType.friendsObj:
                    FriendsObjectCallsRemaining--;
                    break;
                case DataType.tweets:
                    TweetCallsRemaining--;
                    break;
                case DataType.limit:
                    LimitCallsRemaining--;
                    break;
            }
        }

    }
}
