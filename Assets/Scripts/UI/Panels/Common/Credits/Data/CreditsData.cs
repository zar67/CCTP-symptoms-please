using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    [Serializable]
    public struct Title
    {
        public string DisplayName;
        public bool Foldout;
        public List<Heading> Headings;
    }

    [Serializable]
    public struct Heading
    {
        public string DisplayName;
        public bool Foldout;
        public List<Designation> Designations;
    }

    [Serializable]
    public struct Designation
    {
        public string DisplayName;
        public bool Foldout;
        public List<string> Names;
    }

    [CreateAssetMenu(menuName = "SymptomsPlease/UI/CommonPanels/Credits/CreditsData")]
    public class CreditsData : GameScriptableObject
    {
        public CreditsDisplayStyle DisplayStyle;
        [HideInInspector] public List<Title> Data = new List<Title>();

        public int GetNumberOfLines()
        {
            int lines = 0;
            foreach (Title title in Data)
            {
                if (title.DisplayName != "")
                {
                    if (Data.IndexOf(title) == 0)
                    {
                        lines += 1;
                    }
                    else
                    {
                        lines += 3;
                    }
                }

                foreach (Heading heading in title.Headings)
                {
                    if (heading.DisplayName != "")
                    {
                        lines += 2;
                    }

                    foreach (Designation designation in heading.Designations)
                    {
                        if (designation.DisplayName != "")
                        {
                            lines += 2;
                        }

                        foreach (string name in designation.Names)
                        {
                            lines += 1;
                        }
                    }
                }
            }

            return lines;
        }

        public string GetString()
        {
            string text = "";
            foreach (Title title in Data)
            {
                if (title.DisplayName != "")
                {
                    if (text == "")
                    {
                        text += $"{FormatStringWithTags(DisplayStyle.TitleStyle, title.DisplayName)}\n";
                    }
                    else
                    {
                        text += $"\n\n{FormatStringWithTags(DisplayStyle.TitleStyle, title.DisplayName)}\n";
                    }
                }

                foreach (Heading heading in title.Headings)
                {
                    if (heading.DisplayName != "")
                    {
                        text += $"\n{FormatStringWithTags(DisplayStyle.HeadingStyle, heading.DisplayName)}\n";
                    }

                    foreach (Designation designation in heading.Designations)
                    {
                        if (designation.DisplayName != "")
                        {
                            text += $"\n{FormatStringWithTags(DisplayStyle.DesignationStyle, designation.DisplayName)}\n";
                        }

                        foreach (string name in designation.Names)
                        {
                            text += $"{FormatStringWithTags(DisplayStyle.NamesStyle, name)}\n";
                        }
                    }
                }
            }

            return text;
        }

        private string FormatStringWithTags(CreditsEntryStyle style, string tagString)
        {
            string value = "{0}";
            value = string.Format(value, $"<color=#{ColorUtility.ToHtmlStringRGB(style.TextColour)}>{{0}}</color>");
            value = string.Format(value, $"<font={style.TextFont.name}>{{0}}</font>");
            value = string.Format(value, $"<size={style.TextSize}>{{0}}</size>");

            if (style.BoldText)
            {
                value = string.Format(value, $"<b>{{0}}</b>");
            }
            if (style.ItalicText)
            {
                value = string.Format(value, $"<i>{{0}}</i>");
            }
            if (style.UnderlineText)
            {
                value = string.Format(value, $"<u>{{0}}</u>");
            }

            value = string.Format(value, tagString);
            return value;
        }
    }
}