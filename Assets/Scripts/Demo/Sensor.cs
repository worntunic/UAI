using System.Collections;
using System.Collections.Generic;
using UAI.Demo;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class Sensor : MonoBehaviour
    {
        public int radius;
        public Plants plants;
        public DrinkingWater drinkingWater;

        private List<MapNode> GetClosestPlantPath()
        {
            int distance = int.MaxValue;
            List<MapNode> retNodes = new List<MapNode>();
            foreach (GameObject plant in plants.plants)
            {
                if (Vector3.Distance(plant.transform.position, transform.position) <= radius)
                {
                    List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, plant.transform.position);
                    if (nodes.Count > 1 && nodes.Count < distance && nodes[nodes.Count - 1].gCost < radius * 10)
                    {
                        distance = nodes.Count;
                        retNodes = nodes;
                    }
                }

            }
            return retNodes;
        }
        public Vector3 GetClosestPlantPoint()
        {
            List<MapNode> nodes = GetClosestPlantPath();
            return nodes[nodes.Count - 1].worldPoint;
        }
        public int GetDistanceToClosestPlant()
        {
            return GetClosestPlantPath().Count;
        }
        public float GetDistanceToClosestPlantPercent()
        {
            return GetDistanceToClosestPlant() / ((float)radius);
        }

        public List<MapNode> GetClosestWaterPath()
        {
            int distance = int.MaxValue;
            List<MapNode> retNodes = new List<MapNode>();

            foreach (Vector3 waterTile in drinkingWater.drinkingWaterTiles)
            {
                if (Vector3.Distance(transform.position, waterTile) <= radius)
                {
                    List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, waterTile);
                    if (nodes.Count > 1 && nodes.Count < distance && nodes[nodes.Count - 1].gCost < radius * 10)
                    {
                        distance = nodes.Count;
                        retNodes = nodes;
                    }
                }

            }
            return retNodes;
        }
        public Vector3 GetClosestWaterPoint()
        {
            List<MapNode> nodes = GetClosestWaterPath();
            return nodes[nodes.Count - 1].worldPoint;
        }
        public int GetDistanceToClosestWater()
        {
            return GetClosestWaterPath().Count;
        }
        public float GetDistanceToClosestWaterPercent()
        {
            return GetDistanceToClosestWater() / ((float)radius);
        }
    }
}

