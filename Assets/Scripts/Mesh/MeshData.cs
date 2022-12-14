using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen 
{
    public class MeshData
    {
        //Stores values of Mesh
        public int chunk_X;
        public int chunk_Y;
        public int chunk_Z;
        
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();
        public Vector3 origin;
    }
}

