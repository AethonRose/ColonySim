using System.Collections;
using System.Collections.Generic;
using SimpexNoise;
using UnityEngine;

namespace WorldGen
{
    public abstract class NoiseBase : ScriptableObject
    {
        public abstract int Calculate(WorldChunkDetails details, Vector3 noisePosition);
        public int GetNoise(float x, float y, float z, float scale, int maxHeight)
        {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1) * (maxHeight / 2.0f));
        }
    }
}

