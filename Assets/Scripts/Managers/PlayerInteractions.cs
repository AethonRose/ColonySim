using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class PlayerInteractions : MonoBehaviour
    {
        public World world;
        public Transform visualizer;

        Vector3 mousePosition;
        Block currentBlock;
        

        void Update() 
        {
            FindMousePosition();
            VisualizePosition();
        }

        void FindMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                mousePosition = hit.point;

               Block block = world.GetBlockFromWorldPosition(hit.point);
               if (block != null)
               {
                    currentBlock = block;
               }
            }
        }

        void VisualizePosition()
        {
            if(currentBlock != null)
            {
                visualizer.transform.position = currentBlock.worldPosition + Vector3.one;
            }
        }
    }
}
