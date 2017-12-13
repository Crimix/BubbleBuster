using BubbleBuster.Helper.Objects;
using BubbleBuster.Web.ReturnedObjects.RateLimit;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BubbleBuster.Helper
{
    public class LimitHelper
    {
        //Such that each auth object gets its own pool of limits. This is also the dictionary which holds each instance of the limit helper.
        private static Dictionary<AuthObj, LimitHelper> limitsByAuth = new Dictionary<AuthObj, LimitHelper>();
        private static object _lock = new object();


        /// <summary>
        /// Such that the Instance property must be used.
        /// </summary>
        private LimitHelper()
        {

        }

        /// <summary>
        /// Returns the limit helper for the auth object.
        /// If none exist. creates one and returns it.
        /// </summary>
        /// <param name="auth">The auth object</param>
        /// <returns>A limit helper instance</returns>
        public static LimitHelper Instance(AuthObj auth)
        {
            if (limitsByAuth.ContainsKey(auth))
            {
                return limitsByAuth[auth];
            }
            else
            {
                lock (_lock)
                {
                    if (!limitsByAuth.ContainsKey(auth))
                    {
                        limitsByAuth.Add(auth, new LimitHelper());
                    }
                }
                return limitsByAuth[auth];
            }
        }

        //Private varibles used in the propertise, this is because of the need to decrement as a atomic operation because of the threads
        private int _FriendsIdsCallsRemaining = 0;
        private int _FriendsObjectCallsRemaining = 0;
        private int _TweetCallsRemaining = 0;
        private int _LimitCallsRemaining = 0;
        private int _UserCallsRemaining = 0;

        //Propertise for how many request are left
        public int FriendsIdsCallsRemaining { get { return _FriendsIdsCallsRemaining; } private set { _FriendsIdsCallsRemaining = value; } }
        public int FriendsObjectCallsRemaining { get { return _FriendsObjectCallsRemaining; } private set { _FriendsObjectCallsRemaining = value; } }
        public int TweetCallsRemaining { get { return _TweetCallsRemaining; } private set { _TweetCallsRemaining = value; } }
        public int LimitCallsRemaining { get { return _LimitCallsRemaining; } private set { _LimitCallsRemaining = value; } }
        public int UserCallsRemaining { get { return _UserCallsRemaining; } private set { _UserCallsRemaining = value; } }

        //Propertise for when each request reset
        public long FriendsIdsCallsReset { get; private set; }
        public long FriendsObjectCallsReset { get; private set; }
        public long TweetCallsReset { get; private set; }
        public long LimitCallsReset { get; private set; }
        public long UserCallsReset { get; private set; }

        /// <summary>
        /// Inits the different propertise.
        /// </summary>
        /// <param name="limit">THe limits</param>
        public void InitPropertises(Limit limit)
        {
            TweetCallsRemaining = limit.Resources.Statuses.UserTimeLine.Remaining;
            FriendsIdsCallsRemaining = limit.Resources.Friends.Ids.Remaining;
            FriendsObjectCallsRemaining = limit.Resources.Friends.List.Remaining;
            LimitCallsRemaining = limit.Resources.Application.Status.Remaining;
            UserCallsRemaining = limit.Resources.Users.Status.Remaining;

            TweetCallsReset = limit.Resources.Statuses.UserTimeLine.Reset;
            FriendsIdsCallsReset = limit.Resources.Friends.Ids.Reset;
            FriendsObjectCallsReset = limit.Resources.Friends.List.Reset;
            LimitCallsReset = limit.Resources.Application.Status.Reset;
            UserCallsReset = limit.Resources.Users.Status.Reset;
        }

        /// <summary>
        /// Returns if the request is allowed to be made, depending on the type of it
        /// </summary>
        /// <param name="type">The request type</param>
        /// <returns>True if allowed</returns>
        public bool AllowedToMakeRequest(RequestType type)
        {
            switch (type)
            {
                case RequestType.friendsId:
                    return (0 + Constants.REMAINING_OFFSET) < FriendsIdsCallsRemaining;
                case RequestType.friendsObj:
                    return (0 + Constants.REMAINING_OFFSET) < FriendsObjectCallsRemaining;
                case RequestType.tweets:
                    return (0 + Constants.REMAINING_OFFSET) < TweetCallsRemaining;
                case RequestType.limit:
                    return (0 + Constants.REMAINING_OFFSET) < LimitCallsRemaining;
                case RequestType.user:
                    return (0 + Constants.REMAINING_OFFSET) < UserCallsRemaining;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a timespan for how long before the request type is allowed again.
        /// </summary>
        /// <param name="type">The request type</param>
        /// <returns>Timespan</returns>
        public TimeSpan GetResetTime(RequestType type)
        {
            long resetTime = 0;
            switch (type)
            {
                case RequestType.friendsId:
                    resetTime = FriendsIdsCallsReset;
                    break;
                case RequestType.friendsObj:
                    resetTime = FriendsObjectCallsReset;
                    break;
                case RequestType.tweets:
                    resetTime = TweetCallsReset;
                    break;
                case RequestType.limit:
                    resetTime = LimitCallsReset;
                    break;
                case RequestType.user:
                    resetTime = UserCallsReset;
                    break;
            }

            DateTime now = DateTime.Now;
            DateTime to = DateTimeOffset.FromUnixTimeSeconds(resetTime).DateTime.ToLocalTime();
            return to - now;
        }

        /// <summary>
        /// Returns a DateTime object for when the request is allowed again. 
        /// </summary>
        /// <param name="type">The request</param>
        /// <returns>A DateTime</returns>
        public DateTime GetResetDateTime(RequestType type)
        {
            long resetTime = 0;
            switch (type)
            {
                case RequestType.friendsId:
                    resetTime = FriendsIdsCallsReset;
                    break;
                case RequestType.friendsObj:
                    resetTime = FriendsObjectCallsReset;
                    break;
                case RequestType.tweets:
                    resetTime = TweetCallsReset;
                    break;
                case RequestType.limit:
                    resetTime = LimitCallsReset;
                    break;
                case RequestType.user:
                    resetTime = UserCallsReset;
                    break;
            }

            return DateTimeOffset.FromUnixTimeSeconds(resetTime).DateTime.ToLocalTime();
        }

        /// <summary>
        /// Subtracts atomic one from the counter of the request pool for the type.
        /// </summary>
        /// <param name="type">The request type</param>
        public void SubtractFrom(RequestType type)
        {
            switch (type)
            {
                case RequestType.friendsId:
                    Interlocked.Decrement(ref _FriendsIdsCallsRemaining);
                    break;
                case RequestType.friendsObj:
                    Interlocked.Decrement(ref _FriendsObjectCallsRemaining);
                    break;
                case RequestType.tweets:
                    Interlocked.Decrement(ref _TweetCallsRemaining);
                    break;
                case RequestType.limit:
                    Interlocked.Decrement(ref _LimitCallsRemaining);
                    break;
                case RequestType.user:
                    Interlocked.Decrement(ref _UserCallsRemaining);
                    break;
            }
        }
    }
}
