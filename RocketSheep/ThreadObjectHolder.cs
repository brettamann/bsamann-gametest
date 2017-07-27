using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSheep
{
    class ThreadObjectHolder
    {
        private Map map;
        private Player player;

        public ThreadObjectHolder(Player _player, Map _map)
        {
            this.Map = _map;
            this.Player = _player;
        }

        internal Map Map { get => map; set => map = value; }
        internal Player Player { get => player; set => player = value; }
    }
}
