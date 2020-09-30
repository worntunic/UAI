using System.Collections;
using System.Collections.Generic;
using UAI.GeneralAI;
using UAI.Utils;
using UnityEngine;

namespace UAI.Demo
{
    public class BunnyMovement : MonoBehaviour
    {
        public bool drawPath = false;
        private PathInfo currentPath;
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
        public void StartNewMovement(PathInfo path)
        {
            movementInProgress = true;
            currentPath = path;
            stepTimer.Restart();
            if (!currentPath.ValidPath)
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
                MapNode nextNode = currentPath.path[pathIndex];
                transform.rotation = Quaternion.LookRotation(nextNode.worldPoint - transform.position);
                stepTimer.Restart();
                pathIndex++;
                prevPos = target;
                target = nextNode.worldPoint;
                moveTimer.Restart();


            }
            if (currentPath.path.Count == pathIndex && moveTimer.IsTimerDone())
            {
                movementInProgress = false;
            }
        }

        private void Move()
        {
            Vector3 pos = Vector3.Lerp(prevPos, target, moveTimer.GetCurrentTimePercent());
            pos.y = pos.y + jumpCurve.Evaluate(moveTimer.GetCurrentTimePercent());
            transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                if (currentPath.ValidPath)
                {
                    foreach (MapNode node in currentPath.path)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(node.worldPoint, Vector3.one);
                    }
                }
            }
        }
    }
}

