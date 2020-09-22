using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen.Terrain
{
    [System.Serializable]
    public struct NoiseSettings
    {
        [Min(1)]
        public int mapWidth;
        [Min(1)]
        public int mapHeight;
        public float scale;
        [Min(0)]
        public int octaves;
        [Range(0,1)]
        public float persistance;
        [Min(1)]
        public float lacunarity;
        public Vector2 offset;
        public string seed;

        public void RegulateValues()
        {
            if (mapWidth < 1)
            {
                mapWidth = 1;
            }
            if (mapHeight < 1)
            {
                mapHeight = 1;
            }
            if (lacunarity < 1)
            {
                lacunarity = 1;
            }
            if (octaves < 0)
            {
                octaves = 0;
            }
            persistance = Mathf.Clamp01(persistance);
        }
    }
    public static class Noise
    {
        private const int MIN_OCTAVE_OFFSET = -100000, MAX_OCTAVE_OFFSET = 100000;

        public static float[,] GenerateNoise(NoiseSettings nSettings)
        {
            float[,] map = new float[nSettings.mapWidth, nSettings.mapHeight];

            if (nSettings.scale <= 0)
            {
                nSettings.scale = 0.0001f;
            }

            float halfWidth = nSettings.mapWidth / 2f;
            float halfHeight = nSettings.mapHeight / 2f;

            System.Random rng = new System.Random(nSettings.seed.GetHashCode());
            Vector2[] octaveOffsets = new Vector2[nSettings.octaves];
            for (int i = 0; i < octaveOffsets.Length; i++)
            {
                float offsetX = rng.Next(MIN_OCTAVE_OFFSET, MAX_OCTAVE_OFFSET) + nSettings.offset.x;
                float offsetY = rng.Next(MIN_OCTAVE_OFFSET, MAX_OCTAVE_OFFSET) + nSettings.offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;
            for (int y = 0; y < nSettings.mapHeight; y++)
            {
                for (int x = 0; x < nSettings.mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < nSettings.octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / nSettings.scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / nSettings.scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= nSettings.persistance;
                        frequency *= nSettings.lacunarity;
                        
                    }
                    map[x, y] = noiseHeight;

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    } else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }
                }
            }
            for (int y = 0; y < nSettings.mapHeight; y++)
            {
                for (int x = 0; x < nSettings.mapWidth; x++)
                {
                    map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);
                }
            }
            return map;
        }
    }

}
