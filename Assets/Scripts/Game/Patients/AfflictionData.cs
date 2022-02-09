using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    [Serializable]
    public struct AdviceEffectivnessMap
    {
        public string Advice;
        public ActionEffectiveness Effectiveness;
    }

    public int SymptomsCount => m_symptoms.Length;

    public string DisplayName => m_displayName;

    public string Description => m_description;

    public IEnumerable<AdviceEffectivnessMap> Advice => m_adviceEffectivenessMap;

    public Topic Topic => m_topic;

    [SerializeField] private string m_displayName = "";
    [SerializeField] private string m_description = "";
    [SerializeField] private Topic m_topic = default;
    [SerializeField] private string[] m_symptoms = { };

    [SerializeField] private ActionEffectivenessMap[] m_actionEffectivenessMap = { };
    [SerializeField] private AdviceEffectivnessMap[] m_adviceEffectivenessMap = { };

    private Dictionary<ActionType, ActionEffectiveness> m_actionEffectivenessDictionary;
    private Dictionary<string, ActionEffectiveness> m_adviceEffectivenessDictionary;

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

    public IEnumerable<string> GetAdviceTreatment()
    {
        foreach (AdviceEffectivnessMap adviceEffectivness in m_adviceEffectivenessMap)
        {
            if (adviceEffectivness.Effectiveness == ActionEffectiveness.BEST)
            {
                yield return adviceEffectivness.Advice;
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

    public ActionEffectiveness GetAdviceEffectiveness(string advice)
    {
        return m_adviceEffectivenessDictionary[advice];
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

        m_adviceEffectivenessDictionary = new Dictionary<string, ActionEffectiveness>();
        foreach (AdviceEffectivnessMap map in m_adviceEffectivenessMap)
        {
            m_adviceEffectivenessDictionary[map.Advice] = map.Effectiveness;
        }
    }
}