using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace  UAI.Demo.Terrain
{

    public class MapGenerator : MonoBehaviour
    {
        public bool autoUpdate = false;
        public bool generateOnStart = true;
        public NoiseSettings noiseSettings;
        public MapDisplay mapDisplay;
        private MapInfo mapInfo;
        public TerrainRegions region;
        public static event Action<MapInfo> OnMapGenerated;

        private void Awake()
        {
            mapInfo = new MapInfo();
        }
        private void Start()
        {
            GenerateMap();
        }
        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoise(noiseSettings);

            mapInfo.GenerateMap(noiseMap, region);
            mapDisplay.DrawNoiseMap(mapInfo);

            OnMapGenerated?.Invoke(mapInfo);
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            noiseSettings.RegulateValues();
        }
#endif
    }

}

