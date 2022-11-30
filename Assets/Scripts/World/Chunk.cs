using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class Chunk
    {
        public int x;
        public int y;
        public int z;

        public int maxX;
        public int maxY;
        public int maxZ;

        public Block[,,] grid;

        public Vector3 chunkPosition;

        
        //Sets deatils for specific chunk - Called in LoadMeshData in World.cs
        public Chunk(int x, int y, int z, int maxX, int maxY, int maxZ, Block[,,] grid)
        {
            chunkPosition = new Vector3(x, y, z);
            this.x = x;
            this.y = y;
            this.z = z;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;
            this.grid = grid;
        }

        //Called GetBlockFromWorldPosition in World.cs
        public Block GetBlock(Vector3 worldPos)
        {
            //Sets X,Y,Z to get positions of a block
            int x = Mathf.FloorToInt(worldPos.x - chunkPosition.x);
            int y = Mathf.FloorToInt(worldPos.y - chunkPosition.y);
            int z = Mathf.FloorToInt(worldPos.z - chunkPosition.z);

            //Calls GetBlock inside of Chunk.cs
            return GetBlock(x, y, z);
        }

        //Checks if block is null, if valid returns its grid position
        Block GetBlock(int x, int y, int z)
        {
            //Out of Bounds check
            if(x < 0 || y < 0 || z < 0 || x > maxX || y > maxY || z > maxZ)
            {
                return null;
            }

            return grid[x, y, z];
        }
    }
}

