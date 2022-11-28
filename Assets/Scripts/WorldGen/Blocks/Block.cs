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
        public Vector3 worldPosition;
        public bool isSolid;

        //Creates each block at its own respective worldPosition
        public virtual void LoadBlock(MeshData data, World world)
        {
            //Creates all FaceUp, North, South, East, West Blocks
            MeshUtilities.FaceUp(data, worldPosition);
            //Sets north == block north of currentBlock
            Block north = world.GetBlock(x, y, z + 1);
            if(north == null || !north.isSolid)
            {
                //Creates block Facing North
                MeshUtilities.FaceNorth(data, worldPosition);
            }
            //Sets south == block south of currentBlock
            Block south = world.GetBlock(x, y, z - 1);
            if(south == null || !south.isSolid)
            {
                //Creates block Facing South
                MeshUtilities.FaceSouth(data, worldPosition);
            }
            //Sets east == block east of currentBlock
            Block east = world.GetBlock(x + 1, y, z);
            if(east == null || !east.isSolid)
            {
                //Creates block Facing East
                MeshUtilities.FaceEast(data, worldPosition);
            }
            //Sets west == block west of currentBlock
            Block west = world.GetBlock(x - 1, y, z);
            if(west == null || !west.isSolid)
            {
                //Creates block Facing West
                MeshUtilities.FaceWest(data, worldPosition);
            }
        }
    }
}
