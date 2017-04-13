using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;

namespace BntWeb.LimitBuy
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
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/List",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
},
                new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/ListOnPage",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
},
                 new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/Delete",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
},
                 new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/Edit",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "Edit"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
},
                 new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/EditOnPost",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "EditOnPost"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
},
                         new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/LimitBuy/GoodsListOnPage",
                                                         new RouteValueDictionary {
                                                                                      { "area", LimitBuyModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "GoodsListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", LimitBuyModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },

                         };
        }
    }
}