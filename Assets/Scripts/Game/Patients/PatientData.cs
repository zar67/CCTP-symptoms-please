using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/Game/PatientData")]
public class PatientData : GameScriptableObject
{
    [Serializable]
    struct ActionEffectivenessMap
    {
        public ActionEffectiveness Effectiveness;
        public ActionType Type;
    }

    public string PatientName => m_patientName;

    public string MainAppointmentInfo => m_mainAppointmentInfo;

    public int WrittenSymptomsCount => m_writtenSymptoms.Length;

    public int IconSymptomsCount => m_iconSymptoms.Length;

    [SerializeField] private string m_patientName;
    [SerializeField] private string m_mainAppointmentInfo;
    [SerializeField] private string[] m_writtenSymptoms = { };
    [SerializeField] private Sprite[] m_iconSymptoms = { };

    [SerializeField] private ActionEffectivenessMap[] m_actionEffectivenessMap = { };

    private Dictionary<ActionType, ActionEffectiveness> m_actionEffectivenessDictionary;

    public IEnumerable<string> GetRandomStringSymptom(int number)
    {
        for (int i = 0; i < Mathf.Min(number, m_writtenSymptoms.Length); i++)
        {
            yield return m_writtenSymptoms[i];
        }

        //if (number > m_writtenSymptoms.Length)
        //{
        //    for (int i = 0; i < m_writtenSymptoms.Length; i++)
        //    {
        //        yield return m_writtenSymptoms[i];
        //    }
        //}
        //else
        //{
        //    var usedIndexes = new List<int>();
        //    while (usedIndexes.Count < number)
        //    {
        //        int index = Random.Range(0, m_writtenSymptoms.Length - 1);

        //        while (usedIndexes.Contains(index))
        //        {
        //            index = Random.Range(0, m_writtenSymptoms.Length - 1);
        //        }

        //        usedIndexes.Add(index);
        //        yield return m_writtenSymptoms[index];
        //    }
        //}
    }

    public IEnumerable<Sprite> GetRandomIconSymptom(int number)
    {
        for (int i = 0; i < Mathf.Min(number, m_iconSymptoms.Length); i++)
        {
            yield return m_iconSymptoms[i];
        }
        //if (number > m_iconSymptoms.Length)
        //{
        //    for (int i = 0; i < m_iconSymptoms.Length; i++)
        //    {
        //        yield return m_iconSymptoms[i];
        //    }
        //}
        //else
        //{
        //    var usedIndexes = new List<int>();
        //    while (usedIndexes.Count < number)
        //    {
        //        int index = Random.Range(0, m_iconSymptoms.Length - 1);

        //        while (usedIndexes.Contains(index))
        //        {
        //            index = Random.Range(0, m_iconSymptoms.Length - 1);
        //        }

        //        usedIndexes.Add(index);
        //        yield return m_iconSymptoms[index];
        //    }
        //}
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