using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QSLib;
using RestQueueServer.Models;

namespace RestQueueServer.Controllers
{
    public class TwitterAccController : ApiController
    {
        public Response Post([FromForm] TwitterAcc tAcc)
        {
            QSLib.QueueServerInstance.Instance.AddTask(tAcc);



            return new Response("hej");
        }
    }
}
