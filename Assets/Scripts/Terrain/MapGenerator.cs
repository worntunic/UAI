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
        public bool gradientRegions = false;


        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoise(noiseSettings);
            mapDisplay.DrawNoiseMap(noiseMap);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            noiseSettings.RegulateValues();
        }
#endif
    }

}

