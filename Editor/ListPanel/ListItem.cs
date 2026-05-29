using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class ListItem<T> : VisualElement
    {
        private const string ITEM_UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/ListPanel/ListItemUI.uxml";

        private Image _headerIcon;

        private TextField _renameField;

        private Label _elementLabel;

        private T _data;

        protected ListPanel<T> ParentPanel { get; private set; }

        protected ListView ParentListView { get; private set; }

        public ListItem(ListPanel<T> parentPanel, ListView parentListView)
        {
            VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ITEM_UXML_PATH);
            treeAsset.CloneTree(this);

            VisualElement container = this.Q<VisualElement>("list-item-container");
            container.style.flexDirection = FlexDirection.Row;

            _headerIcon = this.Q<Image>("header-icon");
            _headerIcon.style.display = DisplayStyle.None;
            _headerIcon.scaleMode = ScaleMode.ScaleToFit;
            _headerIcon.width = 32;
            _headerIcon.style.flexShrink = 1;
            _headerIcon.style.paddingRight = 10;

            _renameField = this.Q<TextField>("rename-field");

            _elementLabel = this.Q<Label>("element-label");
            _elementLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

            ParentListView = parentListView;
            ParentPanel = parentPanel;

            this.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));
        }

        public void SetIcon(Texture icon)
        {
            _headerIcon.image = icon;
            
            if (icon == null)
            {
                _headerIcon.style.display = DisplayStyle.None;
            }
            else
            {
                _headerIcon.style.display = DisplayStyle.Flex;
            }
        }

        private void CreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.ClearItems();

            evt.menu.AppendAction($"Rename {ParentPanel.ItemDisplayName}", (action) =>
            {
                StartRename();
            });

            evt.menu.AppendAction($"Delete {ParentPanel.ItemDisplayName}", (action) => 
            {
                if (_data != null)
                {
                    ParentListView.itemsSource.Remove(_data);
                    ParentListView.Rebuild();
                }
            });

            evt.menu.AppendAction($"Duplicate {ParentPanel.ItemDisplayName}", (action) => 
            {
                if (_data != null)
                {
                    T copiedData = ParentPanel.CopyItem(_data);
                    ParentListView.itemsSource.Add(copiedData);

                    ParentListView.Rebuild();
                }
            });

            evt.menu.AppendSeparator();
        }

        public void SetName(string name)
        {
            _renameField.value = name;
            _elementLabel.text = name;
        } 

        public void SetData(T data)
        {
            _data = data;
        }

        public void RegisterEvents()
        {
            _renameField.RegisterCallback<BlurEvent>(OnFinishedRename);
        }

        public void UnregisterEvents()
        {
            _renameField.UnregisterCallback<BlurEvent>(OnFinishedRename);
        }

        private void OnFinishedRename(BlurEvent evt)
        {
            _elementLabel.text = _renameField.value;

            if (_data != null)
            {
                int index = ParentListView.itemsSource.IndexOf(_data);

                ParentPanel.SetItemName(ParentListView.itemsSource, index, _renameField.value);
            }

            ParentListView.Rebuild();
        }

        public void StartRename()
        {
            _renameField.style.display = DisplayStyle.Flex;
            _elementLabel.style.display = DisplayStyle.None;

            _renameField.schedule.Execute(() =>
            {
                _renameField.Focus();
                _renameField.SelectAll();
            });
        }
        
        public void StopRename()
        {
            _renameField.style.display = DisplayStyle.None;
            _elementLabel.style.display = DisplayStyle.Flex;
        }
    }
}
