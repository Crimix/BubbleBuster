using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using QSLib;
using System.Runtime.InteropServices;

namespace RestQueueServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public void start()
        {
            Application_Start();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Thread QueueServer = new Thread(new ThreadStart(Run));
            QueueServer.Start();
        }

        private void Run()
        {
            QueueServerInstance.Instance.TaskQueue();
        }


    }
}
