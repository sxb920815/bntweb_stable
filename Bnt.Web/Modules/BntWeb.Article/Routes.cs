using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;

namespace BntWeb.Article
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
            return new[]
            {
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Article/{id}",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Article"},
                            {"action", "NewsInfo"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article/Page",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article/Delete",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "Delete"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article/Edit",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "EditArticle"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article/Create",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "CreateArticle"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Article/{action}",
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area},
                            {"controller", "Admin"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", ArticleModule.Area}
                        },
                        new MvcRouteHandler())
                }
            };
        }

    }
}