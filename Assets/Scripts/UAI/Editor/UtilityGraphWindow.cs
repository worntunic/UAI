using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UAI.AI.Edit
{
    public class UtilityGraphWindow : GraphViewEditorWindow
    {
        private UtilityGraphView graphView;

        public static void OpenEditorWindow(UAIGraphData graphData)
        {
            UtilityGraphWindow window = GetWindow<UtilityGraphWindow>();
            window.titleContent = new GUIContent("Utility AI Editor");

        }
        private void GenerateGraphView()
        {
            graphView = new UtilityGraphView
            {
                name = "Utility AI Graph"
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            toolbar.styleSheets.Add(Resources.Load<StyleSheet>("UAIGraphWindowStylesheet"));

            /*var createScorerBtn = new Button(OnCreateScorerClicked);
            createScorerBtn.text = "Create Scorer";
            toolbar.Add(createScorerBtn);

            var createQualiScorerBtn = new Button(OnCreateQualiScorerClicked);
            createQualiScorerBtn.text = "Create QualiScorer";
            toolbar.Add(createQualiScorerBtn);

            var createQualifierBtn = new Button(OnCreateQualifierClicked);
            createQualifierBtn.text = "Create Qualifier";
            toolbar.Add(createQualifierBtn);*/

            rootVisualElement.Add(toolbar);
        }
        //Events
        private void OnCreateScorerClicked()
        {
            graphView.CreateScorerNode();
        }
        private void OnCreateQualiScorerClicked()
        {
            graphView.CreateQualiScorerNode();
        }
        private void OnCreateQualifierClicked()
        {
            graphView.CreateQualifierNode();
        }
        private void OnEnable()
        {
            GenerateGraphView();
            GenerateToolbar();
            
        }
        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}
