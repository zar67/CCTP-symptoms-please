using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionEffectivenessData", menuName = "SymptomsPlease/Game/ActionEffectivenessData")]
public class ActionEffectivenessData : GameScriptableObject
{
    [Serializable]
    public struct EffectivenessInfo
    {
        public ActionEffectiveness Effectiveness;
        public string DisplayText;
    }

    [SerializeField] private EffectivenessInfo[] m_effectivenessData = { };

    private Dictionary<ActionEffectiveness, EffectivenessInfo> m_effectivnessDictionary = new Dictionary<ActionEffectiveness, EffectivenessInfo>();

    public EffectivenessInfo GetInfoForEffectiveness(ActionEffectiveness effectiveness)
    {
        return m_effectivnessDictionary[effectiveness];
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_effectivnessDictionary = new Dictionary<ActionEffectiveness, EffectivenessInfo>();

        foreach (EffectivenessInfo map in m_effectivenessData)
        {
            m_effectivnessDictionary.Add(map.Effectiveness, map);
        }
    }
}