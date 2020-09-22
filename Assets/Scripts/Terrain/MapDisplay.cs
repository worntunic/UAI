using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen.Terrain
{
    public class MapDisplay : MonoBehaviour
    {
        public enum DrawMode { _2D, _3D };
        public enum ColorType { Grayscale, Terrain, TerrainGradient }

        public DrawMode drawMode;
        public ColorType colorType;
        public MeshRenderer renderer2D;
        public MeshFilter meshFilter;
        public MeshRenderer renderer3D;
        private Texture2D curTexture;
        public FilterMode textureFilterMode;
        public TerrainRegions currentTerrain;
        public TerrainMeshSettings tmSettings;


        public void DrawNoiseMap(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            GenerateTexture(noiseMap);

            if (drawMode == DrawMode._2D)
            {
                Draw2D(width, height);
            } else if (drawMode == DrawMode._3D)
            {
                Draw3D(noiseMap);
            }
        }

        private void Draw3D(float[,] noiseMap)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, tmSettings);
            meshFilter.sharedMesh = meshData.CreateMesh();
            renderer3D.sharedMaterial.mainTexture = curTexture;
            renderer3D.enabled = true;
            renderer2D.enabled = false;
        }

        private void Draw2D(int width, int height)
        {
            renderer2D.sharedMaterial.mainTexture = curTexture;
            renderer2D.transform.localScale = new Vector3(width, 1, height);
            Camera.main.transform.position = new Vector3(0, 1, (height * (-10)));
            renderer2D.enabled = true;
            renderer3D.enabled = false;
        }
        private void GenerateTexture(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            Color[,] color2D = null;
            if (colorType == ColorType.Grayscale)
            {
                color2D = NoiseToBNW(noiseMap);
            }
            else if (colorType == ColorType.Terrain)
            {
                color2D = NoiseToRegionColors(noiseMap, false);
            }
            else if (colorType == ColorType.TerrainGradient)
            {
                color2D = NoiseToRegionColors(noiseMap, true);
            }

            if (curTexture == null || curTexture.width != width || curTexture.height != height)
            {
                curTexture = new Texture2D(width, height);

            }
            curTexture.filterMode = textureFilterMode;
            Color[] colors = Convert2Dto1D(color2D);

            curTexture.SetPixels(colors);
            curTexture.Apply();
        }
        private Color[,] NoiseToRegionColors(float[,] noiseMap, bool gradientColors = false)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            Color[,] colors = new Color[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool foundColor = false;
                    for (int i = 0; i < currentTerrain.regions.Length - 1; i++)
                    {
                        if (noiseMap[x, y] < currentTerrain.regions[i].height)
                        {
                            colors[x, y] = gradientColors ? GetRegionColorGradient(i, noiseMap[x, y]) : GetRegionColor(i);
                            foundColor = true;
                            break;
                        }
                    }
                    if (!foundColor)
                    {
                        colors[x, y] = gradientColors ? GetRegionColorGradient(currentTerrain.regions.Length - 1, noiseMap[x, y]) : GetRegionColor(currentTerrain.regions.Length - 1); ;
                    }
                }
            }
            return colors;
        }
        private Color GetRegionColor(int index)
        {
            return currentTerrain.regions[index].color;
        }
        private Color GetRegionColorGradient(int index, float height)
        {
            if (index == 0)
            {
                return currentTerrain.regions[index].color;
            }
            float t = (height - currentTerrain.regions[index - 1].height) / (currentTerrain.regions[index].height - currentTerrain.regions[index - 1].height);
            return Color.Lerp(currentTerrain.regions[index - 1].color, currentTerrain.regions[index].color, t);
        }
        private Color[,] NoiseToBNW(float[,] noiseMap)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            Color[,] colors = new Color[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colors[x, y] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
                }
            }
            return colors;
        }
        private static Color[] Convert2Dto1D(Color[,] colors2D)
        {
            int width = colors2D.GetLength(0);
            int height = colors2D.GetLength(1);
            Color[] colors = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colors[y * height + x] = colors2D[x, y];
                }
            }
            return colors;
        }
    }
}

