using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    public class Status
    {
        public string name = "";
        public int level;
        public int timeLeft;
        public Boolean permanent = false;
        public ConsoleForeground colorFore;
        public ConsoleBackground colorBack;

        public Status(string s, int lvl, int tLft, ConsoleForeground cF, ConsoleBackground cB)
        {
            name = s;
            level = lvl;
            timeLeft = tLft;
            colorFore = cF;
            colorBack = cB;
        }
        
        public Status(string s, int lvl, Boolean perm, ConsoleForeground cF, ConsoleBackground cB)
        {
            name = s;
            level = lvl;
            colorFore = cF;
            colorBack = cB;
            permanent = perm;
            if (perm)
            {
                timeLeft = 1;
            }
        }
    }

    public class StatusHandler
    {
        public List<Status> statusEffects = new List<Status>();

        public void update()
        {
            for (int x = 0; x < statusEffects.Count; x++)
            {
                if (!statusEffects[x].permanent)
                {
                    statusEffects[x].timeLeft--;
                }
                updateStatus(statusEffects[x]);
                if (statusEffects[x].timeLeft == 0)
                {
                    statusEffects.Remove(statusEffects[x]);
                }
            }
        }

        public Boolean hasAttr(String attrName)
        {
            for (int x = 0; x < statusEffects.Count; x++)
            {
                if (statusEffects[x].name == attrName)
                {
                    return true;
                }
            }
            return false;
        }

        public int getLvl(String attrName)
        {
            if (hasAttr(attrName))
            {
                for (int x = 0; x < statusEffects.Count + 1; x++)
                {
                    if (statusEffects[x].name == attrName)
                    {
                        return statusEffects[x].level;
                    }
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public void updateStatus(Status s)
        {
            //All statuses are handled here
        }

        public void addStatus(Status s)
        {
            statusEffects.Insert(statusEffects.Count, s);
        }

        public void drawStatus(int x, int y)
        {
            for (int a= 0; a < statusEffects.Count; a++)
            {
                Console.SetCursorPosition(x, y + a);
                ConsoleEx.TextColor(statusEffects[a].colorFore, statusEffects[a].colorBack);
                if (statusEffects[a].permanent)
                {
                     Console.Write(statusEffects[a].name + " " + statusEffects[a].level);
                }
                else
                {
                    Console.Write(statusEffects[a].name + " " + statusEffects[a].level + " (" + statusEffects[a].timeLeft + " turns left)");
                }
                ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);                
            }
        }
    }
}
