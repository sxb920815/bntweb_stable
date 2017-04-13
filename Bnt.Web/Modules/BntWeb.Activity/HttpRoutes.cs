using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Mvc.Routes;
using BntWeb.WebApi.Routes;

namespace BntWeb.Activity
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
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/New",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity",
                                                            action = "NewestList"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/MyCreate",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity",
                                                            action = "MyCreateActivitys"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/MyApply",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity",
                                                            action = "MyApplyActivitys"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/Type",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity",
                                                            action = "TypeListCanPublish"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/Search",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity",
                                                            action = "Search"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/{activityId}",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Activity"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/{activityId}/Apply",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "ActivityApply"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/{activityId}/CancelApply",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "ActivityApply"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/{sourceId}/Comments",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Comment"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Activity/{sourceId}/Comments/{id}",
                                                        Defaults = new
                                                        {
                                                            area = ActivityModule.Area,
                                                            controller = "Comment"
                                                        }
                                                    }
                         };
        }

    }
}
