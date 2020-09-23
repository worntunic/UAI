using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.InputControl
{
    public class InputController : MonoBehaviour
    {
        public KeyCode generateTerrainKey = KeyCode.G;
        public Demo.Terrain.MapGenerator mapGenerator;

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

