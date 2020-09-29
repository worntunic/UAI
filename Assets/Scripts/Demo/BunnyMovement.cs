using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UAI.Utils;
using UnityEngine;

namespace UAI.Demo
{
    public class BunnyMovement : MonoBehaviour
    {
        private List<MapNode> currentPath;
        private int pathIndex = 1;
        public float timeBetweenSteps;
        public float moveTime;
        public AnimationCurve jumpCurve;
        private Timer stepTimer, moveTimer;
        private Vector3 prevPos;
        private Vector3 target;
        private bool movementInProgress = false;
        public bool MovementFinished() => !movementInProgress;

        private void Start()
        {
            stepTimer = new Timer(timeBetweenSteps, true);
            moveTimer = new Timer(moveTime, false);
            prevPos = transform.position;
            //GetNewPath();
        }
        public void StartNewMovement(Vector3 target)
        {
            movementInProgress = true;
            currentPath = Starter.PathFinder.FindPath(transform.position, target);
            if (currentPath.Count <= 1)
            {
                movementInProgress = false;
            }
            pathIndex = 1;
        }
        private void Update()
        {
            if (movementInProgress)
            {
                UpdateMovement();
            }
            Move();
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
                    movementInProgress = false;
                }
            }
        }

        private void Move()
        {
            Vector3 pos = Vector3.Lerp(prevPos, target, moveTimer.GetCurrentTimePercent());
            pos.y = pos.y + jumpCurve.Evaluate(moveTimer.GetCurrentTimePercent());
            transform.position = pos;
        }
    }
}

