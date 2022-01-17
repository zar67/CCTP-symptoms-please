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

[Serializable]
public struct AvatarIndexData
{
    public int SkinColourIndex;
    public int HairTypeIndex;
    public int HairColourIndex;
    public int EyeColourIndex;
    public int NoseTypeIndex;
    public int MouthTypeIndex;
    public int ShirtTypeIndex;
    public int ShirtSleeveTypeIndex;
    public int ShirtColourIndex;
    public int PantLegTypeIndex;
    public int PantsColourIndex;
    public int ShoesTypeIndex;
    public int ShoesColourIndex;
}

public class AvatarDisplay : MonoBehaviour
{
    [SerializeField] private AvatarData m_avatarData = default;
    [SerializeField] private AvatarSections m_avatarSections = default;

    public void UpdateSprites(AvatarIndexData data)
    {
        m_avatarData.UpdateSprites(data, m_avatarSections);
    }

    public void ShowAvatar(AvatarIndexData data)
    {
        m_avatarData.UpdateSprites(data, m_avatarSections);
    }
}
