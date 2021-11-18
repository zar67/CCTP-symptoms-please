// -----------------------------------------------
// <copyright file="PropertyEntry.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// This entry class just uses EditorGUILayout.PropertyField to draw a given property.
    /// This is the basic way to use GUITable. It will draw the properties the same way Unity would by default.
    /// </summary>
    public class PropertyEntry : TableEntry
    {
        private SerializedProperty m_serializedProperty;
        private SerializedObject m_serializedObject;
        private string m_propertyPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEntry"/> class.
        /// </summary>
        /// <param name="property">Serialized property to display in the entry.</param>
        public PropertyEntry(SerializedProperty property)
        {
            m_serializedProperty = property;
            m_serializedObject = property.serializedObject;
            m_propertyPath = property.propertyPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEntry"/> class.
        /// </summary>
        /// <param name="so">Serialized object that contains the serialized property.</param>
        /// <param name="propertyPath">Path of the serialized property.</param>
        public PropertyEntry(SerializedObject so, string propertyPath)
        {
            m_serializedObject = so;
            m_propertyPath = propertyPath;
            m_serializedProperty = so.FindProperty(propertyPath);
        }

        /// <summary>
        /// Draws the entry in the GUI table.
        /// </summary>
        /// <param name="width">Width of the entry.</param>
        /// <param name="height">Height of the entry.</param>
        public override void DrawEntry(float width, float height)
        {
            if (m_serializedProperty != null)
            {
                EditorGUILayout.PropertyField(m_serializedProperty, GUIContent.none, GUILayout.Width(width), GUILayout.Height(height));
                m_serializedObject.ApplyModifiedProperties();
            }
            else
            {
                Debug.LogWarningFormat("Property not found: {0} -> {1}", m_serializedObject.targetObject.name, m_propertyPath);
                GUILayout.Space(width + 4f);
            }
        }

        /// <summary>
        /// Gets the value of the property entry to compare for sorting entries.
        /// </summary>
        public override string ComparingValue
        {
            get
            {
                if (m_serializedProperty != null)
                {
                    switch (m_serializedProperty.propertyType)
                    {
                        case SerializedPropertyType.String:
                        case SerializedPropertyType.Character:
                            return m_serializedProperty.stringValue.ToString();
                        case SerializedPropertyType.Float:
                            return m_serializedProperty.doubleValue.ToString();
                        case SerializedPropertyType.Integer:
                        case SerializedPropertyType.LayerMask:
                        case SerializedPropertyType.ArraySize:
                            return m_serializedProperty.intValue.ToString();
                        case SerializedPropertyType.Enum:
                            return m_serializedProperty.enumValueIndex.ToString();
                        case SerializedPropertyType.Boolean:
                            return m_serializedProperty.boolValue.ToString();
                        case SerializedPropertyType.ObjectReference:
                            return m_serializedProperty.objectReferenceValue.name.ToString();
                        case SerializedPropertyType.ExposedReference:
                            return m_serializedProperty.exposedReferenceValue.name.ToString();
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Compare to other table entry to determine sorting.
        /// </summary>
        /// <param name="other">Other object to compare to.</param>
        /// <returns>
        /// <para>Less than zero: this instance precedes other in the sort order.</para>
        /// <para>Zero: this instance occurs in the same position in the sort order as other.</para>
        /// <para>Greater than zero: This instance follows other in the sort order.</para>
        /// </returns>
        public override int CompareTo(object other)
        {
            if (!(other is TableEntry))
            {
                throw new ArgumentException("Object is not a GUITableEntry");
            }

            var otherPropEntry = (PropertyEntry)other;
            if (otherPropEntry == null)
            {
                return base.CompareTo(other);
            }

            SerializedProperty otherSp = otherPropEntry.m_serializedProperty;

            if (m_serializedProperty.propertyType != otherSp.propertyType)
            {
                return base.CompareTo(other);
            }

            if (m_serializedProperty != null)
            {
                switch (m_serializedProperty.propertyType)
                {
                    case SerializedPropertyType.String:
                    case SerializedPropertyType.Character:
                        return m_serializedProperty.stringValue.CompareTo(otherSp.stringValue);
                    case SerializedPropertyType.Float:
                        return m_serializedProperty.doubleValue.CompareTo(otherSp.doubleValue);
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.LayerMask:
                    case SerializedPropertyType.ArraySize:
                        return m_serializedProperty.intValue.CompareTo(otherSp.intValue);
                    case SerializedPropertyType.Enum:
                        return m_serializedProperty.enumValueIndex.CompareTo(otherSp.enumValueIndex);
                    case SerializedPropertyType.Boolean:
                        return m_serializedProperty.boolValue.CompareTo(otherSp.boolValue);
                    case SerializedPropertyType.ObjectReference:
                        return m_serializedProperty.objectReferenceValue.name.CompareTo(otherSp.objectReferenceValue.name);
                    case SerializedPropertyType.ExposedReference:
                        return m_serializedProperty.exposedReferenceValue.name.CompareTo(otherSp.exposedReferenceValue.name);
                }
            }

            return 0;
        }
    }
}