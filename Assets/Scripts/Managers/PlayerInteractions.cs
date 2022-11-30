using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class PlayerInteractions : MonoBehaviour
    {
        public World world;

        void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                FindMousePosition();
            }
        }

        void FindMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
               Block block = world.GetBlockFromWorldPosition(hit.point);
               if (block != null)
               {
                    CreateBox(block.worldPosition + Vector3.up / 2);
               }
            }
        }

        void CreateBox(Vector3 pos)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(go.GetComponent<Collider>());

            go.transform.position = pos;
        }
    }
}
