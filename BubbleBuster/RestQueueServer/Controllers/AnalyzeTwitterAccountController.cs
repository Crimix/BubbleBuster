using QSLib;
using RestQueueServer.Models;
using System.Web.Http;

namespace RestQueueServer.Controllers
{
    public class AnalyzeTwitterAccountController : ApiController
    {
        /// <summary>
        /// Handles the request and adds it to the queue of requests.
        /// </summary>
        /// <param name="tAcc">The twitter account analyse reqyest</param>
        /// <returns>A response</returns>
        public Response Post([Microsoft.AspNetCore.Mvc.FromBody] TwitterAcc tAcc)
        {
            bool wasSuccessful = QSLib.QueueServerInstance.Instance.AddTask(tAcc);

            if (wasSuccessful)
            {
                return new Response("Job successfully created.");
            }

            else
            {
                return new Response("Job failed to be created.");
            }
        }
    }
}
