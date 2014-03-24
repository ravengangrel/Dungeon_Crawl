using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public static class Extensions
    {
        public static bool NextBool(this Random rand)
        {
            return rand.Next(2) == 0;
        }
    }
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
            int xDistance = Math.Abs(start.X - end.X);
            int yDistance = Math.Abs(start.Y - end.Y);
            if (xDistance > yDistance)
            {
                dist = 14 * yDistance + 10 * (xDistance - yDistance);
            }
            else
            {
                dist = 14 * xDistance + 10 * (yDistance - xDistance);
            }
            return dist;
        }
    }
}
