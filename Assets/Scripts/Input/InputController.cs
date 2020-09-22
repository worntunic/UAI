using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen.InputControl
{
    public class InputController : MonoBehaviour
    {
        public KeyCode generateTerrainKey = KeyCode.G;
        public Terrain.MapGenerator mapGenerator;

        private void Update()
        {
            CheckTerrainGen();
        }

        private void CheckTerrainGen()
        {
            if (Input.GetKeyDown(generateTerrainKey))
            {
                mapGenerator.GenerateMap();
            } 
        }
    }
}

