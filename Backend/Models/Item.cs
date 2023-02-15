namespace Unomart.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public float ItemPrice { get; set; }
        public string ItemCategory { get; set; }
        public string ItemImage { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemDescription { get; set; }

        public Item() { }

        public Item(Item i)
        {
            ID= i.ID;
            ItemName= i.ItemName;
            ItemPrice= i.ItemPrice;
            ItemCategory= i.ItemCategory;
            ItemImage= i.ItemImage;
            ItemQuantity= i.ItemQuantity;
            ItemDescription= i.ItemDescription;
        }
    }
}
