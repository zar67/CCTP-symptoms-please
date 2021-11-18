using SymptomsPlease.Utilities.ExtensionMethods;
using System;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.Utilities.Editor
{
    public class ExtendedEditorWindow : EditorWindow
    {
        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;

        private string m_selectedPropertyPath;
        protected SerializedProperty selectedProperty;
        protected int selectedPropertyIndex;

        private Vector2 m_sideBarScrollPosition;

        protected void DrawProperties(SerializedProperty property, bool drawChildren)
        {
            string lastPropertyPath = string.Empty;
            foreach (SerializedProperty prop in property)
            {
                if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (prop.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(prop, drawChildren);
                        EditorGUI.indentLevel--;
                    }

                    lastPropertyPath = prop.propertyPath;
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropertyPath) && prop.propertyPath.Contains(lastPropertyPath))
                    {
                        continue;
                    }

                    lastPropertyPath = prop.propertyPath;
                    EditorGUILayout.PropertyField(prop, drawChildren);
                }
            }
        }

        protected void DrawProperty(SerializedProperty property, bool drawChildren)
        {
            EditorGUILayout.PropertyField(property, drawChildren);
        }

        protected void DrawSidebar(SerializedProperty property)
        {
            if (GUILayout.Button("Add New"))
            {
                property.arraySize++;
                m_selectedPropertyPath = property.GetArrayElementAtIndex(property.arraySize - 1).propertyPath;
                selectedPropertyIndex = property.arraySize - 1;
            }

            m_sideBarScrollPosition = EditorGUILayout.BeginScrollView(m_sideBarScrollPosition);
            int count = 0;
            foreach (SerializedProperty prop in property)
            {
                var style = EditorStyles.toolbarButton;
                style.wordWrap = true;
                if (GUILayout.Button(prop.displayName, style))
                {
                    m_selectedPropertyPath = prop.propertyPath;
                    selectedPropertyIndex = count;
                    EditorGUIUtility.editingTextField = false;
                }
                count++;
            }
            EditorGUILayout.EndScrollView();

            if (!string.IsNullOrEmpty(m_selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(m_selectedPropertyPath);
            }
        }

        protected void DrawSidebarWithFilter(SerializedProperty property, string filter)
        {
            if (GUILayout.Button("Add New"))
            {
                property.arraySize++;
                m_selectedPropertyPath = property.GetArrayElementAtIndex(property.arraySize - 1).propertyPath;
                selectedPropertyIndex = property.arraySize - 1;
            }

            m_sideBarScrollPosition = EditorGUILayout.BeginScrollView(m_sideBarScrollPosition);
            int count = 0;
            foreach (SerializedProperty prop in property)
            {
                if (!string.IsNullOrEmpty(filter) && !prop.displayName.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var style = EditorStyles.toolbarButton;
                style.wordWrap = true;
                if (GUILayout.Button(prop.displayName, style))
                {
                    m_selectedPropertyPath = prop.propertyPath;
                    selectedPropertyIndex = count;
                    EditorGUIUtility.editingTextField = false;
                }

                count++;
            }
            EditorGUILayout.EndScrollView();

            if (!string.IsNullOrEmpty(m_selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(m_selectedPropertyPath);
            }
        }

        protected void DrawField(string propName, bool relative, bool drawChildren)
        {
            if (relative && serializedProperty != null)
            {
                EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative(propName), drawChildren);
            }
            else if (serializedObject != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), drawChildren);
            }
        }

        protected bool ApplyModifiedProperties()
        {
            return serializedObject.ApplyModifiedProperties();
        }
    }
}