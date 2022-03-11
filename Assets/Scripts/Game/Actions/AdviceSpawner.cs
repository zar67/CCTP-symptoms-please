using UnityEngine;
using UnityEngine.UI;

public class AdviceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_adviceObjectPrefab = default;

    [Header("Display References")]
    [SerializeField] private Canvas m_canvas = default;
    [SerializeField] private RectTransform m_canvasRectTransform = default;
    [SerializeField] private RectTransform m_deskRectTransform = default;

    private void OnEnable()
    {
        AdviceBookPopup.OnAdviceSelected += OnAdviceSelected;
    }

    private void OnDisable()
    {
        AdviceBookPopup.OnAdviceSelected -= OnAdviceSelected;
    }

    private void OnAdviceSelected(string advice)
    {
        AdviceObject adviceObject = Instantiate(m_adviceObjectPrefab, transform).GetComponentInChildren<AdviceObject>();
        adviceObject.SetReferences(m_canvas, m_canvasRectTransform, m_deskRectTransform);
        adviceObject.SetAdvice(advice);
    }
}