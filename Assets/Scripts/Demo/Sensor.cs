using System.Collections;
using System.Collections.Generic;
using UAI.Demo;
using UAI.Utils.DataStructures;
using UnityEngine;

namespace UAI.GeneralAI
{
    [System.Serializable]
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
            if (path != null && path.Count > 0 && sensorRadius != 0)
            {
                DistanceSensorPercent = path.Count / (float)sensorRadius;
            } else
            {
                DistanceSensorPercent = 1;
            }
        }
    }
    public class Sensor : MonoBehaviour
    {
        public int radius;
        public PlantSpawner plants;
        public DrinkingWater drinkingWater;
        [SerializeField]
        public PathInfo drinkingWaterPathInfo;
        public List<MapNode> drinkingTiles = new List<MapNode>();

        private void OnDrawGizmos()
        {
            for (int i = 0; i < drinkingTiles.Count; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(drinkingTiles[i].worldPoint, Vector3.one);
            }
        }

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
            drinkingTiles = drinkingWater.GetClosestDrinkTiles(transform.position, 5);
            List<MapNode> closeWaterTiles = drinkingTiles;
            foreach (MapNode waterTile in closeWaterTiles)
            {
                iterations++;
                List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, waterTile.worldPoint);
                if (nodes.Count > 1 && nodes.Count < distance)
                {
                    iterations++;
                    distance = nodes.Count;
                    retNodes = nodes;
                }

            }
            //sw.Stop();
            drinkingWaterPathInfo = new PathInfo(retNodes, radius);
            return drinkingWaterPathInfo;
        }
    }
}

