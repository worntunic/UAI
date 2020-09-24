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
        public NoiseSettings noiseSettings;
        public MapDisplay mapDisplay;
        private MapInfo _mapInfo;
        public MapInfo MapInfo { get { return _mapInfo; } }
        public TerrainRegions region;
        public static event Action<MapInfo> OnMapGenerated;

        public void GenerateMap()
        {

            float[,] noiseMap = Noise.GenerateNoise(noiseSettings);
            _mapInfo = new MapInfo();
            _mapInfo.GenerateMap(noiseMap, region);
            mapDisplay.DrawNoiseMap(_mapInfo);

            OnMapGenerated?.Invoke(_mapInfo);
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            noiseSettings.RegulateValues();
        }
#endif
    }

}

