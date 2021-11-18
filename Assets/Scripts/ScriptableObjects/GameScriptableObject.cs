using UnityEngine;

namespace SymptomsPlease.ScriptableObjects
{
    public class GameScriptableObject : ScriptableObject
    {
        [SerializeField, TextArea] private string m_developerDescription;
    }
}