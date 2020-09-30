using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.GeneralAI
{
    public abstract class Action
    {
        private bool _actionStarted;
        public bool ActionStarted { get => _actionStarted; protected set => _actionStarted = value; }


        private bool _actionFinished;
        public bool ActionFinished { get => _actionFinished; protected set => _actionFinished = value; }

        public virtual void StartAction()
        {
            ActionStarted = true;
            ActionFinished = false;
        }
        public virtual void ExecuteAction()
        {

        }
        public virtual void EndAction()
        {
            ActionStarted = false;
        }
        public virtual bool CompareAction(Action otherAction)
        {
            return this.GetType() == otherAction.GetType();
        }
    }
}

