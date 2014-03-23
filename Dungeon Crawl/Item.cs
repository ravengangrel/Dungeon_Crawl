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

        public int foodFill = 0;

        public Boolean equippable = false;
        public Boolean bound = false;
        public Boolean discoveredBound = false;
        public Boolean edible = false;
        public Boolean consumable = false;

        public static Item[] items = new Item[75000];

        public Item(string n)
        {
            name = n;
        }

        public Item setFood(int i)
        {
            foodFill = i;
            setEdible(true);
            return this;
        }

        public Item setConsumable(Boolean b)
        {
            consumable = b;
            return this;
        }

        public Item setEdible(Boolean b)
        {
            edible = b;
            consumable = b;
            return this;
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

        public void useItem(Player p)
        {
            if (edible)
            {
                p.hunger += foodFill;
            }
        }

        public static Item get(int i)
        {
            return Item.items[i].clone();
        }

        public static void init()
        {
            items[0] = new Item("Bread").setWeight(0.2).setEquippable(false).setEdible(true).setFood(1000); //Refill 1000 food
        }

        public Boolean Equals(Item i)
        {
            //This needs to be updated whenever we add new attributes
            if (i.name != name) { return false; }
            if (i.bound != bound) { return false; } else { i.discoveredBound = discoveredBound; }
            if (i.edible != edible) { return false; }
            if (i.foodFill != foodFill) { return false; }
            return true;
        }

    }
}
