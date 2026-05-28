using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class NavigationPanel : VisualElement
    {
        private const string UI_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/NavigationMenu/UI.uxml";

        public const string BACK_ARROW = "<";
        public const string FORWARD_ARROW = ">";

        private Label _headerLabel;
        private Label _backArrow;

        private VisualElement _panelsContainer;

        public override VisualElement contentContainer => _panelsContainer;

        public string HeaderText { get => _headerLabel.text; set => _headerLabel.text = value; }

        public NavigationPanel(string header)
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            _headerLabel = this.Q<Label>("header");

            _headerLabel.text = header;

            _backArrow = this.Q<Label>("back-arrow");

            _backArrow.style.display = DisplayStyle.None;

            _panelsContainer = this.Q<VisualElement>("subpanels-container");
        }

        public NavigationPanel() : this("Example Header")
        {

        }

        public void SetHeader(string header)
        {
            _headerLabel.text = header;
        }

        internal void SetBackManipulator(Action onBackClicked)
        {
            _headerLabel.AddManipulator(new Clickable(onBackClicked));
        }

        internal void SetBackArrow(bool hasBackArrow)
        {
            if (hasBackArrow)
            {
                _backArrow.style.display = DisplayStyle.Flex;
            }
            else
            {
                _backArrow.style.display = DisplayStyle.None;
            }
        }
    }
}
