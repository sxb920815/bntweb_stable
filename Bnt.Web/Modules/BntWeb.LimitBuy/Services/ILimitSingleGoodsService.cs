using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Data.Services;
using BntWeb.LimitBuy.Models;

namespace BntWeb.LimitBuy.Services
{
    public interface ILimitSingleGoodsService : IDependency
    {
        /// <summary>
        /// 根据id获取单个商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LimitSingleGoods GetItem(Guid? id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(Guid id);
        /// <summary>
        /// 修改秒杀状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateLimitSingleGoodsStatus(LimitSingleGoods model);

        /// <summary>
        /// 获取所有抢购商品
        /// </summary>
        /// <returns></returns>
        List<LimitSingleGoods> GetAll(int pageIndex, int pageSize, out int totalCount,
            params OrderModelField[] orderByExpression);

        /// <summary>
        /// 根据类别查询秒杀商品
        /// </summary>
        /// <returns></returns>
        List<LimitSingleGoods> GetListForNotExpired(int type, int pageIndex, int pageSize, out int totalCount);

        List<LimitSingleGoods> GetListForNotExpired();

        LimitSingleGoods LoadFullLimitSingleGoods(Guid singleGoodsId);
    }
}