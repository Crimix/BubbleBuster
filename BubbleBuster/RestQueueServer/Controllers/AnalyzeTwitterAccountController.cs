using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QSLib;
using RestQueueServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace RestQueueServer.Controllers
{
    public class AnalyzeTwitterAccountController : ApiController
    {
        public Response Post([Microsoft.AspNetCore.Mvc.FromBody] TwitterAcc tAcc) //handles the request and adds it to the queue of requests.
        {
            bool wasSuccessful = QSLib.QueueServerInstance.Instance.AddTask(tAcc);

            if (wasSuccessful)
            {
                return new Response("Job successfully created.");
            }

            else
            {
                return new Response("Job failed successfully.");
            }

            
        }
    }
}
