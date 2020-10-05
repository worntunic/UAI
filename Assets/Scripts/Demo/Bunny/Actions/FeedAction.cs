using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{

    public class FeedAction : UAI.GeneralAI.Action
    {
        Bunny bunny;
        PathInfo pathToPlant;
        Plant plant;

        public FeedAction(Bunny bunny)
        {
            this.bunny = bunny;
            pathToPlant = bunny.sensor.GetClosestPlantPath(out plant);
        }

        public override void StartAction()
        {
            base.StartAction();
            if (!pathToPlant.ValidPath)
            {
                ActionFinished = true;
            } else
            {
                bunny.movement.StartNewMovement(pathToPlant);
            }
        }
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (bunny.movement.MovementFinished())
            {
                if (plant != null)
                {
                    int foodAmount = plant.Eat();

                    bunny.stats.Feed(foodAmount);

                }
                ActionFinished = true;

            }
        }
        public override bool CompareAction(Action otherAction)
        {
            return base.CompareAction(otherAction) && pathToPlant.Target == ((FeedAction)otherAction).pathToPlant.Target;
        }
    }
}