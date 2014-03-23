using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class StatSet
    {
        //This contains all the stats critical to calculations
        public int strength = 0;
        public int dexterity = 0;
        public int intelligence = 0;
        public int wisdom = 0;

        public int health = 0;
        public int mana = 0;
        public int maxHealth = 0;
        public int maxMana = 0;
        public int gold = 0;

        public int xp = 0;
        public int reqXp = 10;
        public int level = 1;

        public StatSet(int s, int d, int i, int w, int h, int m)
        {
            strength = s;
            dexterity = d;
            intelligence = i;
            wisdom = w;
            health = h;
            mana = m;
            maxHealth = h;
            maxMana = m;
        }

        public StatSet addStatMod(StatMod m)
        {
            return new StatSet(strength + m.strength, dexterity + m.dexterity, intelligence + m.intelligence, wisdom + m.wisdom, health + m.health, mana + m.mana);
        }

        public StatSet adjust()
        {
            return new StatSet(Math.Max(strength, 0), Math.Max(dexterity, 0), Math.Max(intelligence, 0), Math.Max(wisdom, 0), Math.Max(health, 0), Math.Max(mana, 0));
        }
    }
}
