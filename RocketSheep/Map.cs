using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    //Enumerations, types you create, go in the namespace
    public enum MapPointTypes { goal = 'E', vWall = '|', hWall = '-', player = 'P', gun = 'G', barricade = 'B', door = 'D', window = 'W', floor = ' ', hazard = '^', monster = 'K', corpse = 'C', bullet = '!', flamethrower = 'F', sword = 'S' }
    class Map
    {
        //char[] mapArray = new char[10];
        Random Random = new Random();
        Utils Utils = new Utils();
        int mapXsize = 10;
        int mapYsize = 10;
        //int mapSize = 10;//this defines the edge of a square
        char[,] coords;//This creates a 10x10 grid. it's called a multidimensional array. you can do 3d with int[, ,]

        public char[,] Coords { get => coords; set => coords = value; }
        public int MapXsize { get => mapXsize; set => mapXsize = value; }
        public int MapYsize { get => mapYsize; set => mapYsize = value; }

        public void CreateMapRandom(int size)
        {
            mapXsize = size;
            mapYsize = size;
            coords = new char[mapXsize, mapYsize];

            //int[][] coords = new int[][] { };//this is one method

            //get a text file
            //parse it in
            for (int y = 0; y < mapYsize; y++)
            {
                //if I just put I, it will be a 45 deg angle, 1,1 2,2 3,3 4,4 etc
                for (int x = 0; x < mapXsize; x++)
                {
                    int selector = Utils.Roll(0, 100);
                    if (selector <= 70)
                        coords[x, y] = (char)MapPointTypes.floor;//floor, greatest chance
                    if (selector > 70 && selector <= 75)
                        coords[x, y] = (char)MapPointTypes.vWall;
                    if (selector > 75 && selector <= 80)
                        coords[x, y] = (char)MapPointTypes.hWall;
                    if (selector > 70 && selector <= 75)
                        coords[x, y] = (char)MapPointTypes.monster;
                    if (selector > 80 && selector <= 81)
                        coords[x, y] = (char)MapPointTypes.flamethrower;
                    if (selector > 81 && selector <= 90)
                        coords[x, y] = (char)MapPointTypes.barricade;
                    if (selector > 90 && selector <= 91)
                        coords[x, y] = (char)MapPointTypes.sword;
                    if (selector > 91 && selector <= 92)
                        coords[x, y] = (char)MapPointTypes.hazard;
                    if (selector > 92 && selector <= 93)
                        coords[x, y] = (char)MapPointTypes.gun;
                    if (selector > 93 && selector <= 95)
                        coords[x, y] = (char)MapPointTypes.door;
                    if (selector > 95 && selector <= 98)
                        coords[x, y] = (char)MapPointTypes.window;
                    if (selector > 98 && selector <= 100)
                        selector = 12;
                    switch (selector)
                    {
                        //case 1: { coords[x, y] = (char)MapPointTypes.vWall; } break;//5
                        //case 2: { coords[x, y] = (char)MapPointTypes.hWall; } break;//1
                        //case 3: { coords[x, y] = (char)MapPointTypes.sword; } break;//5
                        //case 4: { coords[x, y] = (char)MapPointTypes.gun; } break;//1
                        //case 5: { coords[x, y] = (char)MapPointTypes.barricade; } break;//5
                        //case 6: { coords[x, y] = (char)MapPointTypes.door; } break;//2
                        //case 7: { coords[x, y] = (char)MapPointTypes.window; } break;//1
                        //case 8: { coords[x, y] = (char)MapPointTypes.floor; } break;//70
                        //case 9: { coords[x, y] = (char)MapPointTypes.hazard; } break;//1
                        //case 10: { coords[x, y] = (char)MapPointTypes.monster; } break;//5
                        //case 11: { coords[x, y] = (char)MapPointTypes.flamethrower; } break;//1
                    }

                }
            }
            for (int y = 0; y < mapYsize; y++)
            {
                //if I just put I, it will be a 45 deg angle, 1,1 2,2 3,3 4,4 etc
                for (int x = 0; x < mapXsize; x++)
                {
                    if (y == 0 || y == mapYsize - 1)
                        coords[x, y] = (char)MapPointTypes.hWall;
                    else if (x == 0 || x == mapXsize - 1)
                        coords[x, y] = (char)MapPointTypes.vWall;
                }
            }
        }

        public void CreateMapEmpty(Player Player, int size)
        {
            mapXsize = size;
            mapYsize = size;
            coords = new char[mapXsize, mapYsize];
            //get a text file
            //parse it in
            for (int y = 0; y < mapYsize; y++)
            {
                //if I just put I, it will be a 45 deg angle, 1,1 2,2 3,3 4,4 etc
                for (int x = 0; x < mapXsize; x++)
                {
                    if (y == 0 || y == mapYsize - 1)
                        coords[x, y] = (char)MapPointTypes.hWall;
                    else if (x == 0 || x == mapXsize - 1)
                        coords[x, y] = (char)MapPointTypes.vWall;
                    else
                        coords[x, y] = (char)MapPointTypes.floor;
                    if (Player.XCoord == x && Player.YCoord == y)
                        coords[x, y] = (char)MapPointTypes.player;
                }
            }

        }

        public void FireGun(Player Player, char direction)
        {
            //char direction = Utils.GetUserNumberPress();
            switch (direction)
            {
                case '4':
                    {
                        TestBulletMotion(-1, 0, Player);
                    }
                    break;
                case '6':
                    {
                        TestBulletMotion(1, 0, Player);
                    }
                    break;
                case '8':
                    {
                        TestBulletMotion(0, -1, Player);
                    }
                    break;
                case '2':
                    {
                        TestBulletMotion(0, 1, Player);
                    }
                    break;
                case '1':
                    {
                        TestBulletMotion(-1, 1, Player);
                    }
                    break;
                case '3':
                    {
                        TestBulletMotion(1, 1, Player);
                    }
                    break;
                case '7':
                    {
                        TestBulletMotion(-1, -1, Player);
                    }
                    break;
                case '9':
                    {
                        TestBulletMotion(1, -1, Player);
                    }
                    break;
            }
        }

        public void TestBulletMotion(int xChange, int yChange, Player Player)
        {
            int[] bulletposition = new int[2];
            bulletposition[0] = Player.XCoord;//0 = x
            bulletposition[1] = Player.YCoord;//1 = y
            bool hitMonster = false;
            char savePositionID = 'P';
            char pastPositionID = coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
            for (int i = 0; i < Player.WeaponRange; i++)
            {
                if (BulletCanPass(bulletposition[0] + xChange, bulletposition[1] + yChange))
                {
                    if (coords[bulletposition[0] + xChange, bulletposition[1] + yChange] != (char)MapPointTypes.monster)
                    {
                        coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = '!';
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        //PrintMap();
                        Utils.Pause(.05);
                    }
                    else//the next spot is a monster. delete the monster and replace it with a corpse
                    {
                        coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = (char)MapPointTypes.corpse;
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        hitMonster = true;
                        //PrintMap();
                        break;
                    }
                }
                else
                {
                    //the first time it can't move to the location, stop moving it
                    break;
                }
            }
            if (!hitMonster)
                coords[bulletposition[0], bulletposition[1]] = savePositionID;
            //PrintMap();
        }


        public void PrintMap()
        {
            Console.Clear();
            for (int y = 0; y < mapYsize; y++)
            {
                for (int x = 0; x < mapXsize; x++)
                {
                    if (coords[x, y] == (char)MapPointTypes.vWall)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    if (coords[x, y] == (char)MapPointTypes.hWall)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    if (coords[x, y] == (char)MapPointTypes.player)
                    { Console.BackgroundColor = ConsoleColor.DarkGreen; Console.ForegroundColor = ConsoleColor.Yellow; }
                    if (coords[x, y] == (char)MapPointTypes.door)
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                    if (coords[x, y] == (char)MapPointTypes.monster)
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    if (coords[x, y] == (char)MapPointTypes.corpse)
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    if (coords[x, y] == (char)MapPointTypes.hazard)
                    { Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Black; }
                    if (coords[x, y] == (char)MapPointTypes.barricade)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    if (coords[x, y] == (char)MapPointTypes.window)
                    { Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black; }
                    if (coords[x, y] == (char)MapPointTypes.gun)
                    { Console.BackgroundColor = ConsoleColor.DarkGreen; Console.ForegroundColor = ConsoleColor.Black; }
                    Console.Write(coords[x, y]);
                    Console.ResetColor();
                }
                Console.Write("\n");
            }
        }

        

        public void MovePlayer(Player Player, int xTarget, int yTarget)
        {
            if (CanMoveTo(xTarget, yTarget))
            {
                int currentPlayerXCoord = Player.XCoord;
                int currentPlayerYCoord = Player.YCoord;
                coords[Player.XCoord, Player.YCoord] = Player.CurrentLocationID;
                Player.CurrentLocationID = coords[xTarget, yTarget];
                coords[xTarget, yTarget] = (char)MapPointTypes.player;
                Player.XCoord = xTarget;
                Player.YCoord = yTarget;
            }
            else { }//can't move there

        }

        public bool CanMoveTo(int xCoord, int yCoord)
        {
            if (coords[xCoord, yCoord] == (char)MapPointTypes.goal || coords[xCoord, yCoord] == (char)MapPointTypes.sword || coords[xCoord, yCoord] == (char)MapPointTypes.flamethrower || coords[xCoord, yCoord] == (char)MapPointTypes.corpse || coords[xCoord, yCoord] == (char)MapPointTypes.hazard || coords[xCoord, yCoord] == (char)MapPointTypes.gun || coords[xCoord, yCoord] == (char)MapPointTypes.door || coords[xCoord, yCoord] == (char)MapPointTypes.floor)
                return true;
            else
                return false;
        }

        public bool WolfCanMoveTo(int xCoord, int yCoord)
        {
            if (coords[xCoord, yCoord] == (char)MapPointTypes.barricade && Utils.FastRoll(1, 10) > 8)
                return true;
            if (coords[xCoord, yCoord] == (char)MapPointTypes.hazard && Utils.FastRoll(1, 10) > 7)
                return true;
            if (coords[xCoord, yCoord] == (char)MapPointTypes.sword || coords[xCoord, yCoord] == (char)MapPointTypes.flamethrower || coords[xCoord, yCoord] == (char)MapPointTypes.window || coords[xCoord, yCoord] == (char)MapPointTypes.corpse || coords[xCoord, yCoord] == (char)MapPointTypes.gun || coords[xCoord, yCoord] == (char)MapPointTypes.floor || coords[xCoord, yCoord] == (char)MapPointTypes.player)
                return true;
            else
                return false;
        }
        public bool WolfCanSee(int xCoord, int yCoord)
        {
            if (coords[xCoord, yCoord] == (char)MapPointTypes.hazard || coords[xCoord, yCoord] == (char)MapPointTypes.barricade || coords[xCoord, yCoord] == (char)MapPointTypes.sword || coords[xCoord, yCoord] == (char)MapPointTypes.flamethrower || coords[xCoord, yCoord] == (char)MapPointTypes.window || coords[xCoord, yCoord] == (char)MapPointTypes.corpse || coords[xCoord, yCoord] == (char)MapPointTypes.gun || coords[xCoord, yCoord] == (char)MapPointTypes.floor || coords[xCoord, yCoord] == (char)MapPointTypes.player)
                return true;
            else
                return false;
        }

        public bool BulletCanPass(int xCoord, int yCoord)
        {
            if (coords[xCoord, yCoord] == (char)MapPointTypes.window || coords[xCoord, yCoord] == (char)MapPointTypes.flamethrower || coords[xCoord, yCoord] == (char)MapPointTypes.sword || coords[xCoord, yCoord] == (char)MapPointTypes.hazard || coords[xCoord, yCoord] == (char)MapPointTypes.gun || coords[xCoord, yCoord] == (char)MapPointTypes.floor || coords[xCoord, yCoord] == (char)MapPointTypes.monster || coords[xCoord, yCoord] == (char)MapPointTypes.corpse)
                return true;
            else
                return false;
        }

        public void DisplayPointsOfInterest(Player Player)
        {
            Console.Write($"\n\t\t\t(w)(up): ");
            if (IsPointOfInterest(coords[Player.XCoord, Player.YCoord - 1]))//up
            {
                Console.Write(DescribePointOfInterest(coords[Player.XCoord, Player.YCoord - 1]));
            }
            Console.Write($"\n(a)(left): ");
            if (IsPointOfInterest(coords[Player.XCoord - 1, Player.YCoord]))//left
                Console.Write(DescribePointOfInterest(coords[Player.XCoord - 1, Player.YCoord]));
            else
                Console.Write("\t");
            Console.Write($"\t(You): ");
            if (IsPointOfInterest(Player.CurrentLocationID))//left
                Console.Write(DescribePointOfInterest(Player.CurrentLocationID));
            else
                Console.Write("\t");
            Console.Write($"\t\t(d)(right): ");
            if (IsPointOfInterest(coords[Player.XCoord + 1, Player.YCoord]))//right
                Console.Write(DescribePointOfInterest(coords[Player.XCoord + 1, Player.YCoord]));
            Console.Write($"\n\t\t\t(s)(down): ");
            if (IsPointOfInterest(coords[Player.XCoord, Player.YCoord + 1]))//down
                Console.Write(DescribePointOfInterest(coords[Player.XCoord, Player.YCoord + 1]));
        }

        public bool IsPointOfInterest(char point)
        {
            if (point == 'D' || point == 'G' || point == 'W' || point == '^' || point == 'B' || point == 'C' || point == 'F' || point == 'S')
                return true;
            else
                return false;
        }
        public String DescribePointOfInterest(char point)
        {
            if (point == 'D')
                return "Unlocked door";
            if (point == 'G')
                return "A Gun";
            if (point == 'W')
                return "A window";
            if (point == 'B')
                return "A barricade";
            if (point == '^')
                return "Danger!";
            if (point == 'C')
                return "A Corpse";
            if (point == 'F')
                return "Flamethrower";
            if (point == 'S')
                return "A Sword";
            if (point == 'E')
                return "The Goal!";
            else
                return "";
        }
    }
}
