using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.Demo.Terrain
{
    public struct MapTile
    {
        public float noise;
        public int terrainTypeIndex;
        public float height;

        public MapTile(int terrainType, float noise)
        {
            this.terrainTypeIndex = terrainType;
            this.noise = noise;
            this.height = noise;
        }
        public void SetHeight(float height)
        {
            this.height = height;
        }
    }
    public class MapInfo
    {
        public MapTile[,] tiles;
        public float tileScale = 1;
        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); } }
        public TerrainRegions terrain { get; private set; }

        public MapInfo()
        {

        }

        public void GenerateMap(float[,] noiseMap, TerrainRegions terrain)
        {
            this.terrain = terrain;
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);
            tiles = new MapTile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool foundRegion = false;
                    for (int i = 0; i < terrain.regions.Length - 1; i++)
                    {
                        if (noiseMap[x, y] < terrain.regions[i].height)
                        {
                            foundRegion = true;
                            tiles[x, y] = new MapTile(i, noiseMap[x, y]);
                            break;
                        }
                    }
                    if (!foundRegion)
                    {
                        tiles[x, y] = new MapTile(terrain.regions.Length - 1, noiseMap[x, y]);
                    }
                }
            }
        }

        public TerrainType GetTileTerrain(int x, int y)
        {
            return terrain.regions[tiles[x, y].terrainTypeIndex];
        }
    }
}

