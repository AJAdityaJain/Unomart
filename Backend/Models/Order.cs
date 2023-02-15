namespace Unomart.Models
{
    public class OrderItem
    {
        public string IID { get; set; }
        public string OID { get; set; }
        public int ItemAmount { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public float ItemCost { get; set; }
    }
    public class Order
    {
        public string OID { get; set; }
        public string UID { get; set; }
        public string DeliveryAddress { get; set; }
        public float DeliveryCharge { get; set; }
        public float Discount { get; set; }
        public float Total { get; set; }
        public int Items { get; set; }
        public DateTime DateTime { get; set; }
    }
}
