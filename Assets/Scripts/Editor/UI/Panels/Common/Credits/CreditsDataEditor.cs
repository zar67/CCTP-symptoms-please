using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    [CustomEditor(typeof(CreditsData))]
    public class CreditsDataEditor : Editor
    {
        private ReorderableList m_creditsList;

        private CreditsData m_creditsData;
        private SerializedProperty m_data;

        private int m_selectedIndex = -1;

        private readonly Dictionary<string, ReorderableList> m_titlesListDictionary = new Dictionary<string, ReorderableList>();
        private readonly Dictionary<string, ReorderableList> m_headingsListDictionary = new Dictionary<string, ReorderableList>();
        private readonly Dictionary<string, ReorderableList> m_designationsListDictionary = new Dictionary<string, ReorderableList>();
        
        private void OnEnable()
        {
            m_creditsData = (CreditsData)target;
            m_data = serializedObject.FindProperty(nameof(m_creditsData.Data));

            m_creditsList = new ReorderableList(serializedObject, m_data, true, true, true, true);
            m_creditsList.drawHeaderCallback += DrawTitlesHeader;
            m_creditsList.drawElementCallback += DrawTitleElement;

            m_creditsList.onAddCallback += (titlesList) =>
            {
                SerializedProperty addedElement;
                Title addedTitle;
                // if something is selected add after that element otherwise on the end
                if (m_selectedIndex >= 0)
                {
                    titlesList.serializedProperty.InsertArrayElementAtIndex(m_selectedIndex + 1);
                    addedElement = titlesList.serializedProperty.GetArrayElementAtIndex(m_selectedIndex + 1);
                    addedTitle = m_creditsData.Data[m_selectedIndex + 1];
                }
                else if (titlesList.serializedProperty.arraySize == 0)
                {
                    titlesList.serializedProperty.InsertArrayElementAtIndex(0);
                    addedElement = titlesList.serializedProperty.GetArrayElementAtIndex(0);
                }
                else
                {
                    titlesList.serializedProperty.arraySize++;
                    addedElement = titlesList.serializedProperty.GetArrayElementAtIndex(titlesList.serializedProperty.arraySize - 1);
                    addedTitle = m_creditsData.Data[titlesList.serializedProperty.arraySize - 1];
                }

                SerializedProperty name = addedElement.FindPropertyRelative(nameof(Title.DisplayName));
                SerializedProperty foldout = addedElement.FindPropertyRelative(nameof(Title.Foldout));
                SerializedProperty dialogues = addedElement.FindPropertyRelative(nameof(Title.Headings));

                name.stringValue = "";
                foldout.boolValue = true;
                dialogues.arraySize = 0;
            };

            m_creditsList.elementHeightCallback += (index) => GetTitleHeight(m_data.GetArrayElementAtIndex(index));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if (m_creditsList.serializedProperty.arraySize - 1 < m_selectedIndex)
            {
                m_selectedIndex = -1;
            }

            m_creditsList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTitlesHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Titles");
        }

        private void DrawHeadingsHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Headings");
        }

        private void DrawDesignationsHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Designations");
        }

        private void DrawNamesHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Names");
        }

        private void DrawTitleElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (isActive)
            {
                m_selectedIndex = index;
            }

            Title creditObject = m_creditsData.Data[index];
            SerializedProperty credit = m_data.GetArrayElementAtIndex(index);
            var position = new Rect(rect);

            SerializedProperty name = credit.FindPropertyRelative(nameof(creditObject.DisplayName));
            SerializedProperty foldout = credit.FindPropertyRelative(nameof(creditObject.Foldout));
            SerializedProperty headings = credit.FindPropertyRelative(nameof(creditObject.Headings));
            string headingsListKey = credit.propertyPath;

            EditorGUI.indentLevel++;

            foldout.boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? "" : name.stringValue);

            if (foldout.boolValue)
            {
                name.stringValue = EditorGUI.TextField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), name.stringValue);
                position.y += EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

                if (!m_titlesListDictionary.ContainsKey(headingsListKey))
                {
                    var headingsReorderableList = new ReorderableList(credit.serializedObject, headings, true, true, true, true);
                    headingsReorderableList.drawHeaderCallback += DrawHeadingsHeader;

                    headingsReorderableList.drawElementCallback += (titleRect, titleIndex, titleActive, titleFocused) => DrawHeadingElement(m_titlesListDictionary[headingsListKey], titleRect, titleIndex, titleActive, titleFocused);

                    headingsReorderableList.elementHeightCallback += (headingIndex) => GetHeadingHeight(m_titlesListDictionary[headingsListKey].serializedProperty.GetArrayElementAtIndex(headingIndex));

                    headingsReorderableList.onAddCallback += (headingList) =>
                    {
                        headingList.serializedProperty.arraySize++;
                        SerializedProperty addedElement = headingList.serializedProperty.GetArrayElementAtIndex(headingList.serializedProperty.arraySize - 1);

                        SerializedProperty newDialoguesName = addedElement.FindPropertyRelative(nameof(Heading.DisplayName));
                        SerializedProperty newDialoguesFoldout = addedElement.FindPropertyRelative(nameof(Heading.Foldout));
                        SerializedProperty sentences = addedElement.FindPropertyRelative(nameof(Heading.Designations));

                        newDialoguesName.stringValue = "";
                        newDialoguesFoldout.boolValue = true;
                        sentences.arraySize = 0;
                    };

                    m_titlesListDictionary[headingsListKey] = headingsReorderableList;
                }

                m_titlesListDictionary[headingsListKey].DoList(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight));
            }

            EditorGUI.indentLevel--;
        }

        private void DrawHeadingElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
        {
            if (list == null)
            {
                return;
            }

            SerializedProperty heading = list.serializedProperty.GetArrayElementAtIndex(index);
            var position = new Rect(rect);

            SerializedProperty foldout = heading.FindPropertyRelative(nameof(Heading.Foldout));
            SerializedProperty name = heading.FindPropertyRelative(nameof(Heading.DisplayName));

            foldout.boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? "" : name.stringValue);

            string headingsListKey = heading.propertyPath;
            SerializedProperty designations = heading.FindPropertyRelative(nameof(Heading.Designations));

            if (foldout.boolValue)
            {
                name.stringValue = EditorGUI.TextField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), name.stringValue);
                position.y += EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

                if (!m_headingsListDictionary.ContainsKey(headingsListKey))
                {
                    var designationsReorderableList = new ReorderableList(designations.serializedObject, designations, true, true, true, true);
                    designationsReorderableList.drawHeaderCallback += DrawDesignationsHeader;

                    designationsReorderableList.drawElementCallback += (headingRext, headingIndex, headingActive, headingFocused) =>
                    DrawDesignationElement(m_headingsListDictionary[headingsListKey], headingRext, headingIndex, headingActive, headingFocused);

                    designationsReorderableList.elementHeightCallback += (designationIndex) => GetDesignationHeight(m_headingsListDictionary[headingsListKey].serializedProperty.GetArrayElementAtIndex(designationIndex));

                    designationsReorderableList.onAddCallback += (designationsList) =>
                    {
                        designationsList.serializedProperty.arraySize++;
                        SerializedProperty addedElement = designationsList.serializedProperty.GetArrayElementAtIndex(designationsList.serializedProperty.arraySize - 1);
                    };

                    m_headingsListDictionary[headingsListKey] = designationsReorderableList;
                }

                m_headingsListDictionary[headingsListKey].DoList(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight));
            }
        }

        private void DrawDesignationElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
        {
            if (list == null)
            {
                return;
            }

            SerializedProperty designation = list.serializedProperty.GetArrayElementAtIndex(index);
            var position = new Rect(rect);

            SerializedProperty foldout = designation.FindPropertyRelative(nameof(Designation.Foldout));
            SerializedProperty name = designation.FindPropertyRelative(nameof(Designation.DisplayName));

            foldout.boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? "" : name.stringValue);

            string designationsListKey = designation.propertyPath;
            SerializedProperty names = designation.FindPropertyRelative(nameof(Designation.Names));

            if (foldout.boolValue)
            {
                name.stringValue = EditorGUI.TextField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), name.stringValue);
                position.y += EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

                if (!m_designationsListDictionary.ContainsKey(designationsListKey))
                {
                    var namesReorderableList = new ReorderableList(names.serializedObject, names, true, true, true, true);
                    namesReorderableList.drawHeaderCallback += DrawNamesHeader;

                    namesReorderableList.drawElementCallback += (namesRect, namesIndex, namesActive, namesFocused) =>
                    {
                        SerializedProperty designationName = names.GetArrayElementAtIndex(namesIndex);
                        namesRect.height = EditorGUIUtility.singleLineHeight;
                        designationName.stringValue = EditorGUI.TextField(namesRect, designationName.stringValue);
                    };

                    namesReorderableList.elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    namesReorderableList.onAddCallback += (sentList) =>
                    {
                        sentList.serializedProperty.arraySize++;
                        SerializedProperty addedElement = sentList.serializedProperty.GetArrayElementAtIndex(sentList.serializedProperty.arraySize - 1);

                        addedElement.stringValue = "";
                    };

                    m_designationsListDictionary[designationsListKey] = namesReorderableList;
                }

                m_designationsListDictionary[designationsListKey].DoList(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight));
            }
        }

        private float GetTitleHeight(SerializedProperty title)
        {
            SerializedProperty foldout = title.FindPropertyRelative(nameof(Title.Foldout));
            float height = EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

            if (foldout.boolValue)
            {
                height += EditorGUIUtility.singleLineHeight * 3;
                SerializedProperty headings = title.FindPropertyRelative(nameof(Title.Headings));

                for (int i = 0; i < headings.arraySize; i++)
                {
                    SerializedProperty heading = headings.GetArrayElementAtIndex(i);
                    height += GetHeadingHeight(heading);
                }

                if (headings.arraySize == 0)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }
            }

            return height;
        }

        private float GetHeadingHeight(SerializedProperty heading)
        {
            SerializedProperty foldout = heading.FindPropertyRelative(nameof(Heading.Foldout));
            float height = EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

            if (foldout.boolValue)
            {
                height += EditorGUIUtility.singleLineHeight * 3;

                SerializedProperty designations = heading.FindPropertyRelative(nameof(Heading.Designations));

                for (int i = 0; i < designations.arraySize; i++)
                {
                    SerializedProperty designation = designations.GetArrayElementAtIndex(i);
                    height += GetDesignationHeight(designation);
                }

                if (designations.arraySize == 0)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }
            }

            return height;
        }

        private float GetDesignationHeight(SerializedProperty designation)
        {
            SerializedProperty foldout = designation.FindPropertyRelative(nameof(Designation.Foldout));
            float height = EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);

            if (foldout.boolValue)
            {
                height += EditorGUIUtility.singleLineHeight * 3;
                SerializedProperty names = designation.FindPropertyRelative(nameof(Designation.Names));

                height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * Mathf.Max(1, names.arraySize);
            }

            return height;
        }
    }
}