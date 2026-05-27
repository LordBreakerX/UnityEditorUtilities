using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class BuilderListView<T> : ListView
    {
        private const string ARROW = ">";

        private bool _isArrowDisplayed = false;

        public List<T>  BuilderElements { get => (List<T>)itemsSource; }

        public Func<T, Texture> GetTexture { private get; set; }
        public Func<T, string> GetDisplayName { private get; set; }

        public BuilderListView() : base()
        {
            itemsSource = new List<T>();

            GetTexture = (element) => { return null; };
            GetDisplayName = (element) => { return element.GetType().Name; };

            makeItem = MakeItem;
            bindItem = BindItem;
            Rebuild();
        }

        public void SetDisplayArrow(bool isArrowDisplayed)
        {
            _isArrowDisplayed = true;
            Rebuild();
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

            Label arrow = new Label() { name = "display-arrow", text = ARROW };
            arrow.style.unityTextAlign = TextAnchor.MiddleRight;
            arrow.style.paddingRight = 10;

            if (_isArrowDisplayed)
                arrow.style.display = DisplayStyle.Flex;
            else
                arrow.style.display = DisplayStyle.None;

            root.Add(icon);
            root.Add(header);
            root.Add(arrow);

            return root;
        }

        private void BindItem(VisualElement element, int index)
        {
            T builderElement = BuilderElements[index];

            Label displayNameLabel = element.Q<Label>("display-name");
            Image displayIconImage = element.Q<Image>("display-icon");

            if (displayNameLabel != null)
            {
                displayNameLabel.text = GetDisplayName.Invoke(builderElement);
            }

            if (displayIconImage != null)
            {
                displayIconImage.image = GetTexture.Invoke(builderElement);

                Debug.Log(displayIconImage.image);

                if (displayIconImage.image == null)
                    displayIconImage.style.display = DisplayStyle.None;
                else
                    displayIconImage.style.display = DisplayStyle.Flex;
            }
        }

        public void SetElements(params T[] elements)
        {
            BuilderElements.Clear();
            BuilderElements.AddRange(elements);
            Rebuild();
        }

        public void SetElements(List<T> elements)
        {
            BuilderElements.Clear();
            BuilderElements.AddRange(elements);
            Rebuild();
        }
    }
}
