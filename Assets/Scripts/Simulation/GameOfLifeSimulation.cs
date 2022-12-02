using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Simulation
{
    [CreateAssetMenu(menuName = "Simulations/Game Of Life Simulation")]
    public class GameOfLifeSimulation : Simulation
    {
        //Cells to stay alive 
        public int death = 3;
        public int birth = 4;

        public override NodeState GetNodeState(Node pixel, Node[,] gridClone, int maxX, int maxY)
        {
            //result is returning value - basically isAlive - Default NodeState water
            NodeState result = NodeState.water;
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
                    if (neighborNode != null)
                    {
                        //If gotten neighborNode isGround / isAlive; count++
                        if(neighborNode.isGround)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        return NodeState.water;
                    }
                }
            }
            //Conways Game of Life algorithm - https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
            //If cell isAlive / isGround
            if (pixel.isGround)
            {
                if (count < death)
                {
                    result = NodeState.water;
                }
                else
                {
                    result = NodeState.grass;
                }
            }
            //If cell isDead / !isGround
            else
            {
                if (count > birth)
                    result = NodeState.ground;
                else
                    result = NodeState.water;
            }

            return result;
        }
    }
}

