// -----------------------------------------------
// <copyright file="TableDrawer.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// Drawer for the Table Attribute.
    /// See the TableAttribute class documentation for the limitations of this attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(TableAttribute))]
    public class TableDrawer : PropertyDrawer
    {
        private GUITableState m_tableState;
        private Rect m_lastRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check that it is a collection
            Match match = Regex.Match(property.propertyPath, "^([a-zA-Z0-9_]*).Array.data\\[([0-9]*)\\]$");
            if (!match.Success)
            {
                EditorGUI.LabelField(position, label.text, "Use the Table attribute with a collection.");
                return;
            }

            string collectionPath = match.Groups[1].Value;

            // Check that it's the first element
            string index = match.Groups[2].Value;

            if (index != "0")
            {
                return;
            }

            if (GUILayoutUtility.GetLastRect().width > 1f)
            {
                m_lastRect = GUILayoutUtility.GetLastRect();
            }

            var r = new Rect(m_lastRect.x + 15f, m_lastRect.y + 35f, m_lastRect.width, m_lastRect.height);
            GUILayout.BeginArea(r);
            EditorGUI.indentLevel = 0;
            m_tableState = GUITable.DrawTable(property.serializedObject.FindProperty(collectionPath), m_tableState);
            GUILayout.EndArea();
            GUILayout.Space(30f);
        }
    }
}