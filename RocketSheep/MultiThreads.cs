using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RocketSheep
{
    class MultiThreads
    {
        private Thread thread = Thread.CurrentThread;
        
        public void CreateThreadForWolfMover(Map Map, Player Player)
        {
            ThreadObjectHolder holder = new ThreadObjectHolder(Player, Map);
            Thread wolfThread = new Thread(MoveWolves);
            wolfThread.Start(holder);
        }

        public void CreateThreadForPrinting(Map map, Player player)
        {
            ThreadObjectHolder holder = new ThreadObjectHolder(player, map);
            Thread printThread = new Thread(ThreadedPrintMap);//this tells the thread that it's going to be running ThreadedPrintMap
            printThread.Start(holder);//This tells the thread to start and send the object 'map' to its method - ThreadedPrintMap. the method then receives the generic object and casts it as a (Map)
        }

        public void CreateBufferPrintThread(Map Map, Player Player)
        {
            ThreadObjectHolder holder = new ThreadObjectHolder(Player, Map);
            Thread printThread = new Thread(BufferPrintMap);
            printThread.Start(holder);
        }

        public static void BufferPrintMap(Object input)
        {
            Buffer Buffer = new Buffer();
            ThreadObjectHolder holder = (ThreadObjectHolder)input;
            Player Player = (Player)holder.Player;
            Map Map = (Map)holder.Map;
            int valueToCompare = Player.CampaignLevel;
            while (!Player.HasReachedGoal && Player.CampaignLevel == valueToCompare)//without the valuecomparison, I had this odd behavior:
            {//The thread wouldn't kill itself and it would retain the old map data. It would then compete with the next printing thread, and they'd fight over the screen
                //this probably would happen because one map would end and the next would start, all before this thread had a chance to see that the player had reached the goal
                //so it would assume that it hadn't reached the goal, and continue running
                Buffer.Test2(Map, Player);
            }
        }

        public static void ThreadedPrintMap(Object input)
        {
            ThreadObjectHolder holder = (ThreadObjectHolder)input;
            Player Player = (Player)holder.Player;
            Map map = (Map)holder.Map;
            bool shouldContinuePrinting = true;
            while (shouldContinuePrinting)
            {
                Console.Clear();
                for (int y = 0; y < map.MapYsize; y++)
                {
                    for (int x = 0; x < map.MapXsize; x++)
                    {
                        if (map.Coords[x, y] == (char)MapPointTypes.vWall)
                            Console.BackgroundColor = ConsoleColor.Gray;
                        if (map.Coords[x, y] == (char)MapPointTypes.hWall)
                            Console.BackgroundColor = ConsoleColor.Gray;
                        if (map.Coords[x, y] == (char)MapPointTypes.player)
                        { Console.BackgroundColor = ConsoleColor.DarkGreen; Console.ForegroundColor = ConsoleColor.Yellow; }
                        if (map.Coords[x, y] == (char)MapPointTypes.door)
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                        if (map.Coords[x, y] == (char)MapPointTypes.monster)
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        if (map.Coords[x, y] == (char)MapPointTypes.corpse)
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                        if (map.Coords[x, y] == (char)MapPointTypes.hazard)
                        { Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Black; }
                        if (map.Coords[x, y] == (char)MapPointTypes.barricade)
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        if (map.Coords[x, y] == (char)MapPointTypes.window)
                        { Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black; }
                        if (map.Coords[x, y] == (char)MapPointTypes.gun)
                        { Console.BackgroundColor = ConsoleColor.DarkGreen; Console.ForegroundColor = ConsoleColor.Black; }
                        Console.Write(map.Coords[x, y]);
                        Console.ResetColor();
                    }
                    Console.Write("\n");
                }
                map.DisplayPointsOfInterest(Player);
                Thread.Sleep(50);//200 = 5fps. 33 = 30fps. 100 = 10fps.
            }
        }

        public void CreateBulletThread(Map map, Player player, int direction)
        {
            ThreadBulletHolder holder = new ThreadBulletHolder(player, map, direction);
            Thread bulletThread = new Thread(ThreadedFireGun);
            bulletThread.Start(holder);
            //bulletThread.Join();//having this makes it so you can only fire 1 bullet at a time
        }

        public void CreateFlameThread(Map map, Player player, int direction)
        {
            ThreadBulletHolder holder = new ThreadBulletHolder(player, map, direction);
            Thread flameThread = new Thread(ThreadedFlamethrower);
            flameThread.Start(holder);
            //bulletThread.Join();//having this makes it so you can only fire 1 bullet at a time
        }

        public void CreateSwordThread(Map map, Player player, int direction)
        {
            ThreadBulletHolder holder = new ThreadBulletHolder(player, map, direction);
            Thread bulletThread = new Thread(ThreadedFireGun);
            bulletThread.Start(holder);
            //bulletThread.Join();//having this makes it so you can only fire 1 bullet at a time
        }

        public void ThreadedFlamethrower(Object input)
        {
            ThreadBulletHolder holder = (ThreadBulletHolder)input;
            int direction = (int)holder.Direction;
            Player Player = (Player)holder.Player;
            Map Map = (Map)holder.Map;
            switch (direction)
            {
                case '4':
                    {
                        FlamethrowerMotion(-1, 0, Player, Map);
                    }
                    break;
                case '6':
                    {
                        FlamethrowerMotion(1, 0, Player, Map);
                    }
                    break;
                case '8':
                    {
                        FlamethrowerMotion(0, -1, Player, Map);
                    }
                    break;
                case '2':
                    {
                        FlamethrowerMotion(0, 1, Player, Map);
                    }
                    break;
                case '1':
                    {
                        FlamethrowerMotion(-1, 1, Player, Map);
                    }
                    break;
                case '3':
                    {
                        FlamethrowerMotion(1, 1, Player, Map);
                    }
                    break;
                case '7':
                    {
                        FlamethrowerMotion(-1, -1, Player, Map);
                    }
                    break;
                case '9':
                    {
                        FlamethrowerMotion(1, -1, Player, Map);
                    }
                    break;
            }
        }

        public void ThreadedFireGun(Object input)
        {
            ThreadBulletHolder holder = (ThreadBulletHolder)input;
            int direction = (int)holder.Direction;
            Player Player = (Player)holder.Player;
            Map Map = (Map)holder.Map;
            switch (direction)
            {
                case '4':
                    {
                        TestBulletMotion(-1, 0, Player, Map);
                    }
                    break;
                case '6':
                    {
                        TestBulletMotion(1, 0, Player, Map);
                    }
                    break;
                case '8':
                    {
                        TestBulletMotion(0, -1, Player, Map);
                    }
                    break;
                case '2':
                    {
                        TestBulletMotion(0, 1, Player, Map);
                    }
                    break;
                case '1':
                    {
                        TestBulletMotion(-1, 1, Player, Map);
                    }
                    break;
                case '3':
                    {
                        TestBulletMotion(1, 1, Player, Map);
                    }
                    break;
                case '7':
                    {
                        TestBulletMotion(-1, -1, Player, Map);
                    }
                    break;
                case '9':
                    {
                        TestBulletMotion(1, -1, Player, Map);
                    }
                    break;
            }
            
        }

        public void FlamethrowerMotion(int xChange, int yChange, Player Player, Map Map)
        {
            Utils Utils = new Utils();
            int[] bulletposition = new int[2];
            bulletposition[0] = Player.XCoord;//0 = x
            bulletposition[1] = Player.YCoord;//1 = y
            char savePositionID = 'P';
            char pastPositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
            for (int i = 0; i < Player.WeaponRange; i++)
            {
                if (Map.BulletCanPass(bulletposition[0] + xChange, bulletposition[1] + yChange) || Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.barricade)
                {
                    if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] != (char)MapPointTypes.monster && Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] != (char)MapPointTypes.barricade)
                    {
                        if (i == 0)
                        {
                            Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                            savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        }
                        Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = '^';
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        //PrintMap();
                        System.Threading.Thread.Sleep(75);
                        if (i > 0)//create a spreading flame... somehow
                        {
                            if (Utils.FastRoll(1, 4) > 1)//3/4 chance to spread a space
                            {
                                int xSpread = 0;
                                int ySpread = 0;
                                switch (Utils.FastRoll(1,9))
                                {
                                    case 1:
                                        {
                                            xSpread = bulletposition[0] + 1;
                                            ySpread = bulletposition[1] + 1;
                                        }
                                        break;
                                    case 2:
                                        {
                                            xSpread = bulletposition[0] + 1;
                                            ySpread = bulletposition[1];
                                        }
                                        break;
                                    case 3:
                                        {
                                            xSpread = bulletposition[0] + 1;
                                            ySpread = bulletposition[1] - 1;
                                        }
                                        break;
                                    case 4:
                                        {
                                            xSpread = bulletposition[0];
                                            ySpread = bulletposition[1] + 1;
                                        }
                                        break;
                                    case 5:
                                        {
                                            xSpread = bulletposition[0];
                                            ySpread = bulletposition[1];
                                        }
                                        break;
                                    case 6:
                                        {
                                            xSpread = bulletposition[0];
                                            ySpread = bulletposition[1] - 1;
                                        }
                                        break;
                                    case 7:
                                        {
                                            xSpread = bulletposition[0] - 1;
                                            ySpread = bulletposition[1] + 1;
                                        }
                                        break;
                                    case 8:
                                        {
                                            xSpread = bulletposition[0] - 1;
                                            ySpread = bulletposition[1];
                                        }
                                        break;
                                    case 9:
                                        {
                                            xSpread = bulletposition[0] - 1;
                                            ySpread = bulletposition[1] - 1;
                                        }
                                        break;
                                }
                                //if (Utils.FastRoll(0, 1) == 1)
                                //    xSpread = bulletposition[0] + 1;
                                //if (Utils.FastRoll(0, 1) == 1)
                                //    xSpread = bulletposition[0] - 1;
                                //if (Utils.FastRoll(0, 1) == 1)
                                //    ySpread = bulletposition[1] + 1;
                                //if (Utils.FastRoll(0, 1) == 1)
                                //    ySpread = bulletposition[1] - 1;
                                if (Map.BulletCanPass(xSpread, ySpread))
                                {
                                    Map.Coords[xSpread, ySpread] = (char)MapPointTypes.hazard;
                                }
                            }
                        }
                    }
                    else if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.monster)//the next spot is a monster. delete the monster and replace it with a corpse
                    {
                        Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        //Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = (char)MapPointTypes.corpse;
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                    }
                }
                else if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.barricade)
                {
                    Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                    savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                    Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = (char)MapPointTypes.hazard;
                    bulletposition[0] += xChange;
                    bulletposition[1] += yChange;
                    Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                    break;
                }
                else
                {
                    //the first time it can't move to the location, stop moving it
                    break;
                }
            }
        }

        public void TestBulletMotion(int xChange, int yChange, Player Player, Map Map)
        {
            int[] bulletposition = new int[2];
            bulletposition[0] = Player.XCoord;//0 = x
            bulletposition[1] = Player.YCoord;//1 = y
            bool hitMonster = false;
            char savePositionID = 'P';
            char pastPositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
            for (int i = 0; i < Player.WeaponRange; i++)
            {
                if (Map.BulletCanPass(bulletposition[0] + xChange, bulletposition[1] + yChange) || Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.barricade)
                {
                    if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] != (char)MapPointTypes.monster && Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] != (char)MapPointTypes.barricade)
                    {
                        Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = '!';
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        System.Threading.Thread.Sleep(50);
                    }
                    else if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.monster)//the next spot is a monster. delete the monster and replace it with a corpse
                    {
                        Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = (char)MapPointTypes.corpse;
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        hitMonster = true;
                        break;
                    }
                    else if (Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] == (char)MapPointTypes.barricade)
                    {
                        Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
                        savePositionID = Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange];
                        Map.Coords[bulletposition[0] + xChange, bulletposition[1] + yChange] = (char)MapPointTypes.floor;
                        bulletposition[0] += xChange;
                        bulletposition[1] += yChange;
                        hitMonster = true;
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
                Map.Coords[bulletposition[0], bulletposition[1]] = savePositionID;
            //PrintMap();
        }

        public void PathfindingTest(Map Map, Wolf wolf)
        {
            
        }

        public static void MoveWolves(object input)
        {
            ThreadObjectHolder holder = (ThreadObjectHolder)input;
            Map Map = (Map)holder.Map;
            Player Player = (Player)holder.Player;
            Wolf[] wolves = new Wolf[50];
            //first, find all wolves that need to be moved.
            //then in a for loop, for each wolf, calculate the direction needed to move closer and try to move there
            //if it can't move there, try 2 several other directions
            bool wolvesShouldMove = true;
            int[] checkingAlternateRoute = new int[50];
            int[] wolfIDforAlternateRoute = new int[50];

            int wolfAmount = 0;
            int wolfArrayPosition = 0;
            for (int y = 0; y < Map.MapYsize; y++)//find all of the wolves and place them into an array. Could use a list if I wanted list<Wolves> wolfList = new list<Wolves>();
            {//the problem with having this here is that all wolf past info gets deleted, which is problematic for several reasons...but if I have wolves spawn in, then it'll hurt NOT to have it here
                for (int x = 0; x < Map.MapXsize; x++)
                {
                    if (Map.Coords[x, y] == 'K')
                    {
                        wolves[wolfArrayPosition] = new Wolf();
                        wolves[wolfArrayPosition].XCoord = x;
                        wolves[wolfArrayPosition].YCoord = y;
                        wolfArrayPosition++;
                        if (wolfAmount >= 50)
                            break;
                        wolfAmount++;
                    }
                }
            }//As historical note: this used to be in the while (wolvesShouldMove) right after the thread sleep
            while (!Player.HasReachedGoal)
            {
                Thread.Sleep(500);//1 second in between movement ticks
                if (wolfAmount > 0)//No point in running this if there's no wolves
                {
                    for (int i = 0; i < wolfArrayPosition; i++)//for each wolf...
                    {
                        if (!wolves[i].HasSeenPlayer && wolves[i].IsAlive)
                        {//check to see if there's an open line of sight
                            int yDistance = Math.Abs(Player.YCoord - wolves[i].YCoord);
                            int xDistance = Math.Abs(Player.XCoord - wolves[i].XCoord);
                            int stepDistance = 0;
                            if (yDistance > xDistance)
                                stepDistance = yDistance;
                            else if (xDistance >= yDistance)
                                stepDistance = xDistance;
                            int xPosition = wolves[i].XCoord;
                            int yPosition = wolves[i].YCoord;
                            int countYChange = 0;
                            int countXChange = 0;
                            for (int c = 0; c < stepDistance; c++)//check each square leading up to the player
                            {
                                if (Player.YCoord > yPosition)
                                {
                                    countYChange = 1;
                                }
                                else if (Player.YCoord < yPosition)
                                {
                                    countYChange = -1;
                                }
                                if (Player.XCoord > xPosition)
                                {
                                    countXChange = 1;
                                }
                                else if (Player.XCoord < xPosition)
                                {
                                    countXChange = -1;
                                }
                                xPosition += countXChange;
                                yPosition += countYChange;
                                if (Map.WolfCanSee(xPosition, yPosition) && Map.Coords[xPosition, yPosition] == (char)MapPointTypes.player)
                                {
                                    wolves[i].HasSeenPlayer = true;
                                    break;
                                }
                                else if (!Map.WolfCanSee(xPosition, yPosition))
                                    break;
                            }
                        }
                        if (wolves[i].HasSeenPlayer && wolves[i].IsAlive)
                        {
                            if (Map.Coords[wolves[i].XCoord, wolves[i].YCoord] == (char)MapPointTypes.corpse)
                            {
                                wolves[i].Health--;
                                if (wolves[i].Health <= 0)
                                    wolves[i].IsAlive = false;
                            }
                            if (Map.Coords[wolves[i].XCoord, wolves[i].YCoord] == (char)MapPointTypes.hazard)
                            {
                                wolves[i].IsAlive = false;
                                wolves[i].Health = 0;
                            }
                            int yChange = 0;
                            int xChange = 0;//calculate what direction the wolf needs to move to go toward the player. prefers diagonals.
                            if (wolves[i].IsAlive)
                            {
                                
                                if (Player.YCoord > wolves[i].YCoord)
                                {
                                    yChange = 1;
                                }
                                else if (Player.YCoord < wolves[i].YCoord)
                                {
                                    yChange = -1;
                                }
                                if (Player.XCoord > wolves[i].XCoord)
                                {
                                    xChange = 1;
                                }
                                else if (Player.XCoord < wolves[i].XCoord)
                                {
                                    xChange = -1;
                                }
                            }
                            if (checkingAlternateRoute[i] <= 0 && wolves[i].IsAlive == true)
                            {
                                //if the wolf can move to the location, and it won't kill the player
                                if (Map.WolfCanMoveTo(wolves[i].XCoord + xChange, wolves[i].YCoord + yChange) && Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] != (char)MapPointTypes.player)
                                {
                                    int xTarget = wolves[i].XCoord + xChange;
                                    int yTarget = wolves[i].YCoord + yChange;
                                    int currentWolfXCoord = wolves[i].XCoord;
                                    int currentWolfYCoord = wolves[i].YCoord;
                                    //Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                    //wolves[i].CurrentLocationID = Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange];
                                    //Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] = (char)MapPointTypes.monster;
                                    //wolves[i].XCoord = xTarget;
                                    //wolves[i].YCoord = yTarget;
                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.hazard)
                                    {
                                        if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                            Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                        else
                                            Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = ' ';
                                        if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                            wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                        else
                                            wolves[i].CurrentLocationID = ' ';
                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                        wolves[i].XCoord = xTarget;
                                        wolves[i].YCoord = yTarget;
                                    }
                                    else//it dies
                                    {
                                        wolves[i].Health--;
                                        if (wolves[i].Health <= 0)
                                        {
                                            Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                            Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] = (char)MapPointTypes.hazard;
                                            wolves[i].IsAlive = false;
                                        }
                                    }
                                }
                                else if (Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] == (char)MapPointTypes.player)//kill the player
                                {
                                    Player.IsAlive = false;
                                    Player.CanMove = false;
                                    Map.Coords[Player.XCoord, Player.YCoord] = (char)MapPointTypes.corpse;
                                }
                                else if (Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] == (char)MapPointTypes.monster) { checkingAlternateRoute[i] = 1; wolfIDforAlternateRoute[i] = i; }
                                else if (Map.Coords[wolves[i].XCoord + xChange, wolves[i].YCoord + yChange] != (char)MapPointTypes.monster)
                                {
                                    Random rnd = new Random();
                                    checkingAlternateRoute[i] = rnd.Next(1, 3);
                                    wolfIDforAlternateRoute[i] = i;
                                }
                            }
                            else if (wolves[i].IsAlive == true && checkingAlternateRoute[i] > 0)
                            {
                                //CheckAlternateWolfMoves();... do some pathfinding
                                //Declare constants
                                //int mapWidth = Map.MapXsize;
                                //int mapHeight = Map.MapYsize;
                                //int tileSize = 1;
                                //int numberPeople = wolfAmount;
                                //int onClosedList = 10;
                                //int notfinished = 0;
                                //int notStarted = 0;// path-related intants
                                //int found = 1, nonexistent = 2;
                                //int walkable = 0, unwalkable = 1;// walkability array intants

                                ////Create needed arrays
                                //char[,] walkability = new char[mapWidth, mapHeight];
                                //int[] openList = new int[mapWidth * mapHeight + 2]; //1 dimensional array holding ID# of open list items
                                //int[,] whichList = new int[mapWidth + 1, mapHeight + 1];  //2 dimensional array used to record 
                                //                                                          // 		whether a cell is on the open list or on the closed list.
                                //int[] openX = new int[mapWidth * mapHeight + 2]; //1d array stores the x location of an item on the open list
                                //int[] openY = new int[mapWidth * mapHeight + 2]; //1d array stores the y location of an item on the open list
                                //int[,] parentX = new int[mapWidth + 1, mapHeight + 1]; //2d array to store parent of each cell (x)
                                //int[,] parentY = new int[mapWidth + 1, mapHeight + 1]; //2d array to store parent of each cell (y)
                                //int[] Fcost = new int[mapWidth * mapHeight + 2];    //1d array to store F cost of a cell on the open list
                                //int[,] Gcost = new int[mapWidth + 1, mapHeight + 1];    //2d array to store G cost for each cell.
                                //int[] Hcost = new int[mapWidth * mapHeight + 2];    //1d array to store H cost of a cell on the open list
                                //int[] pathLength = new int[numberPeople + 1];     //stores length of the found path for critter
                                //int[] pathLocation = new int[numberPeople + 1];   //stores current position along the chosen path for critter	
                                ////int* pathBank [numberPeople+1]; (see below for an attempt to port over)
                                ////unsafe { int*[] pathBank = new int[numberPeople + 1]; }I dunno how to port this to C# properly

                                ////Path reading variables
                                //int[] pathStatus = new int[numberPeople + 1];
                                //int[] xPath = new int[numberPeople + 1];
                                //int[] yPath = new int[numberPeople + 1];


                                //pathfinding is really complex and I don't want to implement it here, really... I'm thinking I'll just do a random path.
                                checkingAlternateRoute[i]--;
                                Utils Utils = new Utils();//might want to use the Random class, as it's faster
                                int max = 8;
                                int min = 1;
                                int tries = 5;
                                bool success = false;
                                while (tries > 0 && !success)
                                {
                                    int roll = Utils.Roll(min, max);
                                    //if (roll == min)
                                    //    min++;
                                    //if (roll == max)
                                    //    max--;
                                    switch (roll)
                                    {
                                        case 1:
                                            {
                                                int xTarget = wolves[i].XCoord;
                                                int yTarget = wolves[i].YCoord - 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget,yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 2:
                                            {
                                                int xTarget = wolves[i].XCoord + 1;
                                                int yTarget = wolves[i].YCoord - 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 3:
                                            {
                                                int xTarget = wolves[i].XCoord + 1;
                                                int yTarget = wolves[i].YCoord;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 4:
                                            {
                                                int xTarget = wolves[i].XCoord + 1;
                                                int yTarget = wolves[i].YCoord + 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 5:
                                            {
                                                int xTarget = wolves[i].XCoord;
                                                int yTarget = wolves[i].YCoord + 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 6:
                                            {
                                                int xTarget = wolves[i].XCoord - 1;
                                                int yTarget = wolves[i].YCoord + 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 7:
                                            {
                                                int xTarget = wolves[i].XCoord - 1;
                                                int yTarget = wolves[i].YCoord;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                        case 8:
                                            {
                                                int xTarget = wolves[i].XCoord - 1;
                                                int yTarget = wolves[i].YCoord - 1;
                                                if (Map.WolfCanMoveTo(xTarget, yTarget))
                                                {
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard)
                                                    {
                                                        wolves[i].Health--;
                                                        if (wolves[i].Health <= 0)
                                                            wolves[i].IsAlive = false;
                                                    }
                                                    Map.Coords[wolves[i].XCoord, wolves[i].YCoord] = wolves[i].CurrentLocationID;
                                                    if (Map.Coords[xTarget, yTarget] != (char)MapPointTypes.barricade)
                                                        wolves[i].CurrentLocationID = Map.Coords[xTarget, yTarget];
                                                    else
                                                        wolves[i].CurrentLocationID = (char)MapPointTypes.floor;
                                                    if (wolves[i].IsAlive)
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.monster;
                                                    else
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                    wolves[i].XCoord = xTarget;
                                                    wolves[i].YCoord = yTarget;
                                                    success = true;
                                                    if (Map.Coords[xTarget, yTarget] == (char)MapPointTypes.hazard && wolves[i].Health <= 0)
                                                    {
                                                        Map.Coords[xTarget, yTarget] = (char)MapPointTypes.corpse;
                                                        wolves[i].IsAlive = false;
                                                    }
                                                }
                                            }
                                            break;
                                    }//end the switch that grabs an alternate tile
                                    tries--;
                                }//end the while that keeps trying for alternate routes
                            }//end the else that checks for alternate routes
                        }//end the if to check if the wolf has seen the player
                    }//end the for loop for each wolf
                }//end ifwolfamount > 0
            }//end wolvesShouldMove while loop
            
        }//End MoveWolf Method
        

        public static void CheckAlternateWolfMoves(Map Map, int xChange, int yChange)
        {
            xChange = 1;
            yChange = 1;
            double angle = xChange / yChange;
        }

    }//end class multithreads
}//end namespace
