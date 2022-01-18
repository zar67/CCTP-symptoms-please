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

    public int WrittenSymptomsCount => m_writtenSymptoms.Length;

    public int IconSymptomsCount => m_iconSymptoms.Length;

    public Topic Topic => m_topic;

    [SerializeField] private Topic m_topic = default;
    [SerializeField] private string[] m_writtenSymptoms = { };
    [SerializeField] private Sprite[] m_iconSymptoms = { };

    [SerializeField] private ActionEffectivenessMap[] m_actionEffectivenessMap = { };

    private Dictionary<ActionType, ActionEffectiveness> m_actionEffectivenessDictionary;

    public string GetAfflictionSummary()
    {
        return m_writtenSymptoms[Random.Range(0, m_writtenSymptoms.Length)];
    }

    public string GetWrittenSymptomAtIndex(int index)
    {
        return m_writtenSymptoms[index];
    }

    public Sprite GetIconSymptomAtIndex(int index)
    {
        return m_iconSymptoms[index];
    }

    public void GetRandomSymptoms(PatientData patientData, int numberSymptoms, out List<string> writtenSymptoms, out List<Sprite> iconSymptoms)
    {
        var workingWrittenSymptoms = m_writtenSymptoms.ToList();
        var workingIconSymptoms = m_iconSymptoms.ToList();

        int writtenSymptomsCount = 0;
        int iconSymptomsCount = 0;

        writtenSymptoms = new List<string>();
        iconSymptoms = new List<Sprite>();

        for (int i = 0; i < numberSymptoms; i++)
        {
            if (writtenSymptomsCount >= m_writtenSymptoms.Length && iconSymptomsCount >= m_iconSymptoms.Length)
            {
                return;
            }
            else if (writtenSymptomsCount >= m_writtenSymptoms.Length)
            {
                int randomIndex = Random.Range(0, workingIconSymptoms.Count);
                int actualIndex = Array.IndexOf(m_iconSymptoms, workingIconSymptoms[randomIndex]);

                iconSymptomsCount++;
                iconSymptoms.Add(workingIconSymptoms[randomIndex]);
                workingIconSymptoms.RemoveAt(randomIndex);

                patientData.IconSymptomsShown.Add(actualIndex);
            }
            else if (iconSymptomsCount >= m_iconSymptoms.Length)
            {
                int randomIndex = Random.Range(0, workingWrittenSymptoms.Count);
                int actualIndex = Array.IndexOf(m_writtenSymptoms, workingWrittenSymptoms[randomIndex]);

                writtenSymptomsCount++;
                writtenSymptoms.Add(workingWrittenSymptoms[randomIndex]);
                workingWrittenSymptoms.RemoveAt(randomIndex);

                patientData.WrittenSymptomsShown.Add(actualIndex);
            }
            else
            {
                int randomChance = Random.Range(0, 101);
                if (randomChance < 50)
                {
                    int randomIndex = Random.Range(0, workingWrittenSymptoms.Count);
                    int actualIndex = Array.IndexOf(m_writtenSymptoms, workingWrittenSymptoms[randomIndex]);

                    writtenSymptomsCount++;
                    writtenSymptoms.Add(workingWrittenSymptoms[randomIndex]);
                    workingWrittenSymptoms.RemoveAt(randomIndex);

                    patientData.WrittenSymptomsShown.Add(actualIndex);
                }
                else
                {
                    int randomIndex = Random.Range(0, workingIconSymptoms.Count);
                    int actualIndex = Array.IndexOf(m_iconSymptoms, workingIconSymptoms[randomIndex]);

                    iconSymptomsCount++;
                    iconSymptoms.Add(workingIconSymptoms[randomIndex]);
                    workingIconSymptoms.RemoveAt(randomIndex);

                    patientData.IconSymptomsShown.Add(actualIndex);
                }
            }
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