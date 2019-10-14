using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomGen;

namespace Roguelike
{
    public class RoomBehaviour : MonoBehaviour
    {
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        internal RoomDefinition room;

        // Start is called before the first frame update
        public void InitializeRoomGen()
        {
            room = new RoomDefinition(MinWidth, MaxWidth, MinHeight, MaxHeight);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
