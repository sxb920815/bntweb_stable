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


namespace BntWeb.HelpCenter
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
                                                         "HelpCenter/{id}",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "HelpCenter"},
                                                                                      { "action", "HelpInfo"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps/Page",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps/Delete",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps/Edit",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Edit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps/Create",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Create"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Helps/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", HelpCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", HelpCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}