using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ActionObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static event Action<ActionObject> OnDraggableOnPatient;

    public ActionType ActionType => m_actionType;

    [Header("Action Values")]
    [SerializeField] private ActionType m_actionType = default;

    [Header("References")]
    [SerializeField] private Canvas m_canvas = default;
    [SerializeField] private RectTransform m_canvasRectTransform = default;
    [SerializeField] private RectTransform m_deskRectTransform = default;
    [SerializeField] private RectTransform m_dragRectTransform = default;

    private Vector2 m_startingPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_startingPosition = m_dragRectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
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
}