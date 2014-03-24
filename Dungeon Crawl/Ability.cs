using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public enum AbilityEffect
    {
        RESTHEAL
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
            if (effect == AbilityEffect.RESTHEAL)
            {
                p.stats.health += (int)(0.25f * p.stats.maxHealth);
                if (p.stats.health > p.stats.maxHealth)
                {
                    p.stats.health = p.stats.maxHealth;
                }
                Program.msgLog.Add("You feel rested!");
            }
        }
    }
}
