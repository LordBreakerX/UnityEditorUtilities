using UnityEngine;

namespace LordBreakerX.EditorUtilities
{
    public class ElementInfo<T>
    {
        private string _displayName;
        private T _value;

        public string DisplayName { get => _displayName; }
        public T Value { get => _value; }

        public ElementInfo(string displayName, T value)
        {
            _displayName = displayName;
            _value = value;
        }

        public override string ToString()
        {
            return _displayName;
        }
    }
}
