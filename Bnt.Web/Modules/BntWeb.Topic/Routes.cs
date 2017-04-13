/* 
    ======================================================================== 
        File name：		Routes
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:45:20
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;

namespace BntWeb.Topic
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Topics/Page",
                                                         new RouteValueDictionary {
                                                                                      { "area", TopicModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", TopicModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Topics/Delete",
                                                         new RouteValueDictionary {
                                                                                      { "area", TopicModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", TopicModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Topics/SetHot",
                                                         new RouteValueDictionary {
                                                                                      { "area", TopicModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "SetHot"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", TopicModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Topics/Detail",
                                                         new RouteValueDictionary {
                                                                                      { "area", TopicModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Detail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", TopicModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Topics",
                                                         new RouteValueDictionary {
                                                                                      { "area", TopicModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", TopicModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}