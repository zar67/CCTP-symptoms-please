using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private Canvas m_canvas = default;
    [SerializeField] private RectTransform m_canvasRectTransform = default;
    [SerializeField] private RectTransform m_dragRectTransform = default;

    private Vector2 m_startingPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_startingPosition = m_dragRectTransform.anchoredPosition;
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
        // Check if over patient
        // Invoke event if over patient
        // return to starting position if outside of desk area
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_dragRectTransform.SetAsLastSibling();
    }
}