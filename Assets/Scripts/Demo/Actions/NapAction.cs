using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UAI.Utils;
using UnityEngine;

namespace UAI.Demo
{
    public class NapAction : UAI.GeneralAI.Action
    {
        Bunny bunny;
        float minTimeResting = 3;
        Timer restTimer;
        float restAmountPerSec = 10;

        public NapAction(Bunny bunny)
        {
            this.bunny = bunny;

        }

        public override void StartAction()
        {
            base.StartAction();
            restTimer = new Timer(minTimeResting, true, false);
            bunny.movement.StopMovement();
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (restTimer.IsTimerDone()) {
                ActionFinished = true;
            }
            bunny.stats.Rest(restAmountPerSec * Time.deltaTime);
        }

        public override bool CompareAction(Action otherAction)
        {
            return base.CompareAction(otherAction);
        }
    }
}

