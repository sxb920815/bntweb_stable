using System.Collections.Generic;
using BntWeb.Mvc.Routes;
using BntWeb.WebApi.Routes;

namespace BntWeb.LimitBuy
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
                                                        RouteTemplate = "Api/v1/LimitBuy/RushBuy",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "RushBuy"
                                                        }
                                                    },
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/LimitBuy/Stock/{id}",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "Stock"
                                                        }
                                                    },
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/LimitBuy/GetLimitGoods",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "LimitGoodsList"
                                                        }
                                                    },
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/LimitBuy/IsTurn",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "IsTurn"
                                                        }
                                                    },
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/LimitBuy/LimitGoods/{id}",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "LimitGoods"
                                                        }
                                                    },
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/LimitBuy/BuyMember",
                                                        Defaults = new
                                                        {
                                                            area = LimitBuyModule.Area,
                                                            controller = "RushBuy",
                                                            action = "GetMemberImages"
                                                        }
                                                    }
                         };
        }

    }
}
