using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public enum AbilityEffect
    {
        RESTHEAL,
        TOGGLEFLIGHT
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
            }
        }
    }
}
