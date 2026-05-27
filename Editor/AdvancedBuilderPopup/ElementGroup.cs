using System.Collections.Generic;

namespace LordBreakerX.EditorUtilities
{
    public class ElementGroup<T>
    {
        private string _groupName;

        private List<ElementBuilder<T>> _builders = new List<ElementBuilder<T>>();

        public string GroupName { get { return _groupName; } }

        public ElementGroup(string groupName)
        {
            _groupName = groupName;
        }

        public void AddBuilder(ElementBuilder<T> builder)
        {
            if (builder != null)
            {
                _builders.Add(builder);
            }
        }

        public List<ElementBuilder<T>> GetBuilders()
        {
            return new List<ElementBuilder<T>>(_builders);
        }

        public List<ElementBuilder<T>> GetFilteredBuilders(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return new List<ElementBuilder<T>>(_builders);
            }

            List<ElementBuilder<T>> filteredBuilders = new List<ElementBuilder<T>>();

            foreach (ElementBuilder<T> builder in _builders)
            {
                if (builder.DisplayName.Contains(filter))
                {
                    filteredBuilders.Add(builder);
                }
            }

            return filteredBuilders;
        }

        public bool IsGroup(string groupName)
        {
            return groupName == _groupName;
        }
    }
}
