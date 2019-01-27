using ArtistBusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.Injection;

namespace ArtistWebLayer.App_Start
{
    public static class UnityConfig
    {

        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            // Register the two interfaces used.
            container.RegisterType<IArtistBLL, ArtistBLL>();
            container.RegisterType<IArtistReleasesBLL, ArtistReleasesBLL>(); 

            config.DependencyResolver = new UnityResolver(container);

        }


    }
}