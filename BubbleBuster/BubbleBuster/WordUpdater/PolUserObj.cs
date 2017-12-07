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

        /// <summary>
        /// The id of an pol user
        /// </summary>
        public long TwitterId { get; set; } = 0;

        /// <summary>
        /// The affiliation of the pol user
        /// </summary>
        public int Affiliation { get; set; } = 0; //-1 = leftWing, 1 = rightWing

        /// <summary>
        /// Converts a Pol user to a user
        /// </summary>
        /// <returns></returns>
        public User ToUser()
        {
            return new User() { Id = TwitterId };
        }
    }
}
