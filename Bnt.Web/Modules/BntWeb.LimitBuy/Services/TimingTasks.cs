using System;
using BntWeb.Caching;
using BntWeb.Data.Services;
using BntWeb.LimitBuy.Models;
using BntWeb.Logging;
using BntWeb.Task;

namespace BntWeb.LimitBuy.Services
{
    public class TimingTasks : IBackgroundTask
    {
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly ICurrencyService _currencyService;
        private readonly IQueueService _queueService;
        private readonly ILimitSingleGoodsService _limitSingleGoodsService;

        public TimingTasks(ICacheManager cacheManager, ISignals signals, IQueueService queueService, ICurrencyService currencyService, ILimitSingleGoodsService limitSingleGoodsService)
        {
            _cacheManager = cacheManager;
            _signals = signals;
            _queueService = queueService;
            _currencyService = currencyService;
            _limitSingleGoodsService = limitSingleGoodsService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Sweep()
        {
            var hasGoods = true;
            var goodsList = new LimitSingleGoodsService().GetListForNotExpired();
            foreach (var goods in goodsList)
            {
                if (DateTime.Now.AddMinutes(2) >= goods.BeginTime && goods.EndTime > DateTime.Now)
                {
                    _queueService.CreateGoodsCache(goods.Id.ToString(), goods.Stock, goods.BeginTime, goods.EndTime);

                    _queueService.Start();

                    hasGoods = false;
                }
                if (DateTime.Now > goods.EndTime)
                {
                    goods.Status = LimitSingleGoodsStatus.End;
                    _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(goods);
                    //_currencyService.Update(goods);

                    Logger.Warning($"定时任务更新{goods.Id}状态为{LimitSingleGoodsStatus.End}");
                }
                if (DateTime.Now >= goods.BeginTime)
                {
                    goods.Status = LimitSingleGoodsStatus.InSale;
                    _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(goods);
                    //_currencyService.Update(goods);
                    Logger.Warning($"定时任务更新{goods.Id}状态为{LimitSingleGoodsStatus.InSale}");
                }

            }
            if (hasGoods)
            {
                _queueService.Stop();
            }

        }
    }
}