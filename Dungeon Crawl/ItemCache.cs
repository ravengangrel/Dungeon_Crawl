using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class ItemCache
    {
        public Dictionary<Item, int> items = new Dictionary<Item, int>();

        public void addItem(Item i, int amt)
        {
            if (items.ContainsKey(i))
            {
                items[i] += amt;
            }
            else
            {
                items.Add(i.clone(), amt);
            }
        }
    }
}
