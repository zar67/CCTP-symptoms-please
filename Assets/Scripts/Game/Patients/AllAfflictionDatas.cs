using SymptomsPlease.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/All Affliction Datas")]
public class AllAfflictionDatas : GameScriptableObject
{
    public IEnumerable<AfflictionData> AfflictionDatas => m_afflictionDatas;

    public int AfflictionsCount => m_afflictionDatas.Length;

    [SerializeField] private AfflictionData[] m_afflictionDatas;

    public AfflictionData GetAfflictionAtIndex(int index)
    {
        return m_afflictionDatas[index];
    }

    public IEnumerable<AfflictionData> GetAfflictionsWithTopic(Topic topic)
    {
        foreach (AfflictionData data in m_afflictionDatas)
        {
            if (data.Topic == topic)
            {
                yield return data;
            }
        }
    }
}
