﻿using System;
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
        public Equipment equipment;
        public StatusHandler status = new StatusHandler();
        public Item[] inventory = new Item[39];
        public int[] inventoryStacks = new int[39];
        public Boolean[] inventoryEquip = new Boolean[39];
        public int hunger = 1000;
        public Path pathToExit = new Path();
        public string death = "";

        //Racial variables
        public bool _karura_addedFly = false;

        public List<Ability> abilities = new List<Ability>();

        //-5000 to -2500- Starving
        //-2500 to -1000- Hungry
        //-1000 to 0- Slightly Hungry
        //0 to 1000- Fed
        //1000 to 2500- Well Fed
        //2500 to 5000- Stuffed

        public Player(string n, Species sp, Class c)
        {
            name = n;
            species = sp;
            career = c;
            identifier = species.abbrv + career.abbrv;
            stats = species.baseStats.addStatMod(career.statMod).adjust();
            stats.xp = 0;
            if (species == Species._darkElf)
            {
                status.addStatus(new Status("Shadowbound", 1, true, ConsoleForeground.Maroon, ConsoleBackground.Black));
                status.addStatus(new Status("Accursed", 1, true, ConsoleForeground.Red, ConsoleBackground.Black));
            }
            if (species == Species._faerie)
            {
                status.addStatus(new Status("Magic Sight", 1, true, ConsoleForeground.Yellow, ConsoleBackground.Black));
                status.addStatus(new Status("Fly", 1, true, ConsoleForeground.Cyan, ConsoleBackground.Black));
            }
            if (species == Species._woodElf)
            {
                status.addStatus(new Status("rPoison", 1, true, ConsoleForeground.Green, ConsoleBackground.Black));
            }
            if (species == Species._merfolk)
            {
                status.addStatus(new Status("Swimmer", 1, true, ConsoleForeground.Cyan, ConsoleBackground.Black));
            }
            if (species == Species._karura)
            {
                status.addStatus(new Status("Fast Metabolism", 1, true, ConsoleForeground.Maroon, ConsoleBackground.Black));
            }
            if (species == Species._mountainDwarf)
            {
                abilities.Add(new Ability("Extract Gold", AbilityEffect.EXTRACTGOLD, 0, 0));
            }
            //status.addStatus(new Status("Clairvoyance", 1, true, ConsoleForeground.Olive, ConsoleBackground.Black));
            equipment = new Equipment();
            abilities.Add(new Ability("Rest and Heal", AbilityEffect.RESTHEAL, 0, 0));
            //status.addStatus(new Status("Fly", 1, 2000, ConsoleForeground.Cyan, ConsoleBackground.Black));
        }
        /// <summary>
        /// A function for calculating adjacent tiles
        /// </summary>
        /// <returns>Returns a grid of solid tiles adjacent to player (diagonally included)</returns>
        public Tile[,] calcAdj()
        {
            Tile[,] adjTiles = new Tile[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    try
                    {
                        if (Program.world.map[Program.renderX + (x - 1), Program.renderY + (y - 1)].solid)
                        {
                            adjTiles[x, y] = Program.world.map[Program.renderX + (x - 1), Program.renderY + (y - 1)];
                        }
                        else
                        {
                            adjTiles[x, y] = null;
                        }
                    }
                    catch
                    {
                        adjTiles[x, y] = null;
                    }
                }
            }
            return adjTiles;
        }

        public void update()
        {
            if (status.hasAttr("Shadowbound"))
            {
                status.removeAttr("Divine Wrath");
                int wrathCounter = 0;
                for (int x = 0; x < equipment.equipSlots.Length; x++)
                {
                    if (equipment.equipSlots[x] != null)
                    {
                        if (equipment.equipSlots[x].brands.Contains("holy"))
                        {
                            wrathCounter++;
                        }
                    }
                }
                if (wrathCounter > 0)
                {
                    status.addStatus(new Status("Divine Wrath", wrathCounter, true, ConsoleForeground.White, ConsoleBackground.Black));
                }
                if (World.rand.Next(3) != 0)
                {
                    hurt(status.getLvl("Divine Wrath"), true, "The ancients strike against you for tainting their blessings!");
                }
            }
            pathToExit = Path.calcPath(new Point(Program.renderX, Program.renderY), Program.world.suggestedExit, "playerPath", true, false, 10000);
            if (species == Species._karura && stats.level >= 8 && !_karura_addedFly)
            {
                _karura_addedFly = true;
                abilities.Add(new Ability("Fly", AbilityEffect.TOGGLEFLIGHT));
                Program.msgLog.Add("Your vestigal wing stubs grow into feathery appendages!");
            }
            if (stats.xp >= stats.reqXp)
            {
                stats.xp -= stats.reqXp;
                stats.level++;
                Program.msgLog.Add("You are now level " + stats.level);
                stats.reqXp = (int)Math.Pow(stats.level + 1, (7 / 4)) * stats.level;
            }
            Program.world = Program.levelMap[Program.area + ":" + Program.floor];
        }

        public Boolean canEquipSelectedItem()
        {
            try
            {
                if ((inventory[Program.selectedSlot].equipType == species.armor && !inventory[Program.selectedSlot].weapon) || inventory[Program.selectedSlot].weapon)
                {
                    if (inventory[Program.selectedSlot].size == species.size || inventory[Program.selectedSlot].size == Size.ANY)
                    {
                        if (inventoryStacks[Program.selectedSlot] > 0 && inventory[Program.selectedSlot].slotEquip > -1)
                        {
                            return ((inventory[Program.selectedSlot].equipped && equipment.equipSlots[inventory[Program.selectedSlot].slotEquip] != null) || (!inventory[Program.selectedSlot].equipped && equipment.equipSlots[inventory[Program.selectedSlot].slotEquip] == null));
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static double calcInvWeight(Player p)
        {
            double i = 0;
            for (int x = 0; x < p.inventory.Length; x++)
            {
                if (p.inventoryStacks[x] > 0)
                {
                    i += p.inventory[x].weight * p.inventoryStacks[x];
                }
            }
            return i;
        }

        public static string chooseHungerMsg()
        {
            if (World.rand.Next(2) == 0)
            {
                return "Your stomach growls...";
            }
            else
            {
                return "Your stomach rumbles...";
            }
        }

        public void hurt(int amt, Boolean ignoreArmor, string s)
        {
            stats.health -= amt;
            if (stats.health < 0)
            {
                stats.health = 0;
                Program.dead = true;
                death = s;
            }
            if (s != "" && amt > 0)
            {
                Program.msgLog.Add(s);
            }
        }

        public void addToInventory(Item i, int amt, bool msg = true)
        {
            if (status.hasAttr("Magic Sight"))
            {
                i.discoveredBound = true;
            }
            for (int x = 0; x < inventory.Length; x++)
            {
                try
                {
                    if (inventory[x].Equals(i) && inventory[x].consumable)
                    {
                        inventoryStacks[x] += amt;
                        if (msg)
                        {
                            Program.msgLog.Add(amt + " " + i.name + " added to inventory");
                        }
                        return;
                    }
                    else
                    {
                        if (inventoryStacks[x] == 0)
                        {
                            inventory[x] = i;
                            inventoryStacks[x] = amt;
                            if (msg)
                            {
                                Program.msgLog.Add(amt + " " + i.name + " added to inventory");
                            }
                            return;
                        }
                    }
                }
                catch
                {
                    if (inventoryStacks[x] == 0)
                    {
                        inventory[x] = i;
                        inventoryStacks[x] = amt;
                        if (msg)
                        {
                            Program.msgLog.Add(amt + " " + i.name + " added to inventory");
                        }
                        return;
                    }
                }
            }
        }

        public void addToInventory(ItemCache i)
        {
            foreach (KeyValuePair<Item, int> pair in i.items)
            {
                addToInventory(pair.Key, pair.Value);
            }
        }

        public void addGold(int addGold, bool showText = true, bool applyBoosts = true)
        {
            if ((status.hasAttr("Loot") || species == Species._mountainDwarf) && applyBoosts)
            {
                stats.gold += (int)((float)addGold * (1.25f + (0.05f * status.getLvl("Loot"))));
                if (showText)
                {
                    Program.msgLog.Add("You found " + (int)((float)addGold * (1.25f + (0.05f * status.getLvl("Loot")))) + " gold!");
                }
            }
            else if ((status.hasAttr("Greed") || species == Species._trollGnome) && applyBoosts)
            {
                stats.gold += (int)((float)addGold * (0.9f - (0.05f * status.getLvl("Greed"))));
                if (showText)
                {
                    Program.msgLog.Add("You found " + (int)((float)addGold * (0.9f - (0.05f * status.getLvl("Greed")))) + " gold!");
                }
            }
            else
            {
               stats.gold += addGold;
               if (showText)
               {
                   Program.msgLog.Add("You found " + addGold + " gold!");
               }
            }
        }

        public void WriteStats()
        {
            Console.WriteLine("Health: " + stats.health);
            Console.WriteLine("Ether: " + stats.mana);
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
            Console.WriteLine("Ether: " + stats.mana + "/" + stats.maxMana);
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
            if (species != Species._faerie)
            {
                Console.SetCursorPosition(x, y + 10);
                int adjHunger = (Math.Min(Math.Max(hunger, -5000), 7000) + 5000) / 1000;
                ConsoleEx.TextColor(hungerColor(Math.Max(adjHunger - 1, 0)), ConsoleBackground.Black);
                Console.Write(calcHungerStatus() + " ");
                for (int a = 0; a < adjHunger; a++)
                {
                    ConsoleEx.TextColor(hungerColor(a), ConsoleBackground.Black);
                    Console.Write("|");
                }
                ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                Console.Write("\n");
            }
            Console.SetCursorPosition(x, y + 12);
            Console.Write("EV: " + stats.evasion);
            Console.SetCursorPosition(x, y + 13);
            Console.Write("DEF: " + stats.armor);
        }

        public ConsoleForeground hungerColor(int x)
        {
            ConsoleForeground[] colors = new ConsoleForeground[] {
                ConsoleForeground.Maroon,
                ConsoleForeground.Maroon,
                ConsoleForeground.Red,
                ConsoleForeground.Red,
                ConsoleForeground.Red,
                ConsoleForeground.Green,
                ConsoleForeground.Green,
                ConsoleForeground.Olive,
                ConsoleForeground.Olive,
                ConsoleForeground.DarkGreen,
                ConsoleForeground.DarkGreen,
                ConsoleForeground.DarkGreen
            };
            return colors[x];
        }

        public string calcHungerStatus()
        {
            if (species != Species._faerie)
            {
                if (hunger <= -2500)
                {
                    return "Starving";
                }
                else if (hunger <= -1000)
                {
                    return "Hungry";
                }
                else if (hunger < 0)
                {
                    return "Slightly Hungry";
                }
                else if (hunger <= 1000)
                {
                    return "Fed";
                }
                else if (hunger <= 2500)
                {
                    return "Well Fed";
                }
                else if (hunger <= 5000)
                {
                    return "Stuffed";
                }
                else
                {
                    return "Bloated";
                }
            }
            else
            {
                return "Energized";
            }
        }
    }
}
