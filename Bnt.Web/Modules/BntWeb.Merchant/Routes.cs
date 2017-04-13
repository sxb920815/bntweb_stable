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

namespace BntWeb.Merchant
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
                                                         "Merchants/Html/Detail/{id}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "H5"},
                                                                                      { "action", "MerchantDetail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "Merchants/Html/Product/{id}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "H5"},
                                                                                      { "action", "ProductDetail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Types",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "TypeList"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Products",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "MerchantProduct"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Page",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Merchant"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Delete",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Merchant"},
                                                                                      { "action", "Delete"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Detail",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Merchant"},
                                                                                      { "action", "Detail"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Merchant"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Type/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "TypeList"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "Merchant"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Merchants/Products/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MerchantModule.Area},
                                                                                      { "controller", "MerchantProduct"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MerchantModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}