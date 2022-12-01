using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class Block
    {
        //X, Y, Z values of currentBlock
        public int x;
        public int y;
        public int z;

        public Vector3 localPosition;
        public Vector3 worldPosition;

        public bool isSolid;

        //Pathfinding
        public Block parentBlock;
        
        public float hCost;
        public float gCost;
        public float fCost 
        {
            get
            {
                return gCost + hCost;
            }
        }

        //Creates each block at its own respective worldPosition
        public virtual void LoadBlock(MeshData data, WorldGeneration world)
        {
            //Creates all FaceUp, North, South, East, West Blocks
            MeshUtilities.FaceUp(data, localPosition);
            //Sets north == block north of currentBlock
            Block north = world.GetBlock(x, y, z + 1);
            if(north == null || !north.isSolid)
            {
                //Creates block Facing North
                MeshUtilities.FaceNorth(data, localPosition);
            }
            //Sets south == block south of currentBlock
            Block south = world.GetBlock(x, y, z - 1);
            if(south == null || !south.isSolid)
            {
                //Creates block Facing South
                MeshUtilities.FaceSouth(data, localPosition);
            }
            //Sets east == block east of currentBlock
            Block east = world.GetBlock(x + 1, y, z);
            if(east == null || !east.isSolid)
            {
                //Creates block Facing East
                MeshUtilities.FaceEast(data, localPosition);
            }
            //Sets west == block west of currentBlock
            Block west = world.GetBlock(x - 1, y, z);
            if(west == null || !west.isSolid)
            {
                //Creates block Facing West
                MeshUtilities.FaceWest(data, localPosition);
            }
        }
    }
}
