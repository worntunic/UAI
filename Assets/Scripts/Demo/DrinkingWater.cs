using System.Collections;
using System.Collections.Generic;
using UAI.Demo.Terrain;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{
    public class DrinkingWater : MonoBehaviour
    {
        public List<Vector3> drinkingWaterTiles;
        public MapGrid mapGrid;

        public void Init(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
            FindWater();
        }
        private void FindWater()
        {
            drinkingWaterTiles = new List<Vector3>();
            for (int y = 0; y < mapGrid.Height; y++)
            {
                for (int x = 0; x < mapGrid.Width; x++)
                {
                    if (mapGrid.mapInfo.GetTileTerrain(x, y).drinkingWater)
                    {
                        drinkingWaterTiles.Add(mapGrid.nodes[x, y].worldPoint);
                    }
                }
            }
        }
    }
}

