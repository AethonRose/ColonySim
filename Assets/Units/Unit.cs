using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class Unit : MonoBehaviour
    {
        List<Block> path = new List<Block>();
        int pathIndex;
        bool hasPath;
        bool initLerp;
        float t;
        float actualspeed;
        public float speed = 2f;
        Vector3 startPos;
        Vector3 endPos;

        void Update() 
        {
            //When pathIndex > path.Count, path is no longer valid so hasPath = false
            if(pathIndex > path.Count - 1)
            {
                hasPath = false;
                t = 0;
                return;
            }

            if(!initLerp)
            {
                t = t - 1;
                t = Mathf.Clamp01(t);
                
                //Setting startPos = units transform.position
                startPos = transform.position;
                //Setting endPos to the pathIndex of paths worldPosition
                endPos = path[pathIndex].worldPosition + Vector3.up;

                //Scaling speed based on distance?
                float dist = Vector3.Distance(startPos, endPos);
                actualspeed = speed / dist;

                //Setting initLerp to true to kick out of if statement
                initLerp = true;
            }

            t += Time.deltaTime * actualspeed;

            if (t > 1)
            {
                pathIndex++;
                initLerp = false;
            }

            Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);
            transform.position = targetPos;
        }

        public void LoadPath(List<Block> path)
        {
            this.path = path;
            hasPath = true;
            pathIndex = 0;
        }
    }
}

