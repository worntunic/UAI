using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen.Terrain
{
    public class MapAnimator : MonoBehaviour
    {
        public bool animate;
        public Vector2 deltaMove;
        public MapGenerator mapGen;

        public void Update()
        {
            if (animate)
            {
                Move();
            }
        }
        public void Move()
        {
            mapGen.noiseSettings.offset += deltaMove;
            mapGen.GenerateMap();
        }
    }
}

