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

        [Header("Multithreading")]
        public int maxJobs = 4;
        List<WorldGeneration> toDoJobs = new List<WorldGeneration>();
        List<WorldGeneration> currentJobs = new List<WorldGeneration>();

        [Header("World Size")]
        public int worldX = 4;
        public int worldZ = 4;

        [Header("Chunk Size")]
        public int chunkX = 16;
        public int chunkY = 10;
        public int chunkZ = 16;
        [Header("Base Noise Settings")]
        public float baseNoise = 0.02f;
        public int baseNoiseHeight = 4;
        [Header("Frequency Settings")]
        public float frequency = 0.005f;
        public int elevation = 15;
        

        Block[,,] grid;

        public NoiseBase[] noisePatterns;

        void Start()
        {
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
            //Loops through set size of world
            for(int x = 0; x < worldX; x++)
            {
                for(int z = 0; z < worldZ; z++)
                {
                    //Default chunkPosition == 0
                    Vector3 chunkPosition = Vector3.zero;
                    //Adjusts chunkPosition through loop iterations
                    chunkPosition.x = x * chunkX;
                    chunkPosition.z = z * chunkZ;

                    //Requests WorldGeneration at chunkPosition
                    RequestWorldGeneration(chunkPosition);
                }
            }
        }
        //Loads all MeshData data into MeshData arrays - Called after CreateWorld in Start
        public void LoadMeshData(Block[,,] createdGrid, MeshData data)
        {
            grid = createdGrid;

            GameObject go = new GameObject(data.origin.ToString());
            go.transform.position = data.origin;

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
        }  
        
        //Checks if block is null, if valid returns its grid position
        public Block GetBlock(int x, int y, int z)
        {
            //Out of Bounds check
            if(x < 0 || y < 0 || z < 0 || x >= chunkX || y >= elevation || z >= chunkZ)
            {
                return null;
            }

            return grid[x, y, z];
        }
        
        //Sets chunkDetails values in WorldChunkDetails and passed to WorldGeneration function in WorldGeneration.cs using LoadMeshData as a callback
        public void RequestWorldGeneration(Vector3 chunkOrigin)
        {
            WorldChunkDetails details = new WorldChunkDetails
            {
                maxX = chunkX,
                maxY = chunkY,
                maxZ = chunkZ,
                baseNoise = baseNoise,
                baseNoiseHeight = baseNoiseHeight,
                elevation = elevation,
                frequency = frequency,
                origin = chunkOrigin,
                noisePatterns = noisePatterns
            };
            
            WorldGeneration worldGen = new WorldGeneration(details, LoadMeshData);
            toDoJobs.Add(worldGen);


        }
    }
}

