using SymptomsPlease.ScriptableObjects;
using SymptomsPlease.Utilities.Attributes;
using UnityEngine;

namespace SymptomsPlease.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneInfo", menuName = "SymptomsPlease/SceneData/SceneInfo")]
    public class SceneInfo : GameScriptableObject
    {
        [ReadOnly] public int BuildIndex;
#if UNITY_EDITOR
        public UnityEditor.SceneAsset SceneAsset;
#endif
        public string Type;
    }
}
