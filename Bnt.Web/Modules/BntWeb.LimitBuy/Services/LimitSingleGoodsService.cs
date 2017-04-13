using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.LimitBuy.Models;

namespace BntWeb.LimitBuy.Services
{
    public class LimitSingleGoodsService : ILimitSingleGoodsService
    {
        public ILogger Logger { get; set; }

        public LimitSingleGoodsService()
        {
            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 根据id获取单个商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LimitSingleGoods GetItem(Guid? id)
        {
            using (var db = new LimitBuyDbContext())
            {
                return db.LimitSingleGoods.FirstOrDefault(x => x.Id == id);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            using (var db = new LimitBuyDbContext())
            {
                var model = db.LimitSingleGoods.FirstOrDefault(x => x.Id == id);
                if (model != null)
                {
                    model.Status = LimitSingleGoodsStatus.Delete;
                    Logger.Warning($"删除秒杀商品：{model.Id},{model.SingleGoodsName}");
                }
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 修改秒杀状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLimitSingleGoodsStatus(LimitSingleGoods model)
        {
            using (var db = new LimitBuyDbContext())
            {
                var singleGoods = db.LimitSingleGoods.FirstOrDefault(x => x.Id == model.Id);
                if (model != null)
                {
                    if (singleGoods != null) singleGoods.Status = model.Status;
                }
                return db.SaveChanges() > 0;
            }

        }

        /// <summary>
        /// 获取所有抢购商品
        /// </summary>
        /// <returns></returns>
        public List<LimitSingleGoods> GetAll(int pageIndex, int pageSize, out int totalCount, params OrderModelField[] orderByExpression)
        {
            using (var db = new LimitBuyDbContext())
            {
                var list = db.LimitSingleGoods.Where(x => x.Status != LimitSingleGoodsStatus.Delete);

                //创建表达式变量参数
                var parameter = Expression.Parameter(typeof(LimitSingleGoods), "o");
                if (orderByExpression != null && orderByExpression.Length > 0)
                {
                    for (int i = 0; i < orderByExpression.Length; i++)
                    {
                        //根据属性名获取属性
                        var property = typeof(LimitSingleGoods).GetProperty(orderByExpression[i].PropertyName);
                        //创建一个访问属性的表达式
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);


                        string OrderName = orderByExpression[i].IsDesc ? "OrderByDescending" : "OrderBy";


                        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(LimitSingleGoods), property.PropertyType }, list.Expression, Expression.Quote(orderByExp));
                        list = list.Provider.CreateQuery<LimitSingleGoods>(resultExp);
                    }
                }
                totalCount = list.Count();
                return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// 根据类别查询秒杀商品
        /// </summary>
        /// <returns></returns>
        public List<LimitSingleGoods> GetListForNotExpired(int type, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new LimitBuyDbContext())
            {
                var query = db.LimitSingleGoods.Where(x => x.Status != LimitSingleGoodsStatus.Delete);
                var dt = DateTime.Now;
                switch (type)
                {
                    case 0:
                        query = query.Where(x => x.BeginTime > dt);
                        break;
                    case 1:
                        query = query.Where(x => x.BeginTime <= dt && x.EndTime >= dt);
                        break;
                    case 2:
                        query = query.Where(x => x.EndTime < dt);
                        break;
                }
                totalCount = query.Count();
                return query.OrderByDescending(x => x.Status).ThenBy(x => x.BeginTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<LimitSingleGoods> GetListForNotExpired()
        {
            using (var db = new LimitBuyDbContext())
            {
                return db.LimitSingleGoods.Where(x => x.Status != LimitSingleGoodsStatus.Delete && x.Status != LimitSingleGoodsStatus.End).ToList();
            }
        }

        public LimitSingleGoods LoadFullLimitSingleGoods(Guid singleGoodsId)
        {
            using (var dbContext = new LimitBuyDbContext())
            {
                var limitSingleGoods =
                    dbContext.LimitSingleGoods.Include("Goods").FirstOrDefault(g => g.Id.Equals(singleGoodsId));

                return limitSingleGoods;
            }
        }

    }
}