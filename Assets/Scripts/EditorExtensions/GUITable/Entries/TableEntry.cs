// -----------------------------------------------
// <copyright file="TableEntry.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// Base class for all table entries.
    /// DrawEntry needs to be overriden to draw the entry for the cell.
    /// Use this to customize the table however needed.
    /// CompareTo can be overriden to customize the sorting.
    /// comparingValue is used as a fallback for sorting any types of entries, even different types.
    /// </summary>
    public abstract class TableEntry : System.IComparable
    {
        public abstract void DrawEntry(float width, float height);

        public abstract string ComparingValue
        {
            get;
        }

        public virtual int CompareTo(object other)
        {
            var otherEntry = (TableEntry)other;
            return otherEntry == null ? 1 : ComparingValue.CompareTo(otherEntry.ComparingValue);
        }
    }
}