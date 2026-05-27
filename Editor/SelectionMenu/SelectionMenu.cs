using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class SelectionMenu : VisualElement
    {
        private VisualElement _panelsContainer;

        private SelectionPanel _selectorPanel;

        private SelectionPanel _currentPanel;

        private List<SelectionPanel> _panels = new List<SelectionPanel>();

        private string _header;

        public SelectionMenu(string header)
        {
            _header = header;
            _panelsContainer = new VisualElement();
            _panelsContainer.style.flexGrow = 1;

            this.Add(_panelsContainer);

            foreach(VisualElement element in _panelsContainer.Children())
            {
                if (element is SelectionPanel panel)
                {
                    panel.style.display = DisplayStyle.None;
                    _panels.Add(panel);
                }
            }

            CreateSelectorPanel();
        }

        public SelectionMenu() : this("Selector")
        {

        }

        private void CreateSelectorPanel()
        {
            SelectionPanel selector = new SelectionPanel();
            selector.HeaderText = _header;
            selector.name = "selector-container";

            ListView panelsView = new ListView();
            panelsView.style.flexGrow = 1;
            panelsView.name = "selector-view";

            ChangePanel(selector);

            panelsView.makeItem = () =>
            {
                VisualElement itemRoot = new VisualElement();
                itemRoot.style.flexGrow = 1;
                itemRoot.style.flexDirection = FlexDirection.Row;

                Label headerText = new Label() { name = "header-text" };
                headerText.style.flexGrow = 1;
                headerText.style.paddingLeft = 20;
                headerText.style.unityTextAlign = TextAnchor.MiddleLeft;

                itemRoot.Add(headerText);

                Label arrowText = new Label() { text = ">" };
                arrowText.style.paddingRight = 10;
                arrowText.style.unityFontStyleAndWeight = FontStyle.Bold;

                itemRoot.Add(arrowText);

                return itemRoot;
            };

            panelsView.bindItem = (element, index) =>
            {
                Label label = element.Q<Label>("header-text");
                label.text = _panels[index].HeaderText;
            };

            panelsView.selectionChanged += OnSelectionChanged;

            panelsView.itemsSource = _panels;

            selector.Add(panelsView);

            panelsView.Rebuild();

            _selectorPanel = selector;

            _panelsContainer.Insert(0, selector);
        }

        private void OnSelectionChanged(IEnumerable<object> enumerable)
        {
            var listView = _selectorPanel.Q<ListView>();

            Debug.Log(listView.selectedItem);

            if (listView.selectedItem is SelectionPanel panel)
            {
                ChangePanel(panel);
            }
        }

        public void AddPanel(SelectionPanel panel)
        {
            panel.SetBackArrow(true);
            panel.SetBackManipulator(OnBackClicked);

            panel.style.display = DisplayStyle.None;
            _panelsContainer.Add(panel);
            _panels.Add(panel);

            _selectorPanel.Q<ListView>().RefreshItems();
        }

        private void OnBackClicked()
        {
            ChangePanel(_selectorPanel);

            var listView = _selectorPanel.Q<ListView>();
            listView.ClearSelection();
        }

        public void ChangePanel(SelectionPanel currentPanel)
        {
            if (currentPanel == null) return;

            if (_currentPanel != null)
            {
                _currentPanel.style.display = DisplayStyle.None;
            }

            _currentPanel = currentPanel;
            _currentPanel.style.display = DisplayStyle.Flex;
        }
    }
}
