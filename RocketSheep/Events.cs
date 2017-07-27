using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RocketSheep
{
    class Events
    {
        Utils Utils = new Utils();

        public Events()
        {
        }

        public void SpacebarKeypress(Map Map, Player Player)
        {
            switch (Player.CurrentLocationID)
            {
                case 'G'://put the weapon into the inventory
                    {
                        PickUpItem(Player, Map);
                    }
                    break;
                case 'F'://put the weapon into the inventory
                    {
                        PickUpItem(Player, Map);
                    }
                    break;
                case 'S'://put the weapon into the inventory
                    {
                        PickUpItem(Player, Map);
                    }
                    break;
                case 'V'://put the weapon into the inventory
                    {
                        PickUpItem(Player, Map);
                    }
                    break;
                case 'E'://put the weapon into the inventory
                    {
                        //end the game
                        Player.HasReachedGoal = true;
                    }
                    break;
            }
        }

        public String GetWeapon(Player Player)
        {
            if (Player.CurrentLocationID == (char)MapPointTypes.gun)
                return "Gun";
            if (Player.CurrentLocationID == (char)MapPointTypes.flamethrower)
                return "Flamethrower";
            if (Player.CurrentLocationID == (char)MapPointTypes.sword)
                return "Sword";
            return "Error in GetWeapon";
        }

        public void PickUpItem(Player Player, Map Map)
        {
            if (Player.WeaponAmmo <= 0)
            {
                EquipWeapon(Player, Map);
            }
            else
            {
                PlaceNewItemInInventory(Player, Map);
            }
        }

        public void EquipWeapon(Player Player, Map Map)
        {
            if (Player.CurrentLocationID == (char)MapPointTypes.gun)
            {
                Player.WeaponName = "Gun";
                Player.WeaponAmmo = 6;
                Player.WeaponDamage = 1;
                Player.WeaponRange = 12;
                Player.CurrentLocationID = ' ';
            }
            if (Player.CurrentLocationID == (char)MapPointTypes.flamethrower)
            {
                Player.WeaponName = "Flamethrower";
                Player.WeaponAmmo = 1;
                Player.WeaponDamage = 1;
                Player.WeaponRange = 6;
                Player.CurrentLocationID = ' ';
            }
            if (Player.CurrentLocationID == (char)MapPointTypes.sword)
            {
                Player.WeaponName = "Sword";
                Player.WeaponAmmo = 10;
                Player.WeaponDamage = 1;
                Player.WeaponRange = 2;
                Player.CurrentLocationID = ' ';
            }
        }

        public void PlaceNewItemInInventory(Player Player, Map Map)
        {
            String item = "";
            int ammo = 0;
            if (Player.CurrentLocationID == (char)MapPointTypes.gun)
            {
                item = "Gun";
                ammo = 6;
                //Player.WeaponDamage = 1;
                //Player.WeaponRange = 12;
                Player.CurrentLocationID = ' ';
            }
            else if (Player.CurrentLocationID == (char)MapPointTypes.flamethrower)
            {
                item = "Flamethrower";
                ammo = 1;
                //Player.WeaponDamage = 1;
                //Player.WeaponRange = 7;
                Player.CurrentLocationID = ' ';
            }
            else if (Player.CurrentLocationID == (char)MapPointTypes.sword)
            {
                item = "Sword";
                ammo = 10;
                //Player.WeaponDamage = 1;
                //Player.WeaponRange = 1;
                Player.CurrentLocationID = ' ';
            }
            else
            {
                item = "Error in PlaceNewItemInInventory";
            }
            bool itemApplied = false;
            for (int i = 0; i < 10; i++)
            {
                if (Player.Inventory[i, 0] == "")
                {
                    Player.Inventory[i, 0] = item;
                    Player.Inventory[i, 1] = Convert.ToString(ammo);
                    itemApplied = true;
                    Player.CurrentLocationID = ' ';
                    //Player.DisplayInventory(Map);
                    //Console.ReadKey();
                    break;
                }
            }
            if (!itemApplied)
            {
                //Utils.SlowPrintWithBreak($"You don't have room for the {item}.");
                //Player.DisplayInventory(Map);
                //Console.ReadKey();
            }
        }

        public void WaitForMove(Map Map, Player Player, MultiThreads thread)
        {
            MultiThreads wolfMover = new MultiThreads();
            wolfMover.CreateThreadForWolfMover(Map, Player);

            ThreadObjectHolder holder = new ThreadObjectHolder(Player, Map);
            bool canmove = true;
            Console.Clear();
            int i = 0;
            bool continueLoop = true;
            while (!Player.HasReachedGoal)
            {
                while (Player.CanMove && Player.IsAlive && !Player.HasReachedGoal)
                {
                    char fireDirection = '4';
                    char userInput = Utils.GetUserKeyPress();
                    if (char.IsDigit(userInput))
                    {
                        fireDirection = userInput;
                        userInput = 'f';
                    }
                    //Console.Write("\b");
                    switch (userInput)
                    {
                        case 'w': { Map.MovePlayer(Player, Player.XCoord, Player.YCoord - 1); } break;
                        case 'a': { Map.MovePlayer(Player, Player.XCoord - 1, Player.YCoord); } break;
                        case 's': { Map.MovePlayer(Player, Player.XCoord, Player.YCoord + 1); } break;
                        case 'd': { Map.MovePlayer(Player, Player.XCoord + 1, Player.YCoord); } break;
                        case 'f':
                            {
                                if (Player.WeaponAmmo > 0 && Player.WeaponName == "Gun")
                                {
                                    Player.WeaponAmmo--;
                                    MultiThreads newBullet = new MultiThreads();
                                    newBullet.CreateBulletThread(Map, Player, fireDirection);
                                }
                                if (Player.WeaponAmmo > 0 && Player.WeaponName == "Sword")
                                {
                                    Player.WeaponAmmo--;
                                    MultiThreads newBullet = new MultiThreads();
                                    newBullet.CreateBulletThread(Map, Player, fireDirection);
                                }
                                if (Player.WeaponAmmo > 0 && Player.WeaponName == "Flamethrower")
                                {
                                    Player.WeaponAmmo--;
                                    MultiThreads newBullet = new MultiThreads();
                                    newBullet.CreateFlameThread(Map, Player, fireDirection);
                                }
                                if (Player.WeaponAmmo <= 0)//get the next weapon in the inventory
                                {
                                    for (int s = 0; s < 10; s++)
                                    {
                                        if (Player.Inventory[s,0] != "")
                                        {
                                            SwitchItems(Player, s + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case 'i':
                            {
                                int input = Utils.GetUserInt();
                                SwitchItems(Player, input);
                                
                            }break;
                        case ' ': { SpacebarKeypress(Map, Player); } break;
                        default: { /*userInput = Utils.GetUserKeyPress();*/ } break;
                    }
                    i++;
                    if (i > 10)
                    {
                        Console.Clear();
                        i = 0;
                    }
                }//end while loop
                CheckLoseCondition(Player);
                if (!Player.CanMove)
                {
                    Utils.Pause(1.5);
                    Player.CanMove = true;
                }
            }//end full while loop
        }//end WaitForMove

        public void CheckLoseCondition(Player Player)//Not implemented here, really...
        {
            if (!Player.IsAlive)
            {
                Player.HasNotBeenBeaten = false;
                Player.HasReachedGoal = false;
                Player.Health = 0;
            }
        }

        public void SwitchItems(Player Player, int selection)
        {
            if (selection > 0)
                selection--;
            else
                selection = 9;
            if (Player.WeaponAmmo <= 0)
            {
                Player.WeaponName = Player.Inventory[selection, 0];
                Player.WeaponAmmo = Convert.ToInt32(Player.Inventory[selection, 1]);
                SetWeaponRange(Player, Player.Inventory[selection, 0]);
                Player.Inventory[selection, 0] = "";
                Player.Inventory[selection, 1] = "0";
            }
            else
            {
                double ammoHolder = Player.WeaponAmmo;
                String nameHolder = Player.WeaponName;
                Player.WeaponName = Player.Inventory[selection, 0];
                Player.WeaponAmmo = Convert.ToDouble(Player.Inventory[selection, 1]);
                SetWeaponRange(Player, Player.Inventory[selection, 0]);
                Player.Inventory[selection, 0] = nameHolder;
                Player.Inventory[selection, 1] = ammoHolder.ToString();
            }
        }

        public void SetWeaponRange(Player Player, String name)
        {
            if (name == "Gun")
            {
                Player.WeaponRange = 12;
            }
            if (name == "Flamethrower")
            {
                Player.WeaponRange = 6;
            }
            if (name == "Sword")
            {
                Player.WeaponRange = 1;
            }
        }

    }//end class Events
}//end namespace
