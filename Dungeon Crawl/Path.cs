using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dungeon_Crawl
{
    public class Path
    {
        public string name = "";
        public List<Point> points = new List<Point>();

        public void print()
        {
            Directory.CreateDirectory("C:/Users/Ian/Desktop/mobpaths"); 
            StreamWriter writer = new StreamWriter("C:/Users/Ian/Desktop/mobpaths/" + name + "_path" + points[0].X + "." + points[0].Y + " - " + points[points.Count - 1].X + "." + points[points.Count - 1].Y + ".txt");
            for (int x = 0; x < points.Count; x++)
            {
                writer.WriteLine(points[x].X + "/" + points[x].Y);
            }
            writer.Close();
        }

        public bool hasPoint(Point p)
        {
            foreach (Point a in points)
            {
                if (a.X == p.X && a.Y == p.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public static Path calcPath(Point s, Point end, string name, bool cardinal = false, bool ignoreWalls = false, int maxLength = 40000, bool dijkastra = false)
        {
            Path path = new Path();
            bool skipPoint = false;
            path.points.Add(s);
            Point start = path.points[0];
            int timeout = maxLength;
            Point lastPoint = start;
            path.name = name;
            while (start.X != end.X || start.Y != end.Y)
            {
                timeout--;
                start = lastPoint;
                double[,] pointMap = new double[3, 3];
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    { 
                        skipPoint = false;
                        if (start.X + x < 0 || start.X + x > 999 || start.Y + y < 0 || start.Y + y > 999)
                        {
                            skipPoint = true;
                        }
                        else
                        {
                            if ((Program.world.map[start.X + x, start.Y + y].solid && !ignoreWalls) || Program.world.map[start.X + x, start.Y] == Tile.deepWater)
                            {
                                skipPoint = true;
                            }
                            if (cardinal)
                            {
                                if (Math.Abs(x) == Math.Abs(y))
                                {
                                    skipPoint = true;
                                }
                            }
                        }
                        if (!skipPoint && !path.hasPoint(new Point(start.X + x, start.Y + y)))
                        {
                            double heuristic = (Program.world.map[start.X + x, start.Y + y].moveCost * 10);
                            if (dijkastra)
                            {
                                heuristic = 0;
                            }
                            pointMap[x + 1, y + 1] = (double)((Util.calcManhattan(new Point(start.X + x, start.Y + y), end)) + heuristic);
                        }
                        else
                        {
                            pointMap[x + 1, y + 1] = -1;
                        }
                    }
                }
                List<Point> poss = new List<Point>();
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (pointMap[x + 1, y + 1] > 0)
                        {
                            poss.Add(new Point(x + 1, y + 1));
                        }
                    }
                }
                if (timeout == 0 || poss.Count < 1)
                {
                    path.print();
                    return path;
                }
                Point lowestPoint = poss[0];
                for (int a = 0; a < poss.Count; a++)
                {
                    if (pointMap[poss[a].X, poss[a].Y] < pointMap[lowestPoint.X, lowestPoint.Y])
                    {
                        lowestPoint = poss[a];
                    }
                }
                lastPoint = new Point(start.X + (lowestPoint.X - 1), start.Y + (lowestPoint.Y - 1));
                path.points.Add(new Point(start.X + (lowestPoint.X - 1), start.Y + (lowestPoint.Y - 1)));
            }
            path.print();
            return path;
        }
    }
}
