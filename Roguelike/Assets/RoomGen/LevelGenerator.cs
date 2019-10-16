using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomGen
{
    public class LevelGenerator
    {
        private readonly System.Random rand;

        private readonly List<Room> rooms = new List<Room>();
        private readonly List<Room> placedRooms = new List<Room>();
        private readonly List<Room> pendingRooms = new List<Room>();
        private readonly List<Room> adjacentRooms = new List<Room>();
        private readonly List<Room> doorlessRooms = new List<Room>();
        private readonly List<RoomDefinition> roomDefs = new List<RoomDefinition>();
        private readonly List<Door> doors = new List<Door>();

        private readonly List<Vector2Int> positions = new List<Vector2Int>();
        private Room current;

        private uint erroredRooms;
        private uint iteration;

        public uint extraRoomIterations = 10;

        private Func<Vector2Int, bool> debug;

        public Func<Vector2Int, bool> Debug
        {
            set
            {
                debug = value;
            }
        }

        public List<Door> Doors
        {
            get
            {
                return new List<Door>(doors);
            }
        }

        public LevelGenerator(int seed)
        {
            rand = new System.Random(seed);
        }

        public LevelGenerator()
        {
            rand = new System.Random();
        }

        public bool Generate()
        {
            erroredRooms = 0;
            CreateRoomsFromDefinition();
            RandomlyPlaceRooms();
            AddDoors();
            return erroredRooms > 0;
        }

        public void AddRoomDef(RoomDefinition def)
        {
            roomDefs.Add(def);
        }

        private void CreateRoomsFromDefinition()
        {
            rooms.Clear();

            foreach (RoomDefinition def in roomDefs)
            {
                Room room = new Room();
                rooms.Add(room);
                def.SetRoom(room);

                room.width = def.GetRandomWidth(rand);
                room.height = def.GetRandomHeight(rand);
                room.error = false;
            }
        }

        // Get and remove a random room from pending rooms
        private Room PopRandomPending()
        {
            int i = rand.Next(pendingRooms.Count);
            Room r = pendingRooms[i];
            pendingRooms.RemoveAt(i);
            return r;
        }

        private void RandomlyPlaceRooms()
        {
            placedRooms.Clear();
            pendingRooms.Clear();

            pendingRooms.AddRange(rooms);

            if (pendingRooms.Count == 0)
            {
                // No rooms to place, just stop the process
                return;
            }

            Room root = PopRandomPending();
            root.x = 0;
            root.y = 0;
            placedRooms.Add(root);

            iteration = 0;
            while (pendingRooms.Count > 0)
            {
                iteration++;
                GeneratorIteration();
            }
        }

        private bool IsRoomValid(Room room)
        {
            foreach (Room placed in placedRooms)
            {
                if (placed.HasOverlapWith(room))
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckAndAddPosition(int x, int y)
        {
            current.x = x;
            current.y = y;

            foreach (Room room in placedRooms)
            {
                if (room.HasOverlapWith(current))
                {
                    return;
                }
            }
            positions.Add(new Vector2Int(x, y));
        }

        private void ComputeValidPositions()
        {
            foreach (Room room in placedRooms)
            {
                for (int y = (room.Y1 - current.height) + 1; y < room.Y2; y++)
                {
                    int x;

                    x = room.X1 - current.width;
                    CheckAndAddPosition(x, y);
                    x = room.X2;
                    CheckAndAddPosition(x, y);
                }

                for (int x = (room.X1 - current.width) + 1; x < room.X2; x++)
                {
                    int y;

                    y = room.Y1 - current.height;
                    CheckAndAddPosition(x, y);
                    y = room.Y2;
                    CheckAndAddPosition(x, y);
                }
            }
        }

        private void GeneratorIteration()
        {
            positions.Clear();
            current = PopRandomPending();
            ComputeValidPositions();

            if (positions.Count > 0)
            {
                Vector2Int pos = positions[rand.Next(positions.Count)];
                current.Coords1 = pos;
                placedRooms.Add(current);
                current.iteration = iteration;
            }
            else
            {
                current.error = true;
                erroredRooms += 1;
                current.iteration = 0;
            }
        }

        private void CollectAdjacentRooms()
        {
            adjacentRooms.Clear();

            foreach (Room room in placedRooms)
            {
                if (room == current) continue;

                if ((room.X1 == current.X2 || room.X2 == current.X1) && room.Y1 < current.Y2 && current.Y1 < room.Y2)
                {
                    adjacentRooms.Add(room);
                    continue;
                }

                if ((room.Y1 == current.Y2 || room.Y2 == current.Y1) && room.X1 < current.X2 && current.X1 < room.X2)
                {
                    adjacentRooms.Add(room);
                }
            }
        }

        private bool DoesDoorBetweenRoomsExist(Room a, Room b)
        {
            foreach (Door door in doors)
            {
                if ((door.RoomA == a && door.RoomB == b) || (door.RoomB == a && door.RoomA == b))
                {
                    return true;
                }
            }
            return false;
        }

        private Room PopRandomDoorless()
        {
            int i = rand.Next(doorlessRooms.Count);
            Room r = doorlessRooms[i];
            doorlessRooms.RemoveAt(i);
            return r;
        }

        private void AddDoors()
        {
            if (placedRooms.Count == 1)
            {
                return;
            }
            doorlessRooms.Clear();
            doorlessRooms.AddRange(placedRooms);

            while (doorlessRooms.Count > 0)
            {
                DoorIteration(PopRandomDoorless());
            }
            for (int i = 0; i < extraRoomIterations; i++)
            {
                DoorIteration(placedRooms[rand.Next(placedRooms.Count)]);
            }
        }

        private void DoorIteration(Room r)
        {
            current = r;

            CollectAdjacentRooms();

            foreach (Room room in adjacentRooms)
            {
                if (!DoesDoorBetweenRoomsExist(room, current))
                {
                    if (room.X1 == current.X2)
                    {
                        AddDoorX(current, room);
                    }
                    if (room.X2 == current.X1)
                    {
                        AddDoorX(room, current);
                    }
                    if (room.Y1 == current.Y2)
                    {
                        AddDoorY(current, room);
                    }
                    if (room.Y2 == current.Y1)
                    {
                        AddDoorY(room, current);
                    }
                }
            }
        }

        private void AddDoorY(Room lower, Room upper)
        {
            int xmin = Math.Max(lower.X1, upper.X1);
            int xmax = Math.Min(lower.X2, upper.X2);

            int x = rand.Next(xmin, xmax);
            int y = lower.Y2 - 1;

            Door door = new Door();
            door.x = x;
            door.y = y;
            door.direction = EFacing2D.POSY;
            door.roomA = lower;
            door.roomB = upper;
            doors.Add(door);

            doorlessRooms.Remove(lower);
            doorlessRooms.Remove(upper);
        }

        private void AddDoorX(Room left, Room right)
        {
            int ymin = Math.Max(left.Y1, right.Y1);
            int ymax = Math.Min(left.Y2, right.Y2);

            int y = rand.Next(ymin, ymax);
            int x = left.X2 - 1;

            Door door = new Door();
            door.x = x;
            door.y = y;
            door.direction = EFacing2D.POSX;
            door.roomA = left;
            door.roomB = right;
            doors.Add(door);

            doorlessRooms.Remove(left);
            doorlessRooms.Remove(right);
        }
    }
}