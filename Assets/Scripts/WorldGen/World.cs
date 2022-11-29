using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpexNoise;
using System.Threading;

namespace WorldGen
{
    public class World : MonoBehaviour
    {
        //Create var filter of type MeshFilter
        MeshFilter filter;

        //Initialize size of World & noise vars
        public int maxX = 15;
        public int maxZ = 15;
        public float baseNoise = 0.02f;
        public float baseNoiseHeight = 4.0f;
        public int elevation = 15;
        public float frequency = 0.005f;

        Block[,,] grid;

        //Multhreading
        public int maxJobs = 4;
        List<WorldGeneration> toDoJobs = new List<WorldGeneration>();
        List<WorldGeneration> currentJobs = new List<WorldGeneration>();

        void Start()
        {
            RequestWorldGeneration();
        }

        void Update()
        {
            int i = 0;
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
                WorldGeneration job = toDoJobs[0];
                toDoJobs.RemoveAt(0);
                currentJobs.Add(job);

                Thread jobThread = new Thread(job.StartCreatingWorld);
                jobThread.Start();
            }
        }
        //Loads all MeshData data into MeshData arrays - Called after CreateWorld in Start
        public void LoadMeshData(Block[,,] createdGrid, MeshData data)
        {
            grid = createdGrid;

            //Sets filter to MeshFilter reference
            filter = GetComponent<MeshFilter>(); 

            //Creates new mesh and sets its Arrays
            Mesh mesh = new Mesh()
            {
                vertices = data.vertices.ToArray(),
                uv = data.uv.ToArray(),
                triangles = data.triangles.ToArray()
            };

            mesh.RecalculateNormals();
            //Sets Mesh Filter == mesh
            filter.mesh = mesh;
        }  
        //Checks if block is null, if valid returns its grid position
        public Block GetBlock(int x, int y, int z)
        {
            //Out of Bounds check
            if(x < 0 || y < 0 || z < 0 || x >= maxX || y >= elevation || z >= maxZ)
            {
                return null;
            }

            return grid[x, y, z];
        }
        public void RequestWorldGeneration()
        {
            WorldChunkDetails details = new WorldChunkDetails
            {
                maxX = maxX,
                maxZ = maxZ,
                baseNoise = baseNoise,
                baseNoiseHeight = baseNoiseHeight,
                elevation = elevation,
                frequency = frequency
            };
            
            WorldGeneration worldGen = new WorldGeneration(details, LoadMeshData);
            toDoJobs.Add(worldGen);


        }
    }
}

