using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UAI.AI
{

    public class Selector
    {
        public enum SelectorType
        {
            Best, RandomFromBestN, WeightedRandomFromBestN, RandomFromTopPercent, WeightedRandomFromTopPercent, TrueRandom
        }

        public List<Qualifier> qualifiers;
        public SelectorType type;
        public int _bestN;
        public int BestN { get { return _bestN; } set { _bestN = (value > 1) ? value : 1; } }
        private float _bestPercent;
        public float BestPercent { get { return _bestPercent; } set { _bestPercent = Mathf.Clamp01(value); } }

        public Selector()
        {
            qualifiers = new List<Qualifier>();
            type = SelectorType.Best;
        }

        public Selector(SelectorType type)
        {
            qualifiers = new List<Qualifier>();
            this.type = type;
        }

        public void AddQualifier(Qualifier qualifier)
        {
            qualifiers.Add(qualifier);
        }

        public void RemoveQualifier(Qualifier qualifier)
        {
            qualifiers.Remove(qualifier);
        }
        private List<(Qualifier, float)> GetOrdered(Context context)
        {
            List<(Qualifier, float)> qts = new List<(Qualifier, float)>(qualifiers.Count);
            for (int i = 0; i < qualifiers.Count; i++)
            {
                qts.Add((qualifiers[i], qualifiers[i].Evaluate(context)));
            }
            qts.Sort(delegate((Qualifier, float) x, (Qualifier, float) y) {
                return (x.Item2 - y.Item2).RoundOutToInt();
            });
            return qts;
        }
        

        private string EvalBest(Context context)
        {
            return GetOrdered(context)[0].Item1.actionName;
        }
        private string EvalBestRandomFromN(Context context)
        {
            int index = Random.Range(0, BestN);
            return GetOrdered(context)[index].Item1.actionName;
        }
        private string EvalBestWeightedRandomFromN(Context context)
        {
            int index = Random.Range(0, BestN);
            List<(Qualifier, float)> qts = GetOrdered(context);
            float valueSum = qts.GetRange(0, BestN).Sum(qt => qt.Item2);
            float randomNumber = Random.Range(0, valueSum);
            for (int i = 0; i < BestN; i++)
            {
                if (qts[i].Item2 >= randomNumber)
                {
                    return qts[i].Item1.actionName;
                }
            }
            return qts[0].Item1.actionName;
        }
        private string EvalBestRandomTopPercent(Context context)
        {
            List<(Qualifier, float)> qts = GetOrdered(context);
            int topCount = 1;
            float minVal = qts[0].Item2 * (1 - BestPercent);
            for (int i = 1; i < qts.Count; i++)
            {
                if (qts[i].Item2 >= minVal)
                {
                    topCount++;
                } else
                {
                    break;
                }
            }
            int index = Random.Range(0, topCount);
            return qts[index].Item1.actionName;
        }
        private string EvalBestWeightedRandomTopPercent(Context context)
        {
            List<(Qualifier, float)> qts = GetOrdered(context);
            float minVal = qts[0].Item2 * (1 - BestPercent);
            float valueSum = qts[0].Item2;
            for (int i = 1; i < qts.Count; i++)
            {
                if (qts[i].Item2 >= minVal)
                {
                    valueSum += qts[i].Item2;
                }
                else
                {
                    break;
                }
            }
            float randomNumber = Random.Range(0, valueSum);
            for (int i = 0; i < BestN; i++)
            {
                if (qts[i].Item2 >= randomNumber)
                {
                    return qts[i].Item1.actionName;
                }
            }
            return qts[0].Item1.actionName;
        }

        public string EvalRandom(Context context)
        {
            return qualifiers[Random.Range(0, qualifiers.Count)].actionName;
        }
    }
}