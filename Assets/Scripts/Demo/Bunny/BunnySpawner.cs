using System.Collections;
using System.Collections.Generic;
using UAI.AI;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{
    public class BunnySpawner : MonoBehaviour
    {
        public Bunny prefab;
        public int initialBunnyNumber;
        public List<Bunny> bunnies = new List<Bunny>();
        public DecisionMaking bunnyDecider;
        public PlantSpawner plantSpawner;
        public DrinkingWater drinkingWater;

        public void Init()
        {
            for (int i = 0; i < initialBunnyNumber; i++)
            {
                MapNode node = Starter.PathFinder.mapGrid.GetRandomPassableNode();
                bool occupiedNode = true;
                while (occupiedNode)
                {
                    int occupiers = 0;
                    for (int j = 0; j < bunnies.Count; j++)
                    {
                        if (bunnies[j].transform.position == node.worldPoint)
                        {
                            occupiers++;
                        }
                    }
                    if (occupiers > 0)
                    {
                        node = Starter.PathFinder.mapGrid.GetRandomPassableNode();
                    } else
                    {
                        occupiedNode = false;
                    }
                }
                CreateBunny(node.worldPoint);
            }
        }
        private void CreateBunny(Vector3 pos)
        {
            Bunny newBunny = GameObject.Instantiate(prefab, transform);
            newBunny.bunnyDecider = bunnyDecider;
            newBunny.sensor.drinkingWater = drinkingWater;
            newBunny.sensor.plants = plantSpawner;
            newBunny.aiGuid = "bunny" + bunnies.Count;
            bunnies.Add(newBunny);
        }
    }
}

