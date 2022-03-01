using DG.Tweening;
using SymptomsPlease.SceneManagement;
using SymptomsPlease.Transitions;
using SymptomsPlease.UI.Panels;
using SymptomsPlease.UI.Popups;
using System;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static event Action OnDayStarted;

    public static int Score { get; private set; } = 0;

    [Header("Day End Transition Values")]
    [SerializeField] private SceneData m_sceneData = default;
    [SerializeField] private PanelsData m_panelsData = default;
    [SerializeField] private string m_dayEndPanel = "panel_day_end";
    [SerializeField] private SceneTransitionData m_sceneTransitionData = default;

    [Header("FTUE")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string[] m_ftuePopups = new string[] { };

    private int m_ftuePopupIndex = 0;
    private Popup m_currentFTUEPopup = null;

    public static void IncreaseScore(int value)
    {
        Score += value;
    }

    private void Awake()
    {
        TransitionManager.OnTransitionComplete.Subscribe(OnTransitionComplete);

        Score = 0;
    }

    private void OnEnable()
    {
        DayTimer.OnDayTimeComplete += OnDayTimerComplete;
    }

    private void OnDisable()
    {
        DayTimer.OnDayTimeComplete -= OnDayTimerComplete;
    }

    private void OnTransitionComplete(TransitionData data)
    {
        if (FTUEManager.SeenFTUE)
        {
            OnDayStarted?.Invoke();
        }
        else
        {
            ShowNextFTUEPopup();
        }
    }


    private void ShowNextFTUEPopup()
    {
        if (m_currentFTUEPopup != null)
        {
            m_currentFTUEPopup.OnCloseEvent -= ShowNextFTUEPopup;
        }

        if (m_ftuePopupIndex >= m_ftuePopups.Length)
        {
            FTUEManager.CompleteFTUE();
            OnDayStarted?.Invoke();
            return;
        }

        m_currentFTUEPopup = m_popupData.OpenPopup(m_ftuePopups[m_ftuePopupIndex]);
        m_currentFTUEPopup.OnCloseEvent += ShowNextFTUEPopup;

        m_ftuePopupIndex++;
    }

    private void OnDayTimerComplete()
    {
        DOTween.Clear(true);
        m_panelsData.SetupInitialPanel(m_dayEndPanel);
        m_sceneTransitionData.State = TransitionData.TransitionState.OUT;
        m_sceneData.TransitionToScene(m_sceneTransitionData);
    }
}