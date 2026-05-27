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

        private Dictionary<string, ElementGroup<T>> _builderGroups = new Dictionary<string, ElementGroup<T>>();

        private BuilderListView<ElementBuilder<T>> _buildersListView;

        private BuilderListView<ElementGroup<T>> _groupsListView;

        private List<ElementGroup<T>> _groupsList = new List<ElementGroup<T>>();

        public virtual string Title { get => "Builder"; }

        public AdvancedBuilderPopup(Action<ElementBuilder<T>> onClose)
        {
            _onClose = onClose;

            foreach(ElementBuilder<T> builder in GetBuilders())
            {
                string groupName = builder.ElementGroup;

                if (!_builderGroups.ContainsKey(groupName))
                {
                    ElementGroup<T> group = new ElementGroup<T>(groupName);
                    _builderGroups.Add(groupName, group);
                    _groupsList.Add(group);
                }

                _builderGroups[groupName].AddBuilder(builder);
            }
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

            // view for viewing builders
            _buildersListView = new BuilderListView<ElementBuilder<T>>();
            _buildersListView.GetDisplayName = (builder) =>
            {
                return builder.DisplayName;
            };

            _buildersListView.GetTexture = (builder) =>
            {
                return builder.ElementIcon;
            };

            _buildersListView.style.display = DisplayStyle.None;

            _buildersListView.selectionChanged += OnElementSelected;

            // view for viewing groups
            _groupsListView = new BuilderListView<ElementGroup<T>>();
            _groupsListView.GetDisplayName = (group) =>
            {
                return group.GroupName;
            };

            _groupsListView.selectionChanged += OnGroupSelected;
            _groupsListView.SetElements(_groupsList);
            _groupsListView.SetDisplayArrow(true);

            Label label = root.Q<Label>("header-label");
            label.text = Title;

            //_searchField = root.Q<ToolbarSearchField>("options-search");
            //_searchField.RegisterValueChangedCallback(OnFilterChanged);

            root.Add(_buildersListView);
            root.Add(_groupsListView);

            return root;
        }

        private void OnElementSelected(IEnumerable<object> enumerable)
        {
            if (_buildersListView.selectedItem != null && editorWindow != null)
            {
                editorWindow.Close();
            }
        }

        private void OnGroupSelected(IEnumerable<object> enumerable)
        {
            if (_groupsListView.selectedItem is ElementGroup<T> group)
            {
                _groupsListView.style.display = DisplayStyle.None;
                _buildersListView.style.display = DisplayStyle.Flex;

                _buildersListView.SetElements(group.GetBuilders());
            }
        }

        public sealed override void OnClose()
        {

            if (_buildersListView.selectedItem != null)
            {
                ElementBuilder<T> builder = (ElementBuilder<T>)_buildersListView.selectedItem;
                _onClose?.Invoke(builder);
            }
        }
    }
}
