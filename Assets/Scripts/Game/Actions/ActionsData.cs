using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionsData", menuName = "SymptomsPlease/Game/ActionsData")]
public class ActionsData : GameScriptableObject
{
    [Serializable]
    public struct ActionInfo
    {
        public ActionType Action;
        public string DisplayName;
    }

    [SerializeField] private ActionInfo[] m_effectivnessIcons = { };

    private Dictionary<ActionType, ActionInfo> m_actionsDictionary = new Dictionary<ActionType, ActionInfo>();

    public ActionInfo GetInfoForAction(ActionType action)
    {
        return m_actionsDictionary[action];
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_actionsDictionary = new Dictionary<ActionType, ActionInfo>();

        foreach (ActionInfo map in m_effectivnessIcons)
        {
            m_actionsDictionary.Add(map.Action, map);
        }
    }
}