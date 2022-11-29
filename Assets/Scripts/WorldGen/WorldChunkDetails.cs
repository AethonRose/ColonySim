using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class WorldChunkDetails
    {
        //Initialize size of World & noise vars
        public int maxX = 15;
        public int maxZ = 15;
        public float baseNoise = 0.02f;
        public float baseNoiseHeight = 4.0f;
        public int elevation = 15;
        public float frequency = 0.005f;
        public Vector3 origin;
    }
}
