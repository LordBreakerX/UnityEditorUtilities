using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public abstract class ListPanel<T> : VisualElement
    {
        private const string LIST_UXML_PATH = "Packages/com.lordbreakerx.editorutilities/Editor/ListPanel/ListPanelUI.uxml";

        private ListView _listView;

        private Button _addButton;

        private Label _headerTitle;

        public virtual string ItemDisplayName { get => "Element"; }

        public ListPanel()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(LIST_UXML_PATH);
            uiTree.CloneTree(this);

            style.flexGrow = 1;

            _listView = this.Q<ListView>("list-container"); 
            _listView.reorderable = true;
            _listView.reorderMode = ListViewReorderMode.Animated;
            _listView.selectionType = SelectionType.Single;

            _listView.makeItem = MakeItem;
            _listView.destroyItem = DestroyItem;
            _listView.bindItem = BindItem;

            _listView.selectionChanged += (obj) => 
            {
                if (_listView.itemsSource != null)
                {
                    if (_listView.selectedItem is T item)
                    {
                        OnSelectionChanged(item);
                    }
                }
            };

            _listView.AddManipulator(CreateContextualManipulator());

            SetItemsSource(new List<T>());
            _listView.Rebuild();

            _headerTitle = this.Q<Label>("header-label");

            _addButton = this.Q<Button>("header-button");
            _addButton.clicked += () => 
            {
                if (_listView.itemsSource != null)
                {
                    T item = CreateItem();
                    _listView.itemsSource.Add(item);
                    _listView.Rebuild();
                }
            };
        }

        private ContextualMenuManipulator CreateContextualManipulator()
        {
            return new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction($"Create {ItemDisplayName}", (action) => 
                {
                    if (_listView.itemsSource != null)
                    {
                        T item = CreateItem();
                        _listView.itemsSource.Add(item);
                        _listView.Rebuild();
                    }
                });
            });
        }

        protected abstract T CreateItem();

        protected abstract void OnSelectionChanged(T selectedItem);

        private VisualElement MakeItem()
        {
            ListItem<T> item = new ListItem<T>(this, _listView);

            item.RegisterEvents();

            return item;
        }

        private void DestroyItem(VisualElement element)
        {
            if (element is ListItem<T> item)
            {
                item.UnregisterEvents();
            }
        }

        private void BindItem(VisualElement element, int index)
        {
            if (element is ListItem<T> item)
            {
                T data = (T)_listView.itemsSource[index];

                string name = GetItemName(data);

                item.SetName(name);
                item.SetData(data);

                if (name == "")
                {
                    item.SetName(GetDefaultName(index));

                    item.StartRename();
                }
                else
                {
                    item.StopRename();
                }
            }

        }

        public void SetHeaderTitle(string headerTitle)
        {
            _headerTitle.text = headerTitle;
        }

        private string GetDefaultName(int index)
        {
            return $"Element {index}";
        }

        protected virtual string GetItemName(T item)
        {
            return item.ToString();
        }

        public virtual void SetItemName(IList itemsSource, int index, string name)
        {

        }

        public void SetItemsSource(List<T> items)
        {
            _listView.itemsSource = items;

            _listView.SetSelection(-1);

            if (_listView.itemsSource.Count > 0)
                _listView.SetSelection(0);

            _listView.Rebuild();
        }

        public abstract T CopyItem(T data);
    }
}
