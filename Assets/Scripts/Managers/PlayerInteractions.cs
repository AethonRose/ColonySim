using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class PlayerInteractions : MonoBehaviour
    {

        //World ref
        public World world;
        public Transform visualizer;

        //PathfinderMaster ref
        public Pathfinding.PathfinderMaster pathFinderMaster;

        Vector3 mousePosition;
        Block targetBlock;

        void Update() 
        {
            FindMousePosition();
            VisualizePosition();
            RequestPath();
        }

        //Requests a PathFind on targetBlock on MouseButtonDown
        void RequestPath()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (targetBlock != null)
                {
                    //Request a PathFind From: a units WorldPosition and Gets Block they are on, To: currentBlock
                    pathFinderMaster.RequestPathFind(world.GetBlockFromWorldPosition(world.unit.position - Vector3.up), targetBlock);
                }
            }
        }

        //Finding MousePosition using rayCast
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
                    targetBlock = block;
               }
            }
        }

        void VisualizePosition()
        {
            if(targetBlock != null)
            {
                visualizer.transform.position = targetBlock.worldPosition + Vector3.one;
            }
        }
    }
}
