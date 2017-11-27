using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestQueueServer.Models
{
    public class Response
    {
        public Response(string message)
        {
            Message = message;
        }

        public string Message { get; set; }

    }
}