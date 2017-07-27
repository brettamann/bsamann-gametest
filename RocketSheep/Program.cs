using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RocketSheep
{
    class Program
    {

        static void Main(string[] args)
        {
            Buffer Buffer = new Buffer();
            SaveLoad SaveLoad = new SaveLoad();
            Map Map = new Map();
            Player Player = new Player();
            Utils Utils = new Utils();
            MultiThreads MultiThreads = new MultiThreads();
            System.IO.Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\Documents\Rocket_Sheep_Game");
            Player.FilePath = @"C:\Users\" + Environment.UserName + @"\Documents\Rocket_Sheep_Game\Map.txt";
            Player.ReinitializeInventory();
            Console.SetWindowSize(150, 80);
            //TEST TEST
            //Player.TestInventory();
            //END TEST
            
            bool continueLoop = true;
            while (continueLoop)
            {
                Console.Write("\n\n1) Load Campaign\n2) Empty Map\n3) Crazy Randomized Map\n4) Tutorial");
                int userInputInt = Utils.GetUserInputInRange(1, 4);
                switch (userInputInt)
                {
                    case 1:
                        {
                            //if (File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\Movement_Test_Game"))
                            //{
                                //int selectionCounter = 1;
                                //string someFile = @"C:\Users\" + Environment.UserName + @"\Documents\Rocket_Sheep_Game";
                                //string directory = Path.GetDirectoryName(someFile);
                                //String[] allMapsArray = Directory.GetFiles(someFile, "*.txt");
                                //Utils.FastPrintWithBreak($"\nThe current Load Folder is \"{someFile}\"");
                                ////This is the equivalent of "There are {allSavesArray.Length} save files found.\n"
                                //Map[] allSavesMapArray = new Map[allMapsArray.Length];
                                //String[] allSavesNameArray = new String[allMapsArray.Length];
                                //if (allMapsArray.Length > 0)
                                //{
                                //    Console.WriteLine("There are {0} map files found.\n", allMapsArray.Length);
                                //    foreach (var file in Directory.GetFiles(someFile))//"var" is just generic for varialbe here, then I named it "file" so I can manipulate the data found in the GetFiles array
                                //    {
                                //        Console.Write($"{selectionCounter}) ");
                                //        //File.Delete(file);
                                //        String fileName = Path.GetFileName(file);
                                //        Player.FilePath = someFile + @"\" + fileName;
                                //        allSavesMapArray[selectionCounter - 1] = SaveLoad.LoadMapTxt(Player);
                                //        allSavesNameArray[selectionCounter - 1] = Path.GetFileName(file);
                                //        Console.WriteLine(allSavesNameArray[selectionCounter - 1]);
                                //        selectionCounter++;
                                //    }
                                //    int userInput = Utils.GetUserInputInRange(1, selectionCounter);
                                //    userInput--;//have to decrement it because arrays start at [0] and not [1]
                                //    Map = allSavesMapArray[userInput];
                                //    Console.Write($"\n\n{allSavesNameArray[userInput]} Loaded.");
                                //    int campaignLevel = userInput;
                                //    for (int y = 0; y < Map.MapYsize; y++)
                                //    {
                                //        for (int x = 0; x < Map.MapXsize; x++)
                                //        {
                                //            if (Map.Coords[x, y] == (char)MapPointTypes.player)
                                //            {
                                //                Player.XCoord = x;
                                //                Player.YCoord = y;
                                //                break;
                                //            }
                                //        }
                                //    }
                                //    MultiThreads.CreateBufferPrintThread(Map, Player);
                                //    Console.SetBufferSize(Map.Coords.Length + 15, Map.Coords.Length + 15);
                                    Events Events = new Events();
                                //    Events.WaitForMove(Map, Player, MultiThreads);
                                //}
                                //else
                                //{
                                //    Console.WriteLine("There are {0} save files found in {1}\n", allMapsArray.Length, someFile);
                                //}
                                CampaignStart(Player, Map, Events, MultiThreads, Utils);
                            //}
                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine("Enter a map size: ");
                            int mapSize = Convert.ToInt32(Utils.GetUserInputText(100));
                            Map.CreateMapEmpty(Player, mapSize);
                            MultiThreads.CreateBufferPrintThread(Map, Player);
                            Console.SetBufferSize(Map.Coords.Length + 15, Map.Coords.Length + 15);
                            Events Events = new Events();
                            Events.WaitForMove(Map, Player, MultiThreads);
                        }
                        break;
                    case 3:
                        {
                            Console.WriteLine("Enter a map size: ");
                            int mapSize = Convert.ToInt32(Utils.GetUserInputText(100));
                            Map.CreateMapRandom(mapSize);
                            MultiThreads.CreateBufferPrintThread(Map, Player);
                            Console.SetBufferSize(Map.Coords.Length + 15, Map.Coords.Length + 15);
                            Events Events = new Events();
                            Events.WaitForMove(Map, Player, MultiThreads);
                        }
                        break;
                    case 4:
                        {
                            Utils.Turorial();
                        }
                        break;
                }
            }
            
        }//End Main Method

        static void CampaignStart(Player Player, Map Map, Events Events, MultiThreads MultiThreads, Utils Utils )
        {
            //this gets the path of the .exe that's running.
            string path;
            path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string someFile = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;//@"C:\Users\" + Environment.UserName + @"\Documents\Rocket_Sheep_Game";
            someFile = someFile.Remove(someFile.Length - 15);//.Remove deletes ALL characters after the indicated index position (charArray of the String)
                //the removal gets us back to the base of the directory where we'll find the .txt files
            char[] toBeDeleted = "file:////".ToCharArray();//deletes some junk created by the above methods
            someFile = someFile.TrimStart(toBeDeleted);
            Console.WriteLine();
            Console.WriteLine(someFile);

            string directory = Path.GetDirectoryName(someFile);
            String[] allMapsArray = Directory.GetFiles(someFile, "*.txt");
            if (allMapsArray.Length > 0)
            {
                SaveLoad SaveLoad = new SaveLoad();
                int selectionCounter = 1;
                
                Utils.FastPrintWithBreak($"\nThe current Load Folder is \"{someFile}\"");
                //This is the equivalent of "There are {allSavesArray.Length} save files found.\n"
                Map[] allSavesMapArray = new Map[allMapsArray.Length];
                String[] allSavesNameArray = new String[allMapsArray.Length];
                if (allMapsArray.Length > 0)
                {
                    Console.WriteLine("There are {0} map files found.\n", allMapsArray.Length);
                    foreach (var file in Directory.GetFiles(someFile))//"var" is just generic for varialbe here, then I named it "file" so I can manipulate the data found in the GetFiles array
                    {
                        //File.Delete(file);don't remember why this is here
                        String fileName = Path.GetFileName(file);
                        if (fileName.Contains(".txt"))//make sure it's a .txt file
                        {
                            Console.Write($"{selectionCounter}) ");
                            Player.FilePath = someFile + @"\" + fileName;
                            allSavesMapArray[selectionCounter - 1] = SaveLoad.LoadMapTxt(Player);
                            allSavesNameArray[selectionCounter - 1] = Path.GetFileName(file);
                            Console.WriteLine(allSavesNameArray[selectionCounter - 1]);
                            selectionCounter++;
                        }
                    }
                    int userInput = Utils.GetUserInputInRange(1, selectionCounter);
                    userInput--;//have to decrement it because arrays start at [0] and not [1]
                    Map = allSavesMapArray[userInput];
                    Console.Write($"\n\n{allSavesNameArray[userInput]} Loaded.");
                    Player.CampaignLevel = userInput + 1;
                    for (int y = 0; y < Map.MapYsize; y++)
                    {
                        for (int x = 0; x < Map.MapXsize; x++)
                        {
                            if (Map.Coords[x, y] == (char)MapPointTypes.player)
                            {
                                Player.XCoord = x;
                                Player.YCoord = y;
                                break;
                            }
                        }
                    }
                    MultiThreads.CreateBufferPrintThread(Map, Player);
                    Console.SetBufferSize(Map.Coords.Length + 15, Map.Coords.Length + 15);
                    Events.WaitForMove(Map, Player, MultiThreads);
                }
                else
                {
                    Console.WriteLine("There are {0} save files found in {1}\n", allMapsArray.Length, someFile);
                }
            }
            else { Console.Write("Error"); }
            if (Player.IsAlive && Player.HasReachedGoal)
            {
                while (Player.HasNotBeenBeaten)
                {
                    Player.CampaignLevel++;
                    Player.LevelsBeaten++;
                    NextLevel(Player, Map, Events, Utils);
                }
            }
        }

        static void NextLevel(Player Player, Map Map, Events Events, Utils Utils)
        {
            Player.CurrentLocationID = ' ';
            Player.HasReachedGoal = false;
            Player.WeaponAmmo = 0;
            Player.WeaponName = "";
            Player.WeaponRange = 1;
            Console.Clear();
            SaveLoad SaveLoad = new SaveLoad();
            string someFile = @"C:\Users\" + Environment.UserName + @"\Documents\Rocket_Sheep_Game";
            string directory = Path.GetDirectoryName(someFile);
            String[] allMapsArray = Directory.GetFiles(someFile, $"{Player.CampaignLevel}*.txt");
            //This is the equivalent of "There are {allSavesArray.Length} save files found.\n"
            Map[] allSavesMapArray = new Map[allMapsArray.Length];
            String[] allSavesNameArray = new String[allMapsArray.Length];
            if (allMapsArray.Length > 0)
            {
                int selectionCounter = 1;
                //This is the equivalent of "There are {allSavesArray.Length} save files found.\n"
                if (allMapsArray.Length > 0)
                {
                    Console.WriteLine("Next level: {0}", allMapsArray[0]);
                    foreach (var file in Directory.GetFiles(someFile))//"var" is just generic for varialbe here, then I named it "file" so I can manipulate the data found in the GetFiles array
                    {
                        Console.Write($"{selectionCounter}) ");
                        //File.Delete(file);
                        String fileName = Path.GetFileName(file);
                        Player.FilePath = someFile + @"\" + fileName;
                        if (fileName.Contains(Player.CampaignLevel.ToString()))
                        {
                            allSavesMapArray[selectionCounter - 1] = SaveLoad.LoadMapTxt(Player);
                            allSavesNameArray[selectionCounter - 1] = Path.GetFileName(file);
                            Console.WriteLine(allSavesNameArray[selectionCounter - 1]);
                            selectionCounter++;
                        }
                    }
                    Map = allSavesMapArray[0];
                    Console.Write($"\n\n{allSavesNameArray[0]} Loaded.");
                    Player.CampaignLevel++;
                    for (int y = 0; y < Map.MapYsize; y++)
                    {
                        for (int x = 0; x < Map.MapXsize; x++)
                        {
                            if (Map.Coords[x, y] == (char)MapPointTypes.player)
                            {
                                Player.XCoord = x;
                                Player.YCoord = y;
                                Player.CurrentLocationID = (char)MapPointTypes.floor;
                                Player.ReinitializeInventory();
                                break;
                            }
                        }
                    }
                    MultiThreads MultiThreads = new MultiThreads();
                    MultiThreads.CreateBufferPrintThread(Map, Player);
                    Console.SetBufferSize(Map.Coords.Length + 15, Map.Coords.Length + 15);
                    Events.WaitForMove(Map, Player, MultiThreads);
                }
                else
                {
                    Console.WriteLine($"You have beaten all available levels! Good job! You finished {Player.LevelsBeaten} scenarios!");
                    Player.HasNotBeenBeaten = false;
                }
            }
            if (!Player.IsAlive || !Player.HasReachedGoal)
                {
                    Player.HasNotBeenBeaten = false;
                }
        }//END NextLevel
    }
}
