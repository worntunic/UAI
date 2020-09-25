using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class Plants : MonoBehaviour
    {
        public GameObject plantPrefab;
        public List<GameObject> plants;
        public int numberOfPlants = 10;
        public MapGrid mapGrid;

        public void Initiate(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
            CreatePlants();
        }

        private void CreatePlants()
        {
            plants = new List<GameObject>();
            for (int i = 0; i < numberOfPlants; i++)
            {
                MapNode node = mapGrid.GetRandomPassableNode();
                GameObject plant = Instantiate(plantPrefab, transform);
                plant.transform.position = node.worldPoint;
                plants.Add(plant);
            }
        }
    }
}

