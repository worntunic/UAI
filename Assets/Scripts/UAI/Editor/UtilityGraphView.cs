using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using System.Linq;

namespace UAI.AI.Edit {
    public class UtilityGraphView : GraphView
    {
        public UtilityGraphView()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("UAIGraphStylesheet"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        public void CreateScorerNode()
        {
            ScorerNode node = new ScorerNode(Vector2.zero, Guid.NewGuid().ToString());
            this.AddElement(node);
        }
        public void CreateQualiScorerNode()
        {
            QualiScorerNode node = new QualiScorerNode(Vector2.zero, Guid.NewGuid().ToString());
            this.AddElement(node);
        }
        public void CreateQualifierNode()
        {
            QualifierNode node = new QualifierNode(Vector2.zero, Guid.NewGuid().ToString());
            this.AddElement(node);
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            System.Type[] allowedPortTypes;
            if (startPort.direction == Direction.Input)
            {
                allowedPortTypes = ((BaseNode)startPort.node).AllowedOutPorts;
            } else
            {
                allowedPortTypes = ((BaseNode)startPort.node).AllowedInPorts;
            }
            ports.ForEach((port) => {
                if (startPort.node != port.node && startPort.direction != port.direction)
                {
                    for (int i = 0; i < allowedPortTypes.Length; i++)
                    {
                        if (port.node.GetType() == allowedPortTypes[i])
                        {
                            compatiblePorts.Add(port);
                            break;
                        }
                    }
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }
    }
}