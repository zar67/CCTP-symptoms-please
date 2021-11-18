
using System;

namespace SymptomsPlease.Common.Stats
{
    public enum StatModificationType
    {
        FLAT = 100,
        PERCENTAGE_ADDITIVE = 200,
        PERCENTAGE_MULTIPLY = 300,
    }

    [Serializable]
    public class StatModifier
    {
        public float Value;
        public StatModificationType Type;
        public int Priority;
        public object Source;

        public StatModifier(StatModificationType type, float value, int priority, object source)
        {
            Type = type;
            Value = value;
            Priority = priority;
            Source = source;
        }

        public StatModifier(StatModificationType type, float value) : this(type, value, (int)type, null) { }
        public StatModifier(StatModificationType type, float value, int priority) : this(type, value, priority, null) { }
        public StatModifier(StatModificationType type, float value, object source) : this(type, value, (int)type, source) { }
    }
}
