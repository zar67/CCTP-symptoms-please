using SymptomsPlease.ScriptableObjects;
using System;
using TMPro;
using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    [Serializable]
    public struct CreditsEntryStyle
    {
        public TMP_FontAsset TextFont;
        public bool BoldText;
        public bool ItalicText;
        public bool UnderlineText;
        public int TextSize;
        public Color TextColour;
    }

    [CreateAssetMenu(menuName = "SymptomsPlease/UI/CommonPanels/Credits/CreditsDisplayStyle")]
    public class CreditsDisplayStyle : GameScriptableObject
    {
        public CreditsEntryStyle TitleStyle;
        public CreditsEntryStyle HeadingStyle;
        public CreditsEntryStyle DesignationStyle;
        public CreditsEntryStyle NamesStyle;
    }
}