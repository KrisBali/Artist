using ArtistWebLayer.App_Start;
using ArtistWebLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ArtistWebLayer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configure(UnityConfig.Register);

           // Instanciation of the automapper profile inside.
            AutoMapperWebProfile.Run();         

        }
    }
}
