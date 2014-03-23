using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Class
    {
        public string name;
        public string abbrv;
        public StatMod statMod;

        public static Class _fighter = new Class();
        public static Class _ranger = new Class();
        public static Class _mage = new Class();
        public static Class _samurai = new Class();

        public static Class[] classList;

        public static void init()
        {
            //The total modification boost should total to 0

            _fighter.name = "Fighter";
            _fighter.abbrv = "Fi";
            _fighter.statMod = new StatMod(2, 0, -2, -2, 4, -2);

            _ranger.name = "Ranger";
            _ranger.abbrv = "Ra";
            _ranger.statMod = new StatMod(-1, 2, 1, -2, -1, 1);

            _mage.name = "Mage";
            _mage.abbrv = "Ma";
            _mage.statMod = new StatMod(-3, -2, 2, 2, -2, 3);
            
            _samurai.name = "Samurai";
            _samurai.abbrv = "Sa";
            _samurai.statMod = new StatMod(3, 3, 3, 1, -5, 3);


            //All available classes
            //If it's not in the list, it can't be accessed
            classList = new Class[] {
                _fighter,
                _ranger,
                _mage
            };
        }

        public static void drawClass(Class s)
        {
            Console.WriteLine(s.name + " (" + s.abbrv + ")");
        }

        public static void drawAllClasses()
        {
            for (int x = 0; x < classList.Length; x++)
            {
                Console.SetCursorPosition(1, x + 1);
                if (Program.selectedSpecies == x)
                {
                    ConsoleEx.TextColor(ConsoleForeground.Magenta, ConsoleBackground.Yellow);
                    drawClass(classList[x]);
                }
                else
                {
                    ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                    drawClass(classList[x]);
                }
            }
            ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
        }
    }
}
