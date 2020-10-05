using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UAI.Utils;
using UnityEngine;

namespace UAI.Demo
{

    public class WanderAction : UAI.GeneralAI.Action
    {
        Bunny bunny;
        PathInfo wanderPath;
        float minTimeWandering = 3;
        Timer wanderTimer;

        public WanderAction(Bunny bunny)
        {
            this.bunny = bunny;
            wanderPath = bunny.sensor.GetRandomPath();
            if (!wanderPath.ValidPath)
            {
                ActionFinished = true;
            }
        }

        public override void StartAction()
        {
            base.StartAction();
            bunny.movement.StartNewMovement(wanderPath);
            wanderTimer = new Timer(minTimeWandering, true);
        }
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (wanderTimer.IsTimerDone() || bunny.movement.MovementFinished())
            {
                ActionFinished = true;
            }
        }
        public override bool CompareAction(Action otherAction)
        {
            return base.CompareAction(otherAction) /*&& wanderPath.Target == ((WanderAction)otherAction).wanderPath.Target*/;
        }
    }
}