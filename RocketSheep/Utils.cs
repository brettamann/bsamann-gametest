using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    class Utils
    {
        public int slowTextSpeed = 5;
        private ConsoleKeyInfo keypress;

        public Utils()
        {
        }

        public int SlowTextSpeed
        {
            get
            {
                return slowTextSpeed;
            }
            set
            {
                slowTextSpeed = value;
            }
        }

        public void Pause(double seconds)
        {
            System.Threading.Thread.Sleep(Convert.ToInt32(Math.Round(seconds * 1000)));
        }

        public void SlowPrintWithBreak(String inputString)
        {
            Console.WriteLine("");
            char[] stringArray = inputString.ToCharArray();
            for (int i = 0; i < stringArray.Length; i++)
            {
                char letter = stringArray[i];
                System.Threading.Thread.Sleep(slowTextSpeed);
                Console.Write(letter);
            }
        }

        public void FastPrintWithBreak(String inputString)
        {
            Console.WriteLine(inputString);
        }

        public void ChangeTextSpeed(/*Player Player*/)
        {
            Utils Utils = new Utils();
            SlowPrintWithBreak("Select a text speed. The current setting is: ");
            switch (slowTextSpeed)
            {
                case 30: Console.Write($"SLOW"); break;
                case 20: Console.Write($"MEDIUM"); break;
                case 10: Console.Write($"FAST"); break;
                case 1: Console.Write($"ULTRA"); break;
            }
            SlowPrintWithBreak("Note: for convenience this will not affect most menus, and some other text.");

            SlowPrintWithBreak($"1) Slow\n2) Medium\n3) Fast\n4) Ultra\nEnter your selection: ");
            int userInput = GetUserInputInRange(1, 4);
            switch (userInput)
            {
                case 1: slowTextSpeed = 30; break;
                case 2: slowTextSpeed = 20; break;
                case 3: slowTextSpeed = 10; break;
                case 4: slowTextSpeed = 1; break;
            }
        }//End ChangeTextSpeed

        public int GetUserInputInRange(int min, int max)//this calls the validator and returns the input as an int value
        {
            int userInput = 999;
            while (userInput > max || userInput < min)
            {
                keypress = Console.ReadKey();
                if (char.IsDigit(keypress.KeyChar))//this has to be an if to account for the fact that it might not be a digit
                {
                    userInput = int.Parse(keypress.KeyChar.ToString()); // use Parse if it's a Digit. This allows it to change the input into an int. I might be able to just use a convert.### command too
                }
                else { SlowPrintWithBreak($"ERROR, please enter a number between {min}-{max}"); }
            }
            return userInput;
        }//END GETUSERINPUT

        public String GetUserInputText(int max)//this calls the validator and returns the input as an int value
        {
            String userInput = "";
            userInput = Console.ReadLine().ToString();
            while (userInput.Trim() == "" || userInput.Length > max)
            {
                if (userInput.Length > max)
                    SlowPrintWithBreak($"ERROR, Please enter less than {max} characters.");
                if (userInput.Trim() == "")
                    SlowPrintWithBreak("ERROR, Empty response. Please input an actual response.");
                userInput = Console.ReadLine().ToString();
            }
            return userInput;
        }//END GETUSERINPUT

        public char GetUserKeyPress()
        {
            keypress = Console.ReadKey();
            if (char.IsLetter(keypress.KeyChar))
            {
                char userInput = char.Parse(keypress.KeyChar.ToString());
                return userInput;
            }
            if (char.IsDigit(keypress.KeyChar))
            {
                char userInput = char.Parse(keypress.KeyChar.ToString());
                return userInput;
            }
            return ' ';
        }//END GETUSERINPUT

        public char GetUserNumberPress()
        {
            keypress = Console.ReadKey();
            if (char.IsDigit(keypress.KeyChar))
            {
                char userInput = char.Parse(keypress.KeyChar.ToString());
                return userInput;
            }
            return ' ';
        }//END GETUSERINPUT

        public int GetUserInt()
        {
            return GetUserInputInRange(0,9);
        }//END GETUSERINPUT

        public int Roll(int min, int max)
        {//all of the random buffers are to further randomize the values, as without it they tend to be the same
            Random random = new Random(DateTime.Now.Millisecond);
            int buffer = random.Next(min, max);
            System.Threading.Thread.Sleep(random.Next(0, 3));
            buffer = random.Next(3, 5);
            System.Threading.Thread.Sleep(random.Next(buffer, 11));
            System.Threading.Thread.Sleep(random.Next(2, 9));
            System.Threading.Thread.Sleep(random.Next(0, 7));
            buffer = random.Next(min, max);
            return buffer;
        }//END ROLL

        public int FastRoll(int min, int max)
        {//all of the random buffers are to further randomize the values, as without it they tend to be the same
            Random random = new Random(DateTime.Now.Millisecond);
            int buffer = random.Next(min, max);
            return buffer;
        }//END ROLL

        public String GetRandomUsername()
        {
            int name = Roll(1, 26);//26 is found after 18
            switch (name)
            {
                case 1: return "Robert";
                case 2: return "Jon";
                case 3: return "Caleb";
                case 4: return "Shawn";
                case 5: return "Devin";
                case 6: return "Tyler";
                case 7: return "Samantha";
                case 8: return "Julie";
                case 9: return "Anna";
                case 10: return "Alex";
                case 11: return "Taylor";
                case 12: return "James";
                case 13: return "Jamie";
                case 14: return "Horace";
                case 15: return "Bertha";
                case 16: return "Jonny";
                case 17: return "Agatha";
                case 18: return "Koa";
                case 26: return "Billy";
                case 19: return "Sasha";
                case 20: return "Lawasha";
                case 21: return "Ladrya";
                case 22: return "Kip";
                case 23: return "Fred";
                case 24: return "George";
                case 25: return "Lilly";
                default: return "A Total Idiot";
            }
        }

        public int ConvertToIntFromChar(char toConvert)
        {
            //String toString = Convert.ToString(toConvert);
            int toInt = toConvert - '0';
            return toInt;
        }

        public void Turorial()
        {
            SlowPrintWithBreak("This is you -> ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write((char)MapPointTypes.player);
            Console.ResetColor();
            SlowPrintWithBreak("...P for player. You decide what you are, I guess. You can move! use the WASD keys to move when playing. (w) up, (a) left, (s) down, (d) right");
            Console.WriteLine();

            SlowPrintWithBreak("This is where you have to get to beat a level -> ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write((char)MapPointTypes.goal);
            Console.ResetColor();
            SlowPrintWithBreak("Just walk on it and hit space bar. Simple, right?");
            Console.WriteLine();

            SlowPrintWithBreak("Wrong! This is an enemy -> ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write((char)MapPointTypes.monster);
            Console.ResetColor();
            SlowPrintWithBreak("They love you and want to hug you to death. I wouldn't advise letting that happen.\nThey're wolves, or goblins, or zombies, or mutant squirrels... use your imagination.\nHit them with weapons or get them to walk through hazards to dissuade their hugs.");
            Console.WriteLine();

            SlowPrintWithBreak("This is a weapon -> ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write((char)MapPointTypes.gun);
            Console.ResetColor();
            SlowPrintWithBreak("Fire it using numbers 1-3, 4, 6, 7-9. It's most intuitive on a number pad. 4 = left, 7 = up-left, 8 = up, etc.\nWatch your ammo! Enemies sometimes take multiple hits. You can break barricades with a gun.\nAlso, different weapons are marked with different letters, but always a green highlight\nWalk over it and hit spacebar to collect");
            Console.WriteLine();

            SlowPrintWithBreak("This is a barricade -> ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write((char)MapPointTypes.barricade);
            Console.ResetColor();
            SlowPrintWithBreak("You can break them with a gun. Enemies will be temporarily stumped by them, but don't expect it to last forever.");
            Console.WriteLine();

            SlowPrintWithBreak("You can walk through doors -> ");
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write((char)MapPointTypes.door);
            Console.ResetColor();
            SlowPrintWithBreak("You don't have to do anything special. Just walk on through. Enemies CAN'T walk through doors, but they CAN go through windows -> ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write((char)MapPointTypes.window);
            Console.ResetColor();
            Console.Write(" and YOU can't.");
            Console.WriteLine();

            SlowPrintWithBreak("Stay out of hazards -> ");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write((char)MapPointTypes.hazard);
            Console.ResetColor();
            SlowPrintWithBreak(" But take note, they'll harm you and enemies alike. Enemies will try to avoid it, but get them frustrated enough and they just might try going through. Certain weapons can create hazards - keep that in mind.");
            Console.ResetColor();
            Console.WriteLine();

            SlowPrintWithBreak("You have an inventory! You'll always see it at the bottom of the screen. To switch your current weapon with another, press \'i\' and then the number of the weapon you want to switch.\nIf your current weapon is out of ammo, you'll automatically get rid of it.");
            Console.WriteLine();

            SlowPrintWithBreak("A few more notes: enemies can't see or hear you from anywhere. Stay away and out of their line of sight, and they may ignore you completely.\nRemember that they can see - and move - through windows. They can also see through/over/around barricades.");
            SlowPrintWithBreak("If you forget what something is, don't worry! There's a compass that'll pop up above your inventory. It'll inform you about your surroundings.\nJust don't forget what an enemy is. You won't last long inspecting them.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
