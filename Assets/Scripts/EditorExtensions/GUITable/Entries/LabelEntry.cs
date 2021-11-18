// -----------------------------------------------
// <copyright file="LabelEntry.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// This entry class draws a string as a label.
    /// This is useful for properties you want to display in the table
    /// as read only, as the default PropertyField used in PropertyEntry uses editable fields.
    /// </summary>
    public class LabelEntry : TableEntry
    {
        private string m_value;

        /// <summary>
        /// Gets the value to compare entries for sorting.
        /// </summary>
        public override string ComparingValue => m_value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelEntry"/> class.
        /// </summary>
        /// <param name="value">Text to display in the entry.</param>
        public LabelEntry(string value)
        {
            m_value = value;
        }

        /// <summary>
        /// Draws the entry in the GUI.
        /// </summary>
        /// <param name="width">Width of the entry.</param>
        /// <param name="height">Height of the entry.</param>
        public override void DrawEntry(float width, float height)
        {
            EditorGUILayout.LabelField(m_value, GUILayout.Width(width), GUILayout.Height(height));
        }
    }
}