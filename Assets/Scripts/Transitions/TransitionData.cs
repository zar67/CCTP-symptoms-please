using SymptomsPlease.SceneManagement;
using SymptomsPlease.UI.Panels;
using System;
using UnityEngine;

namespace SymptomsPlease.Transitions
{
    [Serializable]
    public class TransitionData
    {
        public enum TransitionState
        {
            IN,
            OUT
        }

        public bool ForceTransition = true;
        public string TransitionType;
        [HideInInspector] public TransitionState State;
        public bool ShowLoadingScreen = false;
    }

    [Serializable]
    public class SceneTransitionData : TransitionData
    {
        public SceneInfo SceneInfo;
    }

    [Serializable]
    public class PanelTransitionData : TransitionData
    {
        public string PanelID;
        public bool AddToPrevious = true;
    }
}