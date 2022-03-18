using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SymptomsPlease.SceneManagement
{
    [CustomEditor(typeof(SceneData))]
    public class SceneDataEditor : Editor
    {
        private SceneData m_buildSceneData;
        private ReorderableList m_scenesList;

        private float m_lineHeight;

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }

            m_lineHeight = EditorGUIUtility.singleLineHeight;

            m_buildSceneData = (SceneData)target;
            m_scenesList = new ReorderableList(serializedObject, serializedObject.FindProperty(nameof(m_buildSceneData.Scenes)), true, true, true, true);

            m_scenesList.drawElementCallback += DrawSceneListElement;
            m_scenesList.drawHeaderCallback += DrawSceneListHeader;
            m_scenesList.onRemoveCallback += OnRemoveButton;
        }

        private void DrawSceneListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = m_scenesList.serializedProperty.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y, rect.width - 30, m_lineHeight), element, GUIContent.none);
                return;
            }

            var elementObj = new SerializedObject(element.objectReferenceValue);

            GUI.enabled = false;
            SceneInfo scene = m_buildSceneData.Scenes[index];
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 30, m_lineHeight), elementObj.FindProperty(nameof(scene.BuildIndex)).intValue.ToString());
            GUI.enabled = true;

            EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y, rect.width - 30, m_lineHeight), element, GUIContent.none);
        }

        private void DrawSceneListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Scenes");
        }

        private void OnRemoveButton(ReorderableList list)
        {
            m_buildSceneData.Scenes.RemoveAt(m_scenesList.index);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build List to Build Settings"))
            {
                m_buildSceneData.BuildSceneDataListToSettings();
            }

            serializedObject.Update();
            m_scenesList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(10);
        }
    }
}
