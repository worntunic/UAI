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
    }

    public enum NodeType { Scorer, Qualiscorer, Qualifier }
    public struct NodeWeightedLink
    {
        string otherNodeID;
        float weight;
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
        public AnimationCurve curve;
    }
    [System.Serializable]
    public class QualiScorerData
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
}

