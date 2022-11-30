using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public static class MeshUtilities
    {
        const float tileScale = 0.25f;
        //Load FaceUp Mesh
        public static void FaceUp(MeshData data, Vector3 offset)
        {
            Vector3[] verticies = new Vector3[4];
            verticies[0] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[1] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z + 0.5f);
            verticies[2] = new Vector3(offset.x + 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            verticies[3] = new Vector3(offset.x - 0.5f, offset.y + 0.5f, offset.z - 0.5f);
            
            data.vertices.AddRange(verticies);
            AddUVs(data, Direction.up);
            AddTriangles(data, verticies);
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
            AddUVs(data);
            AddTriangles(data, verticies);
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
            AddUVs(data);
            AddTriangles(data, verticies);
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
            AddUVs(data);
            AddTriangles(data, verticies);
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
            AddUVs(data);
            AddTriangles(data, verticies);
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
            AddUVs(data);
            AddTriangles(data, verticies);
        }
        //Load Quads/Triangle along & Call AddUVs function
        public static void AddTriangles(MeshData data, Vector3[] vertices)
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
        }
        //Load UV textures onto 2D Quads
        public static void AddUVs(MeshData data, Direction dir = Direction.allElse)
        {
            Vector2[] uv = new Vector2[4];
            Tile tilePos = TexturePosition(dir);

            uv[0] = new Vector2(tileScale * tilePos.x + tileScale, tileScale * tilePos.y);
            uv[1] = new Vector2(tileScale * tilePos.x + tileScale, tileScale * tilePos.y + tileScale);
            uv[2] = new Vector2(tileScale * tilePos.x, tileScale * tilePos.y + tileScale);
            uv[3] = new Vector2(tileScale * tilePos.x, tileScale * tilePos.y);

            data.uv.AddRange(uv);
        }

        public static Tile TexturePosition(Direction d)
        {
            switch(d)
            {
                case Direction.up:
                    return new Tile() { x = 2, y = 0};
                default:
                case Direction.allElse:
                    return new Tile() { x = 3, y = 0};
            }
        }

    }
}

