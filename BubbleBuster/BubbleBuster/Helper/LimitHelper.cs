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
        private Limit Limit {  get;  set; }

        public void SetLimit(Limit limit)
        {
            Limit = limit;
        }

        public bool AllowedToMakeRequest(DataType type)
        {
            return false;
        }

    }
}
