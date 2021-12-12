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

    public void UpdateSprites(AvatarIndexData data, AvatarSections sections)
    {
        sections.Head.sprite = m_skinColourData[data.SkinColourIndex].HeadSprite;
        sections.LeftHand.sprite = m_skinColourData[data.SkinColourIndex].HandSprite;
        sections.RightHand.sprite = m_skinColourData[data.SkinColourIndex].HandSprite;
        sections.Neck.sprite = m_skinColourData[data.SkinColourIndex].NeckSprite;
        sections.LeftArm.sprite = m_skinColourData[data.SkinColourIndex].ArmSprite;
        sections.RightArm.sprite = m_skinColourData[data.SkinColourIndex].ArmSprite;
        sections.LeftLeg.sprite = m_skinColourData[data.SkinColourIndex].LegSprite;
        sections.RightLeg.sprite = m_skinColourData[data.SkinColourIndex].LegSprite;

        sections.Hair.sprite = m_hairValues[data.HairTypeIndex].Sprites[data.HairColourIndex];

        sections.LeftEye.sprite = m_eyeValues[data.EyeColourIndex];
        sections.RightEye.sprite = m_eyeValues[data.EyeColourIndex];

        sections.LeftEyebrow.sprite = m_eyebrowValues[data.HairColourIndex];
        sections.RightEyebrow.sprite = m_eyebrowValues[data.HairColourIndex];

        sections.Nose.sprite = m_skinColourData[data.SkinColourIndex].NoseTypes[data.NoseTypeIndex];

        sections.Mouth.sprite = m_mouthValues[data.MouthTypeIndex];

        sections.Shirt.sprite = m_shirtValues[data.ShirtTypeIndex].Sprites[data.ShirtColourIndex];
        sections.LeftShirtArm.sprite = m_shirtSleeveValues[data.ShirtSleeveTypeIndex].Sprites[data.ShirtColourIndex];
        sections.RightShirtArm.sprite = m_shirtSleeveValues[data.ShirtSleeveTypeIndex].Sprites[data.ShirtColourIndex];

        sections.Pants.sprite = m_pantsValues[data.PantsColourIndex];
        sections.LeftPantLeg.sprite = m_pantLegValues[data.PantLegTypeIndex].Sprites[data.PantsColourIndex];
        sections.RightPantLeg.sprite = m_pantLegValues[data.PantLegTypeIndex].Sprites[data.PantsColourIndex];

        sections.LeftFoot.sprite = m_shoesValues[data.ShoesTypeIndex].Sprites[data.ShoesColourIndex];
        sections.RightFoot.sprite = m_shoesValues[data.ShoesTypeIndex].Sprites[data.ShoesColourIndex];
    }
}
