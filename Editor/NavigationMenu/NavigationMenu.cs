using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class NavigationMenu : VisualElement
    {
        private VisualElement _panelsContainer;

        private NavigationPanel _selectorPanel;

        private NavigationPanel _extraPanel;

        private NavigationPanel _currentPanel;

        private List<NavigationPanel> _panels = new List<NavigationPanel>();

        private string _header;

        public NavigationMenu(string header)
        {
            _header = header;
            _panelsContainer = new VisualElement();
            _panelsContainer.style.flexGrow = 1;

            this.Add(_panelsContainer);

            foreach(VisualElement element in _panelsContainer.Children())
            {
                if (element is NavigationPanel panel)
                {
                    panel.style.display = DisplayStyle.None;
                    _panels.Add(panel);
                }
            }

            CreateSelectorPanel();
        }

        public NavigationMenu() : this("Selector")
        {

        }

        private void CreateSelectorPanel()
        {
            NavigationPanel selector = new NavigationPanel();
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

            if (listView.selectedItem is NavigationPanel panel)
            {
                ChangePanel(panel);
            }
        }

        public void AddPanel(NavigationPanel panel)
        {
            panel.SetBackArrow(true);
            panel.SetBackManipulator(OnBackClicked);

            panel.style.display = DisplayStyle.None;
            _panelsContainer.Add(panel);
            _panels.Add(panel);

            _selectorPanel.Q<ListView>().RefreshItems();
        }

        public void AddExtraPanel(string panelTitle, NavigationPanel panel)
        {
            _extraPanel = panel;
            _extraPanel.style.display = DisplayStyle.None;

            _extraPanel.HeaderText = panelTitle;
            _panelsContainer.Insert(1, _extraPanel);
        }

        public void ChangeToExtraPanel()
        {
            if (_extraPanel != null)
            {
                ChangePanel(_extraPanel);
            }
        }

        private void OnBackClicked()
        {
            ChangePanel(_selectorPanel);
        }

        public void ChangePanel(NavigationPanel currentPanel)
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
