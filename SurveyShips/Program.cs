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

            Console.WriteLine("Please insert map size consisting of two integers. E.g.: 5 5");
            string ReadMapValues = Console.ReadLine();
            string[] mapSizeValues = ReadMapValues.Split(' ');
            int Height = int.Parse(mapSizeValues[0]);
            int Width = int.Parse(mapSizeValues[1]);

            // Creating a 2D map provided
            Map map = Create2DMap(Height, Width);

            Console.WriteLine("Please insert ship's coordinates. E.g.: 1 1 E");
            string ReadCoordinateValues = Console.ReadLine();
            string[] coordinatesValues = ReadCoordinateValues.Split(' ');
            Coordinates Coordinate = new Coordinates(int.Parse(coordinatesValues[0]), int.Parse(coordinatesValues[1]));
            string CD = coordinatesValues[2];

            // Creating a ship
            Ship ship = new Ship(Coordinate, CD, false);

            
            // Reading commands and computing ship's location after input
            Console.WriteLine("Please insert a sequence of commands for the ship(R-right, L-left, F-forward). E.g. RFRFRFRF");
            string ReadCommands = Console.ReadLine();
            char[] Commands = ReadCommands.ToCharArray();
            for(int i = 0; i < Commands.Count(); i++) 
            {
                ComputeCommand(ship, map, Commands[i]);
            }

            // Checking if ship is lost and printing it's location
            if (ship.IsShipLost) {
            Console.WriteLine(ship.Coordinates.CoordinateX + " " + ship.Coordinates.CoordinateY + " " + ship.CardinalDirection + "LOST");
            } else Console.WriteLine(ship.Coordinates.CoordinateX + " " + ship.Coordinates.CoordinateY + " " + ship.CardinalDirection);

            Console.ReadLine();
        }

        // Create the map
        private static Map Create2DMap(int Height, int Width)
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

            Map map = new Map(CoordinatesList, Width, Height, Height*Width);
            return map;
        }

        // Computes the location of the ship
        private static void ComputeCommand(Ship ship, Map map, char Command) 
        {
            switch (Command)
            {
            case 'F':
                if (ship.CardinalDirection == "E") 
                {
                    ship.Coordinates.CoordinateX += 1;
                    if (ship.Coordinates.CoordinateX > map.Width-1) 
                    {
                        ship.IsShipLost = true;
                    }
                }
                else if (ship.CardinalDirection == "S") 
                {
                    ship.Coordinates.CoordinateY -= 1;
                    if (ship.Coordinates.CoordinateY > map.Height-1) 
                    {
                        ship.IsShipLost = true;
                    }
                }
                else if (ship.CardinalDirection == "W") 
                {
                    ship.Coordinates.CoordinateX -= 1;
                    if (ship.Coordinates.CoordinateX < 0) 
                    {
                        ship.IsShipLost = true;
                    }
                }
                else if (ship.CardinalDirection == "N") 
                {
                    ship.Coordinates.CoordinateY += 1;
                    if (ship.Coordinates.CoordinateY < 0) 
                    {
                        ship.IsShipLost = true;
                    }
                }
                break;
            case 'R':
                if (ship.CardinalDirection == "E")
                {
                    ship.CardinalDirection = "S";
                }
                else if (ship.CardinalDirection == "S")
                {
                    ship.CardinalDirection = "W";
                }
                else if (ship.CardinalDirection == "W")
                {
                    ship.CardinalDirection = "N";
                }
                else if (ship.CardinalDirection == "N")
                {
                    ship.CardinalDirection = "E";
                }
                break;
            case 'L':
                if (ship.CardinalDirection == "E")
                {
                    ship.CardinalDirection = "N";
                }
                else if (ship.CardinalDirection == "N")
                {
                    ship.CardinalDirection = "W";
                }
                else if (ship.CardinalDirection == "W")
                {
                    ship.CardinalDirection = "S";
                }
                else if (ship.CardinalDirection == "S")
                {
                    ship.CardinalDirection = "E";
                }
                break;
            }
        }
    }

    public class Ship
    {
        public Coordinates Coordinates { get; set; }
        public string CardinalDirection { get; set; }
        public bool IsShipLost { get; set; }

        public Ship(Coordinates Coord, string CD, bool isLost)
        {
            Coordinates = Coord;
            CardinalDirection = CD;
            IsShipLost = isLost;
        }
    }

    public class Map
    {
        public List<Coordinates> ListOfCoordinates { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MapSize { get; set; }

        public Map(List<Coordinates> CoordList, int width, int height, int mapSize) 
        {
            ListOfCoordinates = CoordList;
            Width = width;
            Height = height;
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
