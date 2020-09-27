using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UAI.AI.Edit
{
    public class BaseNode : Node
    {
        private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public string guid;
        protected System.Type[] _allowedInPorts;
        protected System.Type[] _allowedOutPorts;
        public System.Type[] AllowedInPorts { get { return _allowedInPorts; } }
        public System.Type[] AllowedOutPorts { get { return _allowedOutPorts; } }

        private void Initialize(Vector2 position, string guid)
        {
            this.guid = guid;
            styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            AddToClassList("node");
            Rect positionRect = new Rect(position, defaultNodeSize);
            this.SetPosition(positionRect);
            SetAllowedPorts();
        }
        public BaseNode(Vector2 position, string guid)
        {
            Initialize(position, guid);
        }

        protected void AddPort(string name, Orientation orientation, Direction direction, Port.Capacity capacity, System.Type type = null)
        {
            var port = this.InstantiatePort(orientation, direction, capacity, type);
            port.portName = name;
            if (direction == Direction.Input)
            {
                this.inputContainer.Add(port);
            } else
            {
                this.outputContainer.Add(port);
            }
            Refresh();
        }
        public void Refresh()
        {
            RefreshExpandedState();
            RefreshPorts();
        }
        protected virtual void SetAllowedPorts()
        {
            _allowedInPorts = new System.Type[0];
            _allowedOutPorts = new System.Type[0];
        }
    }
    public class ScorerNode : BaseNode
    {
        public ScorerNode(Vector2 position, string guid) : base(position, guid) {
            title = "Scorer";
            AddPort("Output", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(QualiScorerNode));
        }
        protected override void SetAllowedPorts()
        {
            _allowedInPorts = new System.Type[0];
            _allowedOutPorts = new System.Type[2] { typeof(QualiScorerNode), typeof(QualifierNode) };
        }
    }
    public class QualiScorerNode : BaseNode
    {
        public QualiScorerNode(Vector2 position, string guid) : base(position, guid)
        {
            title = "QualiScorer";
            AddPort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            AddPort("Output", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
        }
        protected override void SetAllowedPorts()
        {
            _allowedInPorts = new System.Type[2] { typeof(ScorerNode), typeof(QualiScorerNode) };
            _allowedOutPorts = new System.Type[2] { typeof(QualiScorerNode), typeof(QualifierNode) };
        }
    }
    public class QualifierNode : BaseNode
    {
        public QualifierNode(Vector2 position, string guid) : base(position, guid)
        {
            title = "Qualifier";
            AddPort("Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

        }
        protected override void SetAllowedPorts()
        {
            _allowedInPorts = new System.Type[2] { typeof(ScorerNode), typeof(QualiScorerNode) };
        }
    }

}
