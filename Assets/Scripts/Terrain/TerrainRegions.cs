using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.Demo.Terrain
{
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
    [CreateAssetMenu(fileName = "TerrainRegions", menuName = "Data/Terrain/Regions", order = 1)]
    public class TerrainRegions : ScriptableObject
    {
        public TerrainType[] regions;

    }
}

