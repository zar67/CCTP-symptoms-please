using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActionObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static event Action<ActionObject> OnDraggableOnPatient;

    public ActionType ActionType => m_actionType;

    [Header("Action Values")]
    [SerializeField] protected ActionType m_actionType = default;
    [SerializeField] protected bool m_disableIfNotNeeded = true;

    [Header("Display References")]
    [SerializeField] protected Canvas m_canvas = default;
    [SerializeField] protected RectTransform m_canvasRectTransform = default;
    [SerializeField] protected RectTransform m_deskRectTransform = default;
    [SerializeField] protected RectTransform m_dragRectTransform = default;
    [SerializeField] protected Image m_clickableImage = default;

    [Header("Affliction References")]
    [SerializeField] protected AllAfflictionDatas m_allAfflictionDatas = default;

    private Vector2 m_startingPosition;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        m_startingPosition = m_dragRectTransform.position;
        AudioManager.Instance.Play(EAudioClipType.PICK_UP);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = eventData.position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasRectTransform, mousePosition, m_canvas.worldCamera, out Vector2 finalPosition);
        finalPosition = m_canvas.transform.TransformPoint(finalPosition);

        var dragTransformSizeByScreenSize = new Vector2(
            m_dragRectTransform.rect.width / m_canvasRectTransform.rect.width * Screen.width / 2,
            m_dragRectTransform.rect.height / m_canvasRectTransform.rect.height * Screen.height / 2
        );

        if (mousePosition.x >= Screen.width - dragTransformSizeByScreenSize.x || mousePosition.x <= dragTransformSizeByScreenSize.x)
        {
            finalPosition.x = m_dragRectTransform.position.x;
        }
        if (mousePosition.y >= Screen.height - dragTransformSizeByScreenSize.y || mousePosition.y <= dragTransformSizeByScreenSize.y)
        {
            finalPosition.y = m_dragRectTransform.position.y;
        }

        m_dragRectTransform.position = finalPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.Instance.Play(EAudioClipType.DROP);
        var corners = new Vector3[4];
        m_deskRectTransform.GetWorldCorners(corners);
        var deskWorldRect = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        if (m_dragRectTransform.position.x > deskWorldRect.position.x &&
            m_dragRectTransform.position.x < deskWorldRect.position.x + deskWorldRect.width &&
            m_dragRectTransform.position.y > deskWorldRect.position.y &&
            m_dragRectTransform.position.y < deskWorldRect.position.y + deskWorldRect.height)
        {

            // Validate Position In Desk
        }
        else
        {
            m_dragRectTransform.position = m_startingPosition;
            OnDraggableOnPatient?.Invoke(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_dragRectTransform.SetAsLastSibling();
    }

    private void Awake()
    {
        if (m_disableIfNotNeeded)
        {
            bool requiredAction = false;
            foreach (AfflictionData affliction in m_allAfflictionDatas.AfflictionDatas)
            {
                foreach (KeyValuePair<Topic, ModificationsManager.ModificationInstanceData> topicData in ModificationsManager.ActiveTopics)
                {
                    if (affliction.Topic == topicData.Key && affliction.GetTreatments().Contains(m_actionType))
                    {
                        requiredAction = true;
                    }
                }
            }

            m_clickableImage.enabled = requiredAction;
        }
    }

    private void OnEnable()
    {
        ModificationsManager.OnTopicActivated += OnTopicActivated;
        ModificationsManager.OnTopicDeactivated += OnTopicDeactivated;
    }

    private void OnDisable()
    {
        ModificationsManager.OnTopicActivated -= OnTopicActivated;
        ModificationsManager.OnTopicDeactivated -= OnTopicDeactivated;
    }

    private void OnTopicDeactivated(Topic topic)
    {
        if (m_disableIfNotNeeded)
        {
            bool requiredAction = false;
            foreach (AfflictionData affliction in m_allAfflictionDatas.AfflictionDatas)
            {
                if (affliction.GetTreatments().Contains(m_actionType))
                {
                    requiredAction = true;
                }
            }

            m_clickableImage.enabled = requiredAction;
        }
    }

    private void OnTopicActivated(Topic topic)
    {
        if (m_disableIfNotNeeded)
        {
            foreach (AfflictionData affliction in m_allAfflictionDatas.AfflictionDatas)
            {
                if (affliction.Topic == topic && affliction.GetTreatments().Contains(m_actionType))
                {
                    m_clickableImage.enabled = true;
                }
            }
        }
    }
}