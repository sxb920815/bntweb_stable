/* 
    ======================================================================== 
        File name：		Routes
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 14:52:28
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;


namespace BntWeb.Activity
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
                                                         HostConstConfig.AdminDirectory + "/Activitys",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Page",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Delete",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Edit",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "EditActivity"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Create",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "CreateActivity"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Types",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "TypeList"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/Types/Edit",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "EditType"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Activitys/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", ActivityModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", ActivityModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}