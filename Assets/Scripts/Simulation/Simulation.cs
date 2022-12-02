using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Simulation
{
    public abstract class Simulation : ScriptableObject
    {
        public abstract NodeState GetNodeState(Node pixel, Node[,] gridClone, int maxX, int maxY);

        //Given an x (width) and y (height) position; returns position of pixel in gridClone
        protected Node GetNodeInClone(int x, int y, int maxX, int maxY, Node[,] gridClone)
        {
            int _x = x;
            int _y = y;

            //Smooths World Generation, by relating edges to eachother (making circular - sort of)
            if (x < 0)
            {
                _x = maxX - 1;
            }

            if (x > maxX - 1)
            {
                _x = 0;
            }

            if(y < 0)
            {
                _y = maxY - 1;
            }

            if (y > maxY - 1)
            {
                _y = 0;
            }

            return gridClone[_x, _y];
        }
    }
}

    
