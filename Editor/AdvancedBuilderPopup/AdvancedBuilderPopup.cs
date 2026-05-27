using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public abstract class AdvancedBuilderPopup<T> : PopupWindowContent
    {
        private const string UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/BuilderPopup/BuilderPopupUI.uxml";

        private Action<ElementBuilder<T>> _onClose;

        private ListView _listView;

        private ToolbarSearchField _searchField;

        private List<ElementBuilder<T>> _builders;

        private List<ElementBuilder<T>> _filteredBuilders;

        public virtual string Title { get => "Builder"; }

        public AdvancedBuilderPopup(Action<ElementBuilder<T>> onClose)
        {
            _onClose = onClose;

            _builders = GetBuilders();

            _filteredBuilders = new List<ElementBuilder<T>>();
            _filteredBuilders.AddRange(_builders);
        }

        protected abstract List<ElementBuilder<T>> GetBuilders();

        public sealed override VisualElement CreateGUI()
        {
            VisualElement root = new VisualElement();
            root.style.flexGrow = 1;
            root.style.width = 300;
            root.style.height = 400;

            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            uiTree.CloneTree(root);

            _listView = root.Q<ListView>("options-container");
            _listView.itemsSource = _filteredBuilders;
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
            _filteredBuilders.Clear();

            if (string.IsNullOrEmpty(evt.newValue))
            {
                _filteredBuilders.AddRange(_builders);
            }
            else
            {
                UpdateFilteredBuilders();
            }

            _listView.RefreshItems();
        }

        private void UpdateFilteredBuilders()
        {
            foreach (ElementBuilder<T> item in _builders)
            {
                Debug.Log($" {item.DisplayName} == {_searchField.value} | {item.DisplayName.Contains(_searchField.value)}");

                if (item.DisplayName.Contains(_searchField.value))
                {
                    _filteredBuilders.Add(item);
                }
            }
        }

        private void BindItem(VisualElement element, int index)
        {
            ElementBuilder<T> elementInfo = (ElementBuilder<T>)_listView.itemsSource[index];

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
                _onClose.Invoke((ElementBuilder<T>)_listView.selectedItem);
            }
        }
    }
}
