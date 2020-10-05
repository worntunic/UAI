using System.Collections;
using System.Collections.Generic;
using UAI.AI;
using UnityEngine;

namespace UAI.Demo { 

    public class PlantContext : Context
    {
        public PlantSpawner plantSpawner;

        public PlantContext(PlantSpawner plantSpawner, string aiGuid)
        {
            this.plantSpawner = plantSpawner;
            this.aiGuid = aiGuid;
        }

        public override void UpdateContext()
        {
            float bunnyDistance = plantSpawner.BunniesDistance(plantSpawner.CurrentSpawnPosition);
            float waterDistance = plantSpawner.ClosestWaterPercentDistance(plantSpawner.CurrentSpawnPosition);
            float tileHeight = plantSpawner.TileHeightPercent(plantSpawner.CurrentSpawnPosition);
            float plantDistance = plantSpawner.ClosestPlantDistancePercent(plantSpawner.CurrentSpawnPosition);
            //Debug.Log($"bD({bunnyDistance}), wD({waterDistance}), tH({tileHeight}), pD({plantDistance})");
            UpdateValue("BunnyDistance", bunnyDistance);
            UpdateValue("WaterDistance", waterDistance);
            UpdateValue("TileHeight", tileHeight);
            UpdateValue("PlantDistance", plantDistance);
        }
    }
}