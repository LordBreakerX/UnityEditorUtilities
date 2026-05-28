using System;
using UnityEngine;

namespace LordBreakerX.EditorUtilities
{
    public class Builder<TValue>
    {
        public Texture Icon { get; private set; }
        public string Group { get; private set; }
        public string Name { get; private set; }

        public TValue Value { get; private set; }

        public Builder(Texture icon, string group, string name, TValue value)
        {
            Icon = icon;
            Group = group;
            Name = name;
            Value = value;
        }

        public Builder(Texture icon, string name, TValue value)
        {
            Icon = icon;
            Group = "Custom";
            Name = name;
            Value = value;
        }

        public Builder(string group, string name, TValue value)
        {
            Icon = null;
            Group = group;
            Name = name;
            Value = value;
        }

        public Builder(string name, TValue value)
        {
            Icon = null;
            Group = "Custom";
            Name = name;
            Value = value;
        }
    }
}
