using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Item
    {
        public String name = "";
        public double weight = 0;
        public Boolean equippable = false;
        public Boolean bound = false;
        public Boolean discoveredBound = false;

        public static Item[] items = new Item[32000];

        public Item(string n)
        {
            name = n;
        }

        public Item setBound(Boolean b)
        {
            bound = b;
            return this;
        }

        public Item setWeight(double d)
        {
            weight = d;
            return this;
        }

        public Item setEquippable(Boolean b)
        {
            equippable = b;
            return this;
        }

        public Item clone()
        {
            return (Item)this.MemberwiseClone();
        }

        public static Item get(int i)
        {
            return Item.items[i].clone();
        }

        public static void init()
        {
            items[0] = new Item("Bread").setWeight(0.2).setEquippable(false);

        }
    }
}
