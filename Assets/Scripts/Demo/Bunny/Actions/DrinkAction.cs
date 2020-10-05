using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{

    public class DrinkAction : UAI.GeneralAI.Action
    {
        Bunny bunny;
        PathInfo pathToWater;

        public DrinkAction(Bunny bunny)
        {
            this.bunny = bunny;
            pathToWater = bunny.sensor.GetClosestWaterPath();
            if (!pathToWater.ValidPath)
            {
                ActionFinished = true;
            }
        }

        public override void StartAction()
        {
            base.StartAction();
            bunny.movement.StartNewMovement(pathToWater);
        }
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (bunny.movement.MovementFinished())
            {
                bunny.stats.Drink();
                ActionFinished = true;
            }
        }
        public override bool CompareAction(Action otherAction)
        {
            return base.CompareAction(otherAction) && pathToWater.Target == ((DrinkAction)otherAction).pathToWater.Target;
        }
    }
}