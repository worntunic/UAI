using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.Demo.Terrain
{
    [System.Serializable]
    public struct TerrainMeshSettings
    {
        public float heightMultiplier;
        public AnimationCurve heightMultDistribution;

        public float GetHeightFactor(float height)
        {
            return heightMultDistribution.Evaluate(height) * heightMultiplier;
        }
    }
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(MapInfo mapInfo, TerrainMeshSettings tmSettings)
        {
            int width = mapInfo.Width;
            int height = mapInfo.Height;
            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            MeshData meshData = new MeshData(width, height);
            int vertexIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //mapInfo.tiles[x, y].SetHeight(mapInfo.tiles[x, y].noise * tmSettings.GetHeightFactor(mapInfo.tiles[x, y].noise));
                    mapInfo.tiles[x, y].SetHeight(tmSettings.GetHeightFactor(mapInfo.tiles[x, y].noise));
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, mapInfo.tiles[x, y].height, topLeftZ - y);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                    if (x < (width - 1) && y < (height - 1))
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }
            return meshData;
        }
    }

    public class MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public int triangleIndex = 0;
        public Vector2[] uvs;

        public MeshData(int width, int height)
        {
            vertices = new Vector3[width * height];
            triangles = new int[(width - 1) * (height - 1) * 6];
            uvs = new Vector2[width * height];
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

