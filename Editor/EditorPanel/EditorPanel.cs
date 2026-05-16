using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class EditorPanel : VisualElement
    {
        private const string UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/EditorPanel/EditorPanelUI.uxml";

        private VisualElement _contentContainer;

        private Label _headerLabel;

        private VisualElement _headerContainer;

        public override VisualElement contentContainer => _contentContainer;

        public EditorPanel()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            uiTree.CloneTree(this);

            _headerLabel = this.Q<Label>("header-label");

            _contentContainer = this.Q<VisualElement>("content-container");
            _headerContainer = this.Q<VisualElement>("header-container");
        }

        public EditorPanel(string headerTitle, int minWidth = 200, int minHeight = 200) : this()
        {
            style.minWidth = minWidth;
            style.minHeight = minHeight;

            _headerLabel.text = headerTitle;
        }

        public Foldout CreateDecoratedFoldout(string title)
        {
            Foldout decoratedFoldout = new Foldout();
            decoratedFoldout.text = title;
            decoratedFoldout.AddToClassList("decorated-foldout");
            Add(decoratedFoldout);

            VisualElement spaceing = new VisualElement();
            spaceing.style.minHeight = 15;
            spaceing.style.minWidth = 15;
            Add(spaceing);

            return decoratedFoldout;
        }

        public Foldout CreateDecoratedFoldout(string title, Action createAction)
        {
            Foldout decoratedFoldout = CreateDecoratedFoldout(title);

            Toggle toggle = decoratedFoldout.Q<Toggle>();

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            toggle.Add(spacer);

            Button createButton = new Button();
            createButton.name = "create-button";
            createButton.clicked += createAction;
            createButton.text = "+";
            createButton.AddToClassList("add-button");

            toggle.Add(createButton);

            return decoratedFoldout;
        }

        public void AddHeaderContent(VisualElement content)
        {
            _headerContainer.Add(content);
        }
    }
}
