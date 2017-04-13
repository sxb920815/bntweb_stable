using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Merchant.Models
{
    [Table(KeyGenerator.TablePrefix + "View_Activity_Merchants")]
    public class MerchantActivity
    {
        public Guid Id { set; get; }

        public string Title { set; get; }

        public string Postion { set; get; }

        public string Description { set; get; }

        public int ApplyNum { set; get; }

        public int LimitNum { set; get; }

        public string StartTime { set; get; }

        public string EndTime { set; get; }

        public string OpenTime { set; get; }

        public DateTime CreateTime { set; get; }

        public int Type { set; get; }

        public int Status { set; get; }

        public bool IsBest { set; get; }
    }
}