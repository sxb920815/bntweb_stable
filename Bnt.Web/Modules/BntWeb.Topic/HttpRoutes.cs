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

namespace BntWeb.Topic
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
                                                        RouteTemplate = "Api/v1/Topic/MyCreate",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Topic",
                                                            action = "MyCreateTopic"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/MyParticipate",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Topic",
                                                            action = "MyParticipateTopic"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Topic"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/{topicId}",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Topic"
                                                        }
                                                    },

                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/{topicId}/Like",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Like"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/{sourceId}/Comment",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Comment"
                                                        }
                                                    },
                             new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Topic/{sourceId}/Comment/{id}",
                                                        Defaults = new
                                                        {
                                                            area = TopicModule.Area,
                                                            controller = "Comment"
                                                        }
                                                    }
                         };
        }

    }
}