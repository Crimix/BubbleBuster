using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using QSLib;
using RestQueueServer;



namespace QueueServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            QueueServerInstance.Instance.TaskQueue();
            (new RestQueueServer.WebApiApplication()).start();
            
        }       
    }
}
