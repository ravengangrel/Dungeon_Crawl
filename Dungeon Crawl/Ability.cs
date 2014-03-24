using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Ability
    {
        public string name = "";
        public int etherCost = 0;
        public int healthCost = 0;

        public Ability(string n, int eC, int hC = 0)
        {
            name = n;
            etherCost = eC;
            healthCost = hC;
        }

        public void useAbility(Player p)
        {
        }
    }
}
