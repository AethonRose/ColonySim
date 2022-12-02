using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Simulation
{
    [CreateAssetMenu(menuName = "Simulations/Beach")]
    public class FindBeachCells : Simulation
    {
        public override NodeState GetNodeState(Node pixel, Node[,] gridClone, int maxX, int maxY)
        {
            //result is returning value - basically isAlive - Default NodeState water
            NodeState result = NodeState.ground;
            
            //If pixel is already water return result
            if (!pixel.isGround)
                return result;

            //count = number of alive pixels around a given pixel
            int count = 0;

            //Looping through -1 and 1 to get neighboring pixels
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Adding loop iteration to node x & y to get positions of neighboring nodes/pixels
                    int targetX = x + pixel.x;
                    int targetY = y + pixel.y;


                    if(targetX == pixel.x && targetY == pixel.y)
                        continue;

                    //Using GetNode to return grid value of neighboring node
                    Node neighborNode = GetNodeInClone(targetX, targetY, maxX, maxY, gridClone);

                    //If neighborNode !isground; count++
                    if(!neighborNode.isGround)
                    {
                        count++;
                        break;
                    }
                }
            }

            //while count > 0 set result = sand NodeState
            if (count > 0)
            {
                result = NodeState.sand;
            }

            return result;
        }
    }
}
