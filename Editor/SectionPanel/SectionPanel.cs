using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    [UxmlElement]
    public partial class SectionPanel : VisualElement
    {
        private string _headerTitle;

        private const string UI_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/SectionPanel/SectionPanel.uxml";

        private Label _headerLabel;

        private VisualElement _contentContainer;

        private VisualElement _headerContentContainer;

        public override VisualElement contentContainer => _contentContainer;

        [UxmlAttribute("header-text")]
        public string HeaderTitle 
        {
            get => _headerTitle;
            set
            {
                _headerTitle = value;
                if (_headerLabel != null) _headerLabel.text = value;
            }
        }

        public SectionPanel()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            _headerLabel = this.Q<Label>("header-label");

            _contentContainer = this.Q<VisualElement>("content-container");
            _headerContentContainer = this.Q<VisualElement>("header-content-container");
        }

        public SectionPanel(string headerTitle) : this()
        {
            HeaderTitle = headerTitle;
        }

        public void AddHeaderContent(VisualElement elementToAdd)
        {
            _headerContentContainer.Add(elementToAdd);
        }
    }
}
