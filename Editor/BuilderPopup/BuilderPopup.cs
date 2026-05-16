using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public abstract class BuilderPopup<T> : PopupWindowContent
    {
        private const string UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/BuilderPopup/BuilderPopupUI.uxml";

        private Action<ElementInfo<T>> _onClose;

        private ListView _listView;

        private ToolbarSearchField _searchField;

        public virtual string Title { get => "Builder"; }

        public BuilderPopup(Action<ElementInfo<T>> onClose)
        {
            _onClose = onClose;
        }

        public sealed override VisualElement CreateGUI()
        {
            VisualElement root = new VisualElement();
            root.style.flexGrow = 1;

            root.style.width = 300;
            root.style.height = 400;

            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            uiTree.CloneTree(root);

            _listView = root.Q<ListView>("options-container");
            _listView.itemsSource = GetItemsSource();
            _listView.selectionChanged += OnSelectionChanged;
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.Rebuild();

            Label label = root.Q<Label>("header-label");
            label.text = Title;

            _searchField = root.Q<ToolbarSearchField>("options-search");
            _searchField.RegisterValueChangedCallback(OnFilterChanged);

            return root;
        }

        private void OnFilterChanged(ChangeEvent<string> evt)
        {
            if (string.IsNullOrEmpty(evt.newValue))
            {
                _listView.itemsSource = GetItemsSource();
            }
            else
            {
                _listView.itemsSource = GetFilteredItems();
            }
        }

        private void BindItem(VisualElement element, int index)
        {
            ElementInfo<T> elementInfo = (ElementInfo<T>)_listView.itemsSource[index];

            if (element is Label label)
            {
                label.text = elementInfo.DisplayName;
            }
        }

        private VisualElement MakeItem()
        {
            Label label = new Label();
            label.style.paddingLeft = 20;
            label.style.paddingRight = 20;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            return label;
        }

        private void OnSelectionChanged(IEnumerable<object> obj)
        {
            if (_listView.selectedItem != null && editorWindow != null) 
            {
                editorWindow.Close();
            }
        }

        public sealed override void OnClose()
        {

            if (_onClose != null && _listView.selectedItem != null)
            {
                _onClose.Invoke((ElementInfo<T>)_listView.selectedItem);
            }
        }

        protected abstract List<ElementInfo<T>> GetItemsSource();

        protected List<ElementInfo<T>> GetFilteredItems()
        {
            List<ElementInfo<T>> items = GetItemsSource();

            List<ElementInfo<T>> filtered = new List<ElementInfo<T>>();

            string filter = _searchField.value.ToLower();

            foreach (ElementInfo<T> item in items)
            {
                string lowerDisplayName = item.DisplayName.ToLower();

                if (lowerDisplayName.Contains(filter))
                {
                    filtered.Add(item);
                }
            }

            return filtered;
        }
    }
}
