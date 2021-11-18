// -----------------------------------------------
// <copyright file="GUITable.cs" company="Zoe Rowbotham">
// Copyright (c) Zoe Rowbotham. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SymptomsPlease.GUIEditorTable
{
    /// <summary>
    /// Main Class of the Table Plugin.
    /// This contains static functions to draw a table, from the most basic
    /// to the most customizable.
    /// </summary>
    public static class GUITable
    {
        /// <summary>
        /// Draw a table just from the collection's property.
        /// This will create columns for all the visible members in the elements' class,
        /// similar to what Unity would show in the classic vertical collection display, but as a table instead.
        /// </summary>
        /// <returns>The updated table state.</returns>
        /// <param name="collectionProperty">The serialized property of the collection.</param>
        /// <param name="tableState">The Table state.</param>
        public static GUITableState DrawTable(
            SerializedProperty collectionProperty,
            GUITableState tableState)
        {
            var properties = new List<string>();
            string firstElementPath = collectionProperty.propertyPath + ".Array.data[0]";
            foreach (SerializedProperty prop in collectionProperty.serializedObject.FindProperty(firstElementPath))
            {
                string subPropName = prop.propertyPath.Substring(firstElementPath.Length + 1);

                // Avoid drawing properties more than 1 level deep
                if (!subPropName.Contains("."))
                {
                    properties.Add(subPropName);
                }
            }

            return DrawTable(collectionProperty, properties, tableState);
        }

        /// <summary>
        /// Draw a table using just the paths of the properties to display.
        /// This will create columns automatically using the property name as title, and will create
        /// PropertyEntry instances for each element.
        /// </summary>
        /// <returns>The updated table state.</returns>
        /// <param name="collectionProperty">The serialized property of the collection.</param>
        /// <param name="properties">The paths (names) of the properties to display.</param>
        /// <param name="tableState">The Table state.</param>
        public static GUITableState DrawTable(
            SerializedProperty collectionProperty,
            List<string> properties,
            GUITableState tableState)
        {
            var columns = properties.Select(prop => new PropertyColumn(
                prop, ObjectNames.NicifyVariableName(prop), 100f)).ToList();

            return DrawTable(collectionProperty, columns, tableState);
        }

        /// <summary>
        /// Draw a table by defining the columns's settings and the path of the corresponding properties.
        /// This will automatically create Property Entries using these paths.
        /// </summary>
        /// <returns>The updated table state.</returns>
        /// <param name="collectionProperty">The serialized property of the collection.</param>
        /// <param name="propertyColumns">The Property columns, that contain the columns properties and the corresponding property path.</param>
        /// <param name="tableState">The Table state.</param>
        public static GUITableState DrawTable(
            SerializedProperty collectionProperty,
            List<PropertyColumn> propertyColumns,
            GUITableState tableState)
        {
            var rows = new List<List<TableEntry>>();

            for (int i = 0; i < collectionProperty.arraySize; i++)
            {
                var row = new List<TableEntry>();
                foreach (PropertyColumn col in propertyColumns)
                {
                    row.Add(new PropertyEntry(
                        collectionProperty.serializedObject,
                        string.Format("{0}.Array.data[{1}].{2}", collectionProperty.propertyPath, i, col.PropertyName)));
                }

                rows.Add(row);
            }

            return DrawTable(propertyColumns.Select((col) => (TableColumn)col).ToList(), rows, tableState);
        }

        /// <summary>
        /// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
        /// that takes a SerializedProperty and returns the TableEntry to put in the corresponding cell.
        /// </summary>
        /// <returns>The updated table state.</returns>
        /// <param name="collectionProperty">The serialized property of the collection.</param>
        /// <param name="columns">The Selector Columns.</param>
        /// <param name="tableState">The Table state.</param>
        public static GUITableState DrawTable(
            SerializedProperty collectionProperty,
            List<SelectorColumn> columns,
            GUITableState tableState)
        {
            var rows = new List<List<TableEntry>>();

            for (int i = 0; i < collectionProperty.arraySize; i++)
            {
                var row = new List<TableEntry>();
                foreach (SelectorColumn col in columns)
                {
                    row.Add(col.Selector.Invoke(collectionProperty.serializedObject.FindProperty(string.Format("{0}.Array.data[{1}].{2}", collectionProperty.propertyPath, i, col.PropertyName))));
                }

                rows.Add(row);
            }

            return DrawTable(columns.Select((col) => (TableColumn)col).ToList(), rows, tableState);
        }

        /// <summary>
        /// Draw a table completely manually.
        /// Each entry has to be created and given as parameter in entries.
        /// </summary>
        /// <returns>The updated table state.</returns>
        /// <param name="columns">The Columns of the table.</param>
        /// <param name="entries">The Entries as a list of rows.</param>
        /// <param name="tableState">The Table state.</param>
        public static GUITableState DrawTable(
            List<TableColumn> columns,
            List<List<TableEntry>> entries,
            GUITableState tableState)
        {
            if (tableState == null)
            {
                tableState = new GUITableState();
            }

            CheckTableState(tableState, columns);

            float rowHeight = EditorGUIUtility.singleLineHeight;

            EditorGUILayout.BeginHorizontal();
            tableState.ScrollPosHoriz = EditorGUILayout.BeginScrollView(tableState.ScrollPosHoriz);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(2f);
            float currentX = 0f;

            RightClickMenu(tableState, columns);

            for (int i = 0; i < columns.Count; i++)
            {
                TableColumn column = columns[i];
                if (!tableState.ColumnVisible[i])
                {
                    continue;
                }

                string columnName = column.Title;
                if (tableState.SortByColumnIndex == i)
                {
                    if (tableState.SortIncreasing)
                    {
                        columnName += " " + '\u25B2'.ToString();
                    }
                    else
                    {
                        columnName += " " + '\u25BC'.ToString();
                    }
                }

                ResizeColumn(tableState, i, currentX);

                GUI.enabled = column.EnabledTitle;

                if (GUILayout.Button(columnName, EditorStyles.miniButtonMid, GUILayout.Width(tableState.ColumnSizes[i] + 4), GUILayout.Height(EditorGUIUtility.singleLineHeight)) && column.IsSortable)
                {
                    if (tableState.SortByColumnIndex == i && tableState.SortIncreasing)
                    {
                        tableState.SortIncreasing = false;
                    }
                    else if (tableState.SortByColumnIndex == i && !tableState.SortIncreasing)
                    {
                        tableState.SortByColumnIndex = -1;
                    }
                    else
                    {
                        tableState.SortByColumnIndex = i;
                        tableState.SortIncreasing = true;
                    }
                }

                currentX += tableState.ColumnSizes[i] + 4f;
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            tableState.ScrollPos = EditorGUILayout.BeginScrollView(tableState.ScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

            List<List<TableEntry>> orderedRows = entries;
            if (tableState.SortByColumnIndex >= 0)
            {
                orderedRows = tableState.SortIncreasing
                    ? entries.OrderBy(row => row[tableState.SortByColumnIndex]).ToList()
                    : entries.OrderByDescending(row => row[tableState.SortByColumnIndex]).ToList();
            }

            foreach (List<TableEntry> row in orderedRows)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < row.Count; i++)
                {
                    if (i >= columns.Count)
                    {
                        Debug.LogWarning("The number of entries in this row is more than the number of columns");
                        continue;
                    }

                    if (!tableState.ColumnVisible[i])
                    {
                        continue;
                    }

                    TableColumn column = columns[i];
                    TableEntry property = row[i];
                    GUI.enabled = column.EnabledEntries;
                    property.DrawEntry(tableState.ColumnSizes[i], rowHeight);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUI.enabled = true;

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            tableState.Save();

            return tableState;
        }

        private static void RightClickMenu(GUITableState tableState, List<TableColumn> columns)
        {
            var rect = new Rect(0, 0, tableState.ColumnSizes.Where((_, i) => tableState.ColumnVisible[i]).Sum(s => s + 4), EditorGUIUtility.singleLineHeight);
            GUI.enabled = true;
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                var contextMenu = new GenericMenu();
                for (int i = 0; i < columns.Count; i++)
                {
                    TableColumn column = columns[i];
                    if (column.Optional)
                    {
                        int index = i;
                        contextMenu.AddItem(new GUIContent(column.Title), tableState.ColumnVisible[i], () => tableState.ColumnVisible[index] = !tableState.ColumnVisible[index]);
                    }
                }

                contextMenu.ShowAsContext();
            }
        }

        private static void ResizeColumn(GUITableState tableState, int indexColumn, float currentX)
        {
            int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
            var resizeRect = new Rect(currentX + tableState.ColumnSizes[indexColumn] + 2, 0, 10, EditorGUIUtility.singleLineHeight);
            EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal, controlId);
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                {
                    if (resizeRect.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlId;
                        Event.current.Use();
                    }

                    break;
                }

                case EventType.MouseDrag:
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        tableState.ColumnSizes[indexColumn] = Event.current.mousePosition.x - currentX - 5;
                        Event.current.Use();
                    }

                    break;
                }

                case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                    }

                    break;
                }
            }
        }

        private static void CheckTableState(GUITableState tableState, List<TableColumn> columns)
        {
            if (tableState.ColumnSizes == null || tableState.ColumnSizes.Count < columns.Count)
            {
                tableState.ColumnSizes = columns.Select((column) => column.Width).ToList();
            }

            if (tableState.ColumnVisible == null || tableState.ColumnVisible.Count < columns.Count)
            {
                tableState.ColumnVisible = columns.Select((column) => column.VisibleByDefault).ToList();
            }
        }
    }
}