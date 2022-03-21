using DG.Tweening;
using SymptomsPlease.UI.Popups;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdviceObject : ActionObject
{
    public string Advice
    {
        get; private set;
    }

    [SerializeField] private Vector3 m_hiddenPosition;
    [SerializeField] private Vector3 m_shownPosition;
    [SerializeField] private float m_tweenInDuration = 0.2f;

    [Header("FTUE")]
    [SerializeField] private PopupData m_popupData = default;
    [SerializeField] private string m_adviceSlipFTUEPopup = "popup_ftue_advice_slip";

    private Tween m_moveInTween = null;

    public void SetAdvice(string advice)
    {
        Advice = advice;
    }

    public void SetReferences(Canvas canvas, RectTransform canvasRectTransform, RectTransform deskRectTransform)
    {
        m_canvas = canvas;
        m_canvasRectTransform = canvasRectTransform;
        m_deskRectTransform = deskRectTransform;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (m_moveInTween != null)
        {
            m_moveInTween.Kill();
            m_moveInTween = null;
        }
    }

    private void Awake()
    {
        if (!FTUEManager.IsFTUETypeHandled(EFTUEType.SEEN_ADVICE_SLIP_FTUE))
        {
            m_popupData.OpenPopup(m_adviceSlipFTUEPopup);
            FTUEManager.HandleFTUEType(EFTUEType.SEEN_ADVICE_SLIP_FTUE);
        }
    }

    private void OnEnable()
    {
        OnDraggableOnPatient += OnAdviceGiven;

        m_dragRectTransform.position = m_hiddenPosition;
        m_moveInTween = m_dragRectTransform.DOMove(m_shownPosition, m_tweenInDuration);
    }

    private void OnDisable()
    {
        OnDraggableOnPatient -= OnAdviceGiven;
    }

    private void OnAdviceGiven(ActionObject advice)
    {
        Destroy(gameObject);
    }
}