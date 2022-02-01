using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SymptomsPlease/Game/AfflictionData")]
public class AfflictionData : GameScriptableObject
{
    [Serializable]
    private struct ActionEffectivenessMap
    {
        public ActionType Type;
        public ActionEffectiveness Effectiveness;
    }

    public int SymptomsCount => m_symptoms.Length;

    public string DisplayName => m_displayName;

    public string Description => m_description;

    public Topic Topic => m_topic;

    [SerializeField] private string m_displayName = "";
    [SerializeField] private string m_description = "";
    [SerializeField] private Topic m_topic = default;
    [FormerlySerializedAs("m_writtenSymptoms")]
    [SerializeField] private string[] m_symptoms = { };

    [SerializeField] private ActionEffectivenessMap[] m_actionEffectivenessMap = { };

    private Dictionary<ActionType, ActionEffectiveness> m_actionEffectivenessDictionary;

    public string GetAfflictionSummary()
    {
        return m_symptoms[Random.Range(0, m_symptoms.Length)];
    }

    public string GetSymptomAtIndex(int index)
    {
        return m_symptoms[index];
    }

    public IEnumerable<ActionType> GetTreatments()
    {
        foreach (ActionEffectivenessMap actionEffectivness in m_actionEffectivenessMap)
        {
            if (actionEffectivness.Effectiveness == ActionEffectiveness.BEST)
            {
                yield return actionEffectivness.Type;
            }
        }
    }

    public IEnumerable<string> GetRandomSymptoms(PatientData patientData, int numberSymptoms)
    {
        var workingSymptoms = m_symptoms.ToList();

        if (numberSymptoms > m_symptoms.Length)
        {
            numberSymptoms = m_symptoms.Length;
        }

        for (int i = 0; i < numberSymptoms; i++)
        {
            int randomIndex = Random.Range(0, workingSymptoms.Count);
            int actualIndex = Array.IndexOf(m_symptoms, workingSymptoms[randomIndex]);

            yield return workingSymptoms[randomIndex];

            workingSymptoms.RemoveAt(randomIndex);
            patientData.SymptomsShown.Add(actualIndex);
        }
    }

    public ActionEffectiveness GetActionEffectiveness(ActionType action)
    {
        return m_actionEffectivenessDictionary[action];
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_actionEffectivenessDictionary = new Dictionary<ActionType, ActionEffectiveness>();
        foreach (ActionEffectivenessMap map in m_actionEffectivenessMap)
        {
            m_actionEffectivenessDictionary[map.Type] = map.Effectiveness;
        }
    }
}