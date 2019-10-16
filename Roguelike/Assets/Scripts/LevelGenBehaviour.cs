using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RoomGen;

namespace Roguelike
{
    public class LevelGenBehaviour : MonoBehaviour
    {
        public LevelGenerator generator = new LevelGenerator();
        public GameObject RoomObject;
        public GameObject DebugObject;
        public GameObject DoorObject;

        private readonly GameObject[] roomObjects = new GameObject[20];
        private GameObject[] doorObjects;

        private int gen = 0;

        // Start is called before the first frame update
        void Start()
        {
        }

        bool Debug(Vector2Int pt)
        {
            // GameObject obj = Instantiate(DebugObject, new Vector3(), new Quaternion());

            // SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            // renderer.color = new Color(0, 0, 1, 1);

            // obj.transform.localScale = new Vector3(1 / 4F, 1 / 4F, 1);
            // obj.transform.position = new Vector3(pt.x + 1 / 2F, pt.y + 1 / 2F, -1);
            return true;
        }

        // Update is called once per frame
        void Update()
        {
            if (gen <= 0)
            {
                generator = new LevelGenerator();
                foreach (GameObject obj in roomObjects)
                {
                    if (obj == null) continue;
                    Destroy(obj);
                }

                if (doorObjects != null)
                {
                    foreach (GameObject obj in doorObjects)
                    {
                        if (obj == null) continue;
                        Destroy(obj);
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    roomObjects[i] = Instantiate(RoomObject, new Vector3(), new Quaternion());
                    RoomBehaviour behaviour = roomObjects[i].GetComponent<RoomBehaviour>();
                    behaviour.InitializeRoomGen();
                    generator.AddRoomDef(behaviour.room);
                }

                generator.Debug = pt => Debug(pt);

                bool b = generator.Generate();
                gen = 60;
                print("Generated: " + !b);
                foreach (GameObject obj in roomObjects)
                {
                    if (obj == null) continue;
                    RoomBehaviour behaviour = obj.GetComponent<RoomBehaviour>();
                    Room room = behaviour.room.Room;

                    SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                    renderer.color = new Color(Random.value * 0.5F + 0.5F, Random.value * 0.5F + 0.5F, Random.value * 0.5F + 0.5F, 1);

                    print(room.Coords1);
                    print(room.Coords2);

                    obj.transform.localScale = new Vector3(room.width / 4F, room.height / 4F, 1);
                    obj.transform.position = new Vector3(room.x + room.width / 2F, room.y + room.height / 2F, 0);
                    print("Placed");
                }

                List<Door> doors = generator.Doors;
                doorObjects = new GameObject[doors.Count];
                uint it = 0;
                foreach (Door door in generator.Doors)
                {
                    GameObject obj = doorObjects[it] = Instantiate(DoorObject, new Vector3(), new Quaternion());

                    obj.transform.localScale = new Vector3(1 / 4F, 1 / 4F, 1);
                    obj.transform.position = new Vector3(door.x + 1 / 2F, door.y + 1 / 2F, 0);
                    float rot = 0;
                    switch (door.Direction)
                    {
                        case EFacing2D.POSX:
                            rot = -90F;
                            break;
                        case EFacing2D.NEGX:
                            rot = 90F;
                            break;
                        case EFacing2D.NEGY:
                            rot = 180F;
                            break;
                    }
                    obj.transform.rotation = Quaternion.Euler(0, 0, rot);
                    it++;
                }
            }
            gen--;
        }
    }
}
