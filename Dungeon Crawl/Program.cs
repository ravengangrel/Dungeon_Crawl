using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon_Crawl
{
    class Program
    {
        public static int selectedSpecies = 0;

        public static Species currSpecies;
        public static Class currClass;
        public static Player player;

        public static int renderX = 0;
        public static int renderY = 0;

        public static int currTurn = 0;
        public static int selectedSlot = 0;

        public static List<string> msgLog = new List<string>();

        public static Item showingItem = null;

        public static bool showAbilities = false;

        static void Main(string[] args)
        {
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;
            Species.init();
            Class.init();
            Tile.init();
            Item.init();
            Mob.init();
            showMainMenu();
        }
        public static void showMainMenu()
        {
            Boolean hasSelectedSpecies = false;
            Boolean hasSelectedClass = false;

            //Draw the species list
            while (!hasSelectedSpecies)
            {
                string bufferClear = "";
                for (int x = 0; x < 69; x++)
                {
                    bufferClear += " ";
                }
                ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 0, 70, Species.speciesList.Length + 3, false);
                Util.writeLn("Select a species", 2, 0);
                Species.drawAllSpecies();
                Util.writeLn(bufferClear, 1, Species.speciesList.Length + 2);
                Util.writeLn(Species.speciesList[selectedSpecies].lore, 1, Species.speciesList.Length + 2);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedSpecies++;
                    if (selectedSpecies > Species.speciesList.Length - 1)
                    {
                        selectedSpecies = 0;
                    }
                }
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedSpecies--;
                    if (selectedSpecies < 0)
                    {
                        selectedSpecies = Species.speciesList.Length - 1;
                    }
                }
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    currSpecies = Species.speciesList[selectedSpecies];
                    hasSelectedSpecies = true;
                }
            }
            Console.Clear();
            selectedSpecies = 0;
            while (!hasSelectedClass)
            {
                ConsoleEx.TextColor(ConsoleForeground.DarkGray, ConsoleBackground.Black);
                ConsoleEx.DrawRectangle(BorderStyle.Text, 0, Class.classList.Length + 2, 70, Species.speciesList.Length + 3, false);
                Util.writeLn("Select a species", Class.classList.Length + 4, 0);
                Species.drawAllSpecies(Class.classList.Length + 2);
                Util.writeLn(currSpecies.lore, 1, Species.speciesList.Length + 2 + Class.classList.Length + 2);

                ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 0, 70, Class.classList.Length + 1, false);
                Util.writeLn("Select a class--Species: " + currSpecies.abbrv, 2, 0);
                Class.drawAllClasses();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedSpecies++;
                    if (selectedSpecies > Species.speciesList.Length - 1)
                    {
                        selectedSpecies = 0;
                    }
                }
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedSpecies--;
                    if (selectedSpecies < 0)
                    {
                        selectedSpecies = Species.speciesList.Length - 1;
                    }
                }
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    currClass = Class.classList[selectedSpecies];
                    hasSelectedClass = true;
                }
            }
            Console.Clear();
            Console.WriteLine("What is your name?");
            player = new Player(Console.ReadLine(), currSpecies, currClass);
            //Console.WriteLine("You are a " + player.identifier);
            Console.WriteLine();
            player.WriteStats();
            player.stats.calcStats();
            Console.ReadLine();
            World.genMap();
            startGame();
        }
        public static void renderGame()
        {
            Console.Clear();
            ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 0, 26, 26, false);

            for (int y = -14; y < 13; y++)
            {
                for (int x = -14; x < 13; x++)
                {
                    while (renderX + x < 0)
                    {
                        x++;
                    }
                    while (renderY + y < 0)
                    {
                        y++;
                    }
                    if (Math.Sqrt(Math.Pow((renderX + x) - renderX, 2) + Math.Pow((renderY + y) - renderY, 2)) < 12.6)
                    {
                        Console.SetCursorPosition(Math.Min(26, Math.Max(1, x + 13)), Math.Min(26, Math.Max(1, y + 13)));
                        try
                        {
                            if (renderX + x == renderX && renderY + y == renderY)
                            {
                                ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                                if (player.status.hasAttr("Invisible"))
                                {
                                    ConsoleEx.TextColor(ConsoleForeground.DarkGray, ConsoleBackground.Black);
                                }
                                Console.Write("P");
                                ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                            }
                            else
                            {
                                if (Mob.getMobsAtPos(renderX + x, renderY + y).Count == 0)
                                {
                                    World.draw(renderX + x, renderY + y);
                                }
                                else
                                {
                                    ConsoleEx.TextColor(Mob.getMobsAtPos(renderX + x, renderY + y)[0].colorFore, Mob.getMobsAtPos(renderX + x, renderY + y)[0].colorBack);
                                    Console.Write(Mob.getMobsAtPos(renderX + x, renderY + y)[0].symbol);
                                    msgLog.Add(Mob.getMobsAtPos(renderX + x, renderY + y)[0].name + " has been spotted near you!");
                                }
                            }

                        }
                        catch
                        {
                            //ConsoleEx.TextColor(ConsoleForeground.Red, ConsoleBackground.Black);
                            //Console.Write('X');
                        }
                        ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
                    }
                    else
                    {
                        Console.SetCursorPosition(Math.Min(26, Math.Max(1, x + 13)), Math.Min(26, Math.Max(1, y + 13)));
                        ConsoleEx.TextColor(ConsoleForeground.DarkGray, ConsoleBackground.Black);
                        Console.Write(':');
                    }
                }
            }
            ConsoleEx.TextColor(ConsoleForeground.LightGray, ConsoleBackground.Black);
            ConsoleEx.DrawRectangle(BorderStyle.Text, 27, 0, 26, 40, false);
            ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 27, 26, 13, false);
            Console.SetCursorPosition(2, 27);
            Console.WriteLine("Turn " + currTurn);
            Console.SetCursorPosition(2, 0);
            Console.Write("Pos: " + renderX + "/" + renderY);
            player.WriteRPGStats(28, 1);
            if (player.status.statusEffects.Count > 0)
            {
                ConsoleEx.DrawRectangle(BorderStyle.Text, 54, 0, 35, player.status.statusEffects.Count + 1, false);
                player.status.drawStatus(55, 1);
            }
            if (!showAbilities)
            {
                for (int x = 0; x < player.inventory.Length; x++)
                {
                    if (player.inventoryStacks[x] > 0)
                    {
                        string s = player.inventoryStacks[x] + " " + player.inventory[x].name;
                        if (Program.selectedSlot == x)
                        {
                            s = "> " + s;
                        }
                        if (player.inventoryEquip[x] || player.inventory[x].equipped)
                        {
                            s = s + " (Equipped)";
                        }
                        if (player.inventory[x].bound && player.inventory[x].discoveredBound)
                        {
                            s = s + " (Bound)";
                        }
                        if (player.status.statusEffects.Count > 0)
                        {
                            Util.writeLn(s, 91, 1 + x);
                        }
                        else
                        {
                            Util.writeLn(s, 54, 1 + x);
                        }
                    }
                    else
                    {
                        string s = "Empty Slot";
                        if (Program.selectedSlot == x)
                        {
                            s = "> " + s;
                        }
                        if (player.status.statusEffects.Count > 0)
                        {
                            Util.writeLn(s, 91, 1 + x);
                        }
                        else
                        {
                            Util.writeLn(s, 54, 1 + x);
                        }
                        player.inventory[x] = null;
                        player.inventoryStacks[x] = 0;
                        player.inventoryEquip[x] = false;
                    }
                }
            }
            else
            {
                int iter = 0;
                string s = "";
                foreach (Ability a in player.abilities)
                {
                    if (iter == Program.selectedSlot)
                    {
                        s = "> ";
                    }
                    s += a.name;
                    if (a.etherCost > 0)
                    {
                        s += "(costs " + a.etherCost + " ether";
                        if (a.healthCost > 0)
                        {
                            s += " and " + a.healthCost + " health)";
                        }
                        else
                        {
                            s += ")";
                        }
                    }
                    if (a.healthCost > 0 && a.etherCost <= 0)
                    {
                        s += "(costs " + a.healthCost + " health)";
                    }
                    if (player.status.statusEffects.Count > 0)
                    {
                        Util.writeLn(s, 91, 1 + iter);
                    }
                    else
                    {
                        Util.writeLn(s, 54, 1 + iter);
                    }
                    iter++;
                }
            }
            ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 41, 88, 6, false);
            for (int x = 0; x < 5; x++)
            {
                try
                {
                    Util.writeLn(msgLog.ToArray()[msgLog.ToArray().Length - (1 + x)], 1, 42 + x);
                }
                catch
                {
                    break;
                }
            }
            player.status.update();
        }
        public static void startGame()
        {
            Boolean turn = false;
            while (true)
            {
                if (showingItem == null)
                {
                    if (!turn)
                    {
                        renderGame();
                        turn = true;
                    }
                    int iteration = 0;
                    while (turn)
                    {
                        if (iteration > 0)
                        {
                            renderGame();
                        }
                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.RightArrow && canMoveRight())
                        {
                            renderX = Math.Min(renderX + 1, 999);
                        }
                        if (keyInfo.Key == ConsoleKey.LeftArrow && canMoveLeft())
                        {
                            renderX = Math.Max(renderX - 1, 0);
                        }
                        if (keyInfo.Key == ConsoleKey.DownArrow && canMoveDown())
                        {
                            renderY = Math.Min(renderY + 1, 999);
                        }
                        if (keyInfo.Key == ConsoleKey.UpArrow && canMoveUp())
                        {
                            renderY = Math.Max(renderY - 1, 0);
                        }
                        if (World.gold[renderX, renderY] > 0)
                        {
                            player.addGold(World.gold[renderX, renderY]);
                            World.gold[renderX, renderY] = 0;
                        }
                        if (World.items[renderX, renderY] != null)
                        {
                            player.addToInventory(World.items[renderX, renderY]);
                            World.items[renderX, renderY] = null;
                        }
                        turn = false;
                        if (keyInfo.Key == ConsoleKey.A)
                        {
                            showAbilities = !showAbilities;
                            turn = true;
                        }
                        if (keyInfo.Key == ConsoleKey.W && !Console.CapsLock)
                        {
                            turn = true;
                            Program.selectedSlot--;
                            if (Program.selectedSlot < 0)
                            {
                                if (!showAbilities)
                                {
                                    Program.selectedSlot = player.inventory.Length - 1;
                                }
                                else
                                {
                                    Program.selectedSlot = player.abilities.Count - 1;
                                }
                            }
                        }
                        if (keyInfo.Key == ConsoleKey.S && !Console.CapsLock)
                        {
                            turn = true;
                            Program.selectedSlot++;
                            if (showAbilities)
                            {
                                if (Program.selectedSlot > player.abilities.Count - 1)
                                {
                                    Program.selectedSlot = 0;
                                }
                            }
                            else
                            {
                                if (Program.selectedSlot > player.inventory.Length - 1)
                                {
                                    Program.selectedSlot = 0;
                                }
                            }
                        }
                        if (keyInfo.Key == ConsoleKey.Enter && showAbilities)
                        {
                            player.abilities[Program.selectedSlot].useAbility(player);
                        }
                        if (keyInfo.Key == ConsoleKey.Q && !Console.CapsLock && !showAbilities)
                        {
                            if (player.inventoryStacks[Program.selectedSlot] > 0)
                            {
                                player.inventory[Program.selectedSlot].useItem(player);
                                if (player.inventory[Program.selectedSlot].consumable)
                                {
                                    player.inventoryStacks[Program.selectedSlot]--;
                                }
                            }
                        }
                        if (keyInfo.Key == ConsoleKey.I && !Console.CapsLock && !showAbilities)
                        {
                            showingItem = player.inventory[Program.selectedSlot];
                        }
                        if (keyInfo.Key == ConsoleKey.E && !Console.CapsLock && !showAbilities)
                        {
                            if (player.inventoryStacks[Program.selectedSlot] > 0 && player.inventory[Program.selectedSlot].equippable && player.canEquipSelectedItem())
                            {
                                try
                                {
                                    if (player.inventoryStacks[Program.selectedSlot] > 0 && !(player.inventory[Program.selectedSlot].bound && player.inventory[Program.selectedSlot].discoveredBound))
                                    {
                                        player.inventory[Program.selectedSlot].equipped = !player.inventory[Program.selectedSlot].equipped;
                                        if (player.inventory[Program.selectedSlot].equipped)
                                        {
                                            player.equipment.equipSlots[player.inventory[Program.selectedSlot].slotEquip] = player.inventory[Program.selectedSlot];
                                            player.inventory[Program.selectedSlot].addBrand("used");
                                        }
                                        else
                                        {
                                            player.equipment.equipSlots[player.inventory[Program.selectedSlot].slotEquip] = null;
                                        }
                                        turn = false;
                                    }
                                    else if (player.inventory[Program.selectedSlot].bound && player.inventory[Program.selectedSlot].discoveredBound && player.inventory[Program.selectedSlot].equipped)
                                    {
                                        msgLog.Add("You can't unequip a bound item!");
                                    }
                                }
                                catch
                                {
                                    player.inventory[Program.selectedSlot].equipped = true;
                                    player.inventory[Program.selectedSlot].addBrand("used");
                                    if (player.inventory[Program.selectedSlot].equipped)
                                    {
                                        player.equipment.equipSlots[player.inventory[Program.selectedSlot].slotEquip] = player.inventory[Program.selectedSlot];
                                    }
                                    else
                                    {
                                        player.equipment.equipSlots[player.inventory[Program.selectedSlot].slotEquip] = null;
                                    }
                                    turn = false;
                                }
                                try
                                {
                                    if (player.inventory[Program.selectedSlot].equipped && !player.inventory[Program.selectedSlot].discoveredBound)
                                    {
                                        player.inventory[Program.selectedSlot].discoveredBound = true;
                                        if (player.inventory[Program.selectedSlot].bound)
                                        {
                                            if (player.status.hasAttr("Accursed"))
                                            {
                                                if (World.rand.Next(10) < player.status.getLvl("Accursed"))
                                                {
                                                    player.inventory[Program.selectedSlot].discoveredBound = false;
                                                    player.inventory[Program.selectedSlot].bound = false;
                                                    player.inventory[Program.selectedSlot].addBrand("was bound");
                                                    msgLog.Add(player.inventory[Program.selectedSlot].name + " was not strong enough to bind to you!");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            player.inventory[Program.selectedSlot].addBrand("not bound");
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else if (!player.canEquipSelectedItem() && player.inventoryStacks[Program.selectedSlot] > 0)
                            {
                                if (player.inventory[Program.selectedSlot].equippable)
                                {
                                    msgLog.Add("You can't equip anything in that slot!");
                                }
                            }
                            else
                            {
                                turn = true;
                            }
                            //else
                            //{
                            //    turn = true;
                            //    msgLog.Add("You are intangible and cannot wield any items!");
                            //}
                        }
                        iteration++;
                    }
                    if (player.species != Species._faerie)
                    {
                        player.hunger--;
                        if (player.hunger < -4500 && World.rand.Next(100) < 73)
                        {
                            player.hurt(World.rand.Next(4) + 1, true, Player.chooseHungerMsg());
                        }
                    }
                    Mob.updatePaths();
                    Mob.updateMobs();
                    player.stats.calcStats();
                    if (World.map[renderX, renderY] == Tile.shallowWater && !player.status.hasAttr("Fly"))
                    {
                        player.status.removeAttr("Wet");
                        player.status.addStatus(new Status("Wet", 1, true, ConsoleForeground.Blue, ConsoleBackground.Black));
                    }
                    else
                    {
                        player.status.removeAttr("Wet");
                    }
                    if (World.map[renderX, renderY] == Tile.deepWater && !player.status.hasAttr("Fly"))
                    {
                        player.status.removeAttr("Wet+");
                        player.status.addStatus(new Status("Wet+", 1, true, ConsoleForeground.Navy, ConsoleBackground.Black));
                    }
                    else
                    {
                        player.status.removeAttr("Wet+");
                    }
                    player.update();
                    currTurn++;
                }
                else
                {
                    Console.Clear();
                    ConsoleEx.DrawRectangle(BorderStyle.Text, 0, 0, Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10, false);
                    Util.writeLn("Press enter to exit...", 2, 2);
                    Util.writeLn(showingItem.name + " (weight: " + showingItem.weight + ")", 2, 4);
                    Util.writeLn(showingItem.compileTags(), 2, 5);
                    Console.SetCursorPosition(Console.LargestWindowWidth - 1, Console.LargestWindowHeight - 1);
                    Console.ReadKey();
                    showingItem = null;
                }
            }
        }
        public static Boolean canMoveRight()
        {
            return !World.map[renderX + 1, renderY].solid && (World.map[renderX + 1, renderY] != Tile.deepWater || player.status.hasAttr("Swimmer") || player.status.hasAttr("Fly"));
        }
        public static Boolean canMoveLeft()
        {
            return !World.map[renderX - 1, renderY].solid && (World.map[renderX - 1, renderY] != Tile.deepWater || player.status.hasAttr("Swimmer") || player.status.hasAttr("Fly"));
        }
        public static Boolean canMoveUp()
        {
            return !World.map[renderX, renderY - 1].solid && (World.map[renderX, renderY - 1] != Tile.deepWater || player.status.hasAttr("Swimmer") || player.status.hasAttr("Fly"));
        }
        public static Boolean canMoveDown()
        {
            return !World.map[renderX, renderY + 1].solid && (World.map[renderX, renderY + 1] != Tile.deepWater || player.status.hasAttr("Swimmer") || player.status.hasAttr("Fly"));
        }
    }
}
