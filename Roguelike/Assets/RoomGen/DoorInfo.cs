using UnityEngine;

namespace RoomGen
{
    public class Door
    {
        internal int x;
        internal int y;
        internal EFacing2D direction;

        internal Room roomA;
        internal Room roomB;

        public int X
        {
            get
            {
                return x;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
        }

        public Vector2Int Coords
        {
            get
            {
                return new Vector2Int(x, y);
            }
        }

        public EFacing2D Direction
        {
            get
            {
                return direction;
            }
        }

        public Room RoomA
        {
            get
            {
                return roomA;
            }
        }

        public Room RoomB
        {
            get
            {
                return roomB;
            }
        }
    }
}