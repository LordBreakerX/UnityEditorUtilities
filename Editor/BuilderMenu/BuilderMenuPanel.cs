using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class BuilderMenuPanel<TValue> : NavigationPanel
    {
        private List<Builder<TValue>> _builders;

        private ListView _buildersView;

        private Action<Builder<TValue>> _onBuilderSelected;

        public List<Builder<TValue>> Builders { get { return _builders; } }

        public Builder<TValue> SelectedBuilder { get; private set; }

        public BuilderMenuPanel(string header) : base(header)
        {
            _builders = new List<Builder<TValue>>();
            _buildersView = new ListView(_builders, -1, MakeItem, BindItem);
            _buildersView.selectionChanged += OnSelectionChanged;
            Add(_buildersView);
        }

        private VisualElement MakeItem()
        {
            VisualElement root = new VisualElement();
            root.style.unityTextAlign = TextAnchor.MiddleLeft;
            root.style.flexDirection = FlexDirection.Row;

            Image icon = new Image() { name = "display-icon" };

            Label header = new Label() { name = "display-name" };
            header.style.unityTextAlign = TextAnchor.MiddleLeft;
            header.style.flexGrow = 1;

            root.Add(icon);
            root.Add(header);

            return root;
        }

        private void BindItem(VisualElement element, int intex)
        {
            Builder<TValue> builder = _builders[intex];

            Label displayNameLabel = element.Q<Label>("display-name");
            Image displayIconImage = element.Q<Image>("display-icon");

            displayNameLabel.text = builder.Name;

            if (builder.Icon == null)
            {
                displayIconImage.style.display = DisplayStyle.None;
                displayNameLabel.style.paddingLeft = 32;
            }
            else
            {
                displayIconImage.image = builder.Icon;
                displayNameLabel.style.paddingLeft = 0;
                displayIconImage.style.display = DisplayStyle.Flex;
            }
        }

        private void OnSelectionChanged(IEnumerable<object> enumerable)
        {
            SelectedBuilder = (Builder<TValue>)_buildersView.selectedItem;

            if (SelectedBuilder != null && _onBuilderSelected != null)
            {
                _onBuilderSelected.Invoke(SelectedBuilder);
            }
        }

        public void AddBuilder(Builder<TValue> builder) 
        {
            _builders.Add(builder);
            _buildersView.RefreshItems();
        }

        public void SetBuilders(List<Builder<TValue>> builders)
        {
            _builders = builders;
            _buildersView.itemsSource = _builders;
            _buildersView.Rebuild();
        }

        public void RemoveBuilders()
        {
            _builders.Clear();
            _buildersView.Rebuild();
        }

        public void SetBuilderSelectedCallback(Action<Builder<TValue>> onBuilderSelected)
        {
            _onBuilderSelected = onBuilderSelected;
        }
    }
}
