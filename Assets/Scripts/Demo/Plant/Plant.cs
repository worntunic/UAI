using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UnityEngine;

namespace UAI.Demo
{
    public class Plant : MonoBehaviour
    {
        public int maxTimesToEat = 5;
        public int currentTimesAte = 0;
        public PlantSpawner plantSpawner;
        public int foodAmount;


        public int Eat()
        {
            currentTimesAte++;
            if (currentTimesAte == maxTimesToEat)
            {
                plantSpawner.PlantEaten(this);
            }
            return foodAmount;
        }
    }
}

