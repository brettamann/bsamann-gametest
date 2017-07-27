using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    class Player
    {
        Utils Utils = new Utils();

        private int campaignLevel = 1;
        private int levelsBeaten = 0;

        private bool hasNotBeenBeaten = true;
        private bool hasReachedGoal = false;
        private bool isAlive = true;
        private bool canMove = true;

        private String name = "Tester";
        private int xCoord = 5;
        private int yCoord = 5;
        private char currentLocationID = ' ';
        private String filePath = "";

        private double maxHealth = 1;
        private double health = 1;

        private String weaponName = "";
        private double weaponDamage = 0;
        private double weaponRange = 0;
        private double weaponAmmo = 0;

        private String armorName = "";
        private double armorDurability = 0;

        private String[,] inventory = new String[10, 2];
        bool inventoryIsEmpty = true;

        public string Name { get => name; set => name = value; }
        public int XCoord { get => xCoord; set => xCoord = value; }
        public int YCoord { get => yCoord; set => yCoord = value; }
        public char CurrentLocationID { get => currentLocationID; set => currentLocationID = value; }
        public string FilePath { get => filePath; set => filePath = value; }
        public double Health { get => health; set => health = value; }
        public double MaxHealth { get => maxHealth; set => maxHealth = value; }
        public string[,] Inventory { get => inventory; set => inventory = value; }
        public string WeaponName { get => weaponName; set => weaponName = value; }
        public double WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public double WeaponRange { get => weaponRange; set => weaponRange = value; }
        public string ArmorName { get => armorName; set => armorName = value; }
        public double ArmorDurability { get => armorDurability; set => armorDurability = value; }
        public bool InventoryIsEmpty { get => inventoryIsEmpty; set => inventoryIsEmpty = value; }
        public double WeaponAmmo { get => weaponAmmo; set => weaponAmmo = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool CanMove { get => canMove; set => canMove = value; }
        public bool HasReachedGoal { get => hasReachedGoal; set => hasReachedGoal = value; }
        public bool HasNotBeenBeaten { get => hasNotBeenBeaten; set => hasNotBeenBeaten = value; }
        public int LevelsBeaten { get => levelsBeaten; set => levelsBeaten = value; }
        public int CampaignLevel { get => campaignLevel; set => campaignLevel = value; }

        public void DisplayInventory(Map Map)
        {
            Console.Clear();
            InventoryIsEmpty = true;
            for (int i = 0; i < Map.MapYsize + 8; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("Inventory: ");
            for (int i = 0; i < 10; i++)
            {
                if (inventory[i, 0] != "")
                {
                    Console.Write($"\n(#{i + 1}) {inventory[i, 0]} (x{inventory[i, 1]})");
                    InventoryIsEmpty = false;
                }
            }
            if (InventoryIsEmpty == true)
                Console.Write("(Empty)");
        }

        public void TestInventory()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    if (y == 0)
                        inventory[x, y] = "Gun";
                    if (y == 1)
                        inventory[x, y] = "2";
                }
            }
        }

        public void ReinitializeInventory()//deletes and/or initializes the inventory array
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    if (y == 0)
                        inventory[x, y] = "";
                    if (y == 1)
                        inventory[x, y] = "0";
                }
            }
        }

        public void UseItem()
        {
            int userInput = Utils.GetUserInputInRange(0, 9) - 1;
            if (userInput == -1)
                userInput = 9;
            if (inventory[userInput, 0] != "")
            {
                int itemAmount = Convert.ToInt32(inventory[userInput, 1]);
                itemAmount--;
                inventory[userInput, 1] = itemAmount.ToString();
                if (itemAmount == 0)
                    inventory[userInput, 0] = "";
            }
            else
            {
                Utils.FastPrintWithBreak($"Sorry, there's nothing in slot {userInput + 1}");
                Console.ReadKey();
            }
        }
    }
}
