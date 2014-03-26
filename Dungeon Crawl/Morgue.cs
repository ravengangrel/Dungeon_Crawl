using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dungeon_Crawl
{
    public class Morgue
    {
        public static void update(Player p)
        {
            Directory.CreateDirectory("/DungeonCrawl/morgue");
            StreamWriter morgue = new StreamWriter("/DungeonCrawl/morgue/" + DateTime.Now.ToBinary() + ".morgue");
            morgue.Write(p.name + " was a " + p.species.name + " " + p.career.name + " (CoD: " + p.death + ") who on turn " + Program.currTurn + " died at " + Program.renderX + "/" + Program.renderY + " on floor " + Program.area + " " + Program.floor);
            morgue.Close();
        }

        public static void show()
        {
            Console.Clear();
            for (int x = 0; x < new DirectoryInfo("/DungeonCrawl/morgue").GetFiles().Length; x++)
            {
                if (new DirectoryInfo("/DungeonCrawl/morgue").GetFiles()[x].Extension == ".morgue" || new DirectoryInfo("/DungeonCrawl/morgue").GetFiles()[x].Extension == "morgue")
                {
                    Console.WriteLine(new StreamReader(new DirectoryInfo("/DungeonCrawl/morgue").GetFiles()[x].FullName).ReadToEnd());
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press enter to exit to main menu...");
            Console.ReadLine();
            Console.Clear();
            Program.showMainMenu();
        }
    }
}
