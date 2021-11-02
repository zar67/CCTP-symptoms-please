// -----------------------------------------------
// <copyright file="SelectorColumn.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using System;
using UnityEditor;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// This class adds a property and a selector to a column.
    /// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>.
    /// </summary>
    public class SelectorColumn : PropertyColumn
    {
        /// <summary>
        /// Selector to get a TableEntry from a SerializedProperty.
        /// </summary>
        public Func<SerializedProperty, TableEntry> Selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorColumn"/> class.
        /// </summary>
        /// <param name="selector">Selector for a serialized property.</param>
        /// <param name="propertyName">Name of the property column.</param>
        /// <param name="name">Name of the column to be displayed on the header.</param>
        /// <param name="width">Initial width of the column.</param>
        public SelectorColumn(Func<SerializedProperty, TableEntry> selector, string propertyName, string name, float width)
            : base(propertyName, name, width)
        {
            Selector = selector;
        }
    }
}