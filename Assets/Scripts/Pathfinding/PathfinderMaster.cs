using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace WorldGen.Pathfinding
{
    public class PathfinderMaster : MonoBehaviour
    {

        //Multithreading Jobs
        public int maxJobs = 4;
        List<Pathfinder> toDoJobs = new List<Pathfinder>();
        List<Pathfinder> currentJobs = new List<Pathfinder>();

        //Delegate callback used in Pathfinder.cs, requires path List of type Block to complete
        public delegate void PathCompleteCallback(List<Block> path);

        //Getting World ref
        public World world;

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
                //Starting job as type Pathfinder, index 0 of todoJobs
                Pathfinder job = toDoJobs[0];
                //Remove 1st toDoJob as it is now saved in job
                toDoJobs.RemoveAt(0);
                //Adding job to currentJobs List
                currentJobs.Add(job);

                //Calls FindPath on new jobThread
                Thread jobThread = new Thread(job.FindPath);
                //Starts jobThread
                jobThread.Start();
            }
        }

        //Starts process of Requesting A* path to be found
        public void RequestPathFind(Block start, Block target)
        {
            //Sets Pathfinder values of current pathJob
            Pathfinder pathJob = new Pathfinder(world, start, target, PathCallback);
            //Adds pathJob to toDoJobs List
            toDoJobs.Add(pathJob);
        }

        //Assigned delegate in Pathfinder.cs through pathJob in RequestPathFind()
        void PathCallback(List<Block> path)
        {
            if (path == null)
            {
                return;
            }

            //Visualize path
            GameObject go = new GameObject();
            LineRenderer l = go.AddComponent<LineRenderer>();
            l.positionCount = path.Count;

            //Loops through path and sets visualizer == path[i].worldPosition to visualize A* pathfinding
            for (int i = 0; i < l.positionCount; i++)
            {
                l.SetPosition(i, path[i].worldPosition + Vector3.up);
            }
        }
    }
}

