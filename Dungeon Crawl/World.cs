using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Tile
    {
        public string name = "";
        public string lore = "";
        public char icon = '*';
        public ConsoleForeground colorFore = ConsoleForeground.LightGray;
        public ConsoleBackground colorBack = ConsoleBackground.Black;
        public Boolean solid = true;

        public static Tile stoneWall = new Tile();
        public static Tile stoneFloor = new Tile();
        public static Tile shallowWater = new Tile();
        public static Tile deepWater = new Tile();

        public static void init()
        {
            stoneWall.name = "Stone Wall";
            stoneWall.lore = "A stone wall";
            stoneWall.icon = '*';
            stoneWall.colorFore = ConsoleForeground.DarkGray;
            stoneWall.colorBack = ConsoleBackground.DarkGray;
            stoneWall.solid = true;

            stoneFloor.name = "Stone Floor";
            stoneFloor.lore = "A stone floor";
            stoneFloor.icon = ' ';
            stoneFloor.solid = false;

            shallowWater.name = "Shallow Water";
            shallowWater.lore = "Looks wet";
            shallowWater.icon = '~';
            shallowWater.solid = false;
            shallowWater.colorFore = ConsoleForeground.Blue;
            
            deepWater.name = "Deep Water";
            deepWater.lore = "Looks really wet";
            deepWater.icon = '~';
            deepWater.solid = false;
            deepWater.colorFore = ConsoleForeground.Navy;
        }

        public void draw()
        {
            ConsoleEx.TextColor(colorFore, colorBack);
            Console.Write(icon);
        }
    }

    public class World
    {
        public static Tile[,] map = new Tile[1000, 1000];
        public static int[,] gold = new int[1000, 1000];
        public static ItemCache[,] items = new ItemCache[1000, 1000];
        public static Random rand = new Random();

        public static void draw(int x, int y)
        {
            if (isEmpty(x, y))
            {
                map[x, y].draw();
            }
            else
            {
                if (items[x, y] != null)
                {
                    ConsoleEx.TextColor(ConsoleForeground.Cyan, ConsoleBackground.Black);
                    Console.Write('@');
                }
                else if (gold[x, y] > 0)
                {
                    ConsoleEx.TextColor(ConsoleForeground.Yellow, ConsoleBackground.Black);
                    Console.Write('G');
                }
            }
        }

        public static Boolean isEmpty(int x, int y)
        {
            return gold[x, y] == 0 && items[x, y] == null;
        }

        public static void genMap()
        {
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    map[x, y] = Tile.stoneWall;
                    gold[x, y] = 0;
                }
            }
            int sX = rand.Next(250, 750);
            int sY = rand.Next(250, 750);
            map[sX, sY] = Tile.stoneFloor;
            Program.renderX = sX;
            Program.renderY = sY;
            //Mob.spawnMob(Mob._mobZombie, Program.renderX + 5, Program.renderY + 5);
            int cX = 0;
            int cY = 0;
            bool genWater = false;
            bool genDeep = false;
            for (int z = 0; z < rand.Next(2, 6) + 3; z++)
            {
                for (int a = 0; a < rand.Next(80000, 150000); a++)
                {
                    if (rand.Next(2) == 0)
                    {
                        if (rand.Next(2) == 0)
                        {
                            cX++;
                        }
                        else
                        {
                            cY--;
                        }
                    }
                    else
                    {
                        if (rand.Next(2) == 0)
                        {
                            cX--;
                        }
                        else
                        {
                            cY++;
                        }
                    }
                    while (sX + cX < 1)
                    {
                        cX++;
                    }
                    while (sX + cX > 998)
                    {
                        cX--;
                    }
                    while (sY + cY < 1)
                    {
                        cY++;
                    }
                    while (sY + cY > 998)
                    {
                        cY--;
                    }
                    if (rand.Next(2000) == 0 && !genWater)
                    {
                        genWater = !genWater;
                    }
                    if (rand.Next(400) == 0 && genWater)
                    {
                        genWater = !genWater;
                    }
                    if (!genWater)
                    {
                        map[sX + cX, sY + cY] = Tile.stoneFloor;
                        if (rand.Next(1200) == 0)
                        {
                            gold[sX + cX, sY + cY] = rand.Next(1, 30);
                        }
                        if (rand.Next(1400) == 0)
                        {
                            items[sX + cX, sY + cY] = new ItemCache();
                            if (rand.Next(2) == 0)
                            {
                                items[sX + cX, sY + cY].addItem(Item.items[0], rand.Next(2) + 1);
                            }
                            else
                            {
                                if (Program.player.species != Species._darkElf)
                                {
                                    items[sX + cX, sY + cY].addItem(Item.get(1).setBound(rand.NextBool()), 1);
                                }
                                else
                                {
                                    items[sX + cX, sY + cY].addItem(Item.get(3).setBound(rand.NextBool()), 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rand.Next(20) == 0)
                        {
                            genDeep = !genDeep;
                        }
                        if (genDeep)
                        {
                            map[sX + cX, sY + cY] = Tile.deepWater;
                        }
                        else
                        {
                            map[sX + cX, sY + cY] = Tile.shallowWater;
                        }
                    }
                }
            }
        }
    }
}
