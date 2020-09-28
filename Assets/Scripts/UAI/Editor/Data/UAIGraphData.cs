using System.Collections;
using System.Collections.Generic;
using UAI.AI.SO;
using UnityEngine;
using static UAI.AI.QualiScorer;

namespace UAI.AI.Edit
{
    [CreateAssetMenu(fileName = "UAI", menuName = "UAI/New Utility AI", order = 2)]
    public class UAIGraphData : ScriptableObject
    {
        public ContextSO context;
        public List<ScorerData> scorers;
        public List<QualiScorerData> qualiScorers;
        public List<QualifierData> qualifiers;
        public SelectorData selectorData;
    }

    public enum NodeType { Scorer, Qualiscorer, Qualifier }
    [System.Serializable]
    public class NodeWeightedLink
    {
        public string otherNodeID;
        public float weight;
    }
    [System.Serializable]
    public class NodeData
    {
        public string guid;
        public Vector2 position;
        public NodeType nodeType;
    }
    [System.Serializable]
    public class ScorerData : NodeData
    {
        public string key;
        public AnimationCurve uFunction;
    }
    [System.Serializable]
    public class QualiScorerData : NodeData
    {
        public QualiType qualiType;
        public float threshold;
        public List<NodeWeightedLink> inLinks = new List<NodeWeightedLink>();
    }
    [System.Serializable]
    public class QualifierData : QualiScorerData
    {
        public string actionName;
    }
    [System.Serializable]
    public class SelectorData
    {
        public Selector.SelectorType selectorType;
        public float bestPercent;
        public int bestN;
    }
}

