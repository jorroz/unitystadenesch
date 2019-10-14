using System;

namespace RoomGen
{
    public class RoomDefinition
    {
        public int minWidth, maxWidth, minHeight, maxHeight;
        private Room room;

        public Room Room
        {
            get
            {
                return room;
            }
        }

        internal void SetRoom(Room room)
        {
            this.room = room;
        }

        public RoomDefinition(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            this.minWidth = minWidth;
            this.maxWidth = maxWidth;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
        }

        public int GetRandomWidth(Random rand)
        {
            return rand.Next(minWidth, maxWidth);
        }

        public int GetRandomHeight(Random rand)
        {
            return rand.Next(minHeight, maxHeight);
        }
    }
}