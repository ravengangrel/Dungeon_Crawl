using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public enum AbilityEffect
    {
        RESTHEAL,
        TOGGLEFLIGHT,
        EXTRACTGOLD
    }
    public class Ability
    {
        public string name = "";
        public int etherCost = 0;
        public int healthCost = 0;
        public AbilityEffect effect;

        public Ability(string n, AbilityEffect e, int eC = 0, int hC = 0)
        {
            name = n;
            effect = e;
            etherCost = eC;
            healthCost = hC;
        }

        public void useAbility(Player p)
        {
            if (p.stats.mana >= etherCost)
            {
                p.stats.mana -= etherCost;
                p.hurt(healthCost, true, "You drained some health to perform " + name + "!");
                if (effect == AbilityEffect.RESTHEAL)
                {
                    p.stats.health += (int)(0.25f * p.stats.maxHealth);
                    if (p.stats.health > p.stats.maxHealth)
                    {
                        p.stats.health = p.stats.maxHealth;
                    }
                    Program.msgLog.Add("You feel rested!");
                }
                if (effect == AbilityEffect.TOGGLEFLIGHT)
                {
                    if (p.status.hasAttr("Fly"))
                    {
                        p.status.removeAttr("Fly");
                    }
                    else
                    {
                        p.status.addStatus(new Status("Fly", 1, true, ConsoleForeground.Cyan, ConsoleBackground.Black));
                    }
                }
                if (effect == AbilityEffect.EXTRACTGOLD)
                {
                    Tile[,] adjTiles = p.calcAdj();
                    Point selectedTile = new Point(World.rand.Next(3), World.rand.Next(3));
                    int iter = 0;
                    bool noSolid = false;
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (adjTiles[x, y] == null)
                            {
                                noSolid = true;
                            }
                        }
                    }
                    if (!noSolid)
                    {
                        while ((Program.world.hasExtracted[Program.renderX + (selectedTile.X - 1), Program.renderY + (selectedTile.Y - 1)] || adjTiles[selectedTile.X, selectedTile.Y] == null) && iter <= 10000)
                        {
                            selectedTile = new Point(World.rand.Next(3), World.rand.Next(3));
                            iter++;
                        }
                    }
                    if (iter >= 10000 || adjTiles[selectedTile.X, selectedTile.Y] == null)
                    {
                        if (iter >= 10000)
                        {
                            Program.msgLog.Add("No luck! You couldn't extract any gold!");
                        }
                        else
                        {
                            Program.msgLog.Add("No luck! You couldn't extract any gold!");
                        }
                    }
                    else
                    {
                        if (Program.world.hasExtracted[Program.renderX + (selectedTile.X - 1), Program.renderY + (selectedTile.Y - 1)])
                        {
                            Program.msgLog.Add("There's no gold left here...");
                        }
                        else
                        {
                            Program.world.hasExtracted[Program.renderX + (selectedTile.X - 1), Program.renderY + (selectedTile.Y - 1)] = true;
                            int goldAmt = World.rand.Next(100) + 20;
                            p.addGold(goldAmt, false, false);
                            Program.msgLog.Add("You managed to extract " + goldAmt + " gold from the surrounding tile!");
                        }
                    }
                }
            }
        }
    }
}
