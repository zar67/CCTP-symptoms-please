using SymptomsPlease.ScriptableObjects;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/Game/SymptomsData")]
public class SymptomsData : GameScriptableObject
{
    public string Description => m_description;

    public Sprite Icon => m_icon;

    [SerializeField] private string m_description = "";
    [SerializeField] private Sprite m_icon = default;
    [SerializeField] private SymptomsData[] m_uncompatibleSymptoms = { };

    public bool IsSymptomCompatible(SymptomsData symptom)
    {
        return !m_uncompatibleSymptoms.Contains(symptom);
    }
}