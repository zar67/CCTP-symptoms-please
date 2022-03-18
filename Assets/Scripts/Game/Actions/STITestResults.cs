using DG.Tweening;
using SymptomsPlease.UI.Popups;
using UnityEngine;
using UnityEngine.UI;

public class STITestResults : MonoBehaviour
{
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_stiResultsPopupID = default;

    [Header("Display References")]
    [SerializeField] private RectTransform m_backgroundTransform = default;
    [SerializeField] private Button m_selectedButton = default;

    [Header("Background Tween Postion")]
    [SerializeField] private Vector3 m_hiddenPosition = new Vector3(0.0f, -5.0f, 0.0f);
    [SerializeField] private Vector3 m_shownPosition = new Vector3(0.0f, -5.0f, 0.0f);

    [Header("Timing Values")]
    [SerializeField] private float m_tweenInDuration = 0.4f;
    [SerializeField] private float m_tweenOutDuration = 0.4f;

    [Header("FTUE")]
    [SerializeField] private string m_testKitResultsFTUEPopup = "popup_ftue_test_kit_result";

    public void ShowResults(PatientData data)
    {
        m_backgroundTransform.DOAnchorPos(m_shownPosition, m_tweenInDuration).OnComplete(OnResultsShown);
    }

    private void OnEnable()
    {
        m_selectedButton.onClick.AddListener(OnSelected);
        m_backgroundTransform.anchoredPosition = m_hiddenPosition;
    }

    private void OnDisable()
    {
        m_selectedButton.onClick.RemoveListener(OnSelected);
    }

    private void OnSelected()
    {
        m_backgroundTransform.DOAnchorPos(m_hiddenPosition, m_tweenOutDuration);
        m_popupData.OpenPopup(m_stiResultsPopupID);
    }

    private void OnResultsShown()
    {
        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_TEST_KIT_RESULTS_FTUE) && PatientManager.PatientSeenInDay == 1)
        {
            m_popupData.OpenPopup(m_testKitResultsFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_TEST_KIT_RESULTS_FTUE);
        }
    }
}