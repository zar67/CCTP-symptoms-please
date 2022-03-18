using SymptomsPlease.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.UI.Panels
{
    public class Panel : MonoBehaviour
    {
        [Header("Base Panel References")]
        [SerializeField] private string m_panelID = "";
        [SerializeField] private bool m_disablePreviousPanel = true;
        [SerializeField] private Selectable[] m_interactables = { };

        public string PanelID => m_panelID;
        public bool DisablePreviousPanel => m_disablePreviousPanel;

        public virtual void OnOpen()
        {

        }

        public virtual void OnClose()
        {

        }

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            TransitionManager.OnTransitionBegin.Subscribe(OnTransitionBegin);
            TransitionManager.OnTransitionComplete.Subscribe(OnTransitionComplete);

            foreach (Selectable interactable in m_interactables)
            {
                interactable.interactable = true;
            }
        }

        private void OnDisable()
        {
            TransitionManager.OnTransitionBegin.UnSubscribe(OnTransitionBegin);
            TransitionManager.OnTransitionComplete.UnSubscribe(OnTransitionComplete);
        }

        private void OnTransitionBegin(TransitionData data)
        {
            foreach (Selectable interactable in m_interactables)
            {
                interactable.interactable = false;
            }
        }

        private void OnTransitionComplete(TransitionData data)
        {
            foreach (Selectable interactable in m_interactables)
            {
                interactable.interactable = true;
            }
        }
    }
}