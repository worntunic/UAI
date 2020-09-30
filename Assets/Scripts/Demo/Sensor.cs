using System.Collections;
using System.Collections.Generic;
using UAI.Demo;
using UnityEngine;

namespace UAI.GeneralAI
{
    public struct PathInfo
    {
        public List<MapNode> path;
        public bool ValidPath { get => path != null && path.Count > 0; }
        public int PathLength { get => ValidPath ? path.Count : 0; }
        public int TileDistance {
            get
            {
                if (ValidPath)
                {
                    return Starter.PathFinder.GetTileDistance(path[0], path[path.Count - 1]);
                }
                return 0;
            }
        }
        public MapNode Target { get => (ValidPath) ? path[path.Count - 1] : null; }
        public float DistanceSensorPercent { get; private set; }
        
        public PathInfo(List<MapNode> path, int sensorRadius)
        {
            this.path = path;
            DistanceSensorPercent = (path != null && path.Count > 0 && sensorRadius != 0) ? path.Count / sensorRadius : 1;
        }
    }
    public class Sensor : MonoBehaviour
    {
        public int radius;
        public PlantSpawner plants;
        public DrinkingWater drinkingWater;

        public PathInfo GetClosestPlantPath(out Plant plant)
        {
            /*System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();*/
            int distance = int.MaxValue;
            List<MapNode> retNodes = new List<MapNode>();
            int iterations = 0;
            plant = null;
            foreach (Plant curPlant in plants.plants)
            {
                if (Starter.PathFinder.GetTileDistance(curPlant.transform.position, transform.position) <= radius)
                {
                    iterations++;
                    List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, curPlant.transform.position);
                    if (nodes.Count > 1 && nodes.Count < distance && nodes[nodes.Count - 1].gCost < radius * 10)
                    {
                        distance = nodes.Count;
                        retNodes = nodes;
                        plant = curPlant;
                    }
                }

            }
            //sw.Stop();
            //Debug.Log($"PlantPath: iterations({iterations}), time({sw.ElapsedMilliseconds}ms)");
            return new PathInfo(retNodes, radius);
        }
        public PathInfo GetClosestWaterPath()
        {
            /*System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();*/
            int distance = int.MaxValue;
            List<MapNode> retNodes = new List<MapNode>();
            int iterations = 0;
            List<MapNode> closeWaterTiles = GetClosestWaterTiles(5);
            foreach (MapNode waterTile in closeWaterTiles)
            {
                iterations++;
                List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, waterTile.worldPoint);
                if (nodes.Count > 1 && nodes.Count < distance && nodes[nodes.Count - 1].gCost < radius * 10)
                {
                    distance = nodes.Count;
                    retNodes = nodes;
                }

            }
            /*sw.Stop();
            Debug.Log($"WaterPath: iterations({iterations}), time({sw.ElapsedMilliseconds}ms)");*/
            return new PathInfo(retNodes, radius);
        }

        private List<MapNode> GetClosestWaterTiles(int count = 1)
        {
            MapNode start = Starter.PathFinder.mapGrid.GetFromWorldPos(transform.position);
            List<MapNode> retList = new List<MapNode>();

            for (int y = 0; y < radius * 2; y++)
            {
                int actY = start.y + ( (y % 2) == 0 ? y/2 : - (y/2));
                for (int x = 0; x < radius * 2; x++)
                {
                    if (retList.Count == count)
                    {
                        break;
                    }
                    int actX = start.x + ((x % 2) == 0 ? x / 2 : -(x / 2));
                    if (Starter.PathFinder.mapGrid.IsInsideMapEdges(actX, actY))
                    {
                        MapNode node = Starter.PathFinder.mapGrid.nodes[actX, actY];
                        if(IsTileDrinkingWater(actX, actY)) {
                            retList.Add(node);
                        }
                    }
                }
            }
            return retList;
        }
        private bool IsTileDrinkingWater(int x, int y)
        {
            return Starter.PathFinder.mapGrid.mapInfo.GetTileTerrain(x, y).drinkingWater;
        }
    }
}

