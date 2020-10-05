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

        public BunnyContext(Bunny bunny, string aiGuid)
        {
            this.bunny = bunny;
            this.aiGuid = aiGuid;
        }

        public override void UpdateContext()
        {
            UpdateValue($"Thirst", bunny.stats.ThirstPercent);
            UpdateValue($"Hunger", bunny.stats.HungerPercent);
            UpdateValue($"FoodDistance", bunny.sensor.GetClosestPlantPath(out _).DistanceSensorPercent);
            float distance = bunny.sensor.GetClosestWaterPath().DistanceSensorPercent;
            UpdateValue($"WaterDistance", distance);
            UpdateValue($"Fatigue", bunny.stats.FatiguePercent);
        }
    }
}

