using SymptomsPlease.SaveSystem;
using SymptomsPlease.UI.Popups;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct AvatarSections
{
    public Image Hair;
    public Image Head;
    public Image LeftEyebrow;
    public Image RightEyebrow;
    public Image LeftEye;
    public Image RightEye;
    public Image Nose;
    public Image Mouth;
    public Image Neck;
    public Image Shirt;
    public Image LeftArm;
    public Image LeftShirtArm;
    public Image RightArm;
    public Image RightShirtArm;
    public Image LeftHand;
    public Image RightHand;
    public Image Pants;
    public Image LeftLeg;
    public Image LeftPantLeg;
    public Image RightLeg;
    public Image RightPantLeg;
    public Image LeftFoot;
    public Image RightFoot;
}

public class CustomiseAvatarPopup : Popup
{
    [SerializeField] private Button m_saveAvatarButton = default;

    [SerializeField] private AvatarAttribute[] m_attributes = default;

    [SerializeField] private AvatarData m_avatarData = default;
    [SerializeField] private AvatarSections m_avatarSections = default;

    private void OnEnable()
    {
        m_saveAvatarButton.onClick.AddListener(OnSaveAvatar);
        foreach (var attribute in m_attributes)
        {
            attribute.OnValueChanged += AttributeValueChanged;
        }

        m_avatarData.UpdateSprites(m_avatarSections);
    }

    private void OnDisable()
    {
        m_saveAvatarButton.onClick.RemoveListener(OnSaveAvatar);
        foreach (var attribute in m_attributes)
        {
            attribute.OnValueChanged -= AttributeValueChanged;
        }
    }

    private void AttributeValueChanged(AvatarAttribute.AttributeID attributeID, int currentIndex)
    {
        switch (attributeID)
        {
            case AvatarAttribute.AttributeID.SKIN_COLOUR:
            {
                GameData.AvatarData.SkinColourIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.HAIR_TYPE:
            {
                GameData.AvatarData.HairTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.HAIR_COLOUR:
            {
                GameData.AvatarData.HairColourIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.EYE_COLOUR:
            {
                GameData.AvatarData.EyeColourIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.NOSE_TYPE:
            {
                GameData.AvatarData.NoseTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.MOUTH_TYPE:
            {
                GameData.AvatarData.MouthTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.SHIRT_TYPE:
            {
                GameData.AvatarData.ShirtTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.SHIRT_SLEEVE_TYPE:
            {
                GameData.AvatarData.ShirtSleeveTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.SHIRT_COLOUR:
            {
                GameData.AvatarData.ShirtColourIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.PANT_LEG_TYPE:
            {
                GameData.AvatarData.PantLegTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.PANTS_COLOUR:
            {
                GameData.AvatarData.PantsColourIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.SHOES_TYPE:
            {
                GameData.AvatarData.ShoesTypeIndex = currentIndex;
                break;
            }
            case AvatarAttribute.AttributeID.SHOES_COLOUR:
            {
                GameData.AvatarData.ShoesColourIndex = currentIndex;
                break;
            }
        }

        m_avatarData.UpdateSprites(m_avatarSections);
    }

    private void OnSaveAvatar()
    {
        SaveSystem.Save();
        m_popupData.ClosePopup(m_popupType);
    }
}