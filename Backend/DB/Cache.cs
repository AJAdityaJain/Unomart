
using Unomart.Managers;
using Unomart.Models;

namespace Unomart.DB
{
    public class Cache
    {
        public static int LIMIT = 16;
        private Item[] Items = new Item[0];
        public bool prepared = false;

        public Cache(){}

        public void Set(Item[] items)
        {
            if (!prepared)
            {
                Items = items;
                prepared = true;
            }
        }


        public string GetImage(string itemName)
        {
            foreach (var item in Items)
            {
                if(item.ItemName == itemName)
                {
                    return item.ItemImage;
                }
            }

            return "";
        }

        public float getItemPrice(string itemName)
        {
            foreach (var item in Items)
            {
                if (item.ItemName == itemName)
                {
                    return item.ItemPrice;
                }
            }

            return 0f;
        }

        public object getItems(string code, string? query, int page)
        {
            query = query.ToUpper();

            List<Item> items = new List<Item>();
            int count = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                Item item = new(Items[i]);
                if (item.ItemName.ToUpper().StartsWith(query) || item.ItemCategory.ToUpper().StartsWith(query))
                {
                    count++;
                    if (count > LIMIT * page && count <= LIMIT * (page + 1))
                    {
                        item.ItemPrice = CurrencyManager.Convert(item.ItemPrice, code);
                        items.Add(item);
                    }
                }
            }
            var obj = new { items = items.ToArray(), pages = (float)Math.Ceiling((double)count / LIMIT) };
            return obj;
        }
        public Item getItem(string code, string? query)
        {
            foreach (var item in Items)
            {

                if(item.ItemName == query)
                {
                    Item i = new(item);
                    i.ItemPrice = CurrencyManager.Convert(item.ItemPrice, code);
                    return i;
                }
            }
            return new Item();
        }
    }
}
