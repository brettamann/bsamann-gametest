using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace RocketSheep
{
    class Buffer
    {//HELP FOR THIS WAS FOUND AT https://stackoverflow.com/questions/2754518/how-can-i-write-fast-colored-output-to-console

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }


        [STAThread]
        public void Main()
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                CharInfo[] buf = new CharInfo[80 * 25];//I think this is the buffer size (80 by 25)
                SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = 80, Bottom = 25 };//this is defined in the above struct. Not sure what it's for

                for (byte character = 65; character < 65 + 26; ++character)//this changes the letter based on its ID
                                                                           //https://msdn.microsoft.com/en-us/library/60ecse8t(v=vs.80).aspx
                                                                           //https://msdn.microsoft.com/en-us/library/9hxt0028(v=vs.80).aspx
                {
                    for (short attribute = 0; attribute < 15; ++attribute)//This changes the foreground color (text color) based on ID
                                                                          //this is based on the 16 colors (0-15) available from the ConsoleColor attribute
                                                                          //https://msdn.microsoft.com/en-us/library/windows/desktop/ms682013(v=vs.85).aspx
                    {//black=0, blue=1, green=2, red=4, ... apparently these can be combined? see link
                        for (int i = 0; i < buf.Length; ++i)//This appears to write the characters 1 by 1 based on the size of the buffer set in "CharInfo[] buf..."
                        {
                            buf[i].Attributes = attribute;//foreground color. This selects the color given in the enclosing for loop (see "attribute")
                            //buf[i].Attributes = (short)(attribute | (2 << 4));//BG color (my addition). Right now it's giving green.
                            //buf[i].Attributes = (short)(attribute | (attribute << 4));//this makes it the same color as the foreground
                            buf[i].Char.AsciiChar = character;//this sets the character based on the largest enclosing for loop (see "character")
                            
                        }
                        Console.ReadKey();
                        bool b = WriteConsoleOutput(h, buf,
                          new Coord() { X = 80, Y = 25 },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);
                    }
                }
            }
            Console.ReadKey();
        }

        //This simply prints out all letters A-Z in all colors. Copied from the site which helped me write this
        /*public void Test(Map Map, Player Player)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                CharInfo[] buf = new CharInfo[Map.MapXsize * Map.MapYsize];//I think this is the buffer size (80 by 25),x * y
                SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = (short)Map.MapXsize, Bottom = (short)Map.MapYsize };//this is defined in the above struct. Not sure what it's for

                for (byte character = 65; character < 65 + 26; ++character)//this changes the letter based on its ID. each iteration = 1 character (EG 'A') going through all colors
                                                                           //https://msdn.microsoft.com/en-us/library/60ecse8t(v=vs.80).aspx
                                                                           //https://msdn.microsoft.com/en-us/library/9hxt0028(v=vs.80).aspx
                {                                                          //the "character < 65 + 26" tells it to go through all letters of the alphabet
                    for (short attribute = 0; attribute < 15; ++attribute)//This changes the foreground color (text color) based on ID
                                                                          //this is based on the 16 colors (0-15) available from the ConsoleColor attribute
                    {
                        for (int i = 0; i < buf.Length; ++i)//This appears to write the characters 1 by 1 based on the size of the buffer set in "CharInfo[] buf..."
                        {
                            buf[i].Attributes = attribute;//foreground color. This selects the color given in the enclosing for loop (see "attribute")
                            buf[i].Attributes = (short)(attribute | (2 << 4));//BG color (my addition). Right now it's giving green.
                            //the '<<' is called a shift operator. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/left-shift-operator
                            buf[i].Attributes = (short)(attribute | (attribute << 4));//this makes it the same color as the foreground. So find a foreground color, and put it where "attribute" is
                            buf[i].Char.AsciiChar = character;//this sets the character based on the largest enclosing for loop (see "character")

                        }
                        Console.ReadKey();
                        bool b = WriteConsoleOutput(h, buf,
                          new Coord() { X = (short)Map.MapXsize, Y = (short)Map.MapYsize },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);//it looks like this is where it writes the values to a rectangular space
                    }
                }
            }
            Console.ReadKey();
        }*/

        //black = 0
        //blue = 1
        //green = 2
        //cyan = 3
        //red = 4
        //magenta = 5
        //yellow = 6
        //white = 7
        //gray = 8 ("intensity")
        //light blue = 9
        //light green = 10
        //light cyan = 11
        //light red = 12
        //light magenta = 13
        //light yellow = 14
        //black = 15
        //BG blue = 10 (so 1, foreground blue, * 10 = BG blue)
        [STAThread]
        public void Test2(Map Map, Player Player)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                CharInfo[] buf = new CharInfo[Map.MapXsize * Map.MapYsize];//I think this is the buffer size (80 by 25),x * y
                SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = (short)Map.MapXsize, Bottom = (short)Map.MapYsize };//this is defined in the above struct. Not sure what it's for
                int i = 0;
                for (int y = 0; y < Map.MapYsize; y++)
                {
                    for (int x = 0; x < Map.MapXsize; x++)
                    {
                        if (Map.Coords[x, y] == (char)MapPointTypes.vWall)
                        {
                            buf[i].Attributes = (short)(8 | (8 << 4));//First number is the foreground color, second number (before the shift) is the background color. Leave the shift value = 4
                        }
                        if (Map.Coords[x, y] == (char)MapPointTypes.hWall)
                            buf[i].Attributes = (short)(8 | (8 << 4));//gray
                        if (Map.Coords[x, y] == (char)MapPointTypes.goal)
                            buf[i].Attributes = (short)(13 | (6 << 4));//gray
                        if (Map.Coords[x, y] == (char)MapPointTypes.player && Player.IsAlive)
                        { buf[i].Attributes = (short)(14 | (2 << 4)); }
                        else if (Map.Coords[x, y] == (char)MapPointTypes.player && !Player.IsAlive) { buf[i].Attributes = (short)(12 | (9 << 4)); }
                        if (Map.Coords[x, y] == (char)MapPointTypes.door)
                            buf[i].Attributes = (short)(7 | (3 << 4));//Cyan
                        if (Map.Coords[x, y] == (char)MapPointTypes.monster)
                            buf[i].Attributes = (short)(7 | (4 << 4));//gray
                        if (Map.Coords[x, y] == (char)MapPointTypes.corpse)
                            buf[i].Attributes = (short)(7 | (6 << 4));//dark yellow
                        if (Map.Coords[x, y] == (char)MapPointTypes.hazard)
                        { buf[i].Attributes = (short)(4 | (4 << 4));}
                        if (Map.Coords[x, y] == (char)MapPointTypes.barricade)
                            buf[i].Attributes = (short)(7 | (1 << 4));//dark blue
                        if (Map.Coords[x, y] == (char)MapPointTypes.window)
                        { buf[i].Attributes = (short)(0 | (8 << 4)); }
                        if (Map.Coords[x, y] == (char)MapPointTypes.gun)
                        { buf[i].Attributes = (short)(0 | (2 << 4));/*dark green*/}
                        if (Map.Coords[x, y] == (char)MapPointTypes.bullet)
                        { buf[i].Attributes = (short)(4 | (0 << 4));/*dark green*/}
                        if (Map.Coords[x, y] == (char)MapPointTypes.flamethrower)
                        { buf[i].Attributes = (short)(7 | (2 << 4)); }
                        if (Map.Coords[x, y] == (char)MapPointTypes.sword)
                        { buf[i].Attributes = (short)(5 | (2 << 4)); }
                        //buf[i].Attributes = 3;//foreground color. This selects the color given in the enclosing for loop (see "attribute")
                        //buf[i].Attributes = (short)(3 | (2 << 4));//BG color (my addition). Right now it's giving green.
                        //the '<<' is called a shift operator. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/left-shift-operator
                        //buf[0].Attributes = (short)(3 | (3 << 4));//this makes it the same color as the foreground. So find a foreground color, and put it where "attribute" is
                        buf[i].Char.AsciiChar = (byte)Map.Coords[x,y];//the casting of (byte) transforms the character stored at the coordinates to its coded value (eg 'A' = 65)
                        i++;
                    }
                }
                bool b = WriteConsoleOutput(h, buf,
                new Coord() { X = (short)Map.MapXsize, Y = (short)Map.MapYsize },
                new Coord() { X = 0, Y = 0 },
                ref rect);//it looks like this is where it writes the values to a rectangular space
                //DisplayCompass(Map, Player);
                CharInfo[] upbuf = new CharInfo[40 * 2];
                CharInfo[] leftbuf = new CharInfo[40 * 2];
                CharInfo[] middlebuf = new CharInfo[40 * 2];
                CharInfo[] rightbuf = new CharInfo[40 * 2];
                CharInfo[] downbuf = new CharInfo[40 * 2];
                SmallRect leftRect = new SmallRect() { Left = 0, Top = (short)(Map.MapYsize + 4), Right = 30, Bottom = (short)(Map.MapYsize + 5) };
                SmallRect rightRect = new SmallRect() { Left = 61, Top = (short)(Map.MapYsize + 4), Right = 100, Bottom = (short)(Map.MapYsize + 5) };
                SmallRect upRect = new SmallRect() { Left = 31, Top = (short)(Map.MapYsize + 2), Right = 90, Bottom = (short)(Map.MapYsize + 3) };
                SmallRect downRect = new SmallRect() { Left = 31, Top = (short)(Map.MapYsize + 6), Right = 90, Bottom = (short)(Map.MapYsize + 7) };
                SmallRect middleRect = new SmallRect() { Left = 31, Top = (short)(Map.MapYsize + 4), Right = 60, Bottom = (short)(Map.MapYsize + 5) };

                char[] upText = "(w : up) ".ToCharArray();
                char[] downText = "(s : down) ".ToCharArray();
                char[] leftText = "(a : left) ".ToCharArray();
                char[] rightText = "(d : right) ".ToCharArray();
                char[] middleText = "(space : you) ".ToCharArray();

                char[] descriptionToPrintLeft = Map.DescribePointOfInterest(Map.Coords[Player.XCoord - 1, Player.YCoord]).ToCharArray();
                char[] descriptionToPrintRight = Map.DescribePointOfInterest(Map.Coords[Player.XCoord + 1, Player.YCoord]).ToCharArray();
                char[] descriptionToPrintUp = Map.DescribePointOfInterest(Map.Coords[Player.XCoord, Player.YCoord - 1]).ToCharArray();
                char[] descriptionToPrintDown = Map.DescribePointOfInterest(Map.Coords[Player.XCoord, Player.YCoord + 1]).ToCharArray();
                char[] descriptionToPrintMiddle = Map.DescribePointOfInterest(Player.CurrentLocationID).ToCharArray();

                if (Map.IsPointOfInterest(Map.Coords[Player.XCoord, Player.YCoord - 1]))
                {
                    int x = 0;//FOR UP
                    for (int j = 0; j < upText.Length; j++)
                    {
                        upbuf[x].Attributes = (short)(13 | (0 << 4));
                        upbuf[x].Char.AsciiChar = (byte)upText[j];
                        x++;
                    }
                    for (int j = 0; j < descriptionToPrintUp.Length; j++)
                    {
                        upbuf[x].Attributes = (short)(15 | (0 << 4));
                        upbuf[x].Char.AsciiChar = (byte)descriptionToPrintUp[j];
                        x++;
                    }
                }
                else
                {
                    for (int j = 0; j < leftbuf.Length; j++)
                    {
                        upbuf[j].Char.AsciiChar = (byte)' ';
                    }
                }
                bool b2 = WriteConsoleOutput(h, upbuf,
                        new Coord() { X = 40, Y = 3 },
                        new Coord() { X = 0, Y = 0 },
                        ref upRect);
                //-----------------------------------------------------
                if (Map.IsPointOfInterest(Map.Coords[Player.XCoord - 1, Player.YCoord]))
                {
                    int x = 0;//FOR LEFT
                    for (int j = 0; j < leftText.Length; j++)
                    {
                        leftbuf[x].Attributes = (short)(14 | (0 << 4));
                        leftbuf[x].Char.AsciiChar = (byte)leftText[j];
                        x++;
                    }
                    for (int j = 0; j < descriptionToPrintLeft.Length; j++)
                    {
                        leftbuf[x].Attributes = (short)(15 | (0 << 4));
                        leftbuf[x].Char.AsciiChar = (byte)descriptionToPrintLeft[j];
                        x++;
                    }
                }
                else
                {
                    for (int j = 0; j < leftbuf.Length; j++)
                    {
                        leftbuf[j].Char.AsciiChar = (byte)' ';
                    }
                }
                bool b3 = WriteConsoleOutput(h, leftbuf,
                        new Coord() { X = 40, Y = 3 },
                        new Coord() { X = 0, Y = 0 },
                        ref leftRect);
                //------------------------------------------------------
                if (Map.IsPointOfInterest(Player.CurrentLocationID))
                {
                    int x = 0;//FOR MIDDLE
                    for (int j = 0; j < middleText.Length; j++)
                    {
                        middlebuf[x].Attributes = (short)(10 | (0 << 4));
                        middlebuf[x].Char.AsciiChar = (byte)middleText[j];
                        x++;
                    }
                    for (int j = 0; j < descriptionToPrintMiddle.Length; j++)
                    {
                        middlebuf[x].Attributes = (short)(15 | (0 << 4));
                        middlebuf[x].Char.AsciiChar = (byte)descriptionToPrintMiddle[j];
                        x++;
                    }
                }
                else
                {
                    for (int j = 0; j < middlebuf.Length; j++)
                    {
                        middlebuf[j].Char.AsciiChar = (byte)' ';
                    }
                }
                bool b4 = WriteConsoleOutput(h, middlebuf,
                        new Coord() { X = 40, Y = 3 },
                        new Coord() { X = 0, Y = 0 },
                        ref middleRect);
                //--------------------------------------------------------
                if (Map.IsPointOfInterest(Map.Coords[Player.XCoord + 1, Player.YCoord]))
                {
                    int x = 0;//FOR right
                    for (int j = 0; j < rightText.Length; j++)
                    {
                        rightbuf[x].Attributes = (short)(11 | (0 << 4));
                        rightbuf[x].Char.AsciiChar = (byte)rightText[j];
                        x++;
                    }
                    for (int j = 0; j < descriptionToPrintRight.Length; j++)
                    {
                        rightbuf[x].Attributes = (short)(15 | (0 << 4));
                        rightbuf[x].Char.AsciiChar = (byte)descriptionToPrintRight[j];
                        x++;
                    }
                }
                else
                {
                    for (int j = 0; j < rightbuf.Length; j++)
                    {
                        rightbuf[j].Char.AsciiChar = (byte)' ';
                    }
                }
                bool b5 = WriteConsoleOutput(h, rightbuf,
                        new Coord() { X = 40, Y = 3 },
                        new Coord() { X = 0, Y = 0 },
                        ref rightRect);
                //----------------------------------------------------------
                if (Map.IsPointOfInterest(Map.Coords[Player.XCoord, Player.YCoord + 1]))
                {
                    int x = 0;//FOR down
                    for (int j = 0; j < downText.Length; j++)
                    {
                        downbuf[x].Attributes = (short)(9 | (0 << 4));
                        downbuf[x].Char.AsciiChar = (byte)downText[j];
                        x++;
                    }
                    for (int j = 0; j < descriptionToPrintDown.Length; j++)
                    {
                        downbuf[x].Attributes = (short)(15 | (0 << 4));
                        downbuf[x].Char.AsciiChar = (byte)descriptionToPrintDown[j];
                        x++;
                    }
                }
                else
                {
                    for (int j = 0; j < downbuf.Length; j++)
                    {
                        downbuf[j].Char.AsciiChar = (byte)' ';
                    }
                }
                bool b6 = WriteConsoleOutput(h, downbuf,
                        new Coord() { X = 40, Y = 3 },
                        new Coord() { X = 0, Y = 0 },
                        ref downRect);
                //----------------------------------------------------------
                //Show player stats and inventory
                int z = 0;
                int printareaX = 50;
                int printareaY = 20;
                CharInfo[] inventorybuf = new CharInfo[printareaX * printareaY];
                SmallRect characterInfoRect = new SmallRect() { Left = 0, Top = (short)(Map.MapYsize + 8), Right = (short)printareaX, Bottom = (short)(Map.MapYsize + 8 + printareaY) };
                char[] characterNameHealth = $"{Player.Name}     {Player.Health}/{Player.MaxHealth} HP".ToCharArray();
                char[] characterWeaponStuff = $"Weapon: { Player.WeaponName}, {Player.WeaponAmmo} ammo, { Player.WeaponDamage} DMG, { Player.WeaponRange}".ToCharArray();
                char[] characterArmorStuff = $"Armor: {Player.ArmorName}, {Player.ArmorDurability} Durability".ToCharArray();
                char[] inventoryText = $"Inventory:".ToCharArray();

                for (int p = 0; p < characterNameHealth.Length; p++)//Name and health
                {
                    inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                    inventorybuf[z].Char.AsciiChar = (byte)characterNameHealth[p];
                    z++;
                }
                int spacesToPrint = RemainingCharactersInLine(50, z);//This value and following for loop will print blank spaces till the end of the line, or print an empty line if called twice
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                spacesToPrint = RemainingCharactersInLine(50, z);
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                for (int p = 0; p < characterWeaponStuff.Length; p++)//Weapon Info
                {
                    inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                    inventorybuf[z].Char.AsciiChar = (byte)characterWeaponStuff[p];
                    z++;
                }
                spacesToPrint = RemainingCharactersInLine(50, z);
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                for (int p = 0; p < characterArmorStuff.Length; p++)//Armor Info
                {
                    inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                    inventorybuf[z].Char.AsciiChar = (byte)characterArmorStuff[p];
                    z++;
                }
                spacesToPrint = RemainingCharactersInLine(50, z);
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                spacesToPrint = RemainingCharactersInLine(50, z);
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                for (int p = 0; p < inventoryText.Length; p++)//Inventory Title text
                {
                    inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                    inventorybuf[z].Char.AsciiChar = (byte)inventoryText[p];
                    z++;
                }
                spacesToPrint = RemainingCharactersInLine(50, z);
                for (int p = 0; p < spacesToPrint; p++)
                {
                    inventorybuf[z].Char.AsciiChar = (byte)' ';
                    z++;
                }
                for (int p = 0; p < 10; p++)//Inventory Stuff
                {
                    if (Player.Inventory[p, 0] != "")
                    {
                        char[] inventoryItem = $"(#{p + 1}) {Player.Inventory[p, 0]} (x{Player.Inventory[p, 1]})".ToCharArray();
                        for (int l = 0; l < inventoryItem.Length; l++)
                        {
                            inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                            inventorybuf[z].Char.AsciiChar = (byte)inventoryItem[l];
                            z++;
                        }
                        Player.InventoryIsEmpty = false;
                        spacesToPrint = RemainingCharactersInLine(50, z);
                        for (int s = 0; s < spacesToPrint; s++)
                        {
                            inventorybuf[z].Char.AsciiChar = (byte)' ';
                            z++;
                        }
                    }
                }
                if (Player.InventoryIsEmpty == true)
                {
                    char[] emptyText = "(Empty)".ToCharArray();
                    for (int p = 0; p < emptyText.Length; p++)
                    {
                        inventorybuf[z].Attributes = (short)(7 | (0 << 4));
                        inventorybuf[z].Char.AsciiChar = (byte)emptyText[p];
                        z++;
                    }
                }
                bool b7 = WriteConsoleOutput(h, inventorybuf,
                        new Coord() { X = 51, Y = 20 },
                        new Coord() { X = 0, Y = 0 },
                        ref characterInfoRect);
            }//keep this at the end
        }//End Test2

        
        public int RemainingCharactersInLine(int xMaxValue, int currentCharCount)
        {
            xMaxValue = 51;
            int currentLineMax = 1;
            for (int i = 1; i < 20; i++)
            {
                if (i * xMaxValue > currentCharCount)
                {
                    currentLineMax = i * xMaxValue;
                    break;
                }
            }
            return currentLineMax - currentCharCount;
        }
    }
}
