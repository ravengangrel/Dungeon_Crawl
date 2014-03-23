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
        public Boolean equipped = false;

        public int slotEquip = -1;
        public int armor = 0;

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

        /// <summary>
        /// Set the equip slot for that item
        /// </summary>
        /// <param name="i">
        /// 0- Head
        /// 1- Arms
        /// 2- Hands
        /// 3- Body
        /// 4- Greaves
        /// 5- Boots
        /// 6- Ring
        /// 7- Amulet
        /// </param>
        /// <returns></returns>
        public Item setEquipSlot(int i)
        {
            slotEquip = i;
            return this;
        }

        public Item setArmor(int i)
        {
            armor = i;
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
            items[1] = new Item("Cloth Robe").setWeight(3).setArmor(2).setEquipSlot(3).setEquippable(true); //1 defense
            items[2] = new Item("Grape").setWeight(0.02).setEquippable(false).setEdible(true).setFood(25); //Refill 25 food
            items[3] = new Item("Dark Acolyte Robe").setWeight(3).setArmor(1).setEquipSlot(3).setEquippable(true); //1 defense

            items[4] = new Item("Leather Gloves").setWeight(2).setArmor(1).setEquipSlot(2).setEquippable(true);
            items[5] = new Item("Leather Bracers").setWeight(2).setArmor(2).setEquipSlot(1).setEquippable(true);
            items[6] = new Item("Leather Tunic").setWeight(4).setArmor(3).setEquipSlot(3).setEquippable(true);
            items[7] = new Item("Leather Cap").setWeight(1).setArmor(1).setEquipSlot(0).setEquippable(true);
            items[8] = new Item("Leather Pants").setWeight(4).setArmor(2).setEquipSlot(4).setEquippable(true);
            items[9] = new Item("Leather Moccasins").setWeight(3).setArmor(1).setEquipSlot(5).setEquippable(true);
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
