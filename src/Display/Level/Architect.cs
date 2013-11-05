using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Flakcore.Physics;

namespace Flakcore.Display.Level
{
    internal class Architect
    {
        private Random Random;

        private List<RoomType> NormalRoomTypes;
        private List<RoomType> StartRoomTypes;
        private List<RoomType> EndRoomTypes;

        private Vector2 StartPosition;
        private Vector2 EndPosition;
        private RoomType[,] Plan;
 
        public Architect()
        {
            this.Random = new Random();
            this.Plan = new RoomType[Level.LEVEL_WIDTH, Level.LEVEL_HEIGHT];
            this.LoadRoomTypes();
        }

        public RoomType[,] GenerateLevel()
        {
            this.PlaceStart();
            this.PlaceEnd();
            this.MakePath();
            this.FillEmptySpace();

            return this.Plan;
        }

        private void LoadRoomTypes()
        {
            // load normal rooms
            DirectoryInfo dir = new DirectoryInfo(Controller.Content.RootDirectory + "\\rooms");
            FileInfo[] files = dir.GetFiles("*.*");

            this.NormalRoomTypes = new List<RoomType>(files.Length);
            foreach (FileInfo file in files)
            {
                string roomName = Path.GetFileNameWithoutExtension(file.Name);
                this.NormalRoomTypes.Add(new RoomType(roomName, RoomTypes.ROUTE));
            }

            // load stars
            dir = new DirectoryInfo(Controller.Content.RootDirectory + "\\rooms\\starts");
            files = dir.GetFiles("*.*");

            this.StartRoomTypes = new List<RoomType>(files.Length);
            foreach (FileInfo file in files)
            {
                string roomName = Path.GetFileNameWithoutExtension(file.Name);
                this.StartRoomTypes.Add(new RoomType("starts/" + roomName, RoomTypes.START));
            }

            
            // load ends
            dir = new DirectoryInfo(Controller.Content.RootDirectory + "\\rooms\\ends");
            files = dir.GetFiles("*.*");

            this.EndRoomTypes = new List<RoomType>(files.Length);
            foreach (FileInfo file in files)
            {
                string roomName = Path.GetFileNameWithoutExtension(file.Name);
                this.EndRoomTypes.Add(new RoomType("ends/" + roomName, RoomTypes.END));
            }
        }

        private void PlaceStart()
        {
            int xPosition = this.Random.Next(1, Level.LEVEL_WIDTH-1);
            this.Plan[xPosition,0] = this.GetRandomStart();

            this.StartPosition = new Vector2(xPosition, 0);
        }

        private void PlaceEnd()
        {
            int xPosition = (this.StartPosition.X < 2 ? 1 : 2) + this.Random.Next(0, 1);
            this.Plan[xPosition, 3] = this.GetRandomEnd();

            this.EndPosition = new Vector2(xPosition, 3);
        }

        private void MakePath()
        {
            bool foundEnd = false;
            Vector2 position = this.StartPosition;
            Directions direction = this.StartPosition.X >= 2 ? Directions.LEFT : Directions.RIGHT;

            while (!foundEnd)
            {
                if (direction == Directions.LEFT)
                {
                    position.X--;

                    if (position == EndPosition)
                        break;
                    
                    if (position.X != 0)
                    {
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(false, false, true, true));
                    }
                    else
                    {
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(false, true, false, true));
                        position.Y++;
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(true, false, false, true));
                        direction = Directions.RIGHT;
                    }
                }
                else
                {
                    position.X++;

                    if (position == EndPosition)
                        break;
                    
                    if (position.X != Level.LEVEL_WIDTH - 1)
                    {
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(false, false, true, true));
                    }
                    else
                    {
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(false, true, true, false));
                        position.Y++;
                        this.Plan[(int)position.X, (int)position.Y] = this.GetRandomRoom(new Sides(true, false, true, false));
                        direction = Directions.LEFT;
                    }
                }

                if (position == EndPosition)
                    break;
            }
        }

        private void FillEmptySpace()
        {
            for (int x = 0; x < Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Level.LEVEL_HEIGHT; y++)
                {
                    if (this.Plan[x, y] == null)
                    {
                        if (x == 0)
                            this.Plan[x, y] = this.GetRandomRoom(new Sides(false, false, false, true));
                        else if(x == Level.LEVEL_WIDTH-1)
                            this.Plan[x, y] = this.GetRandomRoom(new Sides(false, false, true, false));
                    }
                }
            }
        }

        private RoomType GetRandomStart()
        {
 	        return this.StartRoomTypes[Random.Next(0, this.StartRoomTypes.Count)];
        }

        private RoomType GetRandomEnd()
        {
 	        return this.EndRoomTypes[Random.Next(0, this.EndRoomTypes.Count)];
        }

        private RoomType GetRandomRoom(Sides sides)
        {
            List<RoomType> validRoomTypes = new List<RoomType>(this.NormalRoomTypes.Count);

            foreach (RoomType roomType in this.NormalRoomTypes)
            {
                if (roomType.Sides == sides)
                    validRoomTypes.Add(roomType);
            }

            return validRoomTypes[Random.Next(0, validRoomTypes.Count)];
        }
    }

    enum Directions
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
