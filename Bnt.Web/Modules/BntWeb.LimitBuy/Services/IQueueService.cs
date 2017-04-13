using System;
using System.Collections.Generic;
using BntWeb.LimitBuy.ApiModels;
using BntWeb.OrderProcess.Models;

namespace BntWeb.LimitBuy.Services
{
    public interface IQueueService : ISingletonDependency
    {
        /// <summary>
        /// 是否轮到,返回1表示抢购成功,0表示抢购失败,-2表示未轮到
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <param name="errorInfo"></param>
        int IsTurn(string userId, string goodsId,out string errorInfo);

        /// <summary>
        /// 判断是否有库存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        int HasStock(string goodsId);

        /// <summary>
        /// 加入队列，返回1表示该用户已抢购成功，不可再抢,2表示抢购未开始，-1表示没有库存，0表示成功加入队列
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Enqueue(OrderModel model);

        /// <summary>
        /// 创建商品缓存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="stock"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        bool CreateGoodsCache(string goodsId, int stock, DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 判断是否已抢购成功，返回1表示已抢购成功，0表示抢购失败，-4表示未抢过，2表示抢购未开始
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        int HasBuy(string userId, string goodsId);

        /// <summary>
        /// 队列处理,队列为空则等待信号
        /// </summary>
        void Start();

        bool Stop();

        void UpdateGoodsStock(string goodsId, int stock);

        QueuuOrderInfo UpdateCacheMamager(string memberId,string goodsId);
        /// <summary>
        /// 抢购成功者列表
        /// </summary>
        /// <param name="limitBuyId"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        List<Order> GetLimitByRanking(Guid limitBuyId, out int totalCount, int pageNo = 1, int limit = 10);
    }
}
