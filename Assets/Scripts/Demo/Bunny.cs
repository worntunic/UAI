using System.Collections;
using System.Collections.Generic;
using UAI.AI;
using UAI.Demo;
using UAI.Utils;
using UnityEngine;

namespace UAI.GeneralAI
{
    [System.Serializable]
    public class BunnyStats
    {
        public float minThirst, maxThirst;
        public float drinkAmount;
        public float _thirst;
        public float Thirst
        {
            get
            {
                if (thirstDropTimer.IsTimerDone())
                {
                    _thirst = Mathf.Clamp(_thirst + (thirstDropPerSec * thirstDropTimer.GetCycles()), minThirst, maxThirst);
                    thirstDropTimer.Restart();
                }
                return _thirst;
            }
            set { _thirst = value; }
        }
        public float minHunger, maxHunger;
        public float feedAmount;
        public float _hunger;
        public float Hunger
        {
            get
            {
                if (hungerDropTimer.IsTimerDone())
                {
                    _hunger = Mathf.Clamp(_hunger + (hungerDropPerSec * hungerDropTimer.GetCycles()), minHunger, maxHunger);
                    hungerDropTimer.Restart();
                }
                return _hunger;
            }
            set { _hunger = value; }
        }
        public float hungerDropPerSec;
        public float thirstDropPerSec;
        private Timer hungerDropTimer, thirstDropTimer;

        public void Init()
        {
            hungerDropTimer = new Timer(1, true, false);
            thirstDropTimer = new Timer(1, true, false);
        }
        public void Drink()
        {
            Thirst = Thirst - drinkAmount;
        }
        public void Feed()
        {
            Hunger = Hunger - feedAmount;
        }
        public float ThirstPercent => Thirst / maxThirst;
        public float HungerPercent => Hunger / maxHunger;
    }
    public class Bunny : MonoBehaviour
    {

        public Sensor sensor;
        public BunnyStats stats;
        public BunnyContext bunnyContext;
        public BunnyMovement movement;
        public DecisionMaking bunnyDecider;
        private string prevAction = "";

        private void Start()
        {
            stats.Init();
            bunnyContext = new BunnyContext(this);
            if (!bunnyDecider.Initialized)
            {
                bunnyDecider.Init(bunnyContext);
            }
        }
        private void Update()
        {
            if (movement.MovementFinished())
            {
                
                CallForNewAction();
            }
        }
        private void CallForNewAction()
        {
            bunnyContext.UpdateContext();
            string actionName = bunnyDecider.Decide(bunnyContext);
            
            if(actionName == "GetWater")
            {
                Vector3 water = sensor.GetClosestWaterPoint();
                movement.StartNewMovement(water);
            } else if (actionName == "GetFood")
            {
                Vector3 food = sensor.GetClosestPlantPoint();
                movement.StartNewMovement(food);
            }
            prevAction = actionName;
        }
        private void ResolveFinishedAction()
        {
            if (prevAction == "GetWater")
            {
                stats.Drink();
            }
            else if (prevAction == "GetFood")
            {
                stats.Feed();
            }
        }

    }
}

