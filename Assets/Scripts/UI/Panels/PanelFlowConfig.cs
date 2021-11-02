using SymptomsPlease.ScriptableObjects;
using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    [CreateAssetMenu(menuName = "SymptomsPlease/UI/Panels/Panel Flow Config")]
    public class PanelFlowConfig : GameScriptableObject
    {
        public string StartingPanel = string.Empty;
        public PanelsGraph Flow;
    }
}