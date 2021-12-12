using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/AvatarData")]
public class AvatarData : GameScriptableObject
{
    [Serializable]
    public struct SkinColourData
    {
        public Sprite HeadSprite;
        public Sprite NeckSprite;
        public Sprite ArmSprite;
        public Sprite HandSprite;
        public Sprite LegSprite;
        public List<Sprite> NoseTypes;
    }

    [Serializable]
    public struct SpriteList
    {
        public List<Sprite> Sprites;
    }

    [SerializeField] private List<SkinColourData> m_skinColourData = default;
    [SerializeField] private List<SpriteList> m_hairValues = new List<SpriteList>();
    [SerializeField] private List<Sprite> m_eyebrowValues = new List<Sprite>();
    [SerializeField] private List<Sprite> m_eyeValues = new List<Sprite>();
    [SerializeField] private List<Sprite> m_mouthValues = new List<Sprite>();
    [SerializeField] private List<SpriteList> m_shirtValues = new List<SpriteList>();
    [SerializeField] private List<SpriteList> m_shirtSleeveValues = new List<SpriteList>();
    [SerializeField] private List<Sprite> m_pantsValues = new List<Sprite>();
    [SerializeField] private List<SpriteList> m_pantLegValues = new List<SpriteList>();
    [SerializeField] private List<SpriteList> m_shoesValues = new List<SpriteList>();

    public void UpdateSprites(AvatarSections sections)
    {
        sections.Head.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].HeadSprite;
        sections.LeftHand.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].HandSprite;
        sections.RightHand.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].HandSprite;
        sections.Neck.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].NeckSprite;
        sections.LeftArm.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].ArmSprite;
        sections.RightArm.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].ArmSprite;
        sections.LeftLeg.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].LegSprite;
        sections.RightLeg.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].LegSprite;

        sections.Hair.sprite = m_hairValues[GameData.AvatarData.HairTypeIndex].Sprites[GameData.AvatarData.HairColourIndex];

        sections.LeftEye.sprite = m_eyeValues[GameData.AvatarData.EyeColourIndex];
        sections.RightEye.sprite = m_eyeValues[GameData.AvatarData.EyeColourIndex];

        sections.LeftEyebrow.sprite = m_eyebrowValues[GameData.AvatarData.HairColourIndex];
        sections.RightEyebrow.sprite = m_eyebrowValues[GameData.AvatarData.HairColourIndex];

        sections.Nose.sprite = m_skinColourData[GameData.AvatarData.SkinColourIndex].NoseTypes[GameData.AvatarData.NoseTypeIndex];

        sections.Mouth.sprite = m_mouthValues[GameData.AvatarData.MouthTypeIndex];

        sections.Shirt.sprite = m_shirtValues[GameData.AvatarData.ShirtTypeIndex].Sprites[GameData.AvatarData.ShirtColourIndex];
        sections.LeftShirtArm.sprite = m_shirtSleeveValues[GameData.AvatarData.ShirtSleeveTypeIndex].Sprites[GameData.AvatarData.ShirtColourIndex];
        sections.RightShirtArm.sprite = m_shirtSleeveValues[GameData.AvatarData.ShirtSleeveTypeIndex].Sprites[GameData.AvatarData.ShirtColourIndex];

        sections.Pants.sprite = m_pantsValues[GameData.AvatarData.PantsColourIndex];
        sections.LeftPantLeg.sprite = m_pantLegValues[GameData.AvatarData.PantLegTypeIndex].Sprites[GameData.AvatarData.PantsColourIndex];
        sections.RightPantLeg.sprite = m_pantLegValues[GameData.AvatarData.PantLegTypeIndex].Sprites[GameData.AvatarData.PantsColourIndex];

        sections.LeftFoot.sprite = m_shoesValues[GameData.AvatarData.ShoesTypeIndex].Sprites[GameData.AvatarData.ShoesColourIndex];
        sections.RightFoot.sprite = m_shoesValues[GameData.AvatarData.ShoesTypeIndex].Sprites[GameData.AvatarData.ShoesColourIndex];
    }
}
