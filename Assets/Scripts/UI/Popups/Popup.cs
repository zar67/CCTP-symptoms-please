using System;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.UI.Popups
{
    public class Popup : MonoBehaviour
    {
        public event Action OnOpenEvent;
        public event Action OnCloseEvent;

        [SerializeField] protected PopupData m_popupData = default;
        [SerializeField] protected string m_popupType = default;
        [SerializeField] protected bool m_canClose = true;
        [SerializeField] protected bool m_pauseGameWhileOpen = false;

        [Header("Animation References")]
        [SerializeField] protected Animator m_animator = default;
        [SerializeField] protected string m_animatorOpenTrigger = "open";
        [SerializeField] protected string m_animatorCloseTrigger = "close";

        [Header("Button References")]
        [SerializeField] protected Button m_closeButton = default;

        public string Type => m_popupType;
        public bool PauseGameWhenOpen => m_pauseGameWhileOpen;

        public virtual void Initialise()
        {

        }

        public virtual void OnOpenBegin()
        {

        }

        public virtual void OnOpenComplete()
        {
            m_closeButton.interactable = true;
            OnOpenEvent?.Invoke();
        }

        public virtual void OnCloseBegin()
        {

        }

        public virtual void OnCloseComplete()
        {
            gameObject.SetActive(false);
            OnCloseEvent?.Invoke();
        }

        public void PlayOpenAnimation()
        {
            m_animator.SetTrigger(m_animatorOpenTrigger);
            m_closeButton.interactable = false;
        }

        public void PlayCloseAnimation()
        {
            m_animator.SetTrigger(m_animatorCloseTrigger);
            m_closeButton.interactable = false;
        }

        public virtual void OnPositiveClick()
        {
            m_popupData.ClosePopup(Type);
        }

        public virtual void OnNegativeClick()
        {
            m_popupData.ClosePopup(Type);
        }

        private void Awake()
        {
            m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            PlayOpenAnimation();

            m_closeButton.onClick.AddListener(() => m_popupData.ClosePopup(Type));

            if (!m_canClose)
            {
                m_closeButton.onClick.RemoveAllListeners();
                m_closeButton.gameObject.SetActive(false);
            }

            Initialise();
        }
    }
}