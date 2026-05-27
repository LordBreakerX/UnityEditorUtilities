using UnityEditor;
using UnityEngine;

namespace LordBreakerX.EditorUtilities
{
    public class ElementBuilder<T>
    {
        public const string GROUP_ARROW = ">";

        private readonly Texture _elementIcon;

        private readonly string _elementGroup;
        private readonly string _displayName;

        private readonly T _value;

        public Texture ElementIcon { get => _elementIcon; }

        public string ElementGroup { get => _elementGroup; }
        public string DisplayName { get => _displayName; }

        public T Value { get => _value; }

        public ElementBuilder(string elementGroup, string displayName, T value)
        {
            _elementGroup = elementGroup;
            _displayName = displayName;
            _value = value;
        }

        public ElementBuilder(string displayName, T value) : this("Custom", displayName, value)
        {

        }

        public override string ToString()
        {
            return _displayName;
        }
    }
}
