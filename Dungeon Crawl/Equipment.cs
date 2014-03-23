using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Equipment
    {
        //public Item head;
        //public Item arms;
        //public Item hands;
        //public Item chest;
        //public Item greaves;
        //public Item boots;

        //public Item leftHand_ring;
        //public Item rightHand_ring;
        //public Item amulet_one;

        public Item[] equipSlots = new Item[] {
            null, //head 0
            null, //arms 1
            null, //hands 2
            null, //body 3
            null, //greaves 4
            null, //boots 5
            
            null, //ring 6
            null  //amulet 7
        };
    }
}
