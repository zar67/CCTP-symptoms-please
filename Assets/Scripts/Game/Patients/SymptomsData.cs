using SymptomsPlease.ScriptableObjects;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/Game/SymptomsData")]
public class SymptomsData : GameScriptableObject
{
    public string DisplayName => m_displayName;
    public string Description => m_description;

    [SerializeField] private string m_displayName = "";
    [SerializeField] private string m_description = "";
    [SerializeField] private SymptomsData[] m_uncompatibleSymptoms = { };

    public bool IsSymptomCompatible(SymptomsData symptom)
    {
        return !m_uncompatibleSymptoms.Contains(symptom);
    }
}