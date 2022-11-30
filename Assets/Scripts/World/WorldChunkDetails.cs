using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class WorldChunkDetails
    {
        //Initialize size of World & noise vars
        public int maxX = 15;
        public int maxY = 10;
        public int maxZ = 15;
        
        public int chunk_X;
        public int chunk_Y;
        public int chunk_Z;
        
        public float baseNoise = 0.02f;
        public int baseNoiseHeight = 4;
        public int elevation = 15;
        public float frequency = 0.005f;
        public Vector3 origin;

        public NoiseBase[] noisePatterns;
    }
}
