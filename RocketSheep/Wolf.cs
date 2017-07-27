using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    class Wolf
    {
        private int xCoord;
        private int yCoord;
        
        private double health = 2;
        private bool isSpecial;
        private double speed;
        private double[] damage;
        private double energy;

        private char currentLocationID = ' ';
        private bool hasSeenPlayer = false;
        private bool isAlive = true;

        public int XCoord { get => xCoord; set => xCoord = value; }
        public int YCoord { get => yCoord; set => yCoord = value; }
        public double Health { get => health; set => health = value; }
        public bool IsSpecial { get => isSpecial; set => isSpecial = value; }
        public double Speed { get => speed; set => speed = value; }
        public double[] Damage { get => damage; set => damage = value; }
        public double Energy { get => energy; set => energy = value; }
        public char CurrentLocationID { get => currentLocationID; set => currentLocationID = value; }
        public bool HasSeenPlayer { get => hasSeenPlayer; set => hasSeenPlayer = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }

        public void MoveWolf()
        {

        }
    }
}
