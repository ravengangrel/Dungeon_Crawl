using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Player
    {
        public string name = "";
        public string identifier = "";
        public StatSet stats;
        public Species species;
        public Class career;
        public StatusHandler status = new StatusHandler();
        public Item[] inventory = new Item[30];
        public int[] inventoryStacks = new int[30];
        public Boolean[] inventoryEquip = new Boolean[30];

        public Player(string n, Species sp, Class c)
        {
            name = n;
            species = sp;
            career = c;
            identifier = species.abbrv + career.abbrv;
            stats = species.baseStats.addStatMod(career.statMod).adjust();
            stats.xp = 0;
            addToInventory(Item.get(0), 1);
            //status.addStatus(new Status("Loot", 40000, 3, ConsoleForeground.Yellow, ConsoleBackground.Black));
        }

        public void addToInventory(Item i, int amt)
        {
            for (int x = 0; x < inventory.Length; x++)
            {
                if (inventory[x] == i)
                {
                    inventoryStacks[x] += amt;
                    return;
                }
                else
                {
                    if (inventoryStacks[x] == 0)
                    {
                        inventory[x] = i;
                        inventoryStacks[x] = amt;
                        return;
                    }
                }
            }
        }

        public void addGold(int addGold)
        {
            if (status.hasAttr("Loot"))
            {
                stats.gold += (int)((float)addGold * (1.25f + (0.05f * status.getLvl("Loot"))));
                Program.msgLog.Add("You found " + addGold + " gold, but managed to scrounge up " + (((int)((float)addGold * (1.25f + (0.05f * status.getLvl("Loot"))))) - addGold) + " more gold!");
            }
            else
            {
               stats.gold += addGold;
               Program.msgLog.Add("You found " + addGold + " gold!");
            }
        }

        public void WriteStats()
        {
            Console.WriteLine("Health: " + stats.health);
            Console.WriteLine("Mana: " + stats.mana);
            Console.WriteLine("Strength: " + stats.strength);
            Console.WriteLine("Dexterity: " + stats.dexterity);
            Console.WriteLine("Intelligence: " + stats.intelligence);
            Console.WriteLine("Wisdom: " + stats.wisdom);
        }

        public void WriteRPGStats(int x, int y)
        {
            ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Health: " + stats.health + "/" + stats.maxHealth);
            Console.SetCursorPosition(x, y + 1);
            Console.WriteLine("Mana: " + stats.mana + "/" + stats.maxMana);
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine("Gold: " + stats.gold);
            Console.SetCursorPosition(x, y + 4);
            Console.WriteLine("Strength: " + stats.strength);
            Console.SetCursorPosition(x, y + 5);
            Console.WriteLine("Dexterity: " + stats.dexterity);
            Console.SetCursorPosition(x, y + 6);
            Console.WriteLine("Intelligence: " + stats.intelligence);
            Console.SetCursorPosition(x, y + 7);
            Console.WriteLine("Wisdom: " + stats.wisdom);
            Console.SetCursorPosition(x, y + 8);
            Console.WriteLine("Level: " + stats.level + " (%" + (((float)stats.xp / (float)stats.reqXp) * 100) + ")");
        }
    }
}
