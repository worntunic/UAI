using System.Collections;
using System.Collections.Generic;
using UAI.AI;
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
        public DrinkingWater water;
        private Vector3 currentSpawnPosition;
        public Vector3 CurrentSpawnPosition { get => currentSpawnPosition; }
        public string aiGuid = "plantSpawner";
        public DecisionMaking plantDecisionMaking;
        private PlantContext plantContext;
        public BunnySpawner bunnySpawner;
        public int scanSpread = 11;

        public void Init(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;

            spawnTimer = new Timer(timeBetweenSpawnings, true, true);
            plantContext = new PlantContext(this, aiGuid);
            plantDecisionMaking.Init(plantContext);
            CreateInitialPlants();
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
            float bestUtilityValue = float.MinValue;
            Vector3 bestPosition = Vector3.zero;
            int bottomEdge = UnityEngine.Random.Range(mapGrid.BottomEdge, scanSpread);
            int leftEdge = UnityEngine.Random.Range(mapGrid.LeftEdge, scanSpread);
            for (int y = bottomEdge; y < mapGrid.TopEdge; y+=scanSpread)
            {
                for (int x = leftEdge; x < mapGrid.RightEdge; x+=scanSpread)
                {
                    if (mapGrid.nodes[x, y].passable)
                    {
                        currentSpawnPosition = mapGrid.nodes[x, y].worldPoint;
                        plantContext.UpdateContext();
                        ActionValue actVal = plantDecisionMaking.DecideAndReturnValue(plantContext);
                        if (actVal.value > bestUtilityValue)
                        {
                            bestUtilityValue = actVal.value;
                            bestPosition = currentSpawnPosition;
                        }

                    }
                }
            }
            //MapNode node = mapGrid.GetRandomPassableNode();
            Plant plant = Instantiate(plantPrefab, transform);
            plant.transform.position = bestPosition;
            plant.plantSpawner = this;
            plants.Add(plant);
        }
        public void PlantEaten(Plant plant)
        {
            plants.Remove(plant);
            Destroy(plant.gameObject);
        }
        public float BunniesDistance(Vector3 pos)
        {
            int minDistance = int.MaxValue;
            for (int i = 0; i < bunnySpawner.bunnies.Count; i++)
            {
                int newDist = Starter.PathFinder.GetTileDistance(pos, bunnySpawner.bunnies[i].transform.position);
                if (newDist < minDistance)
                {
                    newDist = minDistance;
                }
            }

            return minDistance / (float) mapGrid.Width;
        }



        public float ClosestWaterPercentDistance(Vector3 pos)
        {
            List<MapNode> tiles = water.GetClosestDrinkTiles(pos, 5);
            if(tiles.Count == 0)
            {
                return 1;
            }
            return Starter.PathFinder.GetTileDistance(tiles[0].worldPoint, pos) / (float)mapGrid.Width;
        }
        public float TileHeightPercent(Vector3 pos)
        {
            MapNode node = mapGrid.GetFromWorldPos(pos);
            return mapGrid.mapInfo.tiles[node.x, node.y].noise;
        }
        public float ClosestPlantDistancePercent(Vector3 pos)
        {
            int distance = int.MaxValue;
            int index = 0;
            for (int i = 0; i < plants.Count; i++)
            {
                int newDist = Starter.PathFinder.GetTileDistance(pos, plants[i].transform.position);
                if (newDist < distance)
                {
                    distance = newDist;
                    index = i;
                }
            }
            return distance / (float)mapGrid.Width;
        }

    }
}

