/* 
    ======================================================================== 
        File name：		HttpRoutes
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:46:32
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BntWeb.Mvc.Routes;
using BntWeb.WebApi.Routes;

namespace BntWeb.Merchant
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
                                                        RouteTemplate = "Api/v1/Merchant/Type",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "MerchantType"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Merchant/Product",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "MerchantProduct",
                                                            action = "GetMerchantProducts"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Merchant/HomeProduct",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "MerchantProduct",
                                                            action = "GetHomeMerchant"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Merchant",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "Merchant",
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Merchant/{merchantId}",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "Merchant"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Home/SearchAll",
                                                        Defaults = new
                                                        {
                                                            area = MerchantModule.Area,
                                                            controller = "MerchantAcivity"
                                                        }
                                                    }
                         };
        }

    }
}