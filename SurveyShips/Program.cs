using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShips
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a 2D Array
            //string[,] Array2D = new string[Width, Height];
            Console.WriteLine("Please insert map size consisting of two integers. E.g.: 5 5");
            string ReadMapValues = Console.ReadLine();
            string[] mapSizeValues = ReadMapValues.Split(' ');
            int Height = int.Parse(mapSizeValues[0]);
            int Width = int.Parse(mapSizeValues[1]);

            Create2DMap(Height, Width);

            Console.WriteLine("Please insert ship's coordinates. E.g.: 1 1 E");
            string ReadCoordinateValues = Console.ReadLine();
            string[] coordinatesValues = ReadCoordinateValues.Split(' ');
            Coordinates Coordinate = new Coordinates(int.Parse(coordinatesValues[0]), int.Parse(coordinatesValues[1]));
            string CD = coordinatesValues[2];

            Ship ship = new Ship(Coordinate, CD);

            /*
            Console.WriteLine("Please insert a sequence of commands for the ship(R-right, L-left, F-forward). E.g. RFRFRFRF");
            string ReadCommands = Console.ReadLine();
            char[] Commands = ReadCommands.ToCharArray();
            for(int i = 0; i < Commands.Count(); i++) 
            {
                Console.WriteLine(Commands[i]);
            }
            */



            Console.ReadLine();
        }

        // Create the map
        private static void Create2DMap(int Height, int Width)
        {
            List<Coordinates> CoordinatesList = new List<Coordinates>();
            //Adding the map to 2D array
            for (var Col = 0; Col < Height; Col++)
            {
                for (var Row = 0; Row < Width; Row++)
                {
                    Coordinates Coordinate = new Coordinates(Row, Col);
                    CoordinatesList.Add(Coordinate);
                    Console.WriteLine("Added coordinates. X: " + Coordinate.CoordinateX + ", Y: " + Coordinate.CoordinateY);
                }
            }

            Map map = new Map(CoordinatesList, Height*Width);
        }
    }

    public class Ship
    {
        public Coordinates Coordinates { get; set; }
        public string CardinalDirection { get; set; }

        public Ship(Coordinates Coord, string CD)
        {
            Coordinates = Coord;
            CardinalDirection = CD;
        }
    }

    public class Map
    {
        public List<Coordinates> ListOfCoordinates { get; set; }
        public int MapSize { get; set; }

        public Map(List<Coordinates> CoordList, int mapSize) 
        {
            ListOfCoordinates = CoordList;
            MapSize = mapSize;
        }
    }

    public class Coordinates
    {
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }

        public Coordinates(int CoordX, int CoordY)
        {
            CoordinateX = CoordX;
            CoordinateY = CoordY;
        }
    }

}
