using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    public class Panel : MonoBehaviour
    {
        [Header("Base Panel References")]
        [SerializeField] private string m_panelID;
        [SerializeField] private bool m_disablePreviousPanel = true;

        public string PanelID => m_panelID;
        public bool DisablePreviousPanel => m_disablePreviousPanel;

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}