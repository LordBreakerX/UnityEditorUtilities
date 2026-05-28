using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public abstract class BuilderMenu<TValue> : PopupWindowContent
    {
        private Action<Builder<TValue>> _onClose;

        private NavigationMenu _menu;

        private Dictionary<string, BuilderMenuPanel<TValue>> _builderGroups = new Dictionary<string, BuilderMenuPanel<TValue>>();

        private Builder<TValue> _selectedBuilder;

        protected abstract string BuilderTitle { get; }

        public BuilderMenu(Action<Builder<TValue>> onClose)
        {
            _onClose = onClose;

            _menu = new NavigationMenu(BuilderTitle);

            _menu.style.flexGrow = 1;
            _menu.style.width = 300;
            _menu.style.height = 400;

            RefreshMenu();
        }

        public void RefreshMenu()
        {
            _menu.RemovePanels();

            foreach (Builder<TValue> builder in GetBuilders())
            {
                string group = builder.Group;
                BuilderMenuPanel<TValue> groupPanel;

                if (_builderGroups.ContainsKey(group))
                {
                    groupPanel = _builderGroups[group];
                }
                else
                {
                    groupPanel = new BuilderMenuPanel<TValue>(group);
                    groupPanel.SetBuilderSelectedCallback(OnBuilderSelected);

                    _builderGroups[group] = groupPanel;
                    _menu.AddPanel(groupPanel);
                }

                groupPanel.AddBuilder(builder);
            }
        }

        private void OnBuilderSelected(Builder<TValue> builder)
        {
            if (builder != null && editorWindow != null)
            {
                _selectedBuilder = builder;
                editorWindow.Close();
            }
        }

        protected abstract IEnumerable<Builder<TValue>> GetBuilders();

        public override VisualElement CreateGUI()
        {
            return _menu;
        }

        public override void OnClose()
        {
            _onClose?.Invoke(_selectedBuilder);
        }
    }
}
