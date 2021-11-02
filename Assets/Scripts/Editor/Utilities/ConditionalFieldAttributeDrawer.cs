using SymptomsPlease.Debugging.Logging;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.Utilities.Attributes
{
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var conditionalAttribute = attribute as ConditionalFieldAttribute;
            SerializedProperty comparedProperty = property.serializedObject.FindProperty(conditionalAttribute.FieldToCheck);

            return Show(conditionalAttribute, comparedProperty) ? base.GetPropertyHeight(property, label) : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var conditionalAttribute = attribute as ConditionalFieldAttribute;
            SerializedProperty comparedProperty = property.serializedObject.FindProperty(conditionalAttribute.FieldToCheck);

            if (comparedProperty == null)
            {
                CustomLogger.Error(LoggingChannels.Utilities, $"ConditionalField cannot find property with name: {conditionalAttribute.FieldToCheck}");
                return;
            }

            if (Show(conditionalAttribute, comparedProperty))
            {
                EditorGUI.PropertyField(position, property, true);
            }
        }

        private bool Show(ConditionalFieldAttribute attribute, SerializedProperty targetProperty)
        {
            if (attribute.ANDOperator)
            {
                foreach (object value in attribute.CompareValues)
                {
                    if (!ValueMatches(targetProperty, value))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (object value in attribute.CompareValues)
                {
                    if (ValueMatches(targetProperty, value))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool ValueMatches(SerializedProperty targetProperty, object value)
        {
            switch (targetProperty.type)
            {
                case "bool":
                    return targetProperty.boolValue.Equals(value);
                case "Enum":
                    return targetProperty.enumValueIndex.Equals((int)value);
                default:
                    CustomLogger.Error(LoggingChannels.Utilities, $"ConditionalField does not support {targetProperty.type}");
                    return true;
            }
        }
    }
}