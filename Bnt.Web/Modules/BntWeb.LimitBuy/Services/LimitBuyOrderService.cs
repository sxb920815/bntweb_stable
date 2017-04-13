using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BntWeb.Data.Services;
using BntWeb.LimitBuy.ViewModels;
using BntWeb.Mall;
using BntWeb.Mall.Models;

namespace BntWeb.LimitBuy.Services
{

    public class LimitBuyOrderService : ILimitBuyOrderService
    {
        public List<ViewSingleGoods> GetSingleGoodsList(int pageIndex, int pageSize, string name, out int totalCount, params OrderModelField[] orderModelField)
        {
            using (var db = new MallDbContext())
            {
                var query = from sg in db.SingleGoods
                            join g in db.Goods on sg.GoodsId equals g.Id
                            join a in db.SingleGoodsAttributes on sg.Id equals a.SingleGoodsId
                            where g.Status != GoodsStatus.Delete
                            group new { sg, g, a } by sg.Id into s

                            select new ViewSingleGoods()
                            {
                                Id = s.Select(x => x.sg.Id).FirstOrDefault(),
                                Price = s.Select(x => x.sg.Price).FirstOrDefault(),
                                Name = s.Select(x => x.g.Name).FirstOrDefault(),
                                Stock = s.Select(x => x.sg.Stock).FirstOrDefault(),
                                GoodsId = s.Select(x => x.sg.GoodsId).FirstOrDefault(),
                                lists = s.Select(x => x.a.AttributeValue).ToList()
                            };

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                //创建表达式变量参数
                var parameter = Expression.Parameter(typeof(ViewSingleGoods), "o");
                if (orderModelField != null && orderModelField.Length > 0)
                {
                    for (int i = 0; i < orderModelField.Length; i++)
                    {
                        //根据属性名获取属性
                        var property = typeof(ViewSingleGoods).GetProperty(orderModelField[i].PropertyName);
                        //创建一个访问属性的表达式
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);
                        var OrderName = orderModelField[i].IsDesc ? "OrderByDescending" : "OrderBy";
                        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(ViewSingleGoods), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                        query = query.Provider.CreateQuery<ViewSingleGoods>(resultExp);
                    }
                }
                totalCount = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            }
        }

    }
}