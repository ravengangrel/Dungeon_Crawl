using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class BaseStatSet
    {
        public int strength = 0;
        public int dexterity = 0;
        public int intelligence = 0;
        public int wisdom = 0;
        public int health = 0;
        public int mana = 0;

        public BaseStatSet(int s, int d, int i, int w, int h, int m)
        {
            strength = s;
            dexterity = d;
            intelligence = i;
            wisdom = w;
            health = h;
            mana = m;
        }

        public StatSet addStatMod(StatMod m)
        {
            return new StatSet(strength + m.strength, dexterity + m.dexterity, intelligence + m.intelligence, wisdom + m.wisdom, health + m.health, mana + m.mana);
        }
    }
}
