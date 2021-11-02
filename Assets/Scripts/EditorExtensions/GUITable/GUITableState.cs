// -----------------------------------------------
// <copyright file="GUITableState.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// The current state of the GUITable.
    /// This has to be used the same way state parameters are used in Unity GUI functions, like
    /// the scroll position in BeginScrollView.
    /// It has to be passed from one GUI frame to another by keeping a reference in your calling code.
    /// </summary>
    /// <example>
    /// <code>
    /// GUITableState tableState;
    /// void OnGUI ()
    /// {
    ///     tableState = GUITable.DrawTable(collectionProperty, tableState);
    /// }
    /// </code>
    /// </example>
    public class GUITableState
    {
        public Vector2 ScrollPos;

        public Vector2 ScrollPosHoriz;

        public int SortByColumnIndex = -1;

        public bool SortIncreasing;

        public List<float> ColumnSizes = new List<float>();

        public List<bool> ColumnVisible = new List<bool>();

        private string m_prefsKey;

        public GUITableState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUITableState"/> class.
        /// Initializes a <see cref="GUIExtensions.GUITableState"/> with a key to save it in EditorPrefs.
        /// This constructor can't be used in ScriptableObject's constructor or in the property's declaration,
        /// because it uses the EditorPrefs. Use it in OnEnable or Awake instead.
        /// </summary>
        /// <param name="prefsKey">Prefs key.</param>
        public GUITableState(string prefsKey)
        {
            this.m_prefsKey = prefsKey;
            GUITableState loadedState = Load(prefsKey);
            ScrollPos = loadedState.ScrollPos;
            ScrollPosHoriz = loadedState.ScrollPosHoriz;
            SortByColumnIndex = loadedState.SortByColumnIndex;
            SortIncreasing = loadedState.SortIncreasing;
            ColumnSizes = loadedState.ColumnSizes;
            ColumnVisible = loadedState.ColumnVisible;
        }

        public static GUITableState Load(string prefsKey)
        {
            return EditorPrefs.HasKey(prefsKey)
                ? JsonUtility.FromJson<GUITableState>(EditorPrefs.GetString(prefsKey, ""))
                : new GUITableState();
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(m_prefsKey))
            {
                EditorPrefs.SetString(m_prefsKey, JsonUtility.ToJson(this));
            }
        }
    }
}