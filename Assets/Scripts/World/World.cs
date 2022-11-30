using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpexNoise;
using System.Threading;

namespace WorldGen
{
    public class World : MonoBehaviour
    {
        [Header("Base Material")]
        public Material material;

        [Header("Multithreading - Threads")]
        public int maxJobs = 4;
        List<WorldGeneration> toDoJobs = new List<WorldGeneration>();
        List<WorldGeneration> currentJobs = new List<WorldGeneration>();

        [Header("World Size")]
        public int worldSizeX = 4;
        public int worldSizeZ = 4;

        [Header("Chunk Size")]
        public int chunkSizeX = 16;
        public int chunkSizeY = 10;
        public int chunkSizeZ = 16;

        [Header("Base Noise Settings")]
        public float baseNoise = 0.02f;
        public int baseNoiseHeight = 4;

        [Header("Frequency Settings")]
        public float frequency = 0.005f;
        public int elevation = 15;
        
        public NoiseBase[] noisePatterns;
        public GameObject parentObj;
        public Chunk[,,] chunks;

        public bool isColonistPlaced;
        public Transform colonist;

        void Start()
        {
            parentObj = new GameObject("World Parent");

            CreateWorld();
        }
        void Update()
        {
            int i = 0;
            //If currentJob isDone, call NotifyComplete() and Remove job instance - else i++
            while(i < currentJobs.Count)
            {
                if(currentJobs[i].jobDone)
                {
                    currentJobs[i].NotifyComplete();
                    currentJobs.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if(toDoJobs.Count > 0 && currentJobs.Count < maxJobs)
            {
                //Setting currentJob as type WorldGeneration == 1st toDoJob
                WorldGeneration job = toDoJobs[0];
                //Remove 1st toDoJob as it is now saved in job
                toDoJobs.RemoveAt(0);
                //Adding job to currentJobs List
                currentJobs.Add(job);

                //Calls StartCreatingWorld on job
                Thread jobThread = new Thread(job.StartCreatingWorld);
                jobThread.Start();
            }
        }
        
        void CreateWorld()
        {
            chunks = new Chunk[worldSizeX + 1, 1, worldSizeZ + 1];

            //Loops through set size of world
            for(int x = 0; x < worldSizeX; x++)
            {
                for(int z = 0; z < worldSizeZ; z++)
                {
                    //Requests WorldGeneration, setting self variables for chunks 
                    RequestWorldGeneration(x, z, chunkSizeX, chunkSizeZ);
                }
            }
        }
        //Loads all MeshData data into MeshData arrays - Called after CreateWorld in Start
        public void LoadMeshData(Block[,,] createdGrid, MeshData data)
        {
            //Calling Chunk in Chunk.cs to create a new chunk and add to createdGrid
            Chunk newChunk = new Chunk(data.chunk_X, data.chunk_Y, data.chunk_Z, chunkSizeX, chunkSizeY, chunkSizeZ, createdGrid);
            //Takes x,y,z of newChunk in Chunk[,,] chunks and sets that position == newChunk
            chunks[newChunk.x, newChunk.y, newChunk.z] = newChunk;

            GameObject go = new GameObject(data.origin.ToString());
            //Transforms created gameobject to be at MeshDatas data.origin
            go.transform.position = data.origin;
            //Sets newChunks chunkPosition == MeshDatas data.origin
            newChunk.chunkPosition = data.origin;

            //Adds MeshRenderer w/ Material & MeshFilter to GameObject go
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            MeshFilter filter = go.AddComponent<MeshFilter>();
            renderer.material = material;
            
            //Creates new mesh and sets its Arrays
            Mesh mesh = new Mesh()
            {
                vertices = data.vertices.ToArray(),
                uv = data.uv.ToArray(),
                triangles = data.triangles.ToArray()
            };

            mesh.RecalculateNormals();

            filter.mesh = mesh;
            go.AddComponent<MeshCollider>();

            //Childs all created world meshes unter parentObj
            go.transform.parent = parentObj.transform;

            if(!isColonistPlaced)
            {
                isColonistPlaced = true;
                //Returns a solid block called spawnPoint & spawns colonist 1 above spawnPoint
                Block spawnPoint = newChunk.GetFirstSolidBlock();
                colonist.transform.position = spawnPoint.worldPosition + Vector3.up;
            }
        }  
        
        //Sets chunkDetails values in WorldChunkDetails and passed to WorldGeneration function in WorldGeneration.cs using LoadMeshData as a callback
        public void RequestWorldGeneration(int x, int z, int chunkSizeX, int chunkSizeZ)
        {
            WorldChunkDetails details = new WorldChunkDetails
            {
                maxX = chunkSizeX,
                maxY = chunkSizeY,
                maxZ = chunkSizeZ,
                chunk_X = x,
                chunk_Z = z,
                baseNoise = baseNoise,
                baseNoiseHeight = baseNoiseHeight,
                elevation = elevation,
                frequency = frequency,
                origin = new Vector3(x * chunkSizeX, 0, z * chunkSizeZ),
                noisePatterns = noisePatterns
            };
            
            WorldGeneration worldGen = new WorldGeneration(details, LoadMeshData);
            toDoJobs.Add(worldGen);
        }

        //Called in FindMousePosition - Managers.cs
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

        //Called in GetBlockFromWorldPosition
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

