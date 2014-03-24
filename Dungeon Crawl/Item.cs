using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Item
    {
        public String name = "";

        public List<string> brands = new List<string>();

        public double weight = 0;

        public int foodFill = 0;
        public int slotEquip = -1;
        public int armor = 0;
        public int maxEnchant_attack = 9;
        public int maxEnchant_block = 9;
        public int maxEnchant_crit = 9;

        public Boolean equippable = false;
        public Boolean bound = false;
        public Boolean discoveredBound = false;
        public Boolean edible = false;
        public Boolean consumable = false;
        public Boolean equipped = false;
        public Boolean weapon = false;

        public Armor equipType = Armor.DEFAULT;
        public Size size = Size.ANY;

        public static Item[] items = new Item[115000];

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
        /// 8- Weapon
        /// </param>
        /// <returns></returns>
        public Item setEquipSlot(int i)
        {
            slotEquip = i;
            return this;
        }

        public Item addBrand(string s)
        {
            brands.Add(s);
            return this;
        }

        public Item setSpecial(string s)
        {
            name = s + " " + name;
            return this;
        }

        public Item setArmorType(Armor a)
        {
            equipType = a;
            return this;
        }

        public Item setSize(Size s)
        {
            size = s;
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
            items[1] = new Item("Cloth Robe").setWeight(3).setArmor(2).setEquipSlot(3).setEquippable(true); //2 defense
            items[2] = new Item("Grape").setWeight(0.02).setEquippable(false).setEdible(true).setFood(25); //Refill 25 food
            items[3] = new Item("Dark Acolyte Robe").setWeight(3).setArmor(1).setEquipSlot(3).setEquippable(true); //1 defense

            items[4] = new Item("Leather Gloves").setWeight(2).setArmor(1).setEquipSlot(2).setEquippable(true);
            items[5] = new Item("Leather Bracers").setWeight(2).setArmor(2).setEquipSlot(1).setEquippable(true);
            items[6] = new Item("Leather Tunic").setWeight(4).setArmor(3).setEquipSlot(3).setEquippable(true);
            items[7] = new Item("Leather Cap").setWeight(1).setArmor(1).setEquipSlot(0).setEquippable(true);
            items[8] = new Item("Leather Pants").setWeight(4).setArmor(2).setEquipSlot(4).setEquippable(true);
            items[9] = new Item("Leather Moccasins").setWeight(3).setArmor(1).setEquipSlot(5).setEquippable(true);
            
            items[10] = new Item("Iron Chainmail").setWeight(5).setArmor(4).setEquipSlot(3).setEquippable(true);
            items[11] = new Item("Iron Bracers").setWeight(4).setArmor(3).setEquipSlot(1).setEquippable(true);
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

        public string compileEnchant()
        {
            if (equippable)
            {
                return "Can be enchanted to " + this.maxEnchant_attack + "," + this.maxEnchant_block + "," + this.maxEnchant_crit; 
            }
            return "";
        }
        
        public string compileTags()
        {
            string final = "";
            if (equippable)
            {
                final += "{equip}";
                if (slotEquip < 6)
                {
                    final += "{armor}";
                }
                if (slotEquip == 6 || slotEquip == 7)
                {
                    final += "{jewelry}";
                }
                if (slotEquip == 8)
                {
                    final += "{weapon}";
                }
            }
            if (bound && discoveredBound)
            {
                final += "{bound}";
            }
            if (edible)
            {
                final += "{food}";
            }
            if (consumable)
            {
                final += "{consumable}";
            }
            for (int x = 0; x < brands.Count; x++)
            {
                final += "{" + brands[x] + "}";
            }
            return final;
        }
    }
}
