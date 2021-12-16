using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarAttribute : MonoBehaviour
{
    public enum AttributeID
    {
        SKIN_COLOUR,
        HAIR_TYPE,
        HAIR_COLOUR,
        EYE_COLOUR,
        NOSE_TYPE,
        MOUTH_TYPE,
        SHIRT_TYPE,
        SHIRT_SLEEVE_TYPE,
        SHIRT_COLOUR,
        PANT_LEG_TYPE,
        PANTS_COLOUR,
        SHOES_TYPE,
        SHOES_COLOUR
    }

    public event Action<AttributeID, int> OnValueChanged;

    public int CurrentValue => m_currentIndex;

    [SerializeField] private TextMeshProUGUI m_currentValueText = default;
    [SerializeField] private Button m_negativeButton = default;
    [SerializeField] private Button m_positiveButton = default;

    [SerializeField] private bool m_loopValues = false;
    [SerializeField] private AttributeID m_attributeID;
    [SerializeField] private string[] m_attributeValues = { };

    private int m_currentIndex = 0;

    public void InitialiseAttributeValue()
    {
        switch (m_attributeID)
        {
            case AttributeID.SKIN_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.SkinColourIndex;
                break;
            }
            case AttributeID.HAIR_TYPE:
            {
                m_currentIndex = GameData.AvatarData.HairTypeIndex;
                break;
            }
            case AttributeID.HAIR_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.HairColourIndex;
                break;
            }
            case AttributeID.EYE_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.EyeColourIndex;
                break;
            }
            case AttributeID.NOSE_TYPE:
            {
                m_currentIndex = GameData.AvatarData.NoseTypeIndex;
                break;
            }
            case AttributeID.MOUTH_TYPE:
            {
                m_currentIndex = GameData.AvatarData.MouthTypeIndex;
                break;
            }
            case AttributeID.SHIRT_TYPE:
            {
                m_currentIndex = GameData.AvatarData.ShirtTypeIndex;
                break;
            }
            case AttributeID.SHIRT_SLEEVE_TYPE:
            {
                m_currentIndex = GameData.AvatarData.ShirtSleeveTypeIndex;
                break;
            }
            case AttributeID.SHIRT_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.ShirtColourIndex;
                break;
            }
            case AttributeID.PANT_LEG_TYPE:
            {
                m_currentIndex = GameData.AvatarData.PantLegTypeIndex;
                break;
            }
            case AttributeID.PANTS_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.PantsColourIndex;
                break;
            }
            case AttributeID.SHOES_TYPE:
            {
                m_currentIndex = GameData.AvatarData.ShoesTypeIndex;
                break;
            }
            case AttributeID.SHOES_COLOUR:
            {
                m_currentIndex = GameData.AvatarData.ShoesColourIndex;
                break;
            }
        }

        UpdateDisplay();
    }

    private void Awake()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        m_positiveButton.onClick.AddListener(OnPositiveButton);
        m_negativeButton.onClick.AddListener(OnNegativeButton);
    }

    private void OnDisable()
    {
        m_positiveButton.onClick.RemoveListener(OnPositiveButton);
        m_negativeButton.onClick.RemoveListener(OnNegativeButton);
    }

    private void OnNegativeButton()
    {
        m_currentIndex--;

        if (m_currentIndex < 0)
        {
            if (m_loopValues)
            {
                m_currentIndex = m_attributeValues.Length - 1;
            }
            else
            {
                m_currentIndex = 0;
            }
        }

        UpdateDisplay();
    }

    private void OnPositiveButton()
    {
        m_currentIndex++;

        if (m_currentIndex > m_attributeValues.Length - 1)
        {
            if (m_loopValues)
            {
                m_currentIndex = 0;
            }
            else
            {
                m_currentIndex = m_attributeValues.Length - 1;
            }
        }

        m_currentIndex %= m_attributeValues.Length;

        UpdateDisplay();
        OnValueChanged?.Invoke(m_attributeID, m_currentIndex);
    }

    private void UpdateDisplay()
    {
        m_currentValueText.text = m_attributeValues[m_currentIndex];
        
        m_positiveButton.interactable = m_loopValues || (!m_loopValues && m_currentIndex < m_attributeValues.Length - 1);
        m_negativeButton.interactable = m_loopValues || (!m_loopValues && m_currentIndex > 0);
    }
}