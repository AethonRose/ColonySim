using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Pathfinding
{
    public class Pathfinder
    {
        public volatile bool jobDone;

        Block startBlock;
        Block endBlock;

        Chunk[,,] chunks;
        List<Block> path;

        int worldSizeX = 4;
        int worldSizeZ = 4;

        int chunkSizeX = 16;
        int chunkSizeY = 10;
        int chunkSizeZ = 16;

        Unit unit;

        PathfinderMaster.PathCompleteCallback callback;

        //Used to set values of current pathJob
        public Pathfinder(World w, Block start, Block target, PathfinderMaster.PathCompleteCallback callback, Unit unit)
        {
            startBlock = start;
            endBlock = target;
            this.chunks = w.chunks;

            worldSizeX = w.worldSizeX;
            worldSizeZ = w.worldSizeZ;
            chunkSizeX = w.chunkSizeX;
            chunkSizeY = w.chunkSizeY;
            chunkSizeZ = w.chunkSizeZ;

            this.callback = callback;
            this.unit = unit;
        }

        //Invokes PathCompleteCallback
        public void NotifyComplete()
        {
            if (callback != null)
            {
                callback.Invoke(path, unit);
            }
        }

        //Called on the creation of a new JobThread in PathfinderMaster
        public void FindPath()
        {
            //Calls FindPathActual and sets its finalPath == path
            path = FindPathActual(startBlock, endBlock);
            //Sets jobThread to done
            jobDone = true;
        }

        //Searches for finalPath; Given a startBlock and a targetBlock
        List<Block> FindPathActual(Block start, Block target)
        {
            //Result will = final path at end of method
            List<Block> finalPath = new List<Block>();

            //Creating open & closed set for A* algorith
            List<Block> openSet = new List<Block>();
            //A closed set is a list of Blocks that have already been searched through
            HashSet<Block> closedSet = new HashSet<Block>();

            //Adds given starting block as 1st item in openSet
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                //Creates currentBlock to be == 1st index of openSet
                Block currentBlock = openSet[0];

                //Loops through openSet.Count
                for (int i = 0; i < openSet.Count; i++)
                {
                    //If fCost of openSet[i] < currentBlock or If fCosts are same, but hCost of openSet[i] < curentBlocks
                    //Comparing costs of openSet[i] to currentBlock
                    if (openSet[i].fCost < currentBlock.fCost || 
                        (openSet[i].fCost == currentBlock.fCost && openSet[i].hCost < currentBlock.hCost))
                    {
                        //If currentBlock != openSet[i], set currentBlock = openSet in index of i
                        if(!currentBlock.Equals(openSet[i]))
                        {
                            //Expanding scope of openSet
                            currentBlock = openSet[i];
                        }
                    }
                }

                //Removes currentBlock from openSet and puts it into closedSet as currentBlocks f&hCost values have already been checked
                openSet.Remove(currentBlock);
                closedSet.Add(currentBlock);

                //Once A* algo finds way to target call RetracePath
                if (currentBlock.Equals(target))
                {
                    //Calls RetracePath from start to the currentBlock once currentBlock == target
                    finalPath = RetracePath(start, currentBlock);
                    break;
                }

                //Loops through every Neighboring Block given the currentBlock
                foreach (Block b in GetNeighbors(currentBlock))
                {
                    //Runs if Block b hasn't already been checked
                    if (!closedSet.Contains(b))
                    {
                        //Calculating moveCost to b block by adding currentBlocks gCost and calculating the hCost from currentBlock to b
                        float moveCost = currentBlock.gCost + GetDistance(currentBlock, b);

                        //Don't understand very well
                        if (moveCost < b.gCost || !openSet.Contains(b))
                        {
                            b.gCost = moveCost;
                            //set Block b's hCost == the calculated distance from Block b to target
                            b.hCost = GetDistance(b, target);
                            //parents neighboring block to current block
                            b.parentBlock = currentBlock;

                            if (!openSet.Contains(b))
                            {
                                openSet.Add(b);
                            }
                        }
                    }
                }
            }
            return finalPath;
        }

        //Searches for Neighbors; Given Block b
        List<Block> GetNeighbors(Block b)
        {
            List<Block> neighbors = new List<Block>();

            //Loops from -1 - 1 on x,y,z axis to get neighbor cordinates
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        //Sets targetPos x,y,z to offset vaules of forloop
                        Vector3 targetPos = Vector3.zero;
                        targetPos.x = x;
                        targetPos.y = y;
                        targetPos.z = z;

                        //Adds Block b's worldPosition to all targetPos values to get actual location
                        targetPos += b.worldPosition;

                        //Sets searchPos by calling GetBlockFromWorldPosition by sending set targetPos to know what block to find neighbors from
                        Block searchPos = GetBlockFromWorldPosition(targetPos);
                        if (searchPos != null)
                        {
                            //Removing unwanted diagonal paths between searchPos and Block b
                            //Only allowing paths from N,S,E,W of Block b, no diagonals; so no NW, NE, SE, SW
                            Block pathTarget = GetNeighbor(searchPos, b);
                            if (pathTarget != null)
                            {
                                neighbors.Add(pathTarget);
                            }
                        } 
                    }
                }
            }

            return neighbors;
        }

        //Elimates all diagonal paths by returning them as null so only 4 directions intead of 8
        Block GetNeighbor(Block search, Block current)
        {
            
           // int o_x = search.x - current.x;
            //int o_z = search.z - current.z;

            //If diagonal direction from origin x & z return null
            //if (o_x != 0 && o_z != 0)
            //{
            //    return null;
            //}
            
            return search;
        }

        //Going through parented/childed List of currentBlock and adding to pathRetrace List
        List<Block> RetracePath(Block start, Block current)
        {
            List<Block> pathRetrace = new List<Block>();
            //To retrace need to go backwards
            Block currentBlock = endBlock;

            //Goes up through parent list till currentBlock == starting Block
            while(currentBlock != startBlock)
            {
                //Add currentBlock into pathRetrace List
                pathRetrace.Add(currentBlock);
                //Going up parent list
                currentBlock = currentBlock.parentBlock;
            }

            return pathRetrace;
        }
        
        //Used to Calculate the hCost given 2 blocks distances from eachother
        int GetDistance(Block b1, Block b2)
        {
            //Calculates x,y, z dist from b1 to b2, using absolute value so no negative
            int distX = Mathf.Abs(b1.x - b2.x);
            int distY = Mathf.Abs(b1.y - b2.y);
            int distZ = Mathf.Abs(b1.z - b2.z);

            if (distX > distZ)
            {
                return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
            }

            return 14 * distX + 10 * (distZ - distX) + 10 * distY;
        }

        //Returns worldPos of a Block; Given its worldPos
        public Block GetBlock(Vector3 worldPos)
        {
            return GetBlockFromWorldPosition(worldPos);
        }

        //Gets a blocks cords given a worldPos
        public Block GetBlockFromWorldPosition(Vector3 worldPos)
        {
            //Getting X,Y,Z location of chunk given a worldPosition
            int c_x = Mathf.FloorToInt(worldPos.x / chunkSizeX); 
            int c_y = Mathf.FloorToInt(worldPos.y / chunkSizeY);
            int c_z = Mathf.FloorToInt(worldPos.z / chunkSizeZ);
            //Set chunk == X,Y,Z positions just obtained
            Chunk chunk = GetChunk(c_x, c_y, c_z);

            if(chunk == null)
                return null;

            //Returns a blocks position at a given chunks position
            return chunk.GetBlock(worldPos);
        }

        //Returns chunk at given x,y,z from Chunk[,,] chunks
        public Chunk GetChunk(int x, int y, int z)
        {
            if(x < 0 || y < 0 || z < 0 || x > worldSizeX - 1 || z > worldSizeZ - 1 || y > 0)
            {   
                return null;
            }
            //Returns chunk at given x,y,z from Chunk[,,] chunks
            return chunks[x, y, z];
        }
    }
}

