using System.Collections;
using System.Collections.Generic;
using SimpexNoise;
using UnityEngine;

namespace WorldGen
{
    [CreateAssetMenu(menuName = "Noise/Frequency")]
    public class Frequency : NoiseBase
    {
        //Implemening interface
        public override int Calculate(WorldChunkDetails details, Vector3 noisePosition)
        {
            return GetNoise(noisePosition.x, 0, noisePosition.z, details.frequency, details.elevation);
        }
    }
}
