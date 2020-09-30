using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UAI.Utils;
using UnityEngine;

namespace UAI.Demo
{
    public class PlantSpawner : MonoBehaviour
    {
        public Plant plantPrefab;
        public List<Plant> plants;
        public int initialNumberOfPlants = 10;
        public MapGrid mapGrid;
        public float timeBetweenSpawnings;
        private Timer spawnTimer;
        public void Init(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
            CreateInitialPlants();
            spawnTimer = new Timer(timeBetweenSpawnings, true, true);
        }
        private void Update()
        {
            if (spawnTimer.IsTimerDone())
            {
                CreatePlant();
                spawnTimer.Restart();
            }
        }

        private void CreateInitialPlants()
        {
            plants = new List<Plant>();
            for (int i = 0; i < initialNumberOfPlants; i++)
            {
                CreatePlant();
            }
        }
        private void CreatePlant()
        {
            MapNode node = mapGrid.GetRandomPassableNode();
            Plant plant = Instantiate(plantPrefab, transform);
            plant.transform.position = node.worldPoint;
            plant.plantSpawner = this;
            plants.Add(plant);
        }
        public void PlantEaten(Plant plant)
        {
            plants.Remove(plant);
            Destroy(plant.gameObject);
        }

    }
}

