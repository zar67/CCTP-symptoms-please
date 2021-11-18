// -----------------------------------------------
// <copyright file="ActionEntry.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// This entry class draws a button which, when clicked, will trigger the
    /// action given in the constructor.
    /// </summary>
    public class ActionEntry : TableEntry
    {
        private string m_name;
        private System.Action m_action;

        /// <summary>
        /// Gets the value to compare entries for sorting.
        /// </summary>
        public override string ComparingValue => m_name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionEntry"/> class.
        /// </summary>
        /// <param name="name">Name of the action to be displayed on the button.</param>
        /// <param name="action">Action to be invoked when the entry button is pressed.</param>
        public ActionEntry(string name, System.Action action)
        {
            m_name = name;
            m_action = action;
        }

        /// <summary>
        /// Draws the entry in the GUI.
        /// </summary>
        /// <param name="width">Width of the entry.</param>
        /// <param name="height">Height of the entry.</param>
        public override void DrawEntry(float width, float height)
        {
            if (GUILayout.Button(m_name, GUILayout.Width(width), GUILayout.Height(height)))
            {
                if (m_action != null)
                {
                    m_action.Invoke();
                }
            }
        }
    }
}