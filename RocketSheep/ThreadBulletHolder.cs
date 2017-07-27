using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    class ThreadBulletHolder
    {
        private Map map;
        private Player player;
        private int direction;

        public ThreadBulletHolder(Player _player, Map _map, int _direction)
        {
            this.Map = _map;
            this.Player = _player;
            this.Direction = _direction;
        }

        internal int Direction { get => direction; set => direction = value; }
        internal Map Map { get => map; set => map = value; }
        internal Player Player { get => player; set => player = value; }
    }
}
