// -----------------------------------------------
// <copyright file="TableColumn.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// Base class for all table columns.
    /// It only takes a title and a width in the constructor, but other settings are available to customize the column.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Title of the column to display in the header.
        /// </summary>
        public string Title;

        /// <summary>
        /// Width of the column.
        /// </summary>
        public float Width;

        /// <summary>
        /// Defines if the entries are enabled (interactable) or disabled (grayed out). Default: true.
        /// </summary>
        public bool EnabledEntries = true;

        /// <summary>
        /// Defines if the column is sortable.
        /// </summary>
        public bool IsSortable = true;

        /// <summary>
        /// Defines if the title is enabled (interactable) or disabled (grayed out). Default: true.
        /// </summary>
        public bool EnabledTitle = true;

        /// <summary>
        /// Defines if the column can be hidden by right-clicking the column titles bar. Default: false.
        /// </summary>
        public bool Optional = false;

        /// <summary>
        /// Defines if the column is visible by default. If this is false, and optional is false too. The column can never be viewed. Default: true.
        /// </summary>
        public bool VisibleByDefault = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// Initializes a column with its title and width.
        /// Edit the other public properties for more settings.
        /// </summary>
        /// <param name="title">The column title.</param>
        /// <param name="width">The column width.</param>
        public TableColumn(string title, float width)
        {
            Title = title;
            Width = width;
        }
    }
}