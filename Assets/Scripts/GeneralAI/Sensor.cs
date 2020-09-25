using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class Sensor : MonoBehaviour
    {
        public int radius;
        public Plants plants;

        public Vector3 GetPathToClosestPlant()
        {
            int distance = int.MaxValue;
            Vector3 ret = transform.position;
            foreach (GameObject plant in plants.plants)
            {
                List<MapNode> nodes = Starter.PathFinder.FindPath(transform.position, plant.transform.position);
                if (nodes.Count > 1 && nodes.Count < distance && nodes[nodes.Count - 1].gCost < radius * 10)
                {
                    distance = nodes.Count;
                    ret = plant.transform.position;
                }
            }
            return ret;
        }
    }
}

