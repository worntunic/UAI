using System.Collections;
using System.Collections.Generic;
using UAI.AI;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{

    public class BunnyContext : Context
    {
        public Bunny bunny;

        public BunnyContext(Bunny bunny)
        {
            this.bunny = bunny;
        }

        public override void UpdateContext()
        {
            UpdateValue($"Thirst", bunny.stats.ThirstPercent);
            UpdateValue($"Hunger", bunny.stats.HungerPercent);
            UpdateValue($"FoodDistance", bunny.sensor.GetClosestPlantPath(out _).DistanceSensorPercent);
            UpdateValue($"WaterDistance", bunny.sensor.GetClosestWaterPath().DistanceSensorPercent);
        }
    }
}

