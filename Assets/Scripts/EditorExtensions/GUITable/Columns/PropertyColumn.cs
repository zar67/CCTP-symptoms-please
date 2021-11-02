// -----------------------------------------------
// <copyright file="PropertyColumn.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// Internal Use Only.
    /// This class adds a property to a column.
    /// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>.
    /// </summary>
    public class PropertyColumn : TableColumn
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyColumn"/> class.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="name">Name of entry column.</param>
        /// <param name="width">Initial width of column.</param>
        public PropertyColumn(string propertyName, string name, float width)
            : base(name, width)
        {
            PropertyName = propertyName;
        }
    }
}