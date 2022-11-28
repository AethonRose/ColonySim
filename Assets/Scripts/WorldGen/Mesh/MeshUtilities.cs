using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public static class MeshUtilities
    {
        //Load FaceUp Mesh
        public static void FaceUp(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[1] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[2] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[3] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        } 
        //Load FaceDown Mesh
        public static void FaceDown(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            verticies[1] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            verticies[2] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            verticies[3] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        }
        //Load FaceNorth Mesh
        public static void FaceNorth(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            verticies[1] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[2] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[3] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        }
        //Load FaceSouth Mesh
        public static void FaceSouth(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            verticies[1] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[2] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[3] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        }
        //Load FaceEast Mesh
        public static void FaceEast(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            verticies[1] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[2] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[3] = new Vector3(offset.x + 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        }
        //Load FaceWest Mesh
        public static void FaceWest(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z + 0.5f);
            verticies[1] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[2] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[3] = new Vector3(offset.x - 0.5f, offset.y - 0.5f, offset.z - 0.5f);
            
            data.vertices.AddRange(verticies);

            AddTrianglesAndUVs(data, verticies);
        }
        //Load Quads/Triangle along & Call AddUVs function
        public static void AddTrianglesAndUVs(MeshData data, Vector3[] vertices)
        {
            //Make int array of 6, consists of 3 vertices for each triangle to make a Quad
            int[] vertex = new int[6];
            //Triangle 1 - 1/2 Quad
            vertex[0] = data.vertices.Count - 4;
            vertex[1] = data.vertices.Count - 3;
            vertex[2] = data.vertices.Count - 2;
            //Triangle 2 - 1/2 Quad
            vertex[3] = data.vertices.Count - 4;
            vertex[4] = data.vertices.Count - 2;
            vertex[5] = data.vertices.Count - 1;
            //Add constructed Quad to MeshData triangles List
            data.triangles.AddRange(vertex);
            //Calls AddUVs
            AddUVs(data);
        }
        //Load UV textures onto 2D Quads
        public static void AddUVs(MeshData data)
        {
            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            data.uv.AddRange(uv);
        }


    }
}

