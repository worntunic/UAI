using System.Collections;
using System.Collections.Generic;
using UAI.Utils;
using UnityEngine;

namespace UAI.GeneralAI
{
    public class Bunny : MonoBehaviour
    {
        private List<MapNode> currentPath;
        private int pathIndex = 1;
        public float timeBetweenSteps;
        public float moveTime;
        public AnimationCurve jumpCurve;
        private Timer stepTimer, moveTimer;
        private Vector3 prevPos;
        private Vector3 target;
        public Sensor sensor;

        private void Start()
        {
            stepTimer = new Timer(timeBetweenSteps, true);
            moveTimer = new Timer(moveTime, false);
            prevPos = transform.position;
            GetNewPath();
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (stepTimer.IsTimerDone())
            {
                MapNode nextNode = currentPath[pathIndex];
                transform.rotation = Quaternion.LookRotation(nextNode.worldPoint - transform.position);
                stepTimer.Restart();
                pathIndex++;
                prevPos = target;
                target = nextNode.worldPoint;
                moveTimer.Restart();

                if (currentPath.Count == pathIndex)
                {
                    GetNewPath();
                }
            }
            Move();
        }
        private void Move()
        {
            Vector3 pos = Vector3.Lerp(prevPos, target, moveTimer.GetCurrentTimePercent());
            pos.y = pos.y + jumpCurve.Evaluate(moveTimer.GetCurrentTimePercent());
            transform.position = pos;
        }
        private void GetNewPath()
        {
            currentPath = Starter.PathFinder.FindPath(transform.position, sensor.GetPathToClosestPlant());
            while (currentPath.Count <= 1)
            {
                currentPath = Starter.PathFinder.FindPathToRandomPoint(transform.position);
            }
            pathIndex = 1;
        }
    }
}

