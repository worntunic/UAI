using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UAI
{
    public static class UnityExtensions
    {
        public const string separator = " _ ";
        public static void DebugLogEnumerable<T>(IEnumerable<T> arr)
        {
            StringBuilder strBuilder = new StringBuilder(); 
            foreach(T obj in arr)
            {
                strBuilder.Append(obj.ToString());
                strBuilder.Append(separator);
            }
            Debug.Log(strBuilder.ToString());
        }
    }
}

