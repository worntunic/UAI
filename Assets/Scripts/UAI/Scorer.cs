using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.AI
{
    public interface IScorable
    {
        float Evaluate(Context context);
    }
    public class Scorer : IScorable
    {
        public AnimationCurve curve;
        public string key;

        public float Evaluate(Context context)
        {
            return curve.Evaluate(context.GetValue(key));
        }
    }
    
}

