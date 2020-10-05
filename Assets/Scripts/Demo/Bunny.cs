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
            set { _thirst = Mathf.Clamp(value, minThirst, maxThirst); }
        }
        public float minHunger, maxHunger;
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
            set { _hunger = Mathf.Clamp(value, minHunger, maxHunger); }
        }
        public float hungerDropPerSec;
        public float thirstDropPerSec;
        public float _fatigue;
        public float Fatigue { get => _fatigue; set => _fatigue = Mathf.Clamp(value, minFatigue, maxFatigue); }
        public float minFatigue, maxFatigue;
        public float fatigueDropPerStep;

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
        public void Feed(int foodAmount)
        {
            Hunger = Hunger - foodAmount;
        }
        public void MakeStep()
        {
            Fatigue += fatigueDropPerStep;
        }
        public void Rest(float restAmount)
        {
            Fatigue -= restAmount;
        }
        public float ThirstPercent => Thirst / maxThirst;
        public float HungerPercent => Hunger / maxHunger;
        public float FatiguePercent => Fatigue / maxFatigue;
    }
    public class Bunny : MonoBehaviour
    {
        public string aiGuid = "bunny";
        public Sensor sensor;
        public BunnyStats stats;
        public BunnyContext bunnyContext;
        public BunnyMovement movement;
        public DecisionMaking bunnyDecider;
        public BunnyBubble bubble;
        private string currentActionName = "";
        public float minActionOffsetTime;
        private Timer minActionOffsetTimer;
        Action currentAction = null;

        private void Start()
        {
            minActionOffsetTimer = new Timer(minActionOffsetTime, true, true);
            stats.Init();
            bunnyContext = new BunnyContext(this, aiGuid);
            if (!bunnyDecider.Initialized)
            {
                bunnyDecider.Init(bunnyContext);
            }

        }
        private void Update()
        {
            if (currentAction != null)
            {
                currentAction.ExecuteAction();
            }
            if (minActionOffsetTimer.IsTimerDone() && (currentAction == null || currentAction.ActionFinished))
            {
                CallForNewAction();
            }
        }
        private void CallForNewAction()
        {
            bunnyContext.UpdateContext();
            string actionName = bunnyDecider.Decide(bunnyContext);
            Action newAction = null;
            minActionOffsetTimer.Restart();

            if (actionName == "GetWater")
            {
                 newAction = new DrinkAction(this);
            } else if (actionName == "GetFood")
            {
                newAction = new FeedAction(this);
            } else if (actionName == "Nap")
            {
                newAction = new NapAction(this);
            } else if (actionName == "Wander")
            {
                newAction = new WanderAction(this);
            }
            if (newAction != null && (currentAction == null || !newAction.CompareAction(currentAction)))
            {
                if (currentAction != null)
                {
                    currentAction.EndAction();
                }
                currentAction = newAction;
                currentActionName = actionName;
                currentAction.StartAction();
                bubble.SetImage(currentActionName);
            }
        }
    }
}

