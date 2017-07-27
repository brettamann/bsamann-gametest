using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RocketSheep
{
    class SaveLoad
    {
        public void LoadMap()
        {


            /// <summary>
            /// Reads an object instance from a binary file.
            /// </summary>
            /// <typeparam name="T">The type of object to read from the XML.</typeparam> --------------This was changed to "Player"
            /// <param name="filePath">The file path to read the object instance from.</param>
            /// <returns>Returns a new instance of the object read from the binary file.</returns>

        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void SaveMap<Map>(string filePath, Map objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))//I don't entirely understand why it's set up the way it is here
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static Map LoadMapObject<Map>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Map)binaryFormatter.Deserialize(stream);
            }
        }

        public Map LoadMapTxt(Player Player)
        {

            Map loadedMap = new Map();
            /*
            String[] coordAsTxt = File.ReadAllLines(filePath);
            for (int i = 0; i < loadedMap.Coords.Length; i++)
            {
                String[] temp = coordAsTxt[i].Split('\n');
                loadedMap.Coords[]
            }*/
            FileStream filestream = File.OpenRead(Player.FilePath);
            StreamReader reader = new StreamReader(filestream);
            int xSize = Convert.ToInt32(reader.ReadLine());
            loadedMap.MapXsize = xSize;
            int ySize = Convert.ToInt32(reader.ReadLine());
            loadedMap.MapYsize = ySize;
            loadedMap.Coords = new char[xSize + 2, ySize + 2];
            for (int y = 0; y < ySize; y++)
            {
                char[] xCoords = reader.ReadLine().ToCharArray();
                for (int x = 0; x < xSize; x++)
                {

                    if (y == 0 || y == ySize - 1)
                    {
                        loadedMap.Coords[x, y] = '-';
                    }
                    else if (x == 0 || x == xSize - 1)
                    {
                        loadedMap.Coords[x, y] = '|';
                    }
                    else if (x < xSize - 1)
                        loadedMap.Coords[x, y] = xCoords[x - 1];
                    if (loadedMap.Coords[x, y] == (char)MapPointTypes.player)
                    {
                        Player.XCoord = x;
                        Player.YCoord = y;
                    }
                }
            }
            return loadedMap;
        }

        public static void SavePlayer<Player>(string filePath, Player objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))//I don't entirely understand why it's set up the way it is here
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static Player LoadPlayer<Player>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Player)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
