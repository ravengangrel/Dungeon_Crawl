using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class StatMod
    {
        public int strength = 0;
        public int dexterity = 0;
        public int intelligence = 0;
        public int wisdom = 0;
        public int health = 0;
        public int mana = 0;
        public int stealth = 0;

        /// <summary>
        /// This is the constructor for a statistics modification
        /// </summary>
        /// <param name="s">The modification to strength</param>
        /// <param name="d">The modification to dexterity</param>
        /// <param name="i">The modification to intelligence</param>
        /// <param name="w">The modification to wisdom</param>
        /// <param name="h">The modification to health</param>
        /// <param name="m">The modification to mana</param>
        public StatMod(int s, int d, int i, int w, int h, int m)
        {
            strength = s;
            dexterity = d;
            intelligence = i;
            wisdom = w;
            health = h;
            mana = m;
        }

        public StatMod setStealth(int i)
        {
            stealth = i;
            return this;
        }
    }
}
