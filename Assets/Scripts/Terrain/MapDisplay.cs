using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.Demo.Terrain
{
    public class MapDisplay : MonoBehaviour
    {
        public enum DrawMode { _2D, _3D };
        public enum ColorType { Grayscale, Terrain, TerrainGradient, Passability }

        public DrawMode drawMode;
        public ColorType colorType;
        public MeshRenderer renderer2D;
        public MeshFilter meshFilter;
        public MeshRenderer renderer3D;
        private Texture2D curTexture;
        public FilterMode textureFilterMode;
        public TerrainMeshSettings tmSettings;

        public void DrawNoiseMap(MapInfo mapInfo)
        {
            GenerateTexture(mapInfo);

            if (drawMode == DrawMode._2D)
            {
                Draw2D(mapInfo.Width, mapInfo.Height);
            } else if (drawMode == DrawMode._3D)
            {
                Draw3D(mapInfo);
            } 
        }

        private void Draw3D(MapInfo mapInfo)
        {
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapInfo, tmSettings);
            meshFilter.sharedMesh = meshData.CreateMesh();
            renderer3D.sharedMaterial.mainTexture = curTexture;
            renderer3D.enabled = true;
            renderer2D.enabled = false;
        }

        private void Draw2D(int width, int height)
        {
            renderer2D.sharedMaterial.mainTexture = curTexture;
            //renderer2D.transform.localScale = new Vector3(width, 1, height);
            //Camera.main.transform.position = new Vector3(0, 1, (height * (-10)));
            renderer2D.enabled = true;
            renderer3D.enabled = false;
        }
        private void GenerateTexture(MapInfo mapInfo)
        {
            Color[] colors = null;
            if (colorType == ColorType.Grayscale)
            {
                colors = NoiseToBNW(mapInfo);
            }
            else if (colorType == ColorType.Terrain)
            {
                colors = NoiseToRegionColors(mapInfo, false);
            }
            else if (colorType == ColorType.TerrainGradient)
            {
                colors = NoiseToRegionColors(mapInfo, true);
            } else if (colorType == ColorType.Passability)
            {
                colors = NoiseToPassability(mapInfo);
            }

            if (curTexture == null || curTexture.width != mapInfo.Width || curTexture.height != mapInfo.Height)
            {
                curTexture = new Texture2D(mapInfo.Width, mapInfo.Height);

            }
            curTexture.filterMode = textureFilterMode;
            curTexture.SetPixels(colors);
            curTexture.Apply();
        }
        private Color[] NoiseToRegionColors(MapInfo mapInfo, bool gradientColors = false)
        {
            Color[] colors = new Color[mapInfo.Width * mapInfo.Height];
            for (int y = 0; y < mapInfo.Height; y++)
            {
                for (int x = 0; x < mapInfo.Width; x++)
                {
                    int terrainIndex = mapInfo.tiles[x, y].terrainTypeIndex;
                    if (gradientColors)
                    {
                        colors[y * mapInfo.Height + x] = GetRegionColorGradient(terrainIndex, mapInfo, mapInfo.tiles[x, y].noise);
                    } else
                    {
                        colors[y * mapInfo.Height + x] = GetRegionColor(terrainIndex, mapInfo);
                    }
                }
            }
            return colors;
        }
        private Color GetRegionColor(int index, MapInfo mapInfo)
        {
            return mapInfo.terrain.regions[index].color;
        }
        private Color GetRegionColorGradient(int index, MapInfo mapInfo, float height)
        {
            if (index == 0)
            {
                return mapInfo.terrain.regions[index].color;
            }
            float t = (height - mapInfo.terrain.regions[index - 1].height) / (mapInfo.terrain.regions[index].height - mapInfo.terrain.regions[index - 1].height);
            return Color.Lerp(mapInfo.terrain.regions[index - 1].color, mapInfo.terrain.regions[index].color, t);
        }
        private Color[] NoiseToBNW(MapInfo mapInfo)
        {
            Color[] colors = new Color[mapInfo.Width * mapInfo.Height];
            for (int y = 0; y < mapInfo.Height; y++)
            {
                for (int x = 0; x < mapInfo.Width; x++)
                {
                    colors[y * mapInfo.Height + x] = Color.Lerp(Color.black, Color.white, mapInfo.tiles[x, y].noise);
                }
            }
            return colors;
        }
        private Color[] NoiseToPassability(MapInfo mapInfo)
        {
            Color[] colors = new Color[mapInfo.Width * mapInfo.Height];
            for (int y = 0; y < mapInfo.Height; y++)
            {
                for (int x = 0; x < mapInfo.Width; x++)
                {
                    colors[y * mapInfo.Height + x] = mapInfo.GetTileTerrain(x, y).passable ?Color.gray : Color.white;
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

