using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Mvc.Routes;
using BntWeb.WebApi.Routes;

namespace BntWeb.HelpCenter 
{
    public class HttpRoutes : IHttpRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new HttpRouteDescriptor {
                                                        Priority = 20,
                                                        RouteTemplate = "Api/v1/HelpCenter/HelpCategory",
                                                        Defaults = new
                                                        {
                                                            area = HelpCenterModule.Area,
                                                            controller = "HelpCategory",
                                                            action = "GetHelpCategoriesTree"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/HelpCenter",
                                                        Defaults = new
                                                        {
                                                            area = HelpCenterModule.Area,
                                                            controller = "Help",
                                                            action="List"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority =10,
                                                        RouteTemplate = "Api/v1/HelpCenter/{helpId}",
                                                        Defaults = new
                                                        {
                                                            area = HelpCenterModule.Area,
                                                            controller = "Help",
                                                            action="HelpDetail"
                                                        }
                                                    }
                         };
        }

    }
}
