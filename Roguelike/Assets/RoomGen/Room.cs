using UnityEngine;
using System.Collections.Generic;

namespace RoomGen
{
    public class Room
    {
        public int x;
        public int y;
        public int width;
        public int height;

        internal bool error = false;

        public readonly List<Room> adjacentRooms = new List<Room>();

        /// <summary>A boolean that will be <c>true</c> when the room could not be placed, otherwise it's <c>false</c></summary>
        public bool Error {
            get {
                return error;
            }
        }

        public Vector2Int Coords1
        {
            get
            {
                return new Vector2Int(X1, Y1);
            }
            set
            {
                X1 = value.x;
                Y1 = value.y;
            }
        }

        public Vector2Int Coords2
        {
            get
            {
                return new Vector2Int(X2, Y2);
            }
            set
            {
                X2 = value.x;
                Y2 = value.y;
            }
        }

        public int X1
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int X2
        {
            get
            {
                return x + width;
            }
            set
            {
                x = value - width;
            }
        }

        public int Y1
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Y2
        {
            get
            {
                return y + height;
            }
            set
            {
                y = value - height;
            }
        }

        public bool HasOverlapWith(Room other)
        {
            return other.X1 < X2 && other.X2 > X1 && other.Y1 < Y2 && other.Y2 > Y1;
        }
    }
}