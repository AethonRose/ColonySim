using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpexNoise;

namespace WorldGen
{
    public class WorldGeneration
    {
        //Create var grid of type Block with 3-Dimensions
        Block[,,] grid;
        //Reference to WorldChunkDetails
        WorldChunkDetails chunkDetails;
        MeshData meshData;
        public volatile bool jobDone;

        public delegate void WorldGenerationCallback(Block[,,] grid, MeshData data);
        WorldGenerationCallback finishCallback;


        public WorldGeneration(WorldChunkDetails details, WorldGenerationCallback callback)
        {
            chunkDetails = details;
            finishCallback = callback;
        }
        public void StartCreatingWorld()
        {
            //Calls CreateWorld to create mesh
            meshData = CreateWorld();
            jobDone = true;
        }
        public void NotifyComplete()
        {
            finishCallback(grid, meshData);
        }
        //On call - loops through world grid and loads each block
        MeshData CreateWorld()
        {
            //World grid to identify blocks. Set to bounds of world
            grid = new Block[chunkDetails.maxX, chunkDetails.elevation, chunkDetails.maxZ];
            //Emptpy List of blocks to be filled, then loaded
            List<Block> blocks = new List<Block>();
            
            //Loop through maxX and maxZ and create Quads at targetposition
            for (int x = 0; x < chunkDetails.maxX; x++)
            {
                for (int z = 0; z < chunkDetails.maxZ; z++)
                {
                    //Setting default height value = 0
                    float height = 0;

                    //Creating new Block
                    Block currentBlock = new Block();
                    //Setting currentBlock.x to loop iteration
                    currentBlock.x = x;
                    //Setting currentBlock.z to loop iteration
                    currentBlock.z = z;
                    //Sets currentBlock to Solid
                    currentBlock.isSolid = true;

                    //Sets default targetPosition to 0,0,0
                    Vector3 targetPosition = Vector3.zero;
                    //Adjusts targetPosition to X & Z loop iteration
                    targetPosition.x = x * 1;
                    targetPosition.z = z * 1;

                    //Calls GetNoise function and adds noise to height
                    height += GetNoise(x, 0, z, chunkDetails.frequency, chunkDetails.elevation);
                    //Set targetPosition.y == noise adjusted height
                    targetPosition.y = height;
                    //Set currentBlock's worldPosition == targetPosition
                    currentBlock.worldPosition = targetPosition;
                    //Sets currentBlock.y == nosie adjusted height
                    currentBlock.y = Mathf.RoundToInt(height);

                    //Sets iterations grid value == currentBlock, since no y in for loop - using noise to get Y value
                    grid[x, currentBlock.y, z] = currentBlock;
                    //Adds current block into blocks List
                    blocks.Add(currentBlock);

                    //Loads current block at targetPosition in this world
                    //currentBlock.LoadBlock(data, this, targetPosition);
                }
            }
            //Creates data for mesh of type MeshData
            MeshData data = new MeshData();
            //Loops through List of blocks made in CreateWorld
            for(int i = 0; i < blocks.Count; i++)
            {
                //Calls LoadBlock on every List item in List<Block> blocks
                blocks[i].LoadBlock(data, this);
            }

            return data;
        }
        //Checks if block is null, if valid returns its grid position
        public Block GetBlock(int x, int y, int z)
        {
            //Out of Bounds check
            if(x < 0 || y < 0 || z < 0 || x >= chunkDetails.maxX || y >= chunkDetails.elevation || z >= chunkDetails.maxZ)
            {
                return null;
            }

            return grid[x, y, z];
        }
        //Called in CreateWorld - Function to generate Noise from SimplexNoise library 
        int GetNoise(int x, int y, int z, float scale, int maxHeight)
        {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1) * (maxHeight / 2.0f));
        }
    }
}