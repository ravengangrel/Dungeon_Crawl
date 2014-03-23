using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Util
    {
        public static void writeLn(string s, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(s);
        }

        public static int calcManhattan(Point start, Point end)
        {
            int dist = 0;
            dist += Math.Abs(end.X - start.X) - 1;
            dist += Math.Abs(end.Y - start.Y) - 1;
            return dist;
        }
    }
}
