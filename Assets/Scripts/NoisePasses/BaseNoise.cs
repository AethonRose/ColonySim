using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    [CreateAssetMenu(menuName = "Noise/Base Noise")]
    public class BaseNoise : NoiseBase
    {
        //Implemening interface
        public override int Calculate(WorldChunkDetails details, Vector3 noisePosition)
        {
            return GetNoise(noisePosition.x, 0, noisePosition.z, details.baseNoise, Mathf.RoundToInt(details.baseNoiseHeight));
        }
    }
}

