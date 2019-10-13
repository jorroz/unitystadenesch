using UnityEngine;

namespace RoomGen
{
    public class DoorPos
    {
        public readonly int x;
        public readonly int y;
        public readonly EFacing2D facing;

        public DoorPos(int x, int y, EFacing2D facing)
        {
            this.x = x;
            this.y = y;
            this.facing = facing;
        }

        public Vector2Int Pos {
            get {
                return new Vector2Int( x, y );
            }
        }
    }
}