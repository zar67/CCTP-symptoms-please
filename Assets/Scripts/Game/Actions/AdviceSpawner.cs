using DG.Tweening;
using UnityEngine;

public class AdviceSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform m_adviceObjectHolder = default;
    [SerializeField] private AdviceObject m_adviceObject = default;

    [Header("Tween Config")]
    [SerializeField] private Vector3 m_hiddenPosition;
    [SerializeField] private Vector3 m_shownPosition;
    [SerializeField] private float m_tweenInDuration = 0.2f;

    [Header("Display References")]
    [SerializeField] private Canvas m_canvas = default;
    [SerializeField] private RectTransform m_canvasRectTransform = default;
    [SerializeField] private RectTransform m_deskRectTransform = default;

    private void Awake()
    {
        m_adviceObjectHolder.anchoredPosition = m_hiddenPosition;
    }

    private void OnEnable()
    {
        AdviceBookPopup.OnAdviceSelected += OnAdviceSelected;
        ActionObject.OnDraggableOnPatient += OnAdviceGiven;
    }

    private void OnDisable()
    {
        AdviceBookPopup.OnAdviceSelected -= OnAdviceSelected;
        ActionObject.OnDraggableOnPatient -= OnAdviceGiven;
    }

    private void OnAdviceSelected(string advice)
    {
        m_adviceObject.SetReferences(m_canvas, m_canvasRectTransform, m_deskRectTransform);
        m_adviceObject.ShowAdvice(advice);

        m_adviceObjectHolder.anchoredPosition = m_hiddenPosition;
        m_adviceObjectHolder.DOAnchorPos(m_shownPosition, m_tweenInDuration);
    }

    private void OnAdviceGiven(ActionObject advice)
    {
        m_adviceObjectHolder.anchoredPosition = m_hiddenPosition;
    }
}