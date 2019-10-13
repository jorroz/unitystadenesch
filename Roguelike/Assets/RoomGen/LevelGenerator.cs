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
        private readonly List<RoomDefinition> roomDefs = new List<RoomDefinition>();

        private readonly List<Vector2Int> positions = new List<Vector2Int>();
        private Room current;

        private uint erroredRooms;

        public LevelGenerator(int seed)
        {
            rand = new System.Random(seed);
        }

        public LevelGenerator()
        {
            rand = new System.Random();
        }

        public bool Generate() {
            erroredRooms = 0;
            CreateRoomsFromDefinition();
            RandomlyPlaceRooms();
            return erroredRooms > 0;
        }

        public void AddRoomDef( RoomDefinition def ) {
            roomDefs.Add( def );
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

            while (pendingRooms.Count > 0)
            {
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

                    y = room.X1 - current.width;
                    CheckAndAddPosition(x, y);
                    y = room.X2;
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
                Vector2Int pos = positions[ rand.Next( positions.Count ) ];
                current.Coords1 = pos;
            }
            else
            {
                current.error = true;
                erroredRooms += 1;
            }
        }
    }
}