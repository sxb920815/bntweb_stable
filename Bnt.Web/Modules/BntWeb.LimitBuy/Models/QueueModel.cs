namespace BntWeb.LimitBuy.Models
{
    public class QueueModel
    {
        public string UserId { get; set; }

        public string GoodsId { get; set; }

        public bool IsBuy { get; set; }

        public bool IsTurn { get; set; }
    }
}
