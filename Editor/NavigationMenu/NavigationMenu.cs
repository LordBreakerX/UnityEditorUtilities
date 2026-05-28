using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class NavigationMenu : VisualElement
    {
        private VisualElement _customHeader;

        private VisualElement _customFooter;

        private VisualElement _panelsContainer;

        private NavigationPanel _selectorPanel;

        private NavigationPanel _extraPanel;

        private NavigationPanel _currentPanel;

        private List<NavigationPanel> _panels = new List<NavigationPanel>();

        private string _headerText;

        public NavigationPanel ExtraPanel { get { return _extraPanel; } }

        public NavigationMenu(string header)
        {
            _headerText = header;

            _customHeader = new VisualElement();
            _customFooter = new VisualElement();

            _panelsContainer = new VisualElement();
            _panelsContainer.style.flexGrow = 1;

            this.Add(_customHeader);
            this.Add(_panelsContainer);
            this.Add(_customFooter);

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
            NavigationPanel selector = new NavigationPanel(_headerText);
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

        public void AddHeader(VisualElement headerContent)
        {
            _customHeader.Clear();
            _customHeader.Add(headerContent);
        }

        public void AddFooter(VisualElement footerContent)
        {
            _customFooter.Clear();
            _customFooter.Add(footerContent);
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

        public void AddExtraPanel(NavigationPanel panel)
        {
            _extraPanel = panel;
            _extraPanel.style.display = DisplayStyle.None;

            _panelsContainer.Insert(1, _extraPanel);

            _selectorPanel.Q<ListView>().RefreshItems();
        }

        public void RemovePanel(NavigationPanel panel)
        {
            if (panel == _selectorPanel || panel == _extraPanel) return;

            if (_panelsContainer.Contains(panel))
                _panelsContainer.Remove(panel);

            if (_panels.Contains(panel))
                _panels.Remove(panel);

            _selectorPanel.Q<ListView>().RefreshItems();
        }

        public void RemovePanels()
        {
            foreach (NavigationPanel panel in _panels)
            {
                _panelsContainer.Remove(panel);
            }

            _panels.Clear();
        }

        public void RemoveExtraPanel()
        {
            if (_panelsContainer.Contains(_extraPanel))
            {
                _panelsContainer.Remove(_extraPanel);
            }

            _extraPanel = null;
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
            if (_currentPanel != null)
            {
                _currentPanel.style.display = DisplayStyle.None;
            }

            if (currentPanel != null)
                _currentPanel = currentPanel;
            else
                _currentPanel = _selectorPanel;

            _currentPanel.style.display = DisplayStyle.Flex;
        }
    }
}
