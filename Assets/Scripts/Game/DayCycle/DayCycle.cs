using DG.Tweening;
using SymptomsPlease.SceneManagement;
using SymptomsPlease.Transitions;
using SymptomsPlease.UI.Panels;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public static float Score { get; private set; } = 0;

    [Header("Day End Transition Values")]
    [SerializeField] private SceneData m_sceneData = default;
    [SerializeField] private PanelsData m_panelsData = default;
    [SerializeField] private string m_dayEndPanel = "panel_day_end";
    [SerializeField] private SceneTransitionData m_sceneTransitionData = default;

    public static void IncreaseScore(float value)
    {
        Score += value;
    }

    private void Awake()
    {
        Score = 0;
    }

    private void OnEnable()
    {
        PatientManager.OnPatientSeen += OnPatientSeen;
    }

    private void OnDisable()
    {
        PatientManager.OnPatientSeen -= OnPatientSeen;
    }

    private void OnPatientSeen(PatientManager.PatientSeenData data)
    {
        if (data.IsEndOfDay)
        {
            DOTween.Clear(true);
            m_panelsData.SetupInitialPanel(m_dayEndPanel);
            m_sceneTransitionData.State = TransitionData.TransitionState.OUT;
            m_sceneData.TransitionToScene(m_sceneTransitionData);
        }
    }
}