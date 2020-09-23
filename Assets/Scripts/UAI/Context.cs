using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.AI
{
    public abstract class Context
    {
        //public List<string> keys = new List<string>();
        private Dictionary<string, float> values;

        public Context(List<string> keys)
        {
            values = new Dictionary<string, float>();
            for (int i = 0; i < keys.Count; i++)
            {
                values.Add(keys[i], 0);
            }
        }

        public void UpdateValue(string key, float newValue)
        {
            values[key] = newValue;
        }
        public float GetValue(string key)
        {
            return values[key];
        }

        public abstract void UpdateContext();
    }
}

