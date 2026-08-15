using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public abstract class ListPanelV2<T> : VisualElement where T : class
    {
        private const string LIST_UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/ListPanelV2/ListPanelUI.uxml";

        private ListView _listView;

        private Button _addButton;

        private Label _headerLabel;

        public string Title { get => _headerLabel.text; set => _headerLabel.text = value;  }

        public ListPanelV2(string title)
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(LIST_UXML_PATH);
            uiTree.CloneTree(this);

            style.flexGrow = 1;

            _listView = this.Q<ListView>("list-container");
            _addButton = this.Q<Button>("header-button");
            _headerLabel = this.Q<Label>("header-label");
            
            // setup listview
            _listView.reorderable = true;
            _listView.reorderMode = ListViewReorderMode.Animated;
            _listView.selectionType = SelectionType.Single;
            _listView.makeItem = CreateElementGUI;
            _listView.destroyItem = DestroyElementGUI;
            _listView.bindItem = BindElementGUI;
            _listView.unbindItem = UnbindElementGUI;
            _listView.selectionChanged += (_) => { 
                if (_listView.selectedItem is T element)
                {
                    OnElementSelected(element);
                }
            };

            SetItemsSource(new List<T>());

            _listView.AddManipulator(new ContextualMenuManipulator(CreateContextualMenu));

            // setup header
            Title = title;

            // setup button

            _addButton.clicked += () =>
            {
                if (_listView.itemsSource != null)
                {
                    T item = CreateDefaultElement();

                    if (item != null)
                    {
                        AddElement(item);
                    }
                }
            };
        }

        protected abstract T CreateDefaultElement();

        protected virtual void OnElementSelected(T element)
        {
            
        }

        private void CreateContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Create Element", (action) =>
            {
                if (_listView.itemsSource != null)
                {
                    T item = CreateDefaultElement();
                    AddElement(item);
                }
            });
        }

        protected virtual VisualElement CreateElementGUI()
        {
            Label label = new Label();

            label.AddManipulator(new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Delete Element", (action) =>
                {
                    if (label.userData is int index && _listView.itemsSource[index] is T element)
                    {
                        RemoveElement(element);
                    }
                });

                evt.menu.AppendAction("Duplicate Element", (action) =>
                {
                    if (label.userData is int index && _listView.itemsSource[index] is T element)
                    {
                        T elementCopy = CopyElement(element);
                        AddElement(elementCopy);
                    }
                });
            }));

            return label;
        }

        protected virtual void DestroyElementGUI(VisualElement elementGUI)
        {
            
        }

        protected virtual void BindElementGUI(VisualElement elementGUI, int index)
        {
            if (_listView.itemsSource[index] is T element)
            {
                Label label = elementGUI.Q<Label>();
                label.userData = index;
                label.text = GetElementName(element, index);
            }
        }

        protected virtual string GetElementName(T element, int index)
        {
            return $"{index} Element";
        }

        protected virtual void UnbindElementGUI(VisualElement elementGUI, int index)
        {
            
        }

        public abstract T CopyElement(T toCopy);

        public virtual void AddElement(T item)
        {
            if (item != null)
            {
                _listView.itemsSource.Add(item);
                _listView.Rebuild();
            }
        }

        public virtual void RemoveElement(T item)
        {
            if (item != null)
            {
                _listView.itemsSource.Remove(item);
                _listView.Rebuild();
            }
        }

        public void SetItemsSource(List<T> items)
        {
            _listView.itemsSource = items;

            _listView.SetSelection(-1);

            if (_listView.itemsSource.Count > 0)
                _listView.SetSelection(0);

            _listView.Rebuild();
        }
    }
}
