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
            // List of ships
            List<Ship> ListOfShips = new List<Ship>();
            List<LostShip> LostShips = new List<LostShip>();
            List<string> ListOfCommands = new List<string>();

            /*
            Console.WriteLine("Please insert map size consisting of two integers. E.g.: 5 5");
            string ReadMapValues = Console.ReadLine();
            string[] mapSizeValues = ReadMapValues.Split(' ');
            int Height = int.Parse(mapSizeValues[0]);
            int Width = int.Parse(mapSizeValues[1]);
            */

            Console.WriteLine("Please specify the path of the sample input:");
            // Reads the file
            var path = Console.ReadLine();
            var txtFile = System.IO.File.ReadAllText(path);

            // Split the txt file into lines
            var lines = txtFile.Split(new[] { '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Loop all lines
            int Width = 0, Height = 0;
            int intNr = 0; // used to loop through lines
            int ShipsLoop = 0; // used to continue looping through all ships
            string CD = "";
            int CoordinateX = 0, CoordinateY = 0;
            foreach (var line in lines)
            {
                // Split line by space and loop through all the numeric values
                foreach (var s in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Map info
                    if (intNr == 0)
                    {
                        Height = int.Parse(s);
                        intNr++;
                    }
                    else if (intNr == 1)
                    {
                        Width = int.Parse(s);
                        intNr++;
                    }

                    // Ship info
                    else if (intNr == 2 + ShipsLoop)
                    {
                        CoordinateX = int.Parse(s);
                        intNr++;
                    }
                    else if (intNr == 3 + ShipsLoop)
                    {
                        CoordinateY = int.Parse(s);
                        intNr++;
                    }
                    else if (intNr == 4 + ShipsLoop)
                    {
                        CD = s;
                        intNr++;
                    }
                    else if (intNr == 5 + ShipsLoop)
                    {
                        ListOfCommands.Add(s);
                        Coordinates Coordinate = new Coordinates(CoordinateX, CoordinateY);
                        Ship ship = new Ship(Coordinate, CD, false);
                        ListOfShips.Add(ship);
                        intNr++;
                        ShipsLoop += 4;
                    }
                }
            }

            // Creating a 2D map provided
            Map map = Create2DMap(Height, Width);

            /*
            Console.WriteLine("Please insert ship's coordinates. E.g.: 1 1 E");
            string ReadCoordinateValues = Console.ReadLine();
            string[] coordinatesValues = ReadCoordinateValues.Split(' ');
            Coordinates Coordinate = new Coordinates(int.Parse(coordinatesValues[0]), int.Parse(coordinatesValues[1]));
            string CD = coordinatesValues[2];
            */

            // Creating a ship
            //Ship ship = new Ship(Coordinate, CD, false);

            
            /*
            // Reading commands and computing ship's location after input
            Console.WriteLine("Please insert a sequence of commands for the ship(R-right, L-left, F-forward). E.g. RFRFRFRF");
            string ReadCommands = Console.ReadLine();
            char[] Commands = ReadCommands.ToCharArray();
            */
            for(int i = 0; i < ListOfShips.Count(); i++) {
                char[] Commands = ListOfCommands[i].ToCharArray();
                for(int j = 0; j < Commands.Count(); j++) 
                {
                    ComputeCommand(ListOfShips[i], map, Commands[j], LostShips);
                }
                if (ListOfShips[i].IsShipLost) {
                    Console.WriteLine(ListOfShips[i].Coordinates.CoordinateX + " " + ListOfShips[i].Coordinates.CoordinateY + " " + ListOfShips[i].CardinalDirection + " LOST");
                } 
                else Console.WriteLine(ListOfShips[i].Coordinates.CoordinateX + " " + ListOfShips[i].Coordinates.CoordinateY + " " + ListOfShips[i].CardinalDirection);
            }

            Console.ReadLine();
        }

        // Create the map
        private static Map Create2DMap(int Height, int Width)
        {
            List<Coordinates> CoordinatesList = new List<Coordinates>();
            for (var Col = 0; Col < Height; Col++)
            {
                for (var Row = 0; Row < Width; Row++)
                {
                    Coordinates Coordinate = new Coordinates(Row, Col);
                    CoordinatesList.Add(Coordinate);
                    //Console.WriteLine("Added coordinates. X: " + Coordinate.CoordinateX + ", Y: " + Coordinate.CoordinateY);
                }
            }

            // Returns a created map from provided width and height of the map
            Map map = new Map(CoordinatesList, Width, Height, Height*Width);
            return map;
        }

        // Checks if there is a lost ship at those coordinates already(does not allow another ship to fall off the map there)
        private static bool FoundLostShip(List<LostShip> lostShips, Ship ship) 
        {
            bool foundLostShip = false;
            for (int i = 0; i < lostShips.Count(); i++) 
            {
                if (lostShips[i].LostCoordinates == ship.Coordinates) 
                {
                    foundLostShip = true;
                }
            }
            return foundLostShip;
        }

        // Computes the location and cardinal direction of the ship
        private static void ComputeCommand(Ship ship, Map map, char Command, List<LostShip> ListOfLostShips) 
        {
            switch (Command)
            {
            case 'F':
                if (ship.CardinalDirection == "E") 
                {
                    ship.Coordinates.CoordinateX += 1;
                    if (ship.Coordinates.CoordinateX > map.Width-1 && FoundLostShip(ListOfLostShips, ship) == false) 
                    {
                        ship.IsShipLost = true;
                        LostShip lostShip = new LostShip(ship, ship.Coordinates);
                        ListOfLostShips.Add(lostShip);
                    }
                    else if (ship.Coordinates.CoordinateX > map.Width-1 && FoundLostShip(ListOfLostShips, ship) == true) 
                    {
                        ship.Coordinates.CoordinateX -= 1;
                    }
                }
                else if (ship.CardinalDirection == "S") 
                {
                    ship.Coordinates.CoordinateY -= 1;
                    if (ship.Coordinates.CoordinateY > map.Height-1 && FoundLostShip(ListOfLostShips, ship) == false) 
                    {
                        ship.IsShipLost = true;
                        LostShip lostShip = new LostShip(ship, ship.Coordinates);
                        ListOfLostShips.Add(lostShip);
                    }
                    else if (ship.Coordinates.CoordinateY > map.Height-1 && FoundLostShip(ListOfLostShips, ship) == true) 
                    {
                        ship.Coordinates.CoordinateY += 1;
                    }
                }
                else if (ship.CardinalDirection == "W") 
                {
                    ship.Coordinates.CoordinateX -= 1;
                    if (ship.Coordinates.CoordinateX < 0 && FoundLostShip(ListOfLostShips, ship) == false) 
                    {
                        ship.IsShipLost = true;
                        LostShip lostShip = new LostShip(ship, ship.Coordinates);
                        ListOfLostShips.Add(lostShip);
                    }
                    else if (ship.Coordinates.CoordinateX < 0 && FoundLostShip(ListOfLostShips, ship) == true)
                    {
                        ship.Coordinates.CoordinateX += 1;
                    }
                }
                else if (ship.CardinalDirection == "N") 
                {
                    ship.Coordinates.CoordinateY += 1;
                    if (ship.Coordinates.CoordinateY < 0 && FoundLostShip(ListOfLostShips, ship) == false) 
                    {
                        ship.IsShipLost = true;
                        LostShip lostShip = new LostShip(ship, ship.Coordinates);
                        ListOfLostShips.Add(lostShip);                        
                    }
                    else if (ship.Coordinates.CoordinateY < 0 && FoundLostShip(ListOfLostShips, ship) == true) 
                    {
                        ship.Coordinates.CoordinateY -= 1;
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

    // Lost ship object, used to "block" path for another ship that wants to pass that location
    public class LostShip
    {
        public Ship Ship { get; set; }
        public Coordinates LostCoordinates { get; set; }

        public LostShip(Ship ship, Coordinates LostCoord) 
        {
            Ship = ship;
            LostCoordinates = LostCoord;
        }
    }

    // Ship object
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

        public Map(List<Coordinates> CoordList, int height, int width, int mapSize) 
        {
            ListOfCoordinates = CoordList;
            Height = height;
            Width = width;
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
