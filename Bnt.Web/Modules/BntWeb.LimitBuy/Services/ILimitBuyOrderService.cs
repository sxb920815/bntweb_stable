using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Data.Services;
using BntWeb.LimitBuy.ViewModels;

namespace BntWeb.LimitBuy.Services
{
    public interface ILimitBuyOrderService : IDependency
    {
        List<ViewSingleGoods> GetSingleGoodsList(int pageIndex, int pageSize, string name, out int totalCount,
           params OrderModelField[] orderModelField);


    }
}