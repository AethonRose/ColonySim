using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Simulation
{
    public abstract class Simulation : ScriptableObject
    {
        public abstract bool isAlive(Node n, Node[,] gridClone, int maxX, int maxY);

        //Given an x (width) and y (height) position; returns position of pixel in grid
        protected Node GetNodeInClone(int x, int y, int maxX, int maxY, Node[,] gridClone)
        {
            if (x < 0 || y < 0 || x > maxX - 1 || y > maxY - 1)
            {
                return null;
            }

            return gridClone[x, y];
        }
    }
}

    
