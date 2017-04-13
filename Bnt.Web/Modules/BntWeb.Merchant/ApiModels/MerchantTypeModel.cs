using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.Merchant.ApiModels
{
    public class MerchantTypeModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }

        public int MerchantsCount { get; set; }


        public List<MerchantTypeModel> ChildMerchantTypes { set; get; }

    }
}
