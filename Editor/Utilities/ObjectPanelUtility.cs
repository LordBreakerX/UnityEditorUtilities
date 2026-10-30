using UnityEngine;

namespace LordBreakerX.EditorUtilities
{
    public static class ObjectPanelUtility
    {
        public static void SetDefaultObject<T>(this ObjectPanel<T> objectPanel, bool enableUI = false) where T : ScriptableObject
        {
            T scriptable = ScriptableObject.CreateInstance<T>();
            objectPanel.SetObject(scriptable);
            objectPanel.SetEnabled(enableUI);
        }

        public static void SetDefaultObject<T>(this ObjectPanel objectPanel, bool enableUI = false) where T : ScriptableObject
        {
            T scriptable = ScriptableObject.CreateInstance<T>();
            objectPanel.SetObject(scriptable);
            objectPanel.SetEnabled(enableUI);
        }
    }
}
