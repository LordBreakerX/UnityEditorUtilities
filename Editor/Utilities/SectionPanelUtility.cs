using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public static class SectionPanelUtility
    {
        public static Foldout CreateFoldout(this SectionPanel panel, string title)
        {
            Foldout foldout = new Foldout() { text = title };

            foldout.AddToClassList("darker-foldout");
            panel.Add(foldout);

            return foldout;
        }

        public static Foldout CreateActionFoldout(this SectionPanel panel, string title, System.Action action)
        {
            Foldout foldout = panel.CreateFoldout(title);

            return AddActionButton(foldout, action);
        }

        public static Foldout AddActionButton(this Foldout foldout, System.Action action, string buttonText = "+")
        {
            Toggle toggle = foldout.Q<Toggle>();

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            toggle.Add(spacer);

            Button createButton = new Button(action) { text = buttonText };

            createButton.AddToClassList("add-button");

            toggle.Add(createButton);

            return foldout;
        }
    }
}
