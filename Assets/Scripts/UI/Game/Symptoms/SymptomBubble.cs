using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymptomBubble : MonoBehaviour
{
    public static event Action<SymptomsData> OnSymptomSelected;

    public CanvasGroup CanvasGroup => m_canvasGroup;

    [SerializeField] private CanvasGroup m_canvasGroup = default;
    [SerializeField] private TextMeshProUGUI m_writtenSymptomText = default;
    [SerializeField] private Button m_selectButton = default;

    private SymptomsData m_symptomData = default;

    public void SetData(SymptomsData data)
    {
        m_symptomData = data;
        m_writtenSymptomText.text = data.Description;
    }

    private void OnEnable()
    {
        m_selectButton.onClick.AddListener(OnSelect);   
    }

    private void OnDisable()
    {
        m_selectButton.onClick.RemoveListener(OnSelect);
    }

    private void OnSelect()
    {
        OnSymptomSelected?.Invoke(m_symptomData);
    }
}